using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TamsApi.Core
{
    public class DataAnnotationsValidator<TModel> where TModel : class
    {
        public virtual List<string> Validate(TModel model)
        {
            var context = new ValidationContext(model);
            var issues = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, issues, true);
            return issues.Select(r => r.ErrorMessage).ToList();
        }
    }
}
