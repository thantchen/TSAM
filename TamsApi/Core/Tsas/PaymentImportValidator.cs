using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Core.Tsas
{
    public class PaymentImportValidator : ChildModelRowValidator<Payment>
    {
        public PaymentImportValidator(ITsaRepository repository, DataAnnotationsValidator<Payment> annotationsValidator) :
            base("payment", repository, annotationsValidator)
        {
        }
    }
}
