using FGPE.Application.Services;
using FGPE.Domain.Entities;
using FGPE.Domain.Enums;
using FGPE.Application.Policies;

namespace FGPE.Application.Tests;

public class AcademicCalculationServiceTests
{
    private static AcademicCalculationService CreateAcademicCalculationService()
{
    var standingPolicy = new AcademicStandingPolicy();
    var standingService = new AcademicStandingService(standingPolicy);

    return new AcademicCalculationService(standingService);
}

    [Fact]
    public void CalculateGPA_ShouldReturnCorrectGPA()
    {
        // Arrange
        var service = CreateAcademicCalculationService();

        var course1 = new Course
        {
            Id = Guid.NewGuid(),
            Code = "CSC101",
            Title = "Introduction to Computer Science",
            CreditUnit = 3
        };

        var course2 = new Course
        {
            Id = Guid.NewGuid(),
            Code = "MTH101",
            Title = "Mathematics",
            CreditUnit = 3
        };

        var course3 = new Course
        {
            Id = Guid.NewGuid(),
            Code = "GST101",
            Title = "General Studies",
            CreditUnit = 2
        };

        var semester = new Semester
        {
            Id = Guid.NewGuid(),
            Name = "First Semester",
            Number = 1
        };

        var student = new Student
        {
            Id = Guid.NewGuid(),
            MatricNumber = "FUTA/TEST/001"
        };

        var registration1 = new CourseRegistration
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course1.Id,
            SemesterId = semester.Id,
            Student = student,
            Course = course1,
            Semester = semester,
            AttemptNumber = 1,
            RegistrationType = RegistrationType.Regular
        };

        var registration2 = new CourseRegistration
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course2.Id,
            SemesterId = semester.Id,
            Student = student,
            Course = course2,
            Semester = semester,
            AttemptNumber = 1,
            RegistrationType = RegistrationType.Regular
        };

        var registration3 = new CourseRegistration
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course3.Id,
            SemesterId = semester.Id,
            Student = student,
            Course = course3,
            Semester = semester,
            AttemptNumber = 1,
            RegistrationType = RegistrationType.Regular
        };

        var results = new List<Result>
        {
            new()
            {
                CourseRegistration = registration1,
                Score = 75,
                Grade = "A",
                GradePoint = 5,
                Status = ResultStatus.Published,
                IsQualifyingAttempt = true
            },

            new()
            {
                CourseRegistration = registration2,
                Score = 65,
                Grade = "B",
                GradePoint = 4,
                Status = ResultStatus.Published,
                IsQualifyingAttempt = true
            },

            new()
            {
                CourseRegistration = registration3,
                Score = 55,
                Grade = "C",
                GradePoint = 3,
                Status = ResultStatus.Published,
                IsQualifyingAttempt = true
            }
        };

        // Act
        var gpa = service.CalculateGPA(results);

        // Assert
        Assert.Equal(4.13m, gpa);
    }

    [Fact]
    public void CalculateGPA_ShouldOnlyUseQualifyingAttempt()
    {
        // Arrange
        var service = CreateAcademicCalculationService();

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Code = "CSC201",
            Title = "Data Structures",
            CreditUnit = 3
        };

        var semester = new Semester
        {
            Id = Guid.NewGuid(),
            Name = "First Semester",
            Number = 1
        };

        var student = new Student
        {
            Id = Guid.NewGuid(),
            MatricNumber = "FUTA/TEST/002"
        };

        var firstAttempt = new CourseRegistration
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course.Id,
            SemesterId = semester.Id,
            Student = student,
            Course = course,
            Semester = semester,
            AttemptNumber = 1,
            RegistrationType = RegistrationType.Regular
        };

        var secondAttempt = new CourseRegistration
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course.Id,
            SemesterId = semester.Id,
            Student = student,
            Course = course,
            Semester = semester,
            AttemptNumber = 2,
            RegistrationType = RegistrationType.Repeat
        };

        var results = new List<Result>
        {
            new()
            {
                CourseRegistration = firstAttempt,
                Score = 35,
                Grade = "F",
                GradePoint = 0,
                Status = ResultStatus.Published,
                IsQualifyingAttempt = false
            },

            new()
            {
                CourseRegistration = secondAttempt,
                Score = 75,
                Grade = "A",
                GradePoint = 5,
                Status = ResultStatus.Published,
                IsQualifyingAttempt = true
            }
        };

        // Act
        var gpa = service.CalculateGPA(results);

        // Assert
        Assert.Equal(5.00m, gpa);
    }

    [Fact]
public void CalculateGPA_ShouldOnlyUseApprovedOrPublishedResults()
{
    // Arrange
    var service = CreateAcademicCalculationService();

    var course = new Course
    {
        Id = Guid.NewGuid(),
        Code = "CSC301",
        Title = "Database Systems",
        CreditUnit = 3
    };

    var semester = new Semester
    {
        Id = Guid.NewGuid(),
        Name = "First Semester",
        Number = 1
    };

    var student = new Student
    {
        Id = Guid.NewGuid(),
        MatricNumber = "FUTA/TEST/003"
    };

    var results = new List<Result>();

    var statuses = new[]
    {
        ResultStatus.Draft,
        ResultStatus.Submitted,
        ResultStatus.Reviewed,
        ResultStatus.Rejected,
        ResultStatus.Approved,
        ResultStatus.Published
    };

    foreach (var status in statuses)
    {
        var registration = new CourseRegistration
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course.Id,
            SemesterId = semester.Id,
            Student = student,
            Course = course,
            Semester = semester,
            AttemptNumber = 1,
            RegistrationType = RegistrationType.Regular
        };

        results.Add(new Result
        {
            Id = Guid.NewGuid(),
            CourseRegistration = registration,
            Score = 75,
            Grade = "A",
            GradePoint = 5,
            Status = status,
            IsQualifyingAttempt = true
        });
    }

    // Act
    var gpa = service.CalculateGPA(results);

    // Assert
    Assert.Equal(5.00m, gpa);
}

[Fact]
public void CalculateCGPA_ShouldCombineResultsAcrossMultipleSemesters()
{
    // Arrange
    var service = CreateAcademicCalculationService();

    var student = new Student
    {
        Id = Guid.NewGuid(),
        MatricNumber = "FUTA/TEST/004"
    };

    var semester1 = new Semester
    {
        Id = Guid.NewGuid(),
        Name = "First Semester",
        Number = 1
    };

    var semester2 = new Semester
    {
        Id = Guid.NewGuid(),
        Name = "Second Semester",
        Number = 2
    };

    var course1 = new Course
    {
        Id = Guid.NewGuid(),
        Code = "CSC101",
        Title = "Programming",
        CreditUnit = 3
    };

    var course2 = new Course
    {
        Id = Guid.NewGuid(),
        Code = "MTH101",
        Title = "Mathematics",
        CreditUnit = 3
    };

    var course3 = new Course
    {
        Id = Guid.NewGuid(),
        Code = "CSC102",
        Title = "Data Structures",
        CreditUnit = 2
    };

    var results = new List<Result>
    {
        new()
        {
            CourseRegistration = new CourseRegistration
            {
                Course = course1,
                Semester = semester1,
                Student = student
            },
            Score = 75,
            Grade = "A",
            GradePoint = 5,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true
        },

        new()
        {
            CourseRegistration = new CourseRegistration
            {
                Course = course2,
                Semester = semester1,
                Student = student
            },
            Score = 65,
            Grade = "B",
            GradePoint = 4,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true
        },

        new()
        {
            CourseRegistration = new CourseRegistration
            {
                Course = course3,
                Semester = semester2,
                Student = student
            },
            Score = 55,
            Grade = "C",
            GradePoint = 3,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true
        }
    };

    // Act
    var cgpa = service.CalculateCGPA(results);

    // Assert
    Assert.Equal(4.13m, cgpa);
}

[Fact]
public void CalculateGPA_ShouldReturnZero_WhenThereAreNoEligibleResults()
{
    // Arrange
    var service = CreateAcademicCalculationService();

    var results = new List<Result>
    {
        new()
        {
            Score = 75,
            Grade = "A",
            GradePoint = 5,
            Status = ResultStatus.Draft,
            IsQualifyingAttempt = true
        }
    };

    // Act
    var gpa = service.CalculateGPA(results);

    // Assert
    Assert.Equal(0m, gpa);
}

[Fact]
public void CalculateAcademicSummary_ShouldReturnCompleteSummary()
{
    // Arrange
    var service = CreateAcademicCalculationService();

    var results = new List<Result>
    {
        new()
        {
            Score = 75,
            Grade = "A",
            GradePoint = 5,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true,
            CourseRegistration = new CourseRegistration
            {
                Course = new Course
                {
                    Code = "CSC101",
                    Title = "Introduction to Computer Science",
                    CreditUnit = 3
                }
            }
        },
        new()
        {
            Score = 65,
            Grade = "B",
            GradePoint = 4,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true,
            CourseRegistration = new CourseRegistration
            {
                Course = new Course
                {
                    Code = "MTH101",
                    Title = "Mathematics",
                    CreditUnit = 3
                }
            }
        },
        new()
        {
            Score = 55,
            Grade = "C",
            GradePoint = 3,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true,
            CourseRegistration = new CourseRegistration
            {
                Course = new Course
                {
                    Code = "GST101",
                    Title = "General Studies",
                    CreditUnit = 2
                }
            }
        }
    };

    // Act
    var summary = service.CalculateAcademicSummary(results);

    // Assert
    Assert.Equal(8m, summary.TotalCreditUnits);
    Assert.Equal(33m, summary.TotalQualityPoints);
    Assert.Equal(4.13m, summary.GPA);
    Assert.Equal(4.13m, summary.CGPA);
}

[Fact]
public void CalculateSemesterGPA_ShouldCalculateGPAForSemester()
{
    // Arrange
    var service = CreateAcademicCalculationService();

    var semesterResults = new List<Result>
    {
        new()
        {
            Score = 75,
            Grade = "A",
            GradePoint = 5,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true,
            CourseRegistration = new CourseRegistration
            {
                Course = new Course
                {
                    Code = "CSC101",
                    CreditUnit = 3
                }
            }
        },
        new()
        {
            Score = 65,
            Grade = "B",
            GradePoint = 4,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true,
            CourseRegistration = new CourseRegistration
            {
                Course = new Course
                {
                    Code = "MTH101",
                    CreditUnit = 3
                }
            }
        },
        new()
        {
            Score = 55,
            Grade = "C",
            GradePoint = 3,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true,
            CourseRegistration = new CourseRegistration
            {
                Course = new Course
                {
                    Code = "GST101",
                    CreditUnit = 2
                }
            }
        }
    };

    // Act
    var gpa = service.CalculateSemesterGPA(semesterResults);

    // Assert
    Assert.Equal(4.13m, gpa);
}

[Fact]
public void CalculateCGPA_ShouldUseTotalCreditUnitsAcrossSemesters()
{
    // Arrange
    var service = CreateAcademicCalculationService();

    var results = new List<Result>
    {
        // Semester 1
        new()
        {
            Score = 75,
            Grade = "A",
            GradePoint = 5,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true,
            CourseRegistration = new CourseRegistration
            {
                Semester = new Semester
                {
                    Name = "First Semester",
                    Number = 1
                },
                Course = new Course
                {
                    Code = "CSC101",
                    CreditUnit = 3
                }
            }
        },
        new()
        {
            Score = 65,
            Grade = "B",
            GradePoint = 4,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true,
            CourseRegistration = new CourseRegistration
            {
                Semester = new Semester
                {
                    Name = "First Semester",
                    Number = 1
                },
                Course = new Course
                {
                    Code = "MTH101",
                    CreditUnit = 3
                }
            }
        },

        // Semester 2
        new()
        {
            Score = 55,
            Grade = "C",
            GradePoint = 3,
            Status = ResultStatus.Published,
            IsQualifyingAttempt = true,
            CourseRegistration = new CourseRegistration
            {
                Semester = new Semester
                {
                    Name = "Second Semester",
                    Number = 2
                },
                Course = new Course
                {
                    Code = "GST101",
                    CreditUnit = 2
                }
            }
        }
    };

    // Act
    var cgpa = service.CalculateCGPA(results);

    // Assert
    Assert.Equal(4.13m, cgpa);
}

[Fact]
public void Evaluate_ShouldReturnAFor70()
{
    var service = new GradeEvaluationService(new GradingPolicy());

    var result = service.Evaluate(70m);

    Assert.Equal("A", result.Grade);
    Assert.Equal(5m, result.GradePoint);
}

[Fact]
public void Evaluate_ShouldReturnBFor69()
{
    var service = new GradeEvaluationService(new GradingPolicy());

    var result = service.Evaluate(69m);

    Assert.Equal("B", result.Grade);
    Assert.Equal(4m, result.GradePoint);
}

