using AutoMapper;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;

namespace StoreMarket.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResponse>(MemberList.Destination).ReverseMap();
            CreateMap<Product, ProductDeleteRequest>(MemberList.Destination).ReverseMap();
            CreateMap<Product, ProductUpdateRequest>(MemberList.Destination).ReverseMap();
            CreateMap<Product, ProductCreateRequest>(MemberList.Destination).ReverseMap();
            
        }
    }
}
