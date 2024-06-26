﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.ProjectContext;

public partial class User_UserPost
{
    /// <summary>
    /// 使用者語錄關聯表UUID
    /// </summary>
    [Key]
    public Guid UP_UUID { get; set; }

    /// <summary>
    /// 使用者UUID
    /// </summary>
    public Guid U_UUID { get; set; }

    /// <summary>
    /// 負能量語錄UUID
    /// </summary>
    public Guid P_UUID { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int U_Sort { get; set; }

    /// <summary>
    /// 創建人
    /// </summary>
    [StringLength(50)]
    public string Creator { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    [StringLength(50)]
    public string Updator { get; set; }

    /// <summary>
    /// 更新時間
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? UpdateTime { get; set; }

    [ForeignKey("P_UUID")]
    [InverseProperty("User_UserPost")]
    public virtual Post_Post P_UU { get; set; }

    [ForeignKey("U_UUID")]
    [InverseProperty("User_UserPost")]
    public virtual User_UserAccount U_UU { get; set; }
}