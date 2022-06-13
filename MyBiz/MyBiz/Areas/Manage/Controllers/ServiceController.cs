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
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Services.ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            _context.Services.Add(service);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


        public IActionResult Edit(int id)
        {
            Service service = _context.Services.FirstOrDefault(x => x.Id == id);
            if (service == null)
            {
                return RedirectToAction("error", "dashboard");
            }

            return View(service);
        }

        [HttpPost]
        public IActionResult Edit(int id, Service service)
        {
            Service existService = _context.Services.FirstOrDefault(x => x.Id == id);
            if (existService == null)
            {
                return RedirectToAction("error", "dashboard");
            }
            existService.Title = service.Title;
            existService.Desc = service.Desc;
            existService.Icon = service.Icon;
            _context.SaveChanges();


            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Service service = _context.Services.FirstOrDefault(x => x.Id == id);
            if (service == null)
            {
                return RedirectToAction("error", "dashboard");
            }

            _context.Services.Remove(service);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    
   }
}
