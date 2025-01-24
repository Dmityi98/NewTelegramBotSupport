namespace Bot.Logic.Builder
{
    public class WorkFileBuilder
    {
        private ReportBuilder _reportBuilder;
        public WorkFileBuilder(ReportBuilder reportBuilder)
            { _reportBuilder = reportBuilder; }

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
            Console.WriteLine(value.Count);
            var report = string.Join("\n", value.Select(n => n.NameTeacher));
            return report;
        }
        public string ReportAttendence(string filePath)
        {
            _reportBuilder.FileExcelReadAttendance(filePath);
            var value = _reportBuilder.ReturnTeacherAttendance();
            _reportBuilder.ClearDataModels();
            var report = string.Join("\n", value.Select(n => n.nameTeacher));
            return report;
        }
        public string ReportErrorIssuaedCompletedMonth(string filePath)
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
            _reportBuilder.ClearDataModels();
            var report =  string.Join("\n", value.Select(n => n.nameTeacher).Distinct());
            return report;
        }
        public string ReportErrorStudentHomework(string filePath)
        {
            _reportBuilder.FileExcelReadStudentHomework(filePath);
            var value = _reportBuilder.ReportSutedentHomework();
            _reportBuilder.ClearDataModels();
            var report = string.Join("\n", value.Select(n => n.Name));
            return report;
        }
        public string ReportErrorStudendAverage(string filepath)
        {
            _reportBuilder.FileExcelReadHomework(filepath);
            var value = _reportBuilder.ReturnStudentАverageScore();
            _reportBuilder.ClearDataModels();
            var report = string.Join("\n", value.Select(n => n.NameStudent));
            return report;
        }
    }
}
