using FGPE.Domain.Enums;

namespace FGPE.Domain.Entities;

public class CourseRegistration
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public Guid SemesterId { get; set; }

    public RegistrationType RegistrationType { get; set; }

    public int AttemptNumber { get; set; }

    public Student Student { get; set; } = null!;

    public Course Course { get; set; } = null!;

    public Semester Semester { get; set; } = null!;

    public Result? Result { get; set; }
}