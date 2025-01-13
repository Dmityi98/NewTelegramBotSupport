using Aspose.Cells;
using Bot.Core.Models;
using System.Collections.Generic;

namespace Bot.Logic.Builder
{
    public class ReportBuilder : IReportBuilderInterface
    {
        public TeacherList TList = new TeacherList();
        /// <summary>
        /// Чтение файла
        /// </summary>
        /// <param name="filePath"></param>
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

        // Возвращает  список преподователей у которых проверка дз меньше 75%
        public List<Teacher> PercentageOfHomeworkCompleted() 
        {
            var list = new List<Teacher>();
            foreach(var item in TList.TeachersList)
            {
                if ((item.ValueTeacher[2] == 0) || (item.ValueTeacher[3]) == 0)
                {
                    list.Add(item);
                }
                else if (((item.ValueTeacher[3] / item.ValueTeacher[2]) * 100) <= 75)
                {
                    list.Add (item);
                }
            }   
            return list;  
        }
        

        
    }
}
