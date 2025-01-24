using Bot.Core.Models.Task_5;
using Bot.Core.Models.Task1;
using Bot.Core.Models.Task3;
using Bot.Core.Models.Task6;

namespace Bot.Logic.Builder
{
    public interface IReportBuilderInterface
    {
        public void FileExcelRead(string filePath);
        public void FileExcelReadTopic(string filePath);
        public void FileExcelReadAttendance(string filePath);
        public void FileExcelReadStudentHomework(string filePath);
        public void FileExcelReadHomework(string filePath);
        public List<StudentHomework> ReportSutedentHomework();
        public List<Teacher> PercentageOfIssuaedCompletedMonth();
        public List<Teacher> PercentageOfHomeworkCompletedWeek();
        public List<LessonTopic> ReturnTeacherAttendance();
        public List<Teacher> PercentageOfHomeworkCompletedMonth();
        public List<Student> ReturnStudentАverageScore();
        public List<LessonTopic> ReturnNameTopic();
        public void ClearDataModels();
    }
}
