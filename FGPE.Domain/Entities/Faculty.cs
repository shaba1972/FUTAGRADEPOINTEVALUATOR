namespace FGPE.Domain.Entities;

public class Faculty
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public ICollection<Department> Departments { get; set; }
        = new List<Department>();
}