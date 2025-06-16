using DentAssist.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DentAssist.Controllers
{
    public class Turnoes1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Turnoes1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método privado para cargar los SelectList de Odontologos, Pacientes y Estados
        private void CargarListas(Turno turno = null)
        {
            ViewData["OdontologoId"] = new SelectList(_context.Odontologos, "Id", "Nombre", turno?.OdontologoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", turno?.PacienteId);

            var estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Pendiente", Text = "Pendiente" },
                new SelectListItem { Value = "Confirmado", Text = "Confirmado" },
                new SelectListItem { Value = "Realizado", Text = "Realizado" },
                new SelectListItem { Value = "Cancelado", Text = "Cancelado" }
            };
            ViewData["Estado"] = new SelectList(estados, "Value", "Text", turno?.Estado);
        }

        // GET: Turnoes
        public async Task<IActionResult> Index()
        {
            var turnos = await _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .ToListAsync();

            return View(turnos);
        }

        // GET: Turnoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var turno = await _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (turno == null) return NotFound();

            return View(turno);
        }

        // GET: Turnoes/Create
        public IActionResult Create()
        {
            CargarListas();
            return View();
        }

        // POST: Turnoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaHora,DuracionMinutos,Estado,PacienteId,OdontologoId")] Turno turno)
        {
            if (!ModelState.IsValid)
            {
                CargarListas(turno);
                return View(turno);
            }

            // Validar que el estado sea uno válido
            var estadosValidos = new[] { "Pendiente", "Confirmado", "Realizado", "Cancelado" };
            if (!estadosValidos.Contains(turno.Estado))
            {
                ModelState.AddModelError("Estado", "Estado inválido.");
                CargarListas(turno);
                return View(turno);
            }

            var finTurnoNuevo = turno.FechaHora.AddMinutes(turno.DuracionMinutos);

            bool conflictoOdontologo = _context.Turnos.Any(t =>
                t.OdontologoId == turno.OdontologoId &&
                t.FechaHora < finTurnoNuevo &&
                t.FechaHora.AddMinutes(t.DuracionMinutos) > turno.FechaHora);

            bool conflictoPaciente = _context.Turnos.Any(t =>
                t.PacienteId == turno.PacienteId &&
                t.FechaHora < finTurnoNuevo &&
                t.FechaHora.AddMinutes(t.DuracionMinutos) > turno.FechaHora);

            if (conflictoOdontologo)
                ModelState.AddModelError("", "El odontólogo ya tiene un turno en ese horario.");

            if (conflictoPaciente)
                ModelState.AddModelError("", "El paciente ya tiene un turno en ese horario.");

            if (!ModelState.IsValid)
            {
                CargarListas(turno);
                return View(turno);
            }

            _context.Add(turno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Turnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();

            CargarListas(turno);
            return View(turno);
        }

        // POST: Turnoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaHora,DuracionMinutos,Estado,PacienteId,OdontologoId")] Turno turno)
        {
            if (id != turno.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                CargarListas(turno);
                return View(turno);
            }

            // Validar que el estado sea uno válido
            var estadosValidos = new[] { "Pendiente", "Confirmado", "Realizado", "Cancelado" };
            if (!estadosValidos.Contains(turno.Estado))
            {
                ModelState.AddModelError("Estado", "Estado inválido.");
                CargarListas(turno);
                return View(turno);
            }

            try
            {
                _context.Update(turno);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Turnos.Any(e => e.Id == turno.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Turnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var turno = await _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (turno == null) return NotFound();

            return View(turno);
        }

        // POST: Turnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
                _context.Turnos.Remove(turno);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
