using EventFlow.Models;

namespace EventFlow.ViewModels.Dashboard;

public class DashboardViewModel
{
    public int TotalCategories { get; set; }

    public int TotalEvents { get; set; }

    public int TotalUsers { get; set; }

    public List<EventFlow.Models.Event> RecentEvents { get; set; } = new();
}