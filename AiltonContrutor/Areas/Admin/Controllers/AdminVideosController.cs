using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasaFacilEPS.Context;
using AiltonConstrutor.Models;

namespace CasaFacilEPS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminVideosController : Controller
    {
        private readonly AppDbContext _context;

        public AdminVideosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminVideos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Videos.Include(v => v.Imovel);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/AdminVideos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.Imovel)
                .FirstOrDefaultAsync(m => m.IdVideo == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Admin/AdminVideos/Create
        public IActionResult Create()
        {
            ViewData["IdImovel"] = new SelectList(_context.Imoveis, "IdImovel", "Titulo");
            return View();
        }

        // POST: Admin/AdminVideos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVideo,IdImovel,Url")] Video video)
        {
            if (!string.IsNullOrEmpty(video.Url))
            {
                if (video.Url.Contains("https://youtube.com/shorts/"))
                {
                    video.Url = video.Url.Replace("https://youtube.com/shorts/", "https://www.youtube.com/embed/");
                }
                else if (video.Url.Contains("https://youtu.be/"))
                {
                    video.Url = video.Url.Replace("https://youtu.be/", "https://www.youtube.com/embed/");
                }
                else if (video.Url.Contains("https://www.youtube.com/watch?v="))
                {
                    video.Url = video.Url.Replace("https://www.youtube.com/watch?v=", "https://www.youtube.com/embed/");
                }

                // Se houver parâmetros extras, mantém apenas o ID do vídeo
                int index = video.Url.IndexOf("&");
                if (index != -1)
                {
                    video.Url = video.Url.Substring(0, index);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdImovel"] = new SelectList(_context.Imoveis, "IdImovel", "Descricao", video.IdImovel);
            return View(video);
        }

        // GET: Admin/AdminVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            ViewData["IdImovel"] = new SelectList(_context.Imoveis, "IdImovel", "Descricao", video.IdImovel);
            return View(video);
        }

        // POST: Admin/AdminVideos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVideo,IdImovel,Url")] Video video)
        {
            if (id != video.IdVideo)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(video.Url))
            {
                if (video.Url.Contains("https://youtube.com/shorts/"))
                {
                    video.Url = video.Url.Replace("https://youtube.com/shorts/", "https://www.youtube.com/embed/");
                }
                else if (video.Url.Contains("https://youtu.be/"))
                {
                    video.Url = video.Url.Replace("https://youtu.be/", "https://www.youtube.com/embed/");
                }
                else if (video.Url.Contains("https://www.youtube.com/watch?v="))
                {
                    video.Url = video.Url.Replace("https://www.youtube.com/watch?v=", "https://www.youtube.com/embed/");
                }

                // Se houver parâmetros extras, mantém apenas o ID do vídeo
                int index = video.Url.IndexOf("&");
                if (index != -1)
                {
                    video.Url = video.Url.Substring(0, index);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.IdVideo))
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
            ViewData["IdImovel"] = new SelectList(_context.Imoveis, "IdImovel", "Descricao", video.IdImovel);
            return View(video);
        }

        // GET: Admin/AdminVideos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.Imovel)
                .FirstOrDefaultAsync(m => m.IdVideo == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Admin/AdminVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video != null)
            {
                _context.Videos.Remove(video);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoExists(int id)
        {
            return _context.Videos.Any(e => e.IdVideo == id);
        }
    }
}
