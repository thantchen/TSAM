using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TamsApi.Core.Identity;
using TamsApi.Models;
using TamsEmail.Views.Templates;

namespace TamsApi.Data
{
    public interface IEmailRepository
    {
        Task<AddRequestSubmittedViewModel> GetNewAddRequestAsync(List<int> idList);
        Task<StatusChangeAddLogViewModel> GetUpdatedAddRequestAsync(int id);
        Task<ChangeRequestSubmittedViewModel> GetNewChangeRequestAsync(List<int> idList);
        Task<StatusChangeChangeLogViewModel> GetUpdatedChangeRequestAsync(int id);
        Task<DisputeRequestSubmittedViewModel> GetNewDisputeRequestAsync(List<int> idList);
        Task<StatusChangeDisputeLogViewModel> GetUpdatedDisputeRequestAsync(int id);
    }

}
