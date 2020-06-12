using System;
using System.Collections.Generic;
using System.Text;

namespace PCore.Logica.Models.DB
{
    public class Projects
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }
        public int? TenantId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Tenants Tenant { get; set; }
    }
}
