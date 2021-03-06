﻿using System.Collections.Generic;
using AutoMapper;
using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;

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
            CreateMap<SubscriptionProduct, SubscriptionProductViewModel>();
            CreateMap<SubscriptionProductViewModel, SubscriptionProduct>();
            CreateMap<ApplicationUser, UserViewModel>().ForMember(dest => dest.Password, opts => opts.Ignore());
            CreateMap<UserViewModel, ApplicationUser>().ForMember(dest => dest.PasswordHash, opts => opts.Ignore());
        }
    }
}