using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Toverto.EntityModels;

public partial class Log
{
    [Key]
    public int Id { get; set; }

    public string Message { get; set; } = null!;

    public string? StackTrace { get; set; }

    [Column("innerException")]
    public string? InnerException { get; set; }

    public string? Source { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }
}
