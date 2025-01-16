using Aspose.Cells;
using Bot.Core.Models.Task1;
using Bot.Core.Models.Task3;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bot.Logic.Builder
{
    public class ReportBuilder : IReportBuilderInterface
    {
        public TeacherList TList = new TeacherList();

        public TopicList LessonTopics = new TopicList();
        async public void FileExcelRead(string filePath)
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
                        if (worksheet.Cells[i, j + 1].Value == null)
                        {
                            break;
                        }
                        teather.ValueTeacher.Add(Convert.ToInt32(worksheet.Cells[i, j + 1].Value));
                    }
                    TList.TeachersList.Add(teather);
                }
            }
        }
        async public void FileExcelReadTopic(string filePath)
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

                for (int i = 2; i < rows; i++)
                {
                    var teacherTopic = new LessonTopic();
                    teacherTopic.nameTeacher = worksheet.Cells[i, 4].Value.ToString();

                    teacherTopic.Topic = Convert.ToString(worksheet.Cells[i, 5].Value);
                    LessonTopics.LessonTopic.Add(teacherTopic);
                }
            }
        }
        public List<Teacher> PercentageOfHomeworkCompletedMonth() 
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
        public List<Teacher> PercentageOfHomeworkCompletedWeek()
        {
            var list = new List<Teacher>();
            foreach (var item in TList.TeachersList)
            {
                if ((item.ValueTeacher[7] == 0) || (item.ValueTeacher[8]) == 0)
                {
                    list.Add(item);
                }
                else if (((item.ValueTeacher[8] / item.ValueTeacher[7]) * 100) <= 75)
                {
                    list.Add(item);
                }
            }
            return list;
        }
        public List<LessonTopic> ReturnNameTopic()
        {
            var list = new List<LessonTopic>();
            string wordToFind = @"Урок №\d+\ Тема:";

            foreach (var str in LessonTopics.LessonTopic)
            {
                if (!Regex.IsMatch(str.Topic, wordToFind, RegexOptions.IgnoreCase))
                {
                    list.Add(str);
                }
            }
            return list;
        }
        public void ClearDataModels()
        {
            TList.TeachersList.Clear();
            LessonTopics.LessonTopic.Clear();   
        }
    }
}
