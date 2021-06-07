using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class LibraryService
    {
        private readonly LibraryRepository _itemRepo;

        public LibraryService(LibraryRepository itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task<bool> AddItem(UserEntity user, InvoiceEntity invoice)
        {
            var item = await _itemRepo.Create(user,invoice);
            return item;
        }
    }
}
