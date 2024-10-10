using System.Collections.Generic;
using TamsApi.Core.Excel;
using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Core.Tsas
{
    public abstract class ChildModelRowValidator<TModel> : IExcelRowValidator<TModel> where TModel : class, ITsaChild
    {
        private readonly ITsaRepository _repository;
        private readonly DataAnnotationsValidator<TModel> _annotationsValidator;
        private readonly string _modelName;

        protected ChildModelRowValidator(string modelName, ITsaRepository repository, DataAnnotationsValidator<TModel> annotationsValidator)
        {
            _repository = repository;
            _annotationsValidator = annotationsValidator;
            _modelName = modelName ?? nameof(TModel);
        }

        public List<string> Validate(TModel model)
        {
            var errors = new List<string>(_annotationsValidator.Validate(model));
            if (!_repository.Exists(model.TsaSubId))
            {
                errors.Add($"Cannot import {_modelName} without a TSA schedule with sub ID: {model.TsaSubId}.");
            }
            return errors;
        }
    }
}
