using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RazorCrudUI.Data;
using UI.Utilities;
//using UI.Data;
//using UI.Models;

namespace RazorCrudUI.Pages.Items
{
    public class EditModel : PageModel
    {
        private readonly IItemRepository _repository;
        private readonly IWebHostEnvironment _wep;

        public EditModel(IItemRepository repository, IWebHostEnvironment wep)
        {
            _repository = repository;
            _wep = wep;
        }

        [BindProperty]
        public ItemModel ItemModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemmodel =  await _repository.GetItemByIdAsync(id.Value);
            if (itemmodel == null)
            {
                return NotFound();
            }
            ItemModel = itemmodel;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
    //        if(HttpContext.Request.Form.Count > 0)
      //      {
  //              FileHelper.DeleteOldImage(_wep, ItemModel);
//                ItemModel.pictureUrl = FileHelper.UploadNewImage(_wep, HttpContext.Request.Form.Files[0]);
            //}
            if (!HttpContext.Request.Form.Files.IsNullOrEmpty())
            {
                FileHelper.DeleteOldImage(_wep, ItemModel);
                ItemModel.pictureUrl = FileHelper.UploadNewImage(_wep,
                    HttpContext.Request.Form.Files[0]);
            }

            if (!await _repository.UpdateItemAsync(ItemModel))
            {
                return NotFound();
            }

            

            return RedirectToPage("./Index");
        }
    }
}
