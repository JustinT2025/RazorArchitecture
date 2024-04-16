using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorCrudUI.Data;
//using UI.Data;
//using UI.Models;

namespace RazorCrudUI.Pages.Items
{
    public class DeleteModel : PageModel
    {
        private readonly IItemRepository _repository;

        public DeleteModel(IItemRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public ItemModel ItemModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemmodel = await _repository.GetItemByIdAsync(id.Value);

            if (itemmodel == null)
            {
                return NotFound();
            }
            else
            {
                ItemModel = itemmodel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemmodel = await _repository.GetItemByIdAsync(id.Value);
            if (itemmodel != null)
            {
                ItemModel = itemmodel;
                await _repository.DeleteItemAsync(id.Value);
                await _repository.UpdateItemAsync(itemmodel);

            }
            return RedirectToPage("./Index");
        }
    }
}
