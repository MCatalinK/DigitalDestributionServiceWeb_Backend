#region includes
using AutoMapper;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.BillingAddress;
using DigitalDistribution.Models.Database.Requests.DevelopmentTeam;
using DigitalDistribution.Models.Database.Requests.Profile;
using DigitalDistribution.Models.Database.Responses.BillingAddress;
using DigitalDistribution.Models.Database.Responses.DevelopmentTeam;
using DigitalDistribution.Models.Database.Responses.Product;
using DigitalDistribution.Models.Database.Responses.Profile;


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
            CreateMap<DevelopmentTeamEntity, DevelopmentTeamResponse>();

            CreateMap<ProductEntity, ProductResponse>();
        }
    }
}
