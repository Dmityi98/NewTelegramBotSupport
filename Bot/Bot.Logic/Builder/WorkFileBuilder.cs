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

        public  string CookExel1(string filePath)
        {
            
            _reportBuilder.FileExcelRead(filePath);
            var value = _reportBuilder.PercentageOfHomeworkCompleted();
            var report = string.Join("\n", value.Select(n => n.NameTeacher));
            return report; // Сделать обнуление строки 
        }
        public string CookExel2(string filePath)
        {
            _reportBuilder.FileExcelRead(filePath);
            var value = _reportBuilder.PercentageOfHomeworkCompleted2();
            var report = string.Join("\n", value.Select(n => n.NameTeacher));
            return report;
        }

        public string CookExel3(string filePath)
        {
            _reportBuilder.FileExcelReadTopic(filePath);
            var value = _reportBuilder.ReturnNameTopic();
            var report = string.Join("\n", value.Select(n => n.nameTeacher).Distinct());
            return report;
        }
    }
}
