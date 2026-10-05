using FGPE.Domain.Enums;

namespace FGPE.Domain.Entities;

public class Result
{
    public Guid Id { get; set; }

    public Guid CourseRegistrationId { get; set; }

    public decimal Score { get; set; }

    public string Grade { get; set; } = string.Empty;

    public decimal GradePoint { get; set; }

    public ResultStatus Status { get; set; }

    public bool IsQualifyingAttempt { get; set; }

    public DateTime DateRecorded { get; set; } = DateTime.UtcNow;

    public CourseRegistration CourseRegistration { get; set; } = null!;
}