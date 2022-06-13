using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBiz.DAL;
using MyBiz.Helpers;
using MyBiz.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyBiz.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var data = _context.Sliders.ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider.ImageFile!=null)
            {
                
                if (slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File format must be png or jpeg");
                    return View();
                }
                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size must be less than 2MB");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
           
            slider.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
        
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            if (slider==null)
            {
                return NotFound();
            }
            FileManager.Delete(_env.WebRootPath, "uploads/sliders", slider.Image);
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null)
                return RedirectToAction("error", "dashboard");

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            Slider existSlider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);
            if (slider == null)
                return RedirectToAction("error", "dashboard");


            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File format must be png or jpeg");
                    return View();
                }
                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size must be less than 2MB");
                    return View();
                }

                if (!ModelState.IsValid)
                {
                    return View();
                }

               string newFileName = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
                FileManager.Delete(_env.WebRootPath, "uploads/sliders", existSlider.Image);
                slider.Image = newFileName; 
            }

            existSlider.Title = slider.Title;
            existSlider.Desc = slider.Desc;
            existSlider.BtnText = slider.BtnText;
            existSlider.BtnUrl = slider.BtnUrl;
            existSlider.Order = slider.Order;

            _context.SaveChanges();
            return RedirectToAction("index");

        }
    }  
}
