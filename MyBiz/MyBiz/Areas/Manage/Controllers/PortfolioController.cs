using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyBiz.DAL;
using MyBiz.Helpers;
using MyBiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBiz.Areas.Manage.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var data = _context.Portfolios.ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Portfolio portfolio)
        {
            if (portfolio.ImageFile != null)
            {

                if (portfolio.ImageFile.ContentType != "image/png" && portfolio.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File format must be png or jpeg");
                    return View();
                }
                if (portfolio.ImageFile.Length > 2097152)
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

            portfolio.Image = FileManager.Save(_env.WebRootPath, "uploads/portfolios", portfolio.ImageFile);

            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Portfolio portfolio = _context.Portfolios.FirstOrDefault(x => x.Id == id);

            if (portfolio == null)
            {
                return NotFound();
            }
            FileManager.Delete(_env.WebRootPath, "uploads/portfolios", portfolio.Image);
            _context.Portfolios.Remove(portfolio);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult Edit(int id)
        {
            Portfolio portfolio = _context.Portfolios.FirstOrDefault(x => x.Id == id);
            if (portfolio == null)
                return RedirectToAction("error", "dashboard");

            return View(portfolio);
        }

        [HttpPost]
        public IActionResult Edit(Portfolio portfolio)
        {
            Portfolio existPortfolio = _context.Portfolios.FirstOrDefault(x => x.Id == portfolio.Id);
            if (portfolio == null)
                return RedirectToAction("error", "dashboard");


            if (portfolio.ImageFile != null)
            {
                if (portfolio.ImageFile.ContentType != "image/png" && portfolio.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File format must be png or jpeg");
                    return View();
                }
                if (portfolio.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size must be less than 2MB");
                    return View();
                }

                if (!ModelState.IsValid)
                {
                    return View();
                }

                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/portfolios", portfolio.ImageFile);
                FileManager.Delete(_env.WebRootPath, "uploads/portfolios", existPortfolio.Image);
                portfolio.Image = newFileName;
            }

            existPortfolio.Title = portfolio.Title;
            existPortfolio.Subtitle = portfolio.Subtitle;
            existPortfolio.Icon = portfolio.Icon;
            existPortfolio.DetailIcon = portfolio.DetailIcon;
            
            _context.SaveChanges();
            return RedirectToAction("index");

        }
    }
}
