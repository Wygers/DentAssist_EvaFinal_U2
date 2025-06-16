using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentAssist.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DentAssist.Controllers
{
    public class PlanTratamientoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanTratamientoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlanTratamientoes
        public async Task<IActionResult> Index()
        {
            var planes = _context.PlanesTratamiento
                .Include(p => p.Paciente)
                .Include(p => p.Odontologo)
                .Include(p => p.Pasos)
                .ThenInclude(p => p.Tratamiento);
            return View(await planes.ToListAsync());
        }

        // GET: PlanTratamientoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var plan = await _context.PlanesTratamiento
                .Include(p => p.Paciente)
                .Include(p => p.Odontologo)
                .Include(p => p.Pasos)
                .ThenInclude(p => p.Tratamiento)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null) return NotFound();

            return View(plan);
        }

        // GET: PlanTratamientoes/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "NombreCompleto");
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Especialidad");
            ViewBag.Tratamientos = _context.Tratamientos
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Nombre })
                .ToList();
            return View();
        }

        // POST: PlanTratamientoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlanTratamiento planTratamiento)
        {
            if (ModelState.IsValid)
            {
                // Carga relaciones (opcional si no vienen desde el formulario)
                foreach (var paso in planTratamiento.Pasos)
                {
                    _context.Entry(paso).State = EntityState.Added;
                }

                _context.Add(planTratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "NombreCompleto", planTratamiento.PacienteId);
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Especialidad", planTratamiento.OdontologoId);
            ViewBag.Tratamientos = _context.Tratamientos
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Nombre })
                .ToList();
            return View(planTratamiento);
        }

        // GET: PlanTratamientoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var plan = await _context.PlanesTratamiento
                .Include(p => p.Pasos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null) return NotFound();

            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "NombreCompleto", plan.PacienteId);
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Especialidad", plan.OdontologoId);
            ViewBag.Tratamientos = _context.Tratamientos
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Nombre })
                .ToList();
            return View(plan);
        }

        // POST: PlanTratamientoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlanTratamiento planTratamiento)
        {
            if (id != planTratamiento.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Elimina pasos existentes y reemplaza por los nuevos
                    var pasosExistentes = _context.PasosTratamiento.Where(p => p.PlanTratamientoId == id);
                    _context.PasosTratamiento.RemoveRange(pasosExistentes);
                    await _context.SaveChangesAsync();

                    foreach (var paso in planTratamiento.Pasos)
                    {
                        paso.PlanTratamientoId = id;
                        _context.PasosTratamiento.Add(paso);
                    }

                    _context.Update(planTratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanTratamientoExists(planTratamiento.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "NombreCompleto", planTratamiento.PacienteId);
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Especialidad", planTratamiento.OdontologoId);
            ViewBag.Tratamientos = _context.Tratamientos
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Nombre })
                .ToList();
            return View(planTratamiento);
        }

        // GET: PlanTratamientoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var plan = await _context.PlanesTratamiento
                .Include(p => p.Paciente)
                .Include(p => p.Odontologo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null) return NotFound();

            return View(plan);
        }

        // POST: PlanTratamientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.PlanesTratamiento
                .Include(p => p.Pasos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan != null)
            {
                _context.PasosTratamiento.RemoveRange(plan.Pasos);
                _context.PlanesTratamiento.Remove(plan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlanTratamientoExists(int id)
        {
            return _context.PlanesTratamiento.Any(p => p.Id == id);
        }
    }
}
