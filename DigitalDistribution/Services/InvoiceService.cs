using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _invoiceRepository;
        private readonly CheckoutItemRepository _checkoutItemRepository;
        private readonly ProductRepository _productRepository;
        protected ClaimsPrincipal CurrentUser;
        public InvoiceService(InvoiceRepository invoiceRepository,
            CheckoutItemRepository checkoutItemRepository,
            ProductRepository productRepository,
            IHttpContextAccessor contextAccessor) 
        {
            _invoiceRepository = invoiceRepository;
            CurrentUser = contextAccessor.HttpContext?.User;
            _checkoutItemRepository = checkoutItemRepository;
            _productRepository = productRepository;
        }
        public IQueryable<InvoiceEntity> Get(Expression<Func<InvoiceEntity, bool>> predicate = null)
        {
            return _invoiceRepository.Get(predicate);
        }

        public async Task Commit()
        {
            await _invoiceRepository.Commit();
        }

        public async Task<InvoiceEntity> Create(InvoiceEntity entity, bool commit = true)
        {
            return await _invoiceRepository.Create(entity, commit);
        }
        public async Task<CheckoutItemEntity> AddItem(CheckoutItemEntity item)
        {
            var invoice = await _invoiceRepository.Get(p => p.Id == item.InvoiceId)
                .FirstOrDefaultAsync();
            var product = await _productRepository.Get(p => p.Id == item.ProductId)
                .FirstOrDefaultAsync();

            invoice.Price += product.Price;
            _ = await _invoiceRepository.Update(invoice);
            return await _checkoutItemRepository.Create(item);

        }
        public async Task<CheckoutItemEntity> RemoveItem(CheckoutItemEntity item)
        {
            var invoice = await _invoiceRepository.Get(p => p.Id == item.InvoiceId)
                .FirstOrDefaultAsync();
            var product = await _productRepository.Get(p => p.Id == item.ProductId)
                .FirstOrDefaultAsync();

            invoice.Price += product.Price;
            _ = await _invoiceRepository.Update(invoice);
            return await _checkoutItemRepository.Delete(item);
        }

        public async Task<InvoiceEntity> Update(InvoiceEntity entity, bool commit = true)
        {
            if (CurrentUser != null && CurrentUser.GetUserId() != 0)
            {
                entity.DateModified = DateTime.Now;
            }

            return await _invoiceRepository.Update(entity, commit);
        }

        public async Task<InvoiceEntity> Delete(InvoiceEntity entity, bool commit = true)
        {
            return await _invoiceRepository.Delete(entity, commit);
        }
    }
}
