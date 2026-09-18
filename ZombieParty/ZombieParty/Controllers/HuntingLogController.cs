using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZombieParty.Models;
using ZombieParty.Models.Data;

namespace ZombieParty.Controllers
{
    public class HuntingLogController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public HuntingLogController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }

        // GET: HuntingLogController
        public ActionResult Index()
        {
            List<HuntingLog> huntingLogsList = _baseDonnees.HuntingLogs.OrderBy(h => h.Title).ToList();

            return View(huntingLogsList);
        }

        // GET: HuntingLogController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HuntingLogController/Create
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0) return View(new HuntingLog());
            else return View(_baseDonnees.HuntingLogs.Find(id));
        }

        // POST: HuntingLogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert(HuntingLog huntingLog)
        {
            if (ModelState.IsValid)
            {
                if (huntingLog.Id == 0)
                {
                    // Ajouter à la BD
                    _baseDonnees.HuntingLogs.Add(huntingLog);
                    TempData["Success"] = $"{huntingLog.Title} log added";
                }
                else
                {
                    // Update
                    _baseDonnees.HuntingLogs.Update(huntingLog);
                    TempData["success"] = $"{huntingLog.Title} log updated";
                }
                _baseDonnees.SaveChanges();

                return this.RedirectToAction("Index");
            }

            return this.View(huntingLog);
        }

        public IActionResult Delete(int id)
        {
            HuntingLog? huntingLog = _baseDonnees.HuntingLogs.FirstOrDefault(h => h.Id == id);
            if (huntingLog == null)
            {
                return NotFound();
            }

            return View(huntingLog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            HuntingLog? huntingLog = _baseDonnees.HuntingLogs.Find(id);
            if (huntingLog == null)
            {
                return NotFound();
            }

            _baseDonnees.HuntingLogs.Remove(huntingLog);
            _baseDonnees.SaveChanges();
            TempData["Success"] = $"Log {huntingLog.Title} removed";
            return RedirectToAction("Index");
        }
    }
}
