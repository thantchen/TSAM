using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace TamsApi.Models.MapProfiles
{
    public class TsaMap : Profile
    {
        // https://stackoverflow.com/questions/43947475/how-to-ignore-null-values-for-all-source-members-during-mapping-in-automapper-6
        public TsaMap()
        {
            CreateMap<IFormFile, FileRepository>();
            CreateMap<FileRepository, UploadDto>()
                .ForMember(d => d.File, opt => opt.MapFrom(r => r.FileName))
                .ForMember(d => d.UploadedDate, opt => opt.MapFrom(r => r.CreatedDate));
            CreateMap<ChangeResolutionLog, TsaSchedule>()
                .ForMember(t => t.TsaId, opt => opt.Ignore())
                .ForMember(t => t.TsaSubId, opt => opt.Ignore())
                .ForMember(t => t.CreatedUser, opt => opt.Ignore())
                .ForMember(t => t.CreatedUserId, opt => opt.Ignore())
                .ForMember(t => t.CreatedDate, opt => opt.Ignore())
                .ForMember(t => t.LastModifiedUserId, opt => opt.Ignore())
                .ForMember(t => t.LastModifiedUser, opt => opt.Ignore())
                .ForMember(t => t.LastModifiedDate, opt => opt.Ignore())
                .ForMember(t => t.FileRepositoryId, opt => opt.Ignore())
                .ForMember(t => t.FileRepository, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
