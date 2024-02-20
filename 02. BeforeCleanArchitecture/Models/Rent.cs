﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BeforeCleanArchitecture.Models;

[Table("Rent")]
public partial class Rent
{
    /// <summary>
    /// 대여고유ID
    /// </summary>
    [Key]
    [StringLength(36)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    /// <summary>
    /// 대여도서ID
    /// </summary>
    [StringLength(36)]
    [Unicode(false)]
    public string BookId { get; set; } = null!;

    /// <summary>
    /// 대여자ID
    /// </summary>
    [StringLength(36)]
    [Unicode(false)]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 반납기한
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// 대여일시
    /// </summary>
    public DateTime BorrowedAt { get; set; }

    /// <summary>
    /// 반납일시
    /// </summary>
    public DateTime? ReturnedAt { get; set; }

    /// <summary>
    /// 수정일시
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 생성일시
    /// </summary>
    public DateTime CreatedAt { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("Rents")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Rents")]
    public virtual User User { get; set; } = null!;
}