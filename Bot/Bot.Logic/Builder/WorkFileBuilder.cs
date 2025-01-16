using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Logic.Builder
{
    public class WorkFileBuilder
    {
        private ReportBuilder _reportBuilder = new ReportBuilder();

        public void SetBuilder(ReportBuilder builder)
        {
            _reportBuilder = builder;
        }
        public string ReportErrorMonth(string filePath)
        {
            _reportBuilder.FileExcelRead(filePath);
            var value = _reportBuilder.PercentageOfHomeworkCompletedMonth();
            var report = string.Join("\n", value.Select(n => n.NameTeacher));
            _reportBuilder.ClearDataModels();
            return report;
        }
        public string ReportErrorWeek(string filePath)
        {
            _reportBuilder.FileExcelRead(filePath);
            var value = _reportBuilder.PercentageOfHomeworkCompletedWeek();
            _reportBuilder.ClearDataModels();
            var report = string.Join("\n", value.Select(n => n.NameTeacher));
            return report;
        }

        public string ReportErrorTopic(string filePath)
        {
            _reportBuilder.FileExcelReadTopic(filePath);
            var value = _reportBuilder.ReturnNameTopic();
            var report =  string.Join("\n", value.Select(n => n.nameTeacher).Distinct());
            return report;
        }
    }
}
