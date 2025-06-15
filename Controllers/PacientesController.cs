using Microsoft.AspNetCore.Mvc;
using DentAssist.Models; // Ajusta según tu estructura
using System.Linq;

namespace DentAssist.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public IActionResult Index()
        {
            var pacientes = _context.Pacientes.OrderBy(p => p.Nombre).ToList();
            return View(pacientes);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Paciente paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Pacientes.Add(paciente);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Paciente registrado correctamente.";
                    return RedirectToAction(nameof(Index));
                }

                // Mostrar errores de validación en consola
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error de validación: {error.ErrorMessage}");
                }

                TempData["ErrorMessage"] = "Verifique los datos ingresados. Hay errores en el formulario.";
                return View(paciente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar paciente: {ex.Message}");
                TempData["ErrorMessage"] = "Ocurrió un error al guardar el paciente.";
                return View(paciente);
            }
        }

        // GET: Pacientes/Details/5
        public IActionResult Details(int id)
        {
            var paciente = _context.Pacientes.FirstOrDefault(p => p.Id == id);
            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public IActionResult Edit(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Paciente paciente)
        {
            if (id != paciente.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Paciente actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al editar paciente: {ex.Message}");
                    TempData["ErrorMessage"] = "Ocurrió un error al actualizar el paciente.";
                }
            }

            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public IActionResult Delete(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var paciente = _context.Pacientes.Find(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Paciente eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Paciente no encontrado.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
