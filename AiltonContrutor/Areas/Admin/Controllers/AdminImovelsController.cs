using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AiltonConstrutor.Models;
using CasaFacilEPS.Context;

namespace CasaFacilEPS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminImovelsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminImovelsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminImovels
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Imoveis.Include(i => i.Categoria);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/AdminImovels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(m => m.IdImovel == id);
            if (imovel == null)
            {
                return NotFound();
            }

            return View(imovel);
        }

        // GET: Admin/AdminImovels/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome");
            return View();
        }

        // POST: Admin/AdminImovels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdImovel,Titulo,Descricao,Quartos,Banheiros,AreaGourmet,Churrasqueira,Piscina,Valor,ImagemUrl,ImagemThumbnailUrl,StatusImovel,MetragemImovel,Endereco,CategoriaId")] Imovel imovel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imovel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", imovel.CategoriaId);
            return View(imovel);
        }

        // GET: Admin/AdminImovels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", imovel.CategoriaId);
            return View(imovel);
        }

        // POST: Admin/AdminImovels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdImovel,Titulo,Descricao,Quartos,Banheiros,AreaGourmet,Churrasqueira,Piscina,Valor,ImagemUrl,ImagemThumbnailUrl,StatusImovel,MetragemImovel,Endereco,CategoriaId")] Imovel imovel)
        {
            if (id != imovel.IdImovel)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imovel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImovelExists(imovel.IdImovel))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", imovel.CategoriaId);
            return View(imovel);
        }

        // GET: Admin/AdminImovels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imovel = await _context.Imoveis
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(m => m.IdImovel == id);
            if (imovel == null)
            {
                return NotFound();
            }

            return View(imovel);
        }

        // POST: Admin/AdminImovels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imovel = await _context.Imoveis.FindAsync(id);
            if (imovel != null)
            {
                _context.Imoveis.Remove(imovel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImovelExists(int id)
        {
            return _context.Imoveis.Any(e => e.IdImovel == id);
        }
    }
}
