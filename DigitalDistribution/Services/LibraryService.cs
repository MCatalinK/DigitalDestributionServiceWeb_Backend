using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class LibraryService
    {
        private readonly LibraryRepository _itemRepo;
        private readonly ProductRepository _productRepository;

        public LibraryService(LibraryRepository itemRepo,ProductRepository productRepository)
        {
            _itemRepo = itemRepo;
            _productRepository = productRepository;
        }
        public IQueryable<LibraryProductEntity> Get(Expression<Func<LibraryProductEntity, bool>> predicate = null)
        {
            return _itemRepo.Get(predicate);
        }

        public async Task<bool> AddItem(UserEntity user, InvoiceEntity invoice)
        {
            List<LibraryProductEntity> library = new();
            foreach (var obj in invoice.CheckoutItems)
            {
                var product = await _productRepository.Get(p => p.Id == obj.ProductId).FirstOrDefaultAsync();
                var libraryItem = new LibraryProductEntity
                {
                    DateAdded = DateTime.Now,
                    Licence = HelperExtensionMethods.CreateLicence(),
                    DownloadLink = HelperExtensionMethods.CreateDownloadLink(product.Name),

                    UserId = user.Id,
                    ProductId = obj.ProductId
                };
                library.Add(libraryItem);
            }

            var item = await _itemRepo.Create(library);
            return true;
        }
    }
}
