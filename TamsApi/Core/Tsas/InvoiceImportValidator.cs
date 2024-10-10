using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Core.Tsas
{
    public class InvoiceImportValidator : ChildModelRowValidator<Invoice>
    {
        public InvoiceImportValidator(ITsaRepository repository, DataAnnotationsValidator<Invoice> annotationsValidator) :
            base("invoice", repository, annotationsValidator)
        {
        }
    }
}
