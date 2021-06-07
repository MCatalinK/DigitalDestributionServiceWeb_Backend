using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<CheckoutItemEntity> AddItem(InvoiceEntity invoice,ProductEntity product)
        {
            var item = await _itemRepo.Create(invoice, product);
            return item;
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
