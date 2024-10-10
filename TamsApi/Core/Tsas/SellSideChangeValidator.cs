using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Core.Tsas
{
    public class SellSideChangeValidator : ChildModelRowValidator<SellSideNotificationChange>
    {
        public SellSideChangeValidator(ITsaRepository repository, DataAnnotationsValidator<SellSideNotificationChange> annotationsValidator) : 
            base("sell side change", repository, annotationsValidator)
        {
        }
    }
}
