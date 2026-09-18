using Microsoft.AspNetCore.Mvc;
using ZombieParty.Models;
using ZombieParty.Models.Data;
using ZombieParty.ViewModels;

namespace ZombieParty.Controllers
{
    public class WeaponController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public WeaponController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }

        public IActionResult Index()
        {
            List<Weapon> weapons = _baseDonnees.Weapons.ToList();
            return View(weapons);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0) return View(new Weapon());
            else return View(_baseDonnees.Weapons.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Weapon weapon)
        {
            if (ModelState.IsValid)
            {
                if (weapon.WeaponId == 0)
                {
                    // Ajouter à la BD
                    _baseDonnees.Weapons.Add(weapon);
                    TempData["Success"] = $"{weapon.Name} weapon added";
                }
                else
                {
                    // Update
                    _baseDonnees.Weapons.Update(weapon);
                    TempData["success"] = $"{weapon.Name} weapon updated";
                }
                _baseDonnees.SaveChanges();

                return this.RedirectToAction("Index");
            }

            return this.View(weapon);
        }
    }
}
