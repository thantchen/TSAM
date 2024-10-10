using System.Collections.Generic;
using TamsApi.Data;
using TamsApi.Models;
using TamsApi.Core.Excel;

namespace TamsApi.Core.Tsas
{
    public class TsaRowValidator : IExcelRowValidator<TsaSchedule>
    {
        private readonly ITsaRepository _repository;
        private readonly DataAnnotationsValidator<TsaSchedule> _annotationsValidator;

        public TsaRowValidator(ITsaRepository repository, DataAnnotationsValidator<TsaSchedule> annotationsValidator)
        {
            _repository = repository;
            _annotationsValidator = annotationsValidator;
        }

        public List<string> Validate(TsaSchedule model)
        {
            var errors = new List<string>(_annotationsValidator.Validate(model));
            if (_repository.Exists(model.TsaSubId))
            {
                errors.Add($"Cannot insert duplicate TSA Schedule: {model.TsaSubId}.");
            }

            return errors;
        }
    }
}
