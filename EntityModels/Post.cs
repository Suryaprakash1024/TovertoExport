using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Toverto.EntityModels;

public partial class Post
{
    public int Id { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public int UserId { get; set; }

    [StringLength(64)]
    public string? HashId { get; set; }

    [Key]
    public int PostId { get; set; }
}
