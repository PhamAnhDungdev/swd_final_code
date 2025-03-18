using System;
using System.Collections.Generic;

namespace OnlineLearningIT.Models;

public partial class Lesson
{
    public int LessonId { get; set; }

    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string ContentType { get; set; } = null!;

    public string? ContentUrl { get; set; }

    public int? Duration { get; set; }

    public int OrderNumber { get; set; }

    public bool? IsFree { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Course Course { get; set; } = null!;
}
