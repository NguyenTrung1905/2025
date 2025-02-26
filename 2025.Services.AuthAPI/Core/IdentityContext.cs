using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using _2025.Services.AuthAPI.Core.Entities;

namespace _2025.Services.AuthAPI.Core;

public partial class IdentityContext : DbContext
{

    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ModifiedOn).HasColumnType("timestamp without time zone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
