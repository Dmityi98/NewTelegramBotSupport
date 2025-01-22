using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core
{
    public class Techers
    {
        [Key]
        public int Id { get; set; }
        public string NameTeachet {  get; set; } = string.Empty;
        public string ChatIdTeacher { get; set; } = string.Empty;
    }
}
