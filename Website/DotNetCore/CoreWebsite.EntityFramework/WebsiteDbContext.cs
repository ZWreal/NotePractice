﻿using System;
using System.Collections.Generic;
using System.Text;
using CoreWebsite.EntityFramework.Models;
using CoreWebsite.EntityFramework.Models.EntityRelationTest;
using CoreWebsite.EntityFramework.Models.TreeTest;
using Microsoft.EntityFrameworkCore;

namespace CoreWebsite.EntityFramework
{
    public class WebsiteDbContext : DbContext
    {

        public WebsiteDbContext(DbContextOptions<WebsiteDbContext> options):base(options)
        {

        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityComment> ActivityComments { get; set; }
        public DbSet<AdmissionRecord> AdmissionRecords { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<StudentTeacherRelationship> StudentTeacherRelationships { get; set; }
        //public DbSet<TreeNode> TreeNodes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //导航属性
            modelBuilder.Entity<ActivityComment>()
                .HasOne(p => p.Activity)
                .WithMany(b => b.ActivityComments)
                .HasForeignKey(p => p.ActivityId);
            modelBuilder.Entity<Activity>()
                .HasMany(x => x.ActivityComments)
                .WithOne(x => x.Activity)
                .HasForeignKey(x => x.ActivityId);

            //Student-AdmissionRecord 1:1  设置ForeignKey

            //Student-Class n:1
            modelBuilder.Entity<Class>()
                .HasMany(p => p.Students)
                .WithOne(p => p.Class)
                .HasForeignKey(p => p.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            //下面写法也可以
            //modelBuilder.Entity<Student>()
            //    .HasOne(p => p.Class)
            //    .WithMany(p=>p.Students)
            //    .HasForeignKey(k => k.ClassId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            //Student-Teacher m:n 通过StudentTeacherRelationship中间表
            modelBuilder.Entity<StudentTeacherRelationship>()
                .HasOne(p => p.Student)
                .WithMany(p => p.StudentTeacherRelationships)
                .HasForeignKey(k => k.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<StudentTeacherRelationship>()
                .HasOne(p => p.Teacher)
                .WithMany(p => p.StudentTeacherRelationships)
                .HasForeignKey(k => k.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //单表树状结构
            //modelBuilder.Entity<TreeNode>()
            //    .HasMany(x => x.Children)
            //    .WithOne(x => x.Parent)
            //    .HasForeignKey(x=>x.ParentId);
        }
    }
}
