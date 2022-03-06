using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp12
{
    public class Program
    {
        private readonly DataContext context;
        private readonly Guid anasId;

        public Program()
        {
            this.context = new DataContext();
            this.anasId = Guid.Parse("8c9b0b48-ebf1-4ae1-b454-e78c74f489c0");
        }

        [Benchmark]
        public async Task<object> WithTaskAsync() =>
           await this.context.Students.FindAsync(this.anasId);

        [Benchmark]
        public async ValueTask<object> WithValueTaskAsync() =>
            await this.context.Students.FindAsync(this.anasId);

        static void Main(string[] args)
        {
            var summary =
                BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }

    public class DataContext: DbContext
    {
        public DataContext()
        {
            this.Database.Migrate();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<StudentTeacher> StudentTeachers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentTeacher>().HasKey(sc => new { sc.TeacherId, sc.StudentId });

            modelBuilder.Entity<StudentTeacher>()
                .HasOne(studentTeacher => studentTeacher.Teacher)
                .WithMany(teacher => teacher.StudentTeachers)
                .HasForeignKey(studentTeacher => studentTeacher.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentTeacher>()
                .HasOne(studentTeacher => studentTeacher.Student)
                .WithMany(teacher => teacher.StudentTeachers)
                .HasForeignKey(studentTeacher => studentTeacher.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AnasDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }


    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<StudentTeacher> StudentTeachers { get; set; }
    }

    public class Teacher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<StudentTeacher> StudentTeachers { get; set; }
    }

    public class StudentTeacher {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
