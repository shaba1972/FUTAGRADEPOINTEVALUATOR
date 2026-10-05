using FGPE.Application.Models;
using FGPE.Domain.Entities;
using FGPE.Domain.Enums;

namespace FGPE.Application.Services;

public class AcademicCalculationService
{
    private readonly AcademicStandingService _academicStandingService;
   
    public AcademicCalculationService(
    AcademicStandingService academicStandingService)
  {
    _academicStandingService = academicStandingService;
  }

    public decimal CalculateGPA(IEnumerable<Result> results)
    {
        var qualifyingResults = GetEligibleResults(results);

        return CalculateAverage(qualifyingResults);
    }
     
     public decimal CalculateSemesterGPA(IEnumerable<Result> semesterResults)
{
    var qualifyingResults = GetEligibleResults(semesterResults);

    return CalculateAverage(qualifyingResults);
}
    public decimal CalculateCGPA(IEnumerable<Result> results)
    {
        var qualifyingResults = GetEligibleResults(results);

        return CalculateAverage(qualifyingResults);
    }

    public decimal CalculateQualityPoints(Result result)
    {
        var creditUnit = result.CourseRegistration.Course.CreditUnit;

        return creditUnit * result.GradePoint;
    }

    public AcademicCalculationSummary CalculateAcademicSummary(
        IEnumerable<Result> results)
    {
        var qualifyingResults = GetEligibleResults(results);

        decimal totalCreditUnits = 0m;
        decimal totalQualityPoints = 0m;

        foreach (var result in qualifyingResults)
        {
            var creditUnit = result.CourseRegistration.Course.CreditUnit;

            totalCreditUnits += creditUnit;
            totalQualityPoints += creditUnit * result.GradePoint;
        }

        var average = totalCreditUnits == 0
            ? 0m
            : Math.Round(
                totalQualityPoints / totalCreditUnits,
                2,
                MidpointRounding.AwayFromZero);

        return new AcademicCalculationSummary
        {
            TotalCreditUnits = totalCreditUnits,
            TotalQualityPoints = totalQualityPoints,
            GPA = average,
            CGPA = average
        };
    }

          public AcademicStanding DetermineAcademicStanding(decimal cgpa)
	{
   		 return _academicStandingService.DetermineStanding(cgpa);
	}

    private static List<Result> GetEligibleResults(
        IEnumerable<Result> results)
    {
        return results
            .Where(IsEligibleForCalculation)
            .ToList();
    }

    private static decimal CalculateAverage(
        IEnumerable<Result> results)
    {
        decimal totalQualityPoints = 0m;
        decimal totalCreditUnits = 0m;

        foreach (var result in results)
        {
            var creditUnit = result.CourseRegistration.Course.CreditUnit;

            totalQualityPoints += creditUnit * result.GradePoint;
            totalCreditUnits += creditUnit;
        }

        if (totalCreditUnits == 0)
            return 0m;

        return Math.Round(
            totalQualityPoints / totalCreditUnits,
            2,
            MidpointRounding.AwayFromZero);
    }

    private static bool IsEligibleForCalculation(Result result)
    {
        return result.IsQualifyingAttempt &&
               (result.Status == ResultStatus.Approved ||
                result.Status == ResultStatus.Published);
    }

}