using AutoMapper;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Models;


namespace WebAPI_GiftManagement_BaoTran.Mappers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserRequest, User>().ReverseMap();

            CreateMap<User, UserResponse>().ReverseMap();


            CreateMap<RoleRequest, Role>().ReverseMap();

            CreateMap<Role, RoleResponse>().ReverseMap();


            CreateMap<RolePermissionRequest, RolePermission>().ReverseMap();

            CreateMap<RolePermission, RolePermissionResponse>().ReverseMap();


            CreateMap<PermissionRequest, Permission>().ReverseMap();

            CreateMap<Permission, PermissionResponse>().ReverseMap();


            CreateMap<GiftRequest, Gift>().ReverseMap();

            CreateMap<Gift, GiftResponse>().ReverseMap();


            CreateMap<CategoryRequest, Category>().ReverseMap();

            CreateMap<Category, CategoryResponse>().ReverseMap();


            CreateMap<PromotionRequest, Promotion>().ReverseMap();

            CreateMap<Promotion, PromotionResponse>().ReverseMap();


            CreateMap<CartRequest, Cart>().ReverseMap();

            //CreateMap<Cart, CartResponse>().ReverseMap();

            CreateMap<RankingUser, RankingUserResponse>();


        }
    }
}
