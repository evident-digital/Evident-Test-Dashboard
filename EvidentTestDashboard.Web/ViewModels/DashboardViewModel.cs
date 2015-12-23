using System.Collections.Generic;
using EvidentTestDashboard.Library.Entities;

namespace EvidentTestDashboard.Web.ViewModels
{
    public class DashboardViewModel
    {
        public Dashboard Dashboard { get; set; }
        public ICollection<AggregateBuildsViewModel> builds { get; set; } = new List<AggregateBuildsViewModel>();
    }
}