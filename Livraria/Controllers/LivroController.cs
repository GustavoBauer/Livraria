using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Livraria.Data;
using Livraria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Controllers
{
    public class LivroController : Controller
    {
        private LivrariaContext contexto;

        public LivroController(LivrariaContext context)
        {
            this.contexto = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await contexto.Livro.OrderBy(l => l.Titulo).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Preco,Disponivel")] Livro livro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Add(livro);
                    await contexto.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(livro);
        }

        public async Task<IActionResult> Edit (long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var livro = await contexto.Livro.SingleOrDefaultAsync(l => l.LivroId == id);
            if(livro == null)
            {
                return NotFound();
            }
            return View(livro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("LivroId,Titulo,Preco,Disponivel")] Livro livro)
        {
            if(id != livro.LivroId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    contexto.Update(livro);
                    await contexto.SaveChangesAsync();
                } catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Não foi possível alterar os dados.");
                }
                return RedirectToAction("Index");
            }
            return View(livro);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var livro = await contexto.Livro.SingleOrDefaultAsync(l => l.LivroId == id);
            if(livro == null)
            {
                return NotFound();
            }
            return View(livro);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var livro = await contexto.Livro.SingleOrDefaultAsync(l => l.LivroId == id);
            try
            {
                contexto.Remove(livro);
                await contexto.SaveChangesAsync();
                TempData["Titulo"] = livro.Titulo;
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível excluir os dados.");
            }
            return RedirectToAction("Index");
        }
    }
}