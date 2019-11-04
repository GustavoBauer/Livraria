using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Livraria.Data;
using Microsoft.EntityFrameworkCore;
using Livraria.Models;

namespace Livraria.Controllers
{
    public class EditoraController : Controller
    {
        private LivrariaContext contexto;

        public EditoraController(LivrariaContext context)
        {
            this.contexto = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await contexto.Editora.OrderBy(e => e.Nome).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Site")] Editora editora)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contexto.Add(editora);
                    await contexto.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(editora);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var editora = await contexto.Editora.SingleOrDefaultAsync(e => e.EditoraId == id);
            if (editora == null)
            {
                return NotFound();
            }
            return View(editora);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("EditoraId,Nome,Site")] Editora editora)
        {
            if (id != editora.EditoraId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    contexto.Update(editora);
                    await contexto.SaveChangesAsync();                   
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Não foi possível alterar os dados.");
                }
                return RedirectToAction("Index");
            }
            return View(editora);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var editora = await contexto.Editora.SingleOrDefaultAsync(e => e.EditoraId == id);
            if (editora == null)
            {
                return NotFound();
            }
            return View(editora);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var editora = await contexto.Editora.SingleOrDefaultAsync(e => e.EditoraId == id);
            try
            {
                contexto.Remove(editora);
                await contexto.SaveChangesAsync();
                TempData["Nome"] = editora.Nome;
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível excluir os dados.");
            }
            return RedirectToAction("Index");
        }
    }
}