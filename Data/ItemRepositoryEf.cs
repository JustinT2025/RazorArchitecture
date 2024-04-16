using Microsoft.EntityFrameworkCore;
using System;
//using Domain.Data;
//using UI.Models;
using RazorCrudUI.Models;
using Domain.Models;
using Data;

namespace RazorCrudUI.Data
{
    public class ItemRepositoryEf : IItemRepository
    {
        // our database context class
        private ItemContext _context;

        // we pass our database context through the constructor
        // just like we did to the individual Razor page view-models
        public ItemRepositoryEf(ItemContext context)
        {
            _context = context;
        }

        // get all items
        public IEnumerable<ItemModel> GetItems()
        {
            return _context.Items.ToList();
        }
        public IEnumerable<ItemModel> GetItems(String item)
        {
            return _context.Items.Where(s => s.Name.Contains(item)).ToList();
            
        }


        // we could also write this using the arrow operator instead
        //public IEnumerable<ItemModel> GetItems() => _context.Items.ToList();

        // we are doing the DbUpdateConcurrencyException check here
        // and returning a bool to check in the Razor Page view-models
        // instead of catching the exception in the view-models since
        // the repository is generic and might not even reference a
        // database to throw that particular exception
        public ItemModel GetItemById(int id)
        {
            return _context.Items.First(c => c.Id == id);
        }
        public void InsertItem(ItemModel item)
        {
            _context.Items.Add(item);
            //await _context.SaveChangesAsync();
        }
       public void DeleteItem(int id)
        {
            var item = _context.Items.FirstOrDefault(c => c.Id == id);
            _context.Items.Remove(item);
          //  _context.SaveChangesAsync();
        }
        public bool UpdateItem(ItemModel item)
        {
            _context.Attach(item).State = EntityState.Modified;
            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        // I will leave the rest for you to implement...
        public async Task<IEnumerable<ItemModel>> GetItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }
        public async Task<IEnumerable<ItemModel>> GetItemsAsync(string? filter)
        {
            {
                if (string.IsNullOrEmpty(filter))
                    return await GetItemsAsync();
                return await _context.Items.Where(s => s.Name.Contains(filter)).ToListAsync();

            }
        }
        public async Task<ItemModel> GetItemByIdAsync(int id)
        {
            return await _context.Items.FirstAsync(c => c.Id == id);
        }
        public async Task AddItemAsync(ItemModel item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteItemAsync(int id)
        {
            var item =await _context.Items.FirstOrDefaultAsync(c => c.Id == id);
             _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateItemAsync(ItemModel item)
        {
            _context.Attach(item).State = EntityState.Modified;
            try
            {
              await  _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
            await _context.SaveChangesAsync();
        }
    }
}
