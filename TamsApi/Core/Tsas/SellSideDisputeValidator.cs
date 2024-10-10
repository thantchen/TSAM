using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Core.Tsas
{
    public class SellSideDisputeValidator : ChildModelRowValidator<SellSideNotificationDispute>
    {
        public SellSideDisputeValidator(ITsaRepository repository, DataAnnotationsValidator<SellSideNotificationDispute> annotationsValidator) : 
            base("sell side dispute", repository, annotationsValidator)
        {
        }
    }
}
