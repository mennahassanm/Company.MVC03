﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Company.MVC.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.MVC.DAL.Data.Contexts
{
    public class CompanyDbContext : DbContext
    {
        // CLR

        public CompanyDbContext(DbContextOptions<CompanyDbContext>options) : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = .; Database = Company; Trusted_Connection = True; TrustServerCertificate = True");
        //}
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
