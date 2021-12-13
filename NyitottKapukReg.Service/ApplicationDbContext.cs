using Microsoft.EntityFrameworkCore;
using NyitottKapukReg.Service.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace NyitottKapukReg.Service
{
    public class ApplicationDbContext: DbContext
    {
        private readonly string connectionString;

        public DbSet<Registration> Registrations { get; set; }

        public ApplicationDbContext()
        {
#if DEBUG        
            connectionString = "Server=localhost;Database=NyitottKapukRegisztracio;Uid=root;Pwd=;";
#endif
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        {
            //https://stackoverflow.com/questions/33127296/how-to-get-connectionstring-from-ef7-dbcontext
            var ext = options.FindExtension<MySqlOptionsExtension>();
            connectionString = ext.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<VisitorGroup>().HasIndex(v => v.GroupNumber).IsUnique();

            modelBuilder.Entity<VisitorGroup>().HasData(
                new VisitorGroup() { Id = 1, GroupNumber = 1, ClassroomNumber = "110", Day = null },
                new VisitorGroup() { Id = 2, GroupNumber = 2, ClassroomNumber = "110", Day = null },
                new VisitorGroup() { Id = 3, GroupNumber = 3, ClassroomNumber = "110", Day = null },
                new VisitorGroup() { Id = 4, GroupNumber = 4, ClassroomNumber = "110", Day = null },
                new VisitorGroup() { Id = 5, GroupNumber = 5, ClassroomNumber = "106", Day = null },
                new VisitorGroup() { Id = 6, GroupNumber = 6, ClassroomNumber = "112", Day = null },
                new VisitorGroup() { Id = 7, GroupNumber = 7, ClassroomNumber = "205", Day = null },
                new VisitorGroup() { Id = 8, GroupNumber = 8, ClassroomNumber = "207", Day = null },
                new VisitorGroup() { Id = 9, GroupNumber = 9, ClassroomNumber = "208", Day = null },
                new VisitorGroup() { Id = 10, GroupNumber = 10, ClassroomNumber = "209", Day = null },
                new VisitorGroup() { Id = 11, GroupNumber = 11, ClassroomNumber = "210", Day = null },
                new VisitorGroup() { Id = 12, GroupNumber = 12, ClassroomNumber = "212", Day = null }
            );

            modelBuilder.Entity<Day>().HasData(
                new Day() {Id = 1, Date=new DateTime(2022, 5, 10), MaxVisitors = 192}
            );

        }
    }
}
