using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core
{
    public class TeachersDb
    {
        [Key]
        public int Id { get; set; }
        public string NameTeacher {  get; set; } = string.Empty;
        public long ChatIdTeacher { get; set; } 
    }
}
