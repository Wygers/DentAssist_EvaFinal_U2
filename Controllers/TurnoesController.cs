using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DentAssist.Models;

namespace DentAssist.Controllers
{
    public class Turnoes1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Turnoes1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Turnoes
        public async Task<IActionResult> Index()
        {
            // Incluye Paciente y Odontologo para mostrar sus nombres/datos en la vista si es necesario
            var applicationDbContext = _context.Turnos.Include(t => t.Odontologo).Include(t => t.Paciente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Turnoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turnoes/Create
        public IActionResult Create()
        {
            // Puedes poblar una lista de estados aquí si quieres un DropDownList en lugar de un input de texto
            // Ejemplo: ViewData["Estados"] = new SelectList(new List<string> { "Pendiente", "Confirmado", "Cancelado", "Finalizado" });

            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Especialidad"); // Asumo que "Especialidad" es un buen texto a mostrar
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre"); // Asumo que "Nombre" es un buen texto a mostrar
            return View();
        }

        // POST: Turnoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaHora,DuracionMinutos,Estado,PacienteId,OdontologoId")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repobla ViewData si el modelo no es válido para que el usuario pueda corregir los errores
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Especialidad", turno.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", turno.PacienteId);
            // Si usaras una lista de estados, repóblala aquí también:
            // ViewData["Estados"] = new SelectList(new List<string> { "Pendiente", "Confirmado", "Cancelado", "Finalizado" }, turno.Estado);
            return View(turno);
        }

        // GET: Turnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            // Repobla ViewData para la edición
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Especialidad", turno.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", turno.PacienteId);
            // Si usaras una lista de estados, repóblala aquí también:
            // ViewData["Estados"] = new SelectList(new List<string> { "Pendiente", "Confirmado", "Cancelado", "Finalizado" }, turno.Estado);
            return View(turno);
        }

        // POST: Turnoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaHora,DuracionMinutos,Estado,PacienteId,OdontologoId")] Turno turno)
        {
            if (id != turno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.Id))
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
            // Repobla ViewData si el modelo no es válido
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Especialidad", turno.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", turno.PacienteId);
            // Si usaras una lista de estados, repóblala aquí también:
            // ViewData["Estados"] = new SelectList(new List<string> { "Pendiente", "Confirmado", "Cancelado", "Finalizado" }, turno.Estado);
            return View(turno);
        }

        // GET: Turnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Odontologo)
                .Include(t => t.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.Id == id);
        }
    }
}