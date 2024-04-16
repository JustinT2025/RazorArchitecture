using Data;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorCrudUI.Pages.Items;
//using UI.Data;
//using UI.Models;

namespace UI.Pages
{
    public class MainDetailsModel : PageModel
    {
        private readonly IItemRepository _repository;
        public ItemModel? Item { get; set; }
        public MainDetailsModel(IItemRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {

            return NotFound();
            }
           var itemModel = await _repository.GetItemByIdAsync(id.Value);
            if (itemModel == null)
            {
                return NotFound();
            }
            else
            {
                Item = itemModel;
            }
            return Page();
        }
    }
}
