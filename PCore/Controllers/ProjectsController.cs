using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PCore.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        public ProjectsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Logica.BL.Tenants tenants = new Logica.BL.Tenants();
            var Tenant = tenants.GetTenants(user.Id).FirstOrDefault();

            Logica.BL.Projects projects = new Logica.BL.Projects();
            var result = await _userManager.IsInRoleAsync(user, "Admin") ?
                projects.GetProjects(null, Tenant.Id) :
                projects.GetProjects(null, Tenant.Id, user.Id);

            var listProjects = result.Select(x => new Logica.Models.ViewModel.ProjectsIndexViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Details = x.Details,
                ExpectedCompletionDate = x.ExpectedCompletionDate,
                CreatedAt = x.CreatedAt,
                UpdateAt = x.UpdatedAt
            });

            listProjects = Tenant.Plan.Equals("premium") ?
                listProjects :
                listProjects.Take(1).ToList();


            return View(listProjects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PCore.Logica.Models.BindingModel.ProjectsCreateBindingModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                Logica.BL.Tenants tenants = new Logica.BL.Tenants();
                var Tenant = tenants.GetTenants(user.Id).FirstOrDefault();

                Logica.BL.Projects projects = new Logica.BL.Projects();
                projects.CreateProjects(model.Title, model.Details, model.ExpectedCompletionDate, Tenant.Id);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int? id)
        {
            Logica.BL.Projects project = new Logica.BL.Projects();
            var projects = project.GetProjects(id, null).FirstOrDefault();

            var projectBindingModel = new Logica.Models.BindingModel.ProjectsEditBindingModel
            {
                Id = projects.Id,
                Title = projects.Title,
                Details = projects.Details,
                ExpectedCompletionDate = projects.ExpectedCompletionDate
            };
            return View(projectBindingModel);
        }

        [HttpPost]
        public IActionResult Edit(Logica.Models.BindingModel.ProjectsEditBindingModel model)
        {
            if (ModelState.IsValid)
            {
                Logica.BL.Projects project = new Logica.BL.Projects();
                project.UpdateProjects(model.Id, model.Title, model.Details, model.ExpectedCompletionDate);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Details(int? id)
        {
            Logica.BL.Projects project = new Logica.BL.Projects();
            var projects = project.GetProjects(id, null).FirstOrDefault();
            var projectViewModel = new Logica.Models.ViewModel.ProjectsDetailsViewModel
            {
                Id = projects.Id,
                Title = projects.Title,
                Details = projects.Details,
                ExpectedCompletionDate = projects.ExpectedCompletionDate,
                CreatedAt = projects.CreatedAt,
                UpdateAt = projects.UpdatedAt
            };


            return View(projectViewModel);
        }


        public IActionResult Delete(int id)
        {
            Logica.BL.Projects project = new Logica.BL.Projects();
            project.DeleteProjects(id);
            return RedirectToAction("Index");
        }
    }
}