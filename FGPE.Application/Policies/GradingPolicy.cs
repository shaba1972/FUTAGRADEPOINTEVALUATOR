namespace FGPE.Application.Policies;

public class GradingPolicy
{
    public IReadOnlyList<GradeRule> Rules { get; } =
    [
        new GradeRule(70m, 100m, "A", 5m),
        new GradeRule(60m, 69.99m, "B", 4m),
        new GradeRule(50m, 59.99m, "C", 3m),
        new GradeRule(45m, 49.99m, "D", 2m),
        new GradeRule(40m, 44.99m, "E", 1m),
        new GradeRule(0m, 39.99m, "F", 0m)
    ];
}

public record GradeRule(
    decimal MinimumScore,
    decimal MaximumScore,
    string Grade,
    decimal GradePoint);