using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core.Models.Task_5
{
    public class Student
    {
        public string NameStudent { get; set; } = string.Empty;
        public int Homework { get; set; }
        
        public int Classroom { get; set; }
        public int Attendence { get; set; }
    }
}
