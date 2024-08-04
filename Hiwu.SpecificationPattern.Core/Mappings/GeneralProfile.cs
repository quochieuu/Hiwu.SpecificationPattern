using AutoMapper;
using Hiwu.SpecificationPattern.Core.DataTransferObjects.Product;
using Hiwu.SpecificationPattern.Core.Entities;

namespace Hiwu.SpecificationPattern.Core.Mappings
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
