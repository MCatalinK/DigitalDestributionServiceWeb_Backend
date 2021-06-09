using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Http;
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
        protected ClaimsPrincipal CurrentUser;
        public InvoiceService(InvoiceRepository invoiceRepository,
            IHttpContextAccessor contextAccessor) 
        {
            _invoiceRepository = invoiceRepository;
            CurrentUser = contextAccessor.HttpContext?.User;
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
            if (CurrentUser != null && CurrentUser.GetUserId() != 0)
                entity.CreatedBy = CurrentUser.GetUserId();

            return await _invoiceRepository.Create(entity, commit);
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
