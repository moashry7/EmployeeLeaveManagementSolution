public class DepartmentBudgetVM
{
    public string Name { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public decimal UsedSalaries { get; set; }
    public int UsagePercent => Budget == 0 ? 0 : (int)Math.Min(100, (UsedSalaries / Budget) * 100);
}
