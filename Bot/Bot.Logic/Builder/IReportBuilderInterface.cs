using Bot.Core.Models.Task1;

namespace Bot.Logic.Builder
{
    public interface IReportBuilderInterface
    {
        public void FileExcelRead(string filePath);
        public List<Teacher> PercentageOfHomeworkCompleted();
    }
}
