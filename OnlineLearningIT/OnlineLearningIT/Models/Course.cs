using System;
using System.Collections.Generic;

namespace OnlineLearningIT.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public int? Duration { get; set; }

    public decimal? Price { get; set; }

    public string? Level { get; set; }

    public string? Category { get; set; }

    public int? InstructorId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual User? Instructor { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
