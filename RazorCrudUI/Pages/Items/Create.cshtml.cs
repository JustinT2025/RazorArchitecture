using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RazorCrudUI.Data;
using UI.Utilities;
//using UI.Data;
//using UI.Models;

namespace RazorCrudUI.Pages.Items
{
    public class CreateModel : PageModel
    {
        private readonly IItemRepository _repository;
        private readonly IWebHostEnvironment _wep;

        public CreateModel(IItemRepository repository, IWebHostEnvironment wep)
        {
            _repository = repository;
            _wep = wep;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ItemModel ItemModel { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //if(HttpContext.Request.Form.Files.Count > 0)
            //{
            //  ItemModel.pictureUrl = UploadNewImage(_wep,
            //HttpContext.Request.Form.Files[0]);
            //}
            if (!HttpContext.Request.Form.Files.IsNullOrEmpty())
            {
                ItemModel.pictureUrl = FileHelper.UploadNewImage(_wep,
                    HttpContext.Request.Form.Files[0]);
            }

            await _repository.AddItemAsync(ItemModel);
            

            return RedirectToPage("./Index");
        }
        public static string UploadNewImage(IWebHostEnvironment environment, IFormFile file)
        {
            // this creates a random unique id
            string guid = Guid.NewGuid().ToString();

            // we get what the extension of the file we chose was
            // it should proably be a jpeg, png, or gif
            // but we aren't doing any validation of the file type
            string ext = Path.GetExtension(file.FileName);

            // get the short path
            // which is the relative path to our image folder plus
            // "guid.ext"
            // so something like
            // "images\\Ietms\\08a783ba4567-4f3c-8e7d-4f3c-8e7d.jpg"
            string shortPath = Path.Combine("images\\Items", guid + ext);

            // we need the full path not just the relative path to save the file
            // that is what we get from the environment variable
            string path = Path.Combine(environment.WebRootPath, shortPath);

            // copy the file to our images folder
            // with the "guid.ext" file name
            using (var fs = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            return shortPath;
        }
    }
}

