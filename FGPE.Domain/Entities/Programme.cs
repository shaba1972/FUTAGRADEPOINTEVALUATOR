namespace FGPE.Domain.Entities;

public class Programme
{
    public Guid Id { get; set; }

    public Guid DepartmentId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public Department Department { get; set; } = null!;

    public ICollection<Student> Students { get; set; }
        = new List<Student>();

    public ICollection<ProgrammeCourse> ProgrammeCourses { get; set; }
        = new List<ProgrammeCourse>();
}