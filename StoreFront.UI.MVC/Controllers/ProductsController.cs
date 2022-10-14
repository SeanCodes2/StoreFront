using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;
using StoreFront.UI.MVC.Utilities;
using X.PagedList;

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductsController : Controller
    {
        private readonly StoreFrontContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(StoreFrontContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? id, string searchTerm, int manufacturerId, int page = 1)
        {
            int pageSize = 8;

            //var storeFrontContext = _context.Products.Include(p => p.Category).Include(p => p.Manufacturer);
            var products = _context.Products.Where(p => !p.Discontinued)
               .Include(p => p.Category)
               .Include(p => p.Manufacturer)
               .Include(p => p.ProductOrders).ToList();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName");

            //product manufacturer checkbox filter


            //_layout categories dropdown
            if (id != null)
            {
                products = products.Where(p => p.Category.CategoryId == id).ToList();
            }
            else
            {
                id = null;
            }

            #region Optional Manufacturer Filter
            if (manufacturerId != 0)
            {
                products = products.Where(p => p.Manufacturer.ManufacturerId == manufacturerId).ToList();
                // Recreate the dropdown list so the current category is still selected
                ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", manufacturerId);

            }
            #endregion


            #region Optional Search Filter
            if (!String.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                                           p.ProductName.ToLower().Contains(searchTerm.ToLower())
                                           || p.Manufacturer.ManufacturerName.ToLower().Contains(searchTerm.ToLower())
                                           || p.ProductDescription.ToLower().Contains(searchTerm.ToLower())
                                           || p.Category.CategoryName.ToLower().Contains(searchTerm.ToLower())).ToList();

                ViewBag.SearchTerm = searchTerm;
                ViewBag.NbrResults = products.Count;
            }
            else
            {
                ViewBag.SearchTerm = null;
                ViewBag.NbrResults = null;
            }
            #endregion


            return View(products.ToPagedList(page, pageSize));
        }

        // GET: TableView
        [AllowAnonymous]
        public async Task<IActionResult> TableView()
        {
            //var storeFrontContext = _context.Products.Include(p => p.Category).Include(p => p.Manufacturer);
            var products = _context.Products.Where(p => !p.Discontinued)
               .Include(p => p.Category)
               .Include(p => p.Manufacturer)
               .Include(p => p.ProductOrders);

            return View(await products.ToListAsync());
        }
        [AllowAnonymous]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId,ManufacturerId,ProductName,ProductDescription,ProductPrice,UnitsInStock,UnitsOnOrder,Discontinued,ProductImage,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                #region FILE UPLOAD - CREATE

                if (product.Image != null)
                {
                    string ext = Path.GetExtension(product.Image.FileName);
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        product.ProductImage = Guid.NewGuid() + ext;
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullImagePath = webRootPath + "/img/";
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);

                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }
                else
                {
                    product.ProductImage = "noimage.png";
                }


                #endregion



                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", product.ManufacturerId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", product.ManufacturerId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CategoryId,ManufacturerId,ProductName,ProductDescription,ProductPrice,UnitsInStock,UnitsOnOrder,Discontinued,ProductImage,Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                #region FILE UPLOAD - EDIT
                                
                string oldFileName = product.ProductImage;
                                
                if (product.Image != null)
                {                    
                    string ext = Path.GetExtension(product.Image.FileName);                    
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };
                   
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {                        
                        product.ProductImage = Guid.NewGuid() + ext;                        
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullPath = webRootPath + "/img/";
                        
                        if (oldFileName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldFileName);
                        }
                      
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);
                            using (Image img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;
                                ImageUtility.ResizeImage(fullPath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }
                #endregion

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", product.ManufacturerId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'StoreFrontContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
