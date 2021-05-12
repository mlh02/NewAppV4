using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStoreV2.Models;

namespace OnlineStoreV2.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IHostingEnvironment _environment;

        public ProductController(DataBaseContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _environment = hostingEnvironment;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,UserId")] Product product)
        {
            string fileName = string.Empty;
            string path = string.Empty;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var extension = Path.GetExtension(files[0].FileName);
                fileName = Guid.NewGuid().ToString() + extension;
                path = Path.Combine(_environment.WebRootPath, "ProductImages") + "/" + fileName;
                using (FileStream fs = System.IO.File.Create(path))
                {
                    files[0].CopyTo(fs);
                    fs.Flush();
                }
            }
            product.Image = fileName;
            product.UserId = Int32.Parse(User.FindFirst("Id").Value);
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,UserId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            string fileName = string.Empty;
            string path = string.Empty;

            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var extension = Path.GetExtension(files[0].FileName);
                fileName = Guid.NewGuid().ToString() + extension;

                path = Path.Combine(_environment.WebRootPath, "ProductImages") + "/" + fileName;

                using (FileStream fs = System.IO.File.Create(path))
                {
                    files[0].CopyTo(fs);
                    fs.Flush();
                }
                product.Image = fileName;
            }
            product.UserId = Int32.Parse(User.FindFirst("Id").Value);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
