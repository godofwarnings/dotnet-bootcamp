using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7
{
    internal class Admission
    {
        public Student Student = new Student();
        public Course Course = new Course();
        public DateTime AdmissionDate { get; set; }
    }
}
