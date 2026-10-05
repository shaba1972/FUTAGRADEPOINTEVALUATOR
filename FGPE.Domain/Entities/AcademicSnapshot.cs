using FGPE.Domain.Enums;

namespace FGPE.Domain.Entities;

public class AcademicSnapshot
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid? SemesterId { get; set; }

    public DateTime SnapshotDate { get; set; }

    public decimal TotalCreditUnits { get; set; }

    public decimal TotalQualityPoints { get; set; }

    public decimal GPA { get; set; }

    public decimal CGPA { get; set; }

    public AcademicStanding AcademicStanding { get; set; }

    public Student Student { get; set; } = null!;

    public Semester? Semester { get; set; }
}