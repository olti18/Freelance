using AutoMapper;
using Freelance.Models.Domain;
using Freelance.Models.DTO.ProjectPostDto;
using Freelance.Models.DTO.ProjectPostDTO;

namespace Freelance.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            //Mapping ProjectPost Model
            CreateMap<AddProjectPostDto, ProjectPost>().ReverseMap();
            CreateMap<ProjectPost, ProjectPostDto>().ReverseMap();
            CreateMap<UpdateProjectPostRequestDto, ProjectPost>().ReverseMap();

        }
    }
}
