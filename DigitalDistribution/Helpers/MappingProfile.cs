#region includes
using AutoMapper;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.BillingAddress;
using DigitalDistribution.Models.Database.Requests.DevelopmentTeam;
using DigitalDistribution.Models.Database.Requests.Invoice;
using DigitalDistribution.Models.Database.Requests.Product;
using DigitalDistribution.Models.Database.Requests.Profile;
using DigitalDistribution.Models.Database.Requests.Review;
using DigitalDistribution.Models.Database.Requests.Update;
using DigitalDistribution.Models.Database.Responses.BillingAddress;
using DigitalDistribution.Models.Database.Responses.Invoice;
using DigitalDistribution.Models.Database.Responses.Product;
using DigitalDistribution.Models.Database.Responses.Profile;
using DigitalDistribution.Models.Database.Responses.Review;
using DigitalDistribution.Models.Responses.Update;


#endregion
namespace DigitalDistribution.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateProfileRequest, ProfileEntity>();
            CreateMap<ProfileEntity, ProfileDetailsResponse>();

            CreateMap<UpdateBillingAddress, BillingAddressEntity>();
            CreateMap<BillingAddressEntity, BillingAddressResponse>();

            CreateMap<UpdateDevTeamRequest, DevelopmentTeamEntity>()
                .ForAllMembers(p => p.Condition((q, s, m) => m != null));

            CreateMap<UpdateInvoiceRequest, InvoiceEntity>()
                .ForAllMembers(p => p.Condition((q, s, m) => m != null));

            CreateMap<UpdateProductRequest, ProductEntity>()
                .ForAllMembers(p => p.Condition((q, s, m) => m != null));
            CreateMap<ProductEntity, ProductResponse>();

            CreateMap<InvoiceEntity, InvoiceResponse>();

            CreateMap<UpdateReviewRequest, ReviewEntity>()
                .ForAllMembers(p => p.Condition((q, s, m) => m != null));

            CreateMap<UpdateRequest, UpdateEntity >()
                .ForAllMembers(p => p.Condition((q, s, m) => m != null));
            CreateMap<UpdateEntity, UpdateResponse>();

            CreateMap<ReviewEntity, ReviewResponse>();
            CreateMap<ReviewEntity, ReviewResponseProduct>();
            CreateMap<ReviewEntity, ReviewResponseProfile>();

            
        }
    }
}
