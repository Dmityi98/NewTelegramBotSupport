using Aspose.Cells;
using Bot.Core.Models;
using System.Collections.Generic;

namespace Bot.Logic.Builder
{
    public class ReportBuilder : ReportBuilderInterface
    {
        private Report _report;
        public TeacherList TList = new TeacherList();

        public ReportBuilder()
        {
            _report = new Report();
        }
        public void FileExcelRead(string filePath)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(filePath);

            // Получить все рабочие листы
            WorksheetCollection collection = wb.Worksheets;

            // Перебрать все рабочие листы
            for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
            {

                // Получить рабочий лист, используя его индекс
                Aspose.Cells.Worksheet worksheet = collection[worksheetIndex];

                // Получить количество строк и столбцов
                int rows = worksheet.Cells.MaxDataRow;
                int cols = worksheet.Cells.MaxDataColumn;

                // Цикл по строкам
                for (int i = 2; i < rows; i++)
                {
                    var teather = new Teacher();
                    teather.ValueTeacher.Clear();
                    teather.NameTeacher = worksheet.Cells[i, 1].Value.ToString();
                    // Перебрать каждый столбец в выбранной строке
                    for (int j = 1; j < cols; j++)
                    {
                        teather.ValueTeacher.Add(Convert.ToInt32(worksheet.Cells[i, j + 1].Value));
                        if (worksheet.Cells[i, j + 1].Value == null)
                        {
                            break;
                        }
                    }
                    TList.TeachersList.Add(teather);
                }
            }
        }

        public void PercentageOfHomeworkCompleted()
        {
            
        }
        public List<Teacher> GetTeacherList()
        {
            return TList.TeachersList;
        }

        
    }
}
