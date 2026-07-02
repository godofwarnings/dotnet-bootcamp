using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7
{
    public class AdmissionRepository
    {
        public List<Admission> admissions = new List<Admission>();

        public void SaveAdmission(Admission admission)
        {
            admissions.Add(admission);
        }
    }
}
