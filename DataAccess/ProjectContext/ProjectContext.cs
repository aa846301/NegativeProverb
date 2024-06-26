﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.ProjectContext;

public partial class ProjectContext : DbContext
{
    public ProjectContext(DbContextOptions<ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Post_Post> Post_Post { get; set; }

    public virtual DbSet<Post_PostTag> Post_PostTag { get; set; }

    public virtual DbSet<Post_Tag> Post_Tag { get; set; }

    public virtual DbSet<User_UserAccount> User_UserAccount { get; set; }

    public virtual DbSet<User_UserPost> User_UserPost { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post_Post>(entity =>
        {
            entity.HasKey(e => e.P_UUID).HasName("PK_Post");

            entity.ToTable(tb => tb.HasComment("負能量語錄主表"));

            entity.Property(e => e.P_UUID)
                .ValueGeneratedNever()
                .HasComment("識別編碼");
            entity.Property(e => e.CreateTime).HasComment("創建時間");
            entity.Property(e => e.Creator).HasComment("創建人");
            entity.Property(e => e.P_Post).HasComment("負能量語錄");
            entity.Property(e => e.U_Sort)
                .ValueGeneratedOnAdd()
                .HasComment("排序");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
            entity.Property(e => e.Updator).HasComment("更新人");
        });

        modelBuilder.Entity<Post_PostTag>(entity =>
        {
            entity.Property(e => e.PPT_UUID)
                .ValueGeneratedNever()
                .HasComment("語錄標籤關聯UUID");
            entity.Property(e => e.CreateTime).HasComment("創建時間");
            entity.Property(e => e.Creator).HasComment("創建人");
            entity.Property(e => e.PPT_Sort)
                .ValueGeneratedOnAdd()
                .HasComment("排序");
            entity.Property(e => e.PT_UUID).HasComment("語錄標籤UUID");
            entity.Property(e => e.P_UUID).HasComment("語錄UUID");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
            entity.Property(e => e.Updator).HasComment("更新人");

            entity.HasOne(d => d.PT_UU).WithMany(p => p.Post_PostTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_PostTag_Post_Tag");

            entity.HasOne(d => d.P_UU).WithMany(p => p.Post_PostTag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_PostTag_Post_Post");
        });

        modelBuilder.Entity<Post_Tag>(entity =>
        {
            entity.HasKey(e => e.PT_UUID).HasName("PK_PostTag");

            entity.Property(e => e.PT_UUID).ValueGeneratedNever();
            entity.Property(e => e.CreateTime).HasComment("創建時間");
            entity.Property(e => e.Creator).HasComment("創建人");
            entity.Property(e => e.PT_Sort)
                .ValueGeneratedOnAdd()
                .HasComment("排序");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
            entity.Property(e => e.Updator).HasComment("更新人");
        });

        modelBuilder.Entity<User_UserAccount>(entity =>
        {
            entity.Property(e => e.U_UUID)
                .ValueGeneratedNever()
                .HasComment("UUID");
            entity.Property(e => e.CreateTime).HasComment("創建時間");
            entity.Property(e => e.Creator).HasComment("創建人");
            entity.Property(e => e.U_Account).HasComment("帳號");
            entity.Property(e => e.U_EMail).HasComment("電子信箱");
            entity.Property(e => e.U_Name).HasComment("姓名");
            entity.Property(e => e.U_Pwd).HasComment("密碼");
            entity.Property(e => e.U_Sort)
                .ValueGeneratedOnAdd()
                .HasComment("排序");
            entity.Property(e => e.U_Tel).HasComment("電話");
            entity.Property(e => e.U_Verify).HasComment("是否通過驗證");
            entity.Property(e => e.U_VerifyCode).HasComment("驗證碼");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
            entity.Property(e => e.Updator).HasComment("更新人");
        });

        modelBuilder.Entity<User_UserPost>(entity =>
        {
            entity.Property(e => e.UP_UUID)
                .ValueGeneratedNever()
                .HasComment("使用者語錄關聯表UUID");
            entity.Property(e => e.CreateTime).HasComment("創建時間");
            entity.Property(e => e.Creator).HasComment("創建人");
            entity.Property(e => e.P_UUID).HasComment("負能量語錄UUID");
            entity.Property(e => e.U_Sort)
                .ValueGeneratedOnAdd()
                .HasComment("排序");
            entity.Property(e => e.U_UUID).HasComment("使用者UUID");
            entity.Property(e => e.UpdateTime).HasComment("更新時間");
            entity.Property(e => e.Updator).HasComment("更新人");

            entity.HasOne(d => d.P_UU).WithMany(p => p.User_UserPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserPost_Post_Post");

            entity.HasOne(d => d.U_UU).WithMany(p => p.User_UserPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserPost_User_UserAccount");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}