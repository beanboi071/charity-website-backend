using charity_website_backend.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace charity_website_backend.DB
{
    public class CharityDbContext : DbContext
    {
        public CharityDbContext(DbContextOptions<CharityDbContext> options) : base(options) { }
        public DbSet<EAdmin> Admin { get; set; }
        public DbSet<EDonation> Donations { get; set; }
        public DbSet<EDonor> Donors { get; set; }
        public DbSet<ENGO> NGOs { get; set; }
        public DbSet<EProject> Projects { get; set; }

    }
}
