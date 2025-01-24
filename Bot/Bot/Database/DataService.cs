using Bot.Core;
using Microsoft.EntityFrameworkCore;

namespace Bot.Database
{
    public class DataService
    {
        private readonly DbContextBot _context;
        public DataService(DbContextBot context)
        {
            _context = context;
        }   

        public List<TeachersDb> GetTeachers()
        {
            return _context.TechersDb.ToList();
        }
        public async Task AddTeacherAsync(string name, long chatId)
        {
            var newTeacher = new TeachersDb { NameTeacher = name, ChatIdTeacher = chatId };
            _context.TechersDb.Add(newTeacher);
            await _context.SaveChangesAsync();
        }
        public async Task<TeachersDb?> GetTeacherByChatIdAsync(long chatId)
        {
            return await _context.TechersDb.FirstOrDefaultAsync(x => x.ChatIdTeacher == chatId);
        }
        public async Task<TeachersDb?> GetChatIdTechers(long chatId)
        {
            return await _context.TechersDb.FirstOrDefaultAsync(x => x.ChatIdTeacher == chatId);
        }
        public async Task<long?> GetChatId(string nameTecher)
        {
            var teacher = await _context.TechersDb.FirstOrDefaultAsync(x => x.NameTeacher == nameTecher);
            return teacher?.ChatIdTeacher;
        }
        public async Task<TeachersDb?> GetNameTeachers(string name)
        {
            return await _context.TechersDb.FirstOrDefaultAsync(x => x.NameTeacher == name);
        }
    }
}
