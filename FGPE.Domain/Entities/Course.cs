namespace FGPE.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int CreditUnit { get; set; }

    public int Level { get; set; }

    public ICollection<ProgrammeCourse> ProgrammeCourses { get; set; }
        = new List<ProgrammeCourse>();

    public ICollection<CourseRegistration> Registrations { get; set; }
        = new List<CourseRegistration>();
}