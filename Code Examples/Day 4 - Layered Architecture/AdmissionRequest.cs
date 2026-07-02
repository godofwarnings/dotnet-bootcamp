using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleApp7
{
    public class AdmissionRequest
    {
        public string StudentName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string CourseName {  get; set; } = string.Empty;
    }
}
