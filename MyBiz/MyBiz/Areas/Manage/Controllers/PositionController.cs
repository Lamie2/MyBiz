using Microsoft.AspNetCore.Mvc;
using MyBiz.DAL;
using MyBiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBiz.Areas.Manage.Controllers
{
    [Area("manage")]
    public class PositionController : Controller
    {

        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Positions.ToList();
            return View(data);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Position position)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Positions.Any(x=>x.Name==position.Name))
            {
                ModelState.AddModelError("Name", "This position already exists");
                return View();
            }

            _context.Positions.Add(position);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


        public IActionResult Edit(int id)
        {
            Position position = _context.Positions.FirstOrDefault(x => x.Id == id);
            if (position==null)
            {
                return RedirectToAction("error", "dashboard");
            }

            return View(position);
        }

        [HttpPost]
        public IActionResult Edit(int id, Position position)
        {
            Position existPosition = _context.Positions.FirstOrDefault(x => x.Id == id);
            if(existPosition==null)
            {
                return RedirectToAction("error", "dashboard");
            }

            if (_context.Positions.Any(x=>x.Id==id && x.Name==position.Name))
            {
                ModelState.AddModelError("Name", "This position already exists");
                return View();
            }
            existPosition.Name = position.Name;
            _context.SaveChanges();


            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Position position = _context.Positions.FirstOrDefault(x => x.Id == id);
            if (position == null)
            {
                return RedirectToAction("error", "dashboard");
            }

            _context.Positions.Remove(position);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
