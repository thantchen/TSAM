using System.Collections.Generic;
using TamsApi.Data;
using TamsApi.Models;
using TamsApi.Core.Excel;

namespace TamsApi.Core.Tsas
{
    public class ChangeResolutionImportValidator : IExcelRowValidator<ChangeResolutionLog>
    {
        private readonly ITsaRepository _repository;
        private readonly DataAnnotationsValidator<ChangeResolutionLog> _annotationsValidator;

        public ChangeResolutionImportValidator(ITsaRepository repository, DataAnnotationsValidator<ChangeResolutionLog> annotationsValidator)
        {
            _repository = repository;
            _annotationsValidator = annotationsValidator;
        }

        public List<string> Validate(ChangeResolutionLog model)
        {
            var errors = new List<string>(_annotationsValidator.Validate(model));
            if (!_repository.Exists(model.TsaSubId))
            {
                errors.Add($"Cannot import changes without a TSA schedule with sub ID: {model.TsaSubId}.");
            }
            return errors;
        }
    }
}
