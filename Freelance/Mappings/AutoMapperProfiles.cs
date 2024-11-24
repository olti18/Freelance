using AutoMapper;
using Freelance.Models.Domain;
using Freelance.Models.DTO.ProjectPostDto;
using Freelance.Models.DTO.ProjectPostDTO;
using Freelance.Models.DTO.ProposalDTO;
using Freelance.Models.DTO.RatingDTO;

namespace Freelance.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            //Mapping ProjectPost Model Reverse
            CreateMap<AddProjectPostDto, ProjectPost>().ReverseMap();
            CreateMap<ProjectPost, ProjectPostDto>().ReverseMap();
            CreateMap<UpdateProjectPostRequestDto, ProjectPost>().ReverseMap();
			//Mapping Proposal Model Reverse
			CreateMap<Proposal, ProposalDto>().ReverseMap();
			CreateMap<AddProposalDto, Proposal>().ReverseMap();
			CreateMap<UpdateProposalDto, Proposal>().ReverseMap();
			//Mapping Ratings Model Reverse
			CreateMap< Rating, AddRatingDto>().ReverseMap();

		}
    }
}
