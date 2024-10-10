using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsApi.Core.Excel
{
    public interface IEvaluatedCell
    {
        ICell RawCell { get; }
        CellType CellType { get; }
        int ColumnIndex { get; }
        int RowIndex { get; }

        bool BooleanCellValue { get; }
        string StringCellValue { get; }
        double NumericCellValue { get; }
        DateTime? DateCellValue { get; }
    }

    public class WrappedCell : IEvaluatedCell
    {
        public WrappedCell(ICell cell)
        {
            RawCell = cell;
        }

        public ICell RawCell { get; }

        public virtual CellType CellType => RawCell?.CellType ?? CellType.Unknown;

        public int ColumnIndex => RawCell.ColumnIndex;

        public int RowIndex => RawCell.RowIndex;

        public virtual bool BooleanCellValue => RawCell.BooleanCellValue;

        public virtual string StringCellValue => RawCell.StringCellValue;

        public virtual double NumericCellValue => RawCell.NumericCellValue;

        public virtual DateTime? DateCellValue => RawCell.DateCellValue;

        public static IEvaluatedCell Wrap(ICell cell, IFormulaEvaluator evaluator)
        {
            if (cell == null || cell.CellType != CellType.Formula)
            {
                return new WrappedCell(cell);
            }

            if (evaluator == null)
            {
                throw new ArgumentNullException("Cannot evaluate formula cell without an evaluator.");
            }

            return new EvaluatedCell(evaluator.Evaluate(cell), cell);
        }
    }

    public class EvaluatedCell : WrappedCell
    {
        public CellValue EvaluatedValue { get; }

        public EvaluatedCell(CellValue evaluatedValue, ICell cell) : base(cell)
        {
            EvaluatedValue = evaluatedValue;
        }

        public override CellType CellType => EvaluatedValue.CellType;
        public override bool BooleanCellValue => EvaluatedValue.BooleanValue;
        public override string StringCellValue => EvaluatedValue.StringValue;
        public override double NumericCellValue => EvaluatedValue.NumberValue;
    }
}
