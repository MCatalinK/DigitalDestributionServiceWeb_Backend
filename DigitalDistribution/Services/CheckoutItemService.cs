using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class CheckoutItemService
    {
        private readonly CheckoutItemRepository _itemRepo;

        public CheckoutItemService(CheckoutItemRepository itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task<CheckoutItemEntity> AddItem(int userId,CheckoutItemEntity item)
        {
            var result = await _itemRepo.Create(userId,item);
            return result;
        }
        public async Task<CheckoutItemEntity> Delete(CheckoutItemEntity item)
        {
            return await _itemRepo.Delete(item);
        }
        public async Task<List<CheckoutItemEntity>> GetAll()
        {
            return await _itemRepo.GetAll();
        }
    }
}