[Fact]
public void Evaluate_ShouldReturnCFor59()
{
    var service = new GradeEvaluationService(new GradingPolicy());

    var result = service.Evaluate(59m);

    Assert.Equal("C", result.Grade);
    Assert.Equal(3m, result.GradePoint);
}

[Fact]
public void Evaluate_ShouldReturnDFor45()
{
    var service = new GradeEvaluationService(new GradingPolicy());

    var result = service.Evaluate(45m);

    Assert.Equal("D", result.Grade);
    Assert.Equal(2m, result.GradePoint);
}

[Fact]
public void Evaluate_ShouldReturnEFor40()
{
    var service = new GradeEvaluationService(new GradingPolicy());

    var result = service.Evaluate(40m);

    Assert.Equal("E", result.Grade);
    Assert.Equal(1m, result.GradePoint);
}

[Fact]
public void Evaluate_ShouldReturnFFor39()
{
    var service = new GradeEvaluationService(new GradingPolicy());

    var result = service.Evaluate(39m);

    Assert.Equal("F", result.Grade);
    Assert.Equal(0m, result.GradePoint);
}

[Fact]
public void Evaluate_ShouldReturnAFor100()
{
    var service = new GradeEvaluationService(new GradingPolicy());

    var result = service.Evaluate(100m);

    Assert.Equal("A", result.Grade);
    Assert.Equal(5m, result.GradePoint);
}

[Fact]
public void Evaluate_ShouldThrowForScoreAbove100()
{
    var service = new GradeEvaluationService(new GradingPolicy());

    var exception = Assert.Throws<ArgumentOutOfRangeException>(
        () => service.Evaluate(101m));

    Assert.Equal("score", exception.ParamName);
}

[Fact]
public void Evaluate_ShouldThrowForNegativeScore()
{
    var service = new GradeEvaluationService(new GradingPolicy());

    var exception = Assert.Throws<ArgumentOutOfRangeException>(
        () => service.Evaluate(-1m));

    Assert.Equal("score", exception.ParamName);
}

[Fact]
public void DetermineStanding_ShouldReturnGoodStandingForHighCgpa()
{
    var service = new AcademicStandingService(
        new AcademicStandingPolicy());

    var result = service.DetermineStanding(4.50m);

       Assert.Equal<AcademicStanding>(
    AcademicStanding.GoodStanding,
    result);
}

[Fact]
public void DetermineStanding_ShouldReturnProbationForLowCgpa()
{
    var service = new AcademicStandingService(
        new AcademicStandingPolicy());

    var result = service.DetermineStanding(3.00m);

    Assert.Equal<AcademicStanding>(
    AcademicStanding.Probation,
    result);
}

[Fact]
public void DetermineStanding_ShouldReturnProbationAtZeroCgpa()
{
    var service = new AcademicStandingService(
        new AcademicStandingPolicy());

    var result = service.DetermineStanding(0m);

    Assert.Equal<AcademicStanding>(
    AcademicStanding.Probation,
    result);
}

[Fact]
public void DetermineStanding_ShouldReturnGoodStandingAtMaximumCgpa()
{
    var service = new AcademicStandingService(
        new AcademicStandingPolicy());

    var result = service.DetermineStanding(5.00m);

    Assert.Equal<AcademicStanding>(
    AcademicStanding.GoodStanding,
    result);
}

[Fact]
public void DetermineStanding_ShouldThrowForCgpaAboveFive()
{
    var service = new AcademicStandingService(
        new AcademicStandingPolicy());

    Assert.Throws<ArgumentOutOfRangeException>(
        () => service.DetermineStanding(5.01m));
}

[Fact]
public void DetermineStanding_ShouldThrowForNegativeCgpa()
{
    var service = new AcademicStandingService(
        new AcademicStandingPolicy());

    Assert.Throws<ArgumentOutOfRangeException>(
        () => service.DetermineStanding(-0.01m));
}

[Fact]
public void DetermineAcademicStanding_ShouldDelegateToStandingService_ForGoodStanding()
{
    var service = CreateAcademicCalculationService();

    var result = service.DetermineAcademicStanding(4.50m);

    Assert.Equal<AcademicStanding>(
        AcademicStanding.GoodStanding,
        result);
}

[Fact]
public void DetermineAcademicStanding_ShouldDelegateToStandingService_ForProbation()
{
    var service = CreateAcademicCalculationService();

    var result = service.DetermineAcademicStanding(3.00m);

    Assert.Equal<AcademicStanding>(
        AcademicStanding.Probation,
        result);
}
}