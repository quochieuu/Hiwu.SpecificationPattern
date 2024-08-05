using AutoMapper;
using Hiwu.SpecificationPattern.Application.DataTransferObjects.Product;
using Hiwu.SpecificationPattern.Domain.Entities;

namespace Hiwu.SpecificationPattern.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            // Product
            CreateMap<CreateProductRequest, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
