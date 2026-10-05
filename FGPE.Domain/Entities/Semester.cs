namespace FGPE.Domain.Entities;

public class Semester
{
    public Guid Id { get; set; }

    public Guid AcademicSessionId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Number { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public AcademicSession AcademicSession { get; set; } = null!;

    public ICollection<CourseRegistration> CourseRegistrations { get; set; }
        = new List<CourseRegistration>();
}