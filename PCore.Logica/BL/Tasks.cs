using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PCore.Logica.BL
{
    public class Tasks
    {
        public List<Models.DB.Tasks>GetTasks(int? projectId, int? id)
        {
            DAL.Models.PCoreContext _context = new DAL.Models.PCoreContext();
            var listTasks = (from _tasks in _context.Tasks
                             join _states in _context.States on _tasks.StateId equals _states.Id
                             join _activities in _context.Activities on _tasks.ActivityId equals _activities.Id
                             join _priorities in _context.Priorities on _tasks.PriorityId equals _priorities.Id
                             select new Models.DB.Tasks
                             {
                                 Id = _tasks.Id,
                                 Details = _tasks.Details,
                                 Effort = _tasks.Effort,
                                 Title = _tasks.Title,
                                 ExpirationDate = _tasks.ExpirationDate,
                                 RemainingWork = _tasks.RemainingWork,
                                 IsCompleted = _tasks.IsCompleted,
                                 StateId = _tasks.StateId,
                                 State = new Models.DB.States
                                 {
                                     Name = _states.Name
                                 },
                                 PriorityId = _tasks.PriorityId,
                                 Priority = new Models.DB.Priorities
                                 {
                                     Name = _priorities.Name
                                 },
                                 ActivityId = _tasks.ActivityId,
                                 Activity = new Models.DB.Activities
                                 {
                                     Name = _activities.Name
                                 },
                                 ProjectId = _tasks.ProjectId

                             }).ToList();

            if(projectId != null)
            {
                listTasks = listTasks.Where(x => x.ProjectId == projectId).ToList();
            }
            if(id != null)
            {
                listTasks = listTasks.Where(x => x.Id == id).ToList();
            }

            return listTasks;
        }
        
        public void CreateTasks(string title,
            string details,
            DateTime? expirationDate,
            bool? isCompleted,
            int? effort,
            int? remaininWork,
            int? statedId,
            int? activityId,
            int? priorityId,
            int? projectId)
        {
            DAL.Models.PCoreContext _context = new DAL.Models.PCoreContext();
            _context.Tasks.Add(new DAL.Models.Tasks
            {
                Title = title,
                ActivityId = activityId,
                Details = details,
                Effort = effort,
                ExpirationDate = expirationDate,
                IsCompleted = isCompleted,
                PriorityId = priorityId,
                ProjectId = projectId,
                RemainingWork = remaininWork,
                StateId = statedId
            });

            _context.SaveChanges();
        }
    }
}
