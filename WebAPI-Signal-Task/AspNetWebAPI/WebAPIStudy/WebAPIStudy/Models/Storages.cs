using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIStudy.Models
{
    public static class Storages
    {
        static Storages()
        {
            Students = new List<Student>
            {
                new Student{ Id=1,Name="找嘿嘿1",Age=12,Gender=false},
                new Student{ Id=2,Name="找嘿嘿2",Age=12,Gender=false},
                new Student{ Id=3,Name="找嘿嘿3",Age=13,Gender=false},
                new Student{ Id=4,Name="找嘿嘿4",Age=14,Gender=false},
                new Student{ Id=5,Name="找嘿嘿5",Age=15,Gender=false},
            };
            Teachers = new List<Teacher>();
        }
        public static IEnumerable<Student> Students { get; set; }
        public static IEnumerable<Teacher> Teachers { get; set; }
    }
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
    }
    public class Student : Person
    {

    }
    public class Teacher : Person
    {
    }

}