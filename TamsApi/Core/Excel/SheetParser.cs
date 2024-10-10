using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using TamsApi.Core.Excel.Mappings;
using TamsApi.Models;

namespace TamsApi.Core.Excel
{
    public class SheetParser<TModel> : ISheetParser<TModel> where TModel : class, new()
    {
        private Stream _excelStream;
        private IWorkbook _workbook;
        private IFormulaEvaluator _evaluator;
        private ISheet _sheet;
        private readonly ExcelMap _map;
        private readonly IMapper _mapper;
        private readonly IExcelRowValidator<TModel> _validator;
        private readonly ILogger<TModel> _logger;
        private HeaderIndexMap _primaryKey;

        public SheetParser(IExcelMapProvider<TModel> mapProvider, IMapper mapper, IExcelRowValidator<TModel> validator, ILogger<TModel> logger)
        {
            _map = mapProvider.ExcelMap;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }

        public IWorkbook Workbook => (_workbook ??= Excel.Workbook.OpenRead(_excelStream));
        public ISheet Sheet => (_sheet ??= Workbook.GetSheetAt(0));
        public List<HeaderIndexMap> MergedHeaderIndex { get; } = new List<HeaderIndexMap>();
        public List<HeaderIndexMap> UnmergedHeaderIndex { get; } = new List<HeaderIndexMap>();
        public IFormulaEvaluator CellEvaluator => _evaluator ??= Workbook.GetCreationHelper().CreateFormulaEvaluator();

        public ParseResult<TModel> Parse(Stream stream)
        {
            _excelStream = stream;
            _primaryKey = BuildIndexOnValidSheet();

            var result = new ParseResult<TModel>();

            var rowCount = Sheet.LastRowNum;
            IDictionary<string, object> mergeData = null;

            for (var rowNum = Sheet.FirstRowNum + 1; rowNum <= rowCount; rowNum++)
            {
                IRow row = null;
                try
                {
                    row = Sheet.GetRow(rowNum);
                    var parsedRow = ParseRow(row, mergeData ?? new Dictionary<string, object>());
                    mergeData = parsedRow.MergeData;
                    var errors = _validator.Validate(parsedRow.Model);
                    if (errors.Blank())
                    {
                        result.Add(parsedRow.Model);
                    }
                    else
                    {
                        result.Add(ExcelRowCoord(row.RowNum),
                                parsedRow.ParsedCells,
                                errors);
                    }
                }
                catch (Exception exception)
                {
                    throw new ExcelParseException($"Error encountered at row {ExcelRowCoord(row.RowNum)}.<br>{exception.Message}");
                }
            }

            return result;
        }

        private HeaderIndexMap BuildIndexOnValidSheet()
        {
            if (Sheet.LastRowNum == 0)
            {
                throw new ExcelParseException($"Worksheet has no rows: {Sheet.SheetName}");
            }
            return BuildHeaderIndexes();
        }

        private HeaderIndexMap BuildHeaderIndexes()
        {
            var headerRow = Sheet.GetRow(Sheet.FirstRowNum);
            var lookup = _map.Mappings.ToDictionary(c => c.ExcelHeader.ToLowerInvariant());
            HeaderIndexMap? primaryKey = null;
            for (var cell = 0; cell <= headerRow.LastCellNum; cell++)
            {
                var header = headerRow.GetCell(cell)?.StringCellValue?.Trim();
                var cellValue = header?.ToLowerInvariant();
                if (cellValue.IsPresent() && lookup.ContainsKey(cellValue))
                {
                    ExcelColumnMapping cellMap = lookup[cellValue];
                    var headerMap = new HeaderIndexMap(cell, header, cellMap);
                    if (headerMap.ColumnMap.DbColumn.ToLowerInvariant() == _map.PrimaryKey.ToLowerInvariant())
                    {
                        primaryKey = headerMap;
                    }
                    
                    (cellMap.IsMergedCell ? MergedHeaderIndex : UnmergedHeaderIndex).Add(headerMap);
                }
            }

            if (primaryKey == null)
            {
                var excelHeader = _map.Mappings.FirstOrDefault(
                    m => m.DbColumn.Equals(_map.PrimaryKey, StringComparison.InvariantCultureIgnoreCase))
                    ?.ExcelHeader;
                throw new ExcelParseException($"Unrecognized worksheet: {Sheet.SheetName} (no {excelHeader} cell)");
            }

            return primaryKey.Value;
        }

        private ParsedRow<TModel> ParseRow(IRow row, IDictionary<string, object> parentMergeData)
        {
            var keyCell = row.GetCell(_primaryKey.CellIndex);
            var keyCellIsMergedCell = keyCell != null && keyCell.IsMergedCell;
            IDictionary<string, object> sharedCells;
            if (keyCell.IsBlank() && keyCellIsMergedCell && parentMergeData.IsNotEmpty())
            {
               sharedCells = parentMergeData;
            }
            else
            {
                sharedCells = ParseCells(row, MergedHeaderIndex);
            }

            var uniqCells = ParseCells(row, UnmergedHeaderIndex);
            var model = CreateModel(sharedCells, uniqCells);
            return new ParsedRow<TModel>(model, uniqCells.Merge(sharedCells), sharedCells);
        }

        private IDictionary<string, object> ParseCells(IRow row, List<HeaderIndexMap> cellsMap)
        {
            var fields = new Dictionary<string, object>();
            foreach (var headerInfo in cellsMap)
            {
                var cellValue = CastCell(row.GetCell(headerInfo.CellIndex), headerInfo.ColumnMap);
                fields.Add(headerInfo.ColumnMap.DbColumn, cellValue);
            }
            return fields;
        }

        private TModel CreateModel(params IDictionary<string, object>[] parsedCells)
        {
           var model = new TModel();
           foreach(var data in parsedCells)
           {
                _mapper.Map(data, model);
           }
           return model;
        }

        private static string ExcelCoords(ICell cell)
        {
            return ExcelCoords(cell.ColumnIndex, cell.RowIndex);
        }

        private static string ExcelCoords(int cellIndex, int rowIndex)
        {
            // Add 1 to offset 0-based indexing to 1-based Excel indexing for Cell Coord
            return $"[Cell: {cellIndex + 1}, Row: {ExcelRowCoord(rowIndex)}]";
        }

        // Add 1 to offset 0-based indexing
        private static int ExcelRowCoord(int rowIndex)
        {
            return rowIndex + 1;
        }

        public object CastCell(ICell cell, ExcelColumnMapping columnMap)
        {
            var evaluatedCell = WrappedCell.Wrap(cell, CellEvaluator);
            var typeName = columnMap.CastType.ToLowerInvariant();
            if (CellUtils.TypeMap.ContainsKey(typeName))
            {
                try
                {
                    return CellUtils.TypeMap[typeName].Invoke(evaluatedCell);
                }
                catch(InvalidOperationException e)
                {
                    throw new ExcelParseException(
                        $"Failed to cast cell to {typeName}: {ExcelCoords(cell)} => {columnMap.ExcelHeader}", e);
                }
            }

            throw new ExcelParseException(
                $"Unsupported cast {typeName}: {ExcelCoords(cell)} => {columnMap.ExcelHeader}");
        }
    }

    public readonly struct HeaderIndexMap
    {
        public HeaderIndexMap(int column, string text, ExcelColumnMapping columnMap)
        {
            CellIndex = column;
            Text = text;
            ColumnMap = columnMap;
        }

        public int CellIndex { get; }
        public string Text { get; }
        public ExcelColumnMapping ColumnMap { get; }
    }

    public class ParsedRow<TModel> where TModel : class
    {
        public TModel Model { get; set; }
        public IDictionary<string, object> ParsedCells { get; set; }
        public IDictionary<string, object> MergeData { get; set; }
        public ParsedRow(TModel model, IDictionary<string, object> parsedCells, IDictionary<string, object> mergeData)
        {
            Model = model;
            ParsedCells = parsedCells;
            MergeData = mergeData;
        }
    }
}
