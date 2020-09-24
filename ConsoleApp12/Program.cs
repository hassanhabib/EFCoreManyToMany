using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp12
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid studentId = Guid.NewGuid();
            Guid teacherId = Guid.NewGuid();

            var student = new Student
            {
                Id = studentId,
                Name = "Anas",
                StudentTeachers = new List<StudentTeacher>
                {
                    new StudentTeacher
                    {
                        StudentId = studentId,
                        TeacherId = teacherId,
                        Teacher = new Teacher
                        {
                            Id = teacherId,
                            Name = "Hassan"
                        }
                    }
                }
            };

            var dbContext = new DataContext();
            dbContext.Students.Add(student);
            dbContext.SaveChanges();

            Student storageStudent = 
                dbContext.Students.Find(studentId);

            Console.WriteLine(storageStudent.Name);
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
