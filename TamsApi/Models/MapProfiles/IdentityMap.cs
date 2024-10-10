using AutoMapper;
using TamsApi.InputModels;

namespace TamsApi.Models.MapProfiles
{
    public class IdentityMap : Profile
    {
        public IdentityMap()
        {
            CreateMap<NewUser, User>();
        }
    }
}
