﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.ProjectContext;

public partial class User_UserAccount
{
    /// <summary>
    /// 排序
    /// </summary>
    public int U_Sort { get; set; }

    /// <summary>
    /// UUID
    /// </summary>
    [Key]
    public Guid U_UUID { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    [Required]
    [StringLength(50)]
    public string U_Account { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    [Required]
    [StringLength(255)]
    public string U_Pwd { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [StringLength(50)]
    public string U_Name { get; set; }

    /// <summary>
    /// 電子信箱
    /// </summary>
    [Required]
    [StringLength(255)]
    public string U_EMail { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    [StringLength(50)]
    public string U_Tel { get; set; }

    /// <summary>
    /// 是否通過驗證
    /// </summary>
    public bool? U_Verify { get; set; }

    /// <summary>
    /// 驗證碼
    /// </summary>
    [StringLength(50)]
    [Unicode(false)]
    public string U_VerifyCode { get; set; }

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

    [InverseProperty("U_UU")]
    public virtual ICollection<User_UserPost> User_UserPost { get; set; } = new List<User_UserPost>();
}