using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NuGet.Protocol.Core.Types;
using RazorCrudUI.Data;
//using UI.Data;
//using UI.Models;

namespace RazorCrudUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _repository;
        private object item;
        private readonly ILogger _logger;

        public IndexModel(IItemRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<ItemModel> Items { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? filter { get; set; }
        public IList<ItemModel> ItemList { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public async Task OnGetAsync()
        {
            ItemList = (IList<ItemModel>)await _repository.GetItemsAsync(SearchString);
        }
    }
}
