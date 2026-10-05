using FGPE.Domain.Enums;

namespace FGPE.Domain.Entities;

public class ProgrammeCourse
{
    public Guid Id { get; set; }

    public Guid ProgrammeId { get; set; }

    public Guid CourseId { get; set; }

    public int Level { get; set; }

    public int SemesterNumber { get; set; }

    public CourseType CourseType { get; set; }

    public bool IsCompulsory { get; set; }

    public Programme Programme { get; set; } = null!;

    public Course Course { get; set; } = null!;
}