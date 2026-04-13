using AutoMapper;
using Products.Infrastructure.Contracts.Entities;
using Products.Library.Contracts.DTO;

namespace Products.Library.Implementation.Mappers.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}
