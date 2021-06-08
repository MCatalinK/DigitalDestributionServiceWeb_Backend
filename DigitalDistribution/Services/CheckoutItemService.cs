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

        public async Task<CheckoutItemEntity> AddItem(CheckoutItemEntity item)
        {
            var result = await _itemRepo.Create(item);
            return result;
        }
        public async Task<bool> Delete(InvoiceEntity invoice, ProductEntity product)
        {
            return await _itemRepo.Delete(invoice, product);
        }
        public async Task<List<CheckoutItemEntity>> GetAll()
        {
            return await _itemRepo.GetAll();
        }
    }
}
