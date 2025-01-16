using Bot.Core.Models.Task1;
using Bot.Core.Models.Task3;

namespace Bot.Logic.Builder
{
    public interface IReportBuilderInterface
    {
        public void FileExcelRead(string filePath);
        public void FileExcelReadTopic(string filePath);
        public List<Teacher> PercentageOfHomeworkCompletedMonth();
        public List<Teacher> PercentageOfHomeworkCompletedWeek();
        public List<LessonTopic> ReturnNameTopic();
        public void ClearDataModels();
    }
}
