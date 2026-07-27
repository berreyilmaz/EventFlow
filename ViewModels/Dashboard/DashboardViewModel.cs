namespace EventFlow.ViewModels.Dashboard;

public class DashboardViewModel
{
    public int TotalUsers { get; set; }

    public int TotalEvents { get; set; }

    public int TotalRegistrations { get; set; }

    public int TotalAuditLogs { get; set; }

    public int UnauthorizedAttempts { get; set; }

    public int TotalExceptions { get; set; }
}