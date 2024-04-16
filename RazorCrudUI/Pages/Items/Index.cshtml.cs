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
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _repository;
        private object item;

        public IndexModel(IItemRepository repository)
        {
            _repository = repository;
        }

        public IList<ItemModel> ItemModel { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public async Task OnGetAsync()
        {
            ItemModel = (IList<ItemModel>)await _repository.GetItemsAsync(SearchString);

            //if (!string.IsNullOrEmpty(SearchString))
            // {
            //ItemModel = (await _repository.GetItemsAsync(SearchString)).ToList();

            // }

            //ItemModel = (await _repository.GetItemsAsync(SearchString)).ToList();

        }
    }
}