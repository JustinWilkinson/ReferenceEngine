﻿using LatexReferences.Models.Format;
using Microsoft.EntityFrameworkCore;

namespace LatexReferences.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<BibEntryFormat> BibEntryFormats { get; set; }

        public DbSet<Style> Styles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=LatexReferences.sqlite");
        }
    }
}