using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PCore.Logica.BL
{
    public class Priorities
    {
        public List<Models.DB.Priorities> GetPriorities()
        {
            DAL.Models.PCoreContext _context = new DAL.Models.PCoreContext();

            var listPriorities = (from _priorities in _context.Priorities
                                  where _priorities.Active == true
                                  select new Models.DB.Priorities
                                  {
                                      Id = _priorities.Id,
                                      Name = _priorities.Name,
                                      Active = _priorities.Active
                                  }).ToList();

            return listPriorities;
        }
    }
}
