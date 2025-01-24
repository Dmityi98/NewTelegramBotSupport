using System.ComponentModel.DataAnnotations;

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
