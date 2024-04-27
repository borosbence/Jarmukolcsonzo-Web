using Jarmukolcsonzo.Web.Data;
using Jarmukolcsonzo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Jarmukolcsonzo.Web.Controllers
{
    public class RendelesekController : Controller
    {
        private readonly JKContext _context;

        public RendelesekController(JKContext context)
        {
            _context = context;
        }

        // GET: Rendelesek
        public async Task<IActionResult> Index()
        {
            var jKContext = _context.rendelesek.Include(r => r.jarmu).Include(r => r.ugyfel).OrderByDescending(x => x.datum);
            return View(await jKContext.ToListAsync());
        }

        // GET: Rendelesek/Reszletek/5
        public async Task<IActionResult> Reszletek(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendeles = await _context.rendelesek
                .Include(r => r.jarmu)
                .Include(r => r.ugyfel)
                .FirstOrDefaultAsync(m => m.id == id);
            if (rendeles == null)
            {
                return NotFound();
            }

            return View(rendeles);
        }

        // GET: Rendelesek/Letrehozas
        public IActionResult Letrehozas()
        {
            var jarmuvek = _context.jarmuvek.Where(x => x.elerheto).OrderBy(x => x.rendszam);
            var ugyfelek = _context.ugyfelek.OrderBy(x => x.vezeteknev).ThenBy(x => x.keresztnev);
            ViewData["jarmu_id"] = new SelectList(jarmuvek, "id", "rendszam");
            ViewData["ugyfel_id"] = new SelectList(ugyfelek, "id", "TeljesNev");
            return View();
        }

        // POST: Rendelesek/Letrehozas
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Letrehozas([Bind("id,ugyfel_id,jarmu_id,datum,napok_szama,ar")] Rendeles rendeles)
        {
            var jarmu = await _context.jarmuvek.FindAsync(rendeles.jarmu_id);
            if (ModelState.IsValid && jarmu != null)
            {
                rendeles.ar = jarmu.dij * rendeles.napok_szama;

                _context.Add(rendeles);
                // Jármű már nem elérhető
                jarmu.elerheto = false;
                _context.Update(jarmu);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var jarmuvek = _context.jarmuvek.Where(x => x.elerheto).OrderBy(x => x.rendszam);
            var ugyfelek = _context.ugyfelek.OrderBy(x => x.vezeteknev).ThenBy(x => x.keresztnev);
            ViewData["jarmu_id"] = new SelectList(jarmuvek, "id", "rendszam");
            ViewData["ugyfel_id"] = new SelectList(ugyfelek, "id", "TeljesNev");
            return View(rendeles);
        }

        // GET: Rendelesek/Szerkesztes/5
        public async Task<IActionResult> Szerkesztes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendeles = await _context.rendelesek.FindAsync(id);
            if (rendeles == null)
            {
                return NotFound();
            }
            var jarmuvek = _context.jarmuvek.OrderBy(x => x.rendszam);
            var ugyfelek = _context.ugyfelek.OrderBy(x => x.vezeteknev).ThenBy(x => x.keresztnev);
            ViewData["jarmu_id"] = new SelectList(jarmuvek, "id", "rendszam");
            ViewData["ugyfel_id"] = new SelectList(ugyfelek, "id", "TeljesNev");
            return View(rendeles);
        }

        // POST: Rendelesek/Szerkesztes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Szerkesztes(int id, [Bind("id,ugyfel_id,jarmu_id,datum,napok_szama,ar,Arkalkulacio")] Rendeles rendeles)
        {
            if (id != rendeles.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var regiRendeles = await _context.rendelesek.AsNoTracking().FirstAsync(x => x.id == rendeles.id);
                // régi jármű újra elérhető, az új nem elérhető
                if (regiRendeles.jarmu_id != rendeles.jarmu_id)
                {
                    var regiJarmu = await _context.jarmuvek.FindAsync(regiRendeles.jarmu_id);
                    var ujJarmu = await _context.jarmuvek.FindAsync(rendeles.jarmu_id);

                    regiJarmu.elerheto = true;
                    ujJarmu.elerheto = false;
                }
                // ár újra kiszámítása
                if (rendeles.Arkalkulacio)
                {
                    var jarmu = await _context.jarmuvek.FindAsync(rendeles.jarmu_id);
                    rendeles.ar = jarmu.dij * rendeles.napok_szama;
                }
                try
                {
                    _context.Update(rendeles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RendelesExists(rendeles.id))
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
            var jarmuvek = _context.jarmuvek.OrderBy(x => x.rendszam);
            var ugyfelek = _context.ugyfelek.OrderBy(x => x.vezeteknev).ThenBy(x => x.keresztnev);
            ViewData["jarmu_id"] = new SelectList(jarmuvek, "id", "rendszam");
            ViewData["ugyfel_id"] = new SelectList(ugyfelek, "id", "TeljesNev");
            return View(rendeles);
        }

        // GET: Rendelesek/Torles/5
        public async Task<IActionResult> Torles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendeles = await _context.rendelesek
                .Include(r => r.jarmu)
                .Include(r => r.ugyfel)
                .FirstOrDefaultAsync(m => m.id == id);
            if (rendeles == null)
            {
                return NotFound();
            }

            return View(rendeles);
        }

        // POST: Rendelesek/Torles/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Torles(int id)
        {
            var rendeles = await _context.rendelesek.FindAsync(id);
            var jarmu = await _context.jarmuvek.FindAsync(rendeles?.jarmu_id);
            
            if (rendeles != null && jarmu != null)
            {
                _context.rendelesek.Remove(rendeles);

                jarmu.elerheto = true;
                _context.Update(jarmu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RendelesExists(int id)
        {
            return _context.rendelesek.Any(e => e.id == id);
        }
    }
}
