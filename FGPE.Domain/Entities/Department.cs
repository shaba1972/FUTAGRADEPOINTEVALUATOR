namespace FGPE.Domain.Entities;

public class Department
{
    public Guid Id { get; set; }

    public Guid FacultyId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public Faculty Faculty { get; set; } = null!;

    public ICollection<Programme> Programmes { get; set; }
        = new List<Programme>();
}
