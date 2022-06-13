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
    [Area("manage")]
    public class TeamMemberController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamMemberController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var data = _context.TeamMembers.ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TeamMember teamMember)
        {
            if (teamMember.ImageFile != null)
            {

                if (teamMember.ImageFile.ContentType != "image/png" && teamMember.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File format must be png or jpeg");
                    return View();
                }
                if (teamMember.ImageFile.Length > 2097152)
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

            teamMember.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", teamMember.ImageFile);

            _context.TeamMembers.Add(teamMember);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            TeamMember teamMember = _context.TeamMembers.FirstOrDefault(x => x.Id == id);

            if (teamMember == null)
            {
                return NotFound();
            }
            FileManager.Delete(_env.WebRootPath, "uploads/sliders", teamMember.Image);
            _context.TeamMembers.Remove(teamMember);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult Edit(int id)
        {
            TeamMember teamMember = _context.TeamMembers.FirstOrDefault(x => x.Id == id);
            if (teamMember == null)
                return RedirectToAction("error", "dashboard");

            return View(teamMember);
        }

        [HttpPost]
        public IActionResult Edit(TeamMember teamMember)
        {
            TeamMember existTeamMember = _context.TeamMembers.FirstOrDefault(x => x.Id == teamMember.Id);
            if (teamMember == null)
                return RedirectToAction("error", "dashboard");


            if (teamMember.ImageFile != null)
            {
                if (teamMember.ImageFile.ContentType != "image/png" && teamMember.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File format must be png or jpeg");
                    return View();
                }
                if (teamMember.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size must be less than 2MB");
                    return View();
                }

                if (!ModelState.IsValid)
                {
                    return View();
                }

                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/sliders", teamMember.ImageFile);
                FileManager.Delete(_env.WebRootPath, "uploads/sliders", existTeamMember.Image);
                teamMember.Image = newFileName;
            }

            existTeamMember.FullName = teamMember.FullName;
            existTeamMember.Desc = teamMember.Desc;
            //existTeamMember. = teamMember.BtnText;
            //existTeamMember.BtnUrl = teamMember.BtnUrl;
            //existTeamMember.Order = teamMember.Order;

            _context.SaveChanges();
            return RedirectToAction("index");

        }
    }
}
