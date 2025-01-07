using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core.Models
{
    public class Teacher
    {
        public string NameTeacher { get; set; } = string.Empty;
        public List<int> ValueTeacher { get; set; } = new List<int>();
    }
}
