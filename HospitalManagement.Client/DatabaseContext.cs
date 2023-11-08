using HospitalManagement.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Client
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        #region DBSets
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>(x =>
            {
                x.HasMany(r => r.Users).WithOne(u => u.Role).HasForeignKey(u => u.RoleId);
                x.HasData(
                    new Role { Id = 1, Name = "Admin" },
                    new Role { Id = 2, Name = "Doctor" },
                    new Role { Id = 3, Name = "User" }
                );
            });

            modelBuilder.Entity<User>(x =>
            {
                x.HasOne(r => r.Role).WithMany(u => u.Users).HasForeignKey(u => u.RoleId);
                x.HasData(
                    new User
                    {
                        Id = 1,
                        Email = "duyadmin@gmail.com",
                        Password = "Travel#123",
                        FullName = "Duy Admin",
                        RoleId = 1,
                        IsEmailVerified = true
                    }, new User
                    {
                        Id = 2,
                        Email = "duydoctor@gmail.com",
                        Password = "Travel#123",
                        FullName = "Duy Doctor",
                        RoleId = 2,
                        IsEmailVerified = true
                    }, new User
                    {
                        Id = 3,
                        Email = "duyuser@gmail.com",
                        Password = "Travel#123",
                        FullName = "Duy User",
                        RoleId = 3,
                        IsEmailVerified = true
                    });
            });

            modelBuilder.Entity<Hospital>(x =>
            {
                x.HasMany(r => r.Doctors).WithOne(u => u.Hospital).HasForeignKey(u => u.HospitalId);
                x.HasData(
                    new Hospital { Id = 1, Name = "Bệnh viện Đa khoa Quốc tế Vinmec Times City", Address = "458 Minh Khai, Hai Bà Trưng, Hà Nội" },
                    new Hospital { Id = 2, Name = "Bệnh viện Đa khoa Quốc tế Vinmec Hạ Long", Address = "Đường 25/4, P. Bãi Cháy, TP. Hạ Long, Quảng Ninh" },
                    new Hospital { Id = 3, Name = "Bệnh viện Đa khoa Quốc tế Vinmec Nha Trang", Address = "42 đường 23/10, P. Phước Tiến, TP. Nha Trang, Khánh Hòa" },
                    new Hospital { Id = 4, Name = "Bệnh viện Đa khoa Quốc tế Vinmec Phú Quốc", Address = "Đường 30/4, P. Dương Đông, TP. Phú Quốc, Kiên Giang" }
                    );
            });

            modelBuilder.Entity<Doctor>(x =>
            {
                x.HasOne(r => r.Hospital).WithMany(u => u.Doctors).HasForeignKey(u => u.HospitalId);
                x.HasData(new Doctor { Id = 1, UserId = 2, Description = "Bác sĩ chuyên khoa nhi", HospitalId = 1 });
            });

            modelBuilder.Entity<Appointment>(x =>
            {
                x.HasOne(r => r.User).WithMany(u => u.Appointments).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(r => r.Doctor).WithMany(u => u.Appointments).HasForeignKey(u => u.DoctorId);
            });
        }
    }
}
