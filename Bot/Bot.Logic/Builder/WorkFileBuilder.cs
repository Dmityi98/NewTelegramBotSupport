using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string ReportAttendence(string filePath)
        {
            _reportBuilder.FileExcelReadAttendance(filePath);
            var value = _reportBuilder.ReturnTeacherAttendance();
            var report = string.Join("\n", value.Select(n => n.nameTeacher));
            return report;
        }
        public string ReportErrorMonth2(string filePath)
        {
            _reportBuilder.FileExcelRead(filePath);
            var value = _reportBuilder.PercentageOfIssuaedCompletedMonth();
            var report = string.Join("\n", value.Select(n => n.NameTeacher));
            _reportBuilder.ClearDataModels();
            return report;
        }
        public string ReportErrorTopic(string filePath)
        {
            _reportBuilder.FileExcelReadTopic(filePath);
            var value = _reportBuilder.ReturnNameTopic();
            var report =  string.Join("\n", value.Select(n => n.nameTeacher).Distinct());
            return report;
        }

        public string reportErrorHomework(string filepath)
        {
            _reportBuilder.FileExcelReadHomework(filepath);
            var value = _reportBuilder.ReturnStudentHomework();
            var report = string.Join("\n", value.Select(n => n.NameStudent));
            return report;
        }
    }
}
