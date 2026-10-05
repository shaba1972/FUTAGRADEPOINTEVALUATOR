using FGPE.Domain.Enums;

namespace FGPE.Application.Policies;

public class AcademicStandingPolicy
{
    public IReadOnlyList<AcademicStandingRule> Rules { get; } =
    [
        new AcademicStandingRule(
            4.50m,
            5.00m,
            AcademicStanding.GoodStanding),

        new AcademicStandingRule(
            0.00m,
            4.49m,
            AcademicStanding.Probation)
    ];
}

public record AcademicStandingRule(
    decimal MinimumCgpa,
    decimal MaximumCgpa,
    AcademicStanding Standing);