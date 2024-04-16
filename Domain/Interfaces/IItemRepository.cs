using Domain.Models;

namespace Data
{
    public interface IItemRepository
    {
        Task<IEnumerable<ItemModel>> GetItemsAsync();
        Task<IEnumerable<ItemModel>> GetItemsAsync(string? filter);
        Task<ItemModel> GetItemByIdAsync(int id);
        Task AddItemAsync(ItemModel item);
        Task DeleteItemAsync(int id);
        Task<bool> UpdateItemAsync(ItemModel item);
       /* IEnumerable<ItemModel> GetItems();

        IEnumerable<ItemModel> GetItems(string filter);

        ItemModel GetItemById(int id);

        void InsertItem(ItemModel item);

        void DeleteItem(int id);

        bool UpdateItem(ItemModel item);
       */
    }
}
