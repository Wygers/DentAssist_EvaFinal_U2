using Microsoft.AspNetCore.Mvc;
using DentAssist.Models;
using System;
using System.Linq;

namespace DentAssist.Controllers
{
    public class TratamientoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TratamientoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tratamientos
        public IActionResult Index()
        {
            var tratamientos = _context.Tratamientos.OrderBy(t => t.Nombre).ToList();
            return View(tratamientos);
        }

        // GET: Tratamientos/Details/5
        public IActionResult Details(int id)
        {
            var tratamiento = _context.Tratamientos.FirstOrDefault(t => t.Id == id);
            if (tratamiento == null) return NotFound();

            return View(tratamiento);
        }

        // GET: Tratamientos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tratamientos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tratamiento tratamiento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Tratamientos.Add(tratamiento);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Tratamiento registrado correctamente.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    Console.WriteLine($"Error de validación: {error.ErrorMessage}");

                TempData["ErrorMessage"] = "Verifique los datos ingresados. Hay errores en el formulario.";
                return View(tratamiento);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar tratamiento: {ex.Message}");
                TempData["ErrorMessage"] = "Ocurrió un error al guardar el tratamiento.";
                return View(tratamiento);
            }
        }

        // GET: Tratamientos/Edit/5
        public IActionResult Edit(int id)
        {
            var tratamiento = _context.Tratamientos.Find(id);
            if (tratamiento == null) return NotFound();

            return View(tratamiento);
        }

        // POST: Tratamientos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Tratamiento tratamiento)
        {
            if (id != tratamiento.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tratamiento);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Tratamiento actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al editar tratamiento: {ex.Message}");
                    TempData["ErrorMessage"] = "Ocurrió un error al actualizar el tratamiento.";
                }
            }

            return View(tratamiento);
        }

        // GET: Tratamientos/Delete/5
        public IActionResult Delete(int id)
        {
            var tratamiento = _context.Tratamientos.Find(id);
            if (tratamiento == null) return NotFound();

            return View(tratamiento);
        }

        // POST: Tratamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tratamiento = _context.Tratamientos.Find(id);
            if (tratamiento != null)
            {
                _context.Tratamientos.Remove(tratamiento);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Tratamiento eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Tratamiento no encontrado.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
