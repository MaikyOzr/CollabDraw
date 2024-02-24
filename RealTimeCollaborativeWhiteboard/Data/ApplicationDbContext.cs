﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeCollaborativeWhiteboard.Models;

namespace RealTimeCollaborativeWhiteboard.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<Notes> Notes { get; set; }

        public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<AppUser>
        {
            public void Configure(EntityTypeBuilder<AppUser> builder)
            {
                builder.Property(u => u.Name);
            }
        }
    }
}
