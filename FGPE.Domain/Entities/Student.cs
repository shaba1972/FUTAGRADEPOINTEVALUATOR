using FGPE.Domain.Enums;

namespace FGPE.Domain.Entities;

public class Student
{
     public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ProgrammeId { get; set; }

    public string MatricNumber { get; set; } = string.Empty;

    public int AdmissionYear { get; set; }

    public int CurrentLevel { get; set; }

    public StudentStatus Status { get; set; }

    public User User { get; set; } = null!;

    public Programme Programme { get; set; } = null!;

    public ICollection<CourseRegistration> CourseRegistrations { get; set; }
        = new List<CourseRegistration>();

    public ICollection<AcademicSnapshot> AcademicSnapshots { get; set; }
        = new List<AcademicSnapshot>();
}
