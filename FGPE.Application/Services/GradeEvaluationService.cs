using FGPE.Application.Models;
using FGPE.Application.Policies;

namespace FGPE.Application.Services;

public class GradeEvaluationService
{
    private readonly GradingPolicy _gradingPolicy;

    public GradeEvaluationService(GradingPolicy gradingPolicy)
    {
        _gradingPolicy = gradingPolicy;
    }

    public GradeEvaluationResult Evaluate(decimal score)
    {
        if (score < 0m || score > 100m)
        {
            throw new ArgumentOutOfRangeException(
                nameof(score),
                score,
                "Score must be between 0 and 100.");
        }

        var rule = _gradingPolicy.Rules
            .FirstOrDefault(r =>
                score >= r.MinimumScore &&
                score <= r.MaximumScore);

        if (rule is null)
        {
            throw new InvalidOperationException(
                $"No grading rule is configured for score {score}.");
        }

        return new GradeEvaluationResult
        {
            Grade = rule.Grade,
            GradePoint = rule.GradePoint
        };
    }
}