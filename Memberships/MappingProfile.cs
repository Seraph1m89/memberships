using System.Collections.Generic;
using AutoMapper;
using Memberships.Areas.Admin.Models;
using Memberships.Entities;

namespace Memberships
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();
            CreateMap<ProductItem, ProductItemViewModel>();
            CreateMap<ProductItemViewModel, ProductItem>();
        }
    }
}