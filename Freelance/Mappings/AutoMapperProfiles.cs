using AutoMapper;
using Freelance.Models.Domain;
using Freelance.Models.DTO.ProjectPostDto;
using Freelance.Models.DTO.ProjectPostDTO;
using Freelance.Models.DTO.ProposalDTO;

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
			//Mapping Proposal Model
			CreateMap<Proposal, ProposalDto>().ReverseMap();
			CreateMap<AddProposalDto, Proposal>().ReverseMap();
			CreateMap<UpdateProposalDto, Proposal>().ReverseMap();

		}
    }
}
