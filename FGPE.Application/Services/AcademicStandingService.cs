using FGPE.Application.Policies;
using FGPE.Domain.Enums;

namespace FGPE.Application.Services;

public class AcademicStandingService
{
    private readonly AcademicStandingPolicy _policy;

    public AcademicStandingService(AcademicStandingPolicy policy)
    {
        _policy = policy;
    }

    public AcademicStanding DetermineStanding(decimal cgpa)
    {
        if (cgpa < 0m || cgpa > 5m)
        {
            throw new ArgumentOutOfRangeException(
                nameof(cgpa),
                cgpa,
                "CGPA must be between 0 and 5.");
        }

        var rule = _policy.Rules
            .FirstOrDefault(r =>
                cgpa >= r.MinimumCgpa &&
                cgpa <= r.MaximumCgpa);

        if (rule is null)
        {
            throw new InvalidOperationException(
                $"No academic standing rule is configured for CGPA {cgpa}.");
        }

        return rule.Standing;
    }
}