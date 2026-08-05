using PMDSSystems.Models;

public class PerformanceReview
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public int CycleId { get; set; }
    public PerformanceCycle Cycle { get; set; }

    public int KPI1 { get; set; }
    public int KPI2 { get; set; }
    public int KPI3 { get; set; }

    public string Comments { get; set; }

    public int FinalScore { get; set; }
}