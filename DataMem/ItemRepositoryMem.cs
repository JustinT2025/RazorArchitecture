using Data;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UI.Data;
//using UI.Models;

namespace DataMem
{
    public class ItemRepositoryMem : IItemRepository
    {
        IList<ItemModel> _list;

        // constructor
        // init our list with our starting items
        public ItemRepositoryMem()
        {
            _list = new List<ItemModel>
        {
            new ItemModel { Id = 1, Name = "Item 1",
            Description = "Description 1", Price = 1.99m },

            new ItemModel { Id = 2, Name = "Item 2",
            Description = "Description 2", Price = 2.99m },

            new ItemModel { Id = 3, Name = "Item 3",
            Description = "Description 3", Price = 3.99m },

            new ItemModel { Id = 4, Name = "Item 4",
            Description = "Description 4", Price = 4.99m },

            new ItemModel { Id = 5, Name = "Item 5",
            Description = "Description 5", Price = 5.99m }
        };
        }

        // we need to use the async methods since those are what
        // we are replacing from our Entity Framework repository
        // but we aren't actually doing anything async
        // so we need to manually wrap our return results in a Task
        // this is done for you automatically when you use async/await
        // to do it manually you wrap your return result
        // with Task.FromResult(RETURN VALUE HERE)
        public Task<IEnumerable<ItemModel>> GetItemsAsync()
        {
            return Task.FromResult(_list.AsEnumerable());
        }

        public Task<IEnumerable<ItemModel>> GetItemsAsync(string? searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return GetItemsAsync();
            return Task.FromResult(_list.Where(i => i.Name.Contains(searchString)));
        }

        // we just return a Task here instead of Task<SOME RETURN VALUE>
        // so we don't need to wrap a return result but it is still expecting
        // a return type of Task so return Task.CompletedTask
        public Task AddItemAsync(ItemModel item)
        {
            // ID PROBLEM
            // the database took care of creating new unique ids for us
            // that isn't happening anymore and we will need to do it ourselves
            // so you will have to generate and set a new id that will be
            // unique in the list before adding it
            item.Id = _list.Max(x => x.Id) + 1;

            _list.Add(item);
            return Task.CompletedTask;
        }

        public Task DeleteItemAsync(int id)
        {
            // find the item
            // return false if you can not
            var item = _list.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return Task.CompletedTask;
            // we found item so delete it            		    
            _list.Remove(item);
            return Task.CompletedTask;
        }

        public Task<ItemModel?> GetItemByIdAsync(int id)
        {
            return Task.FromResult(_list.FirstOrDefault(x => x.Id == id));
        }

        public Task<bool> UpdateItemAsync(ItemModel item)
        {
            // find the item
            // return false if you can not
            var existingItem = _list.FirstOrDefault(x => x.Id == item.Id);
            if (existingItem == null)
                return Task.FromResult(false);

            // we found existing item       
            // existingItem is a reference type
            // so making changes to it here WILL change it in the list as well
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.Price = item.Price;
            return Task.FromResult(true);
        }

    }
   
}
