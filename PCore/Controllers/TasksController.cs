using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PCore.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index(int? projectId)
        {

            Logica.BL.Tasks tasks = new Logica.BL.Tasks();
            var listTask = tasks.GetTasks(projectId, null);

            var listTaskViewModel = listTask.Select(x => new Logica.Models.ViewModel.TasksViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Activity = x.Activity.Name,
                Details = x.Details,
                Effort = x.Effort,
                ExpirationDate = x.ExpirationDate,
                IsCompleted = x.IsCompleted,
                Priority = x.Priority.Name,
                RemainingWork = x.RemainingWork,
                State = x.State.Name
            });

            Logica.BL.Projects projects = new Logica.BL.Projects();
            var project = projects.GetProjects(projectId, null).FirstOrDefault();

            ViewBag.Project = project;
            
            return View(listTaskViewModel);
        }

        public IActionResult Create(int? projectId)
        {
            var taskBindingModel = new Logica.Models.BindingModel.TasksCreateBindingModel
            {
                ProjectId = projectId
            };

            Logica.BL.Activities activities = new Logica.BL.Activities();
            ViewBag.Activities = activities.GetActivities();

            Logica.BL.Priorities priorities = new Logica.BL.Priorities();
            ViewBag.Priorities = priorities.GetPriorities();

            Logica.BL.States states = new Logica.BL.States();
            ViewBag.States = states.GetStates();

            return View(taskBindingModel);
        }

        [HttpPost]
        public IActionResult Create(Logica.Models.BindingModel.TasksCreateBindingModel model)
        {
            if (ModelState.IsValid)
            {
                Logica.BL.Tasks task = new Logica.BL.Tasks();
                task.CreateTasks(model.Title,
                    model.Details,
                    model.ExpirationDate,
                    model.IsCompleted,
                    model.Effort,
                    model.RemainingWork,
                    model.StateId,
                    model.ActivityId,
                    model.PriorityId,
                    model.ProjectId);

                return RedirectToAction("Index", new { projectId=model.ProjectId});
            }
            Logica.BL.Activities activities = new Logica.BL.Activities();
            ViewBag.Activities = activities.GetActivities();

            Logica.BL.Priorities priorities = new Logica.BL.Priorities();
            ViewBag.Priorities = priorities.GetPriorities();

            Logica.BL.States states = new Logica.BL.States();
            ViewBag.States = states.GetStates();

            return View(model);
        }


    }
}