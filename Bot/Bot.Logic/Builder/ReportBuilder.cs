using Aspose.Cells;
using Bot.Core.Models.Task_5;
using Bot.Core.Models.Task1;
using Bot.Core.Models.Task3;
using Bot.Core.Models.Task6;
using System.Collections.Generic;
using Bot;
using System.Text.RegularExpressions;

namespace Bot.Logic.Builder
{
    public class ReportBuilder : IReportBuilderInterface
    {
        public TeacherList TList = new TeacherList();
        public TopicList LessonTopics = new TopicList();
        public SrudentList StudentLists = new SrudentList();
        public StudentLists StudentHomework= new StudentLists();
        public void FileExcelRead(string filePath)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(filePath);

            WorksheetCollection collection = wb.Worksheets;

            for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
            {

                // Получить рабочий лист, используя его индекс
                Aspose.Cells.Worksheet worksheet = collection[worksheetIndex];

                // Получить количество строк и столбцов
                int rows = worksheet.Cells.MaxDataRow;
                int cols = worksheet.Cells.MaxDataColumn;

                // Цикл по строкам
                for (int i = 2; i < rows +1; i++)
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
                        teather.ValueTeacher.Add(Convert.ToDouble(worksheet.Cells[i, j + 1].Value));
                    }
                    TList.TeachersList.Add(teather);
                }
            }
        }
        public void FileExcelReadTopic(string filePath)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(filePath);


            WorksheetCollection collection = wb.Worksheets;

            for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
            {
                Aspose.Cells.Worksheet worksheet = collection[worksheetIndex];

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
        public void FileExcelReadAttendance(string filePath)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(filePath);

            WorksheetCollection collection = wb.Worksheets;

            for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
            {
                Aspose.Cells.Worksheet worksheet = collection[worksheetIndex];

                int rows = worksheet.Cells.MaxDataRow;
                int cols = worksheet.Cells.MaxDataColumn - 1;

                for (int i = 2; i < rows; i++)
                {
                    var teacherTopic = new LessonTopic();
                    teacherTopic.nameTeacher = worksheet.Cells[i, 0].Value.ToString();

                    teacherTopic.Topic = Regex.Replace(worksheet.Cells[i, 4].Value.ToString(), "%", "");
                    LessonTopics.LessonTopic.Add(teacherTopic);
                }
            }
        }
        public void FileExcelReadStudentHomework(string filePath)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(filePath);

            WorksheetCollection collection = wb.Worksheets;

            for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
            {
                Aspose.Cells.Worksheet worksheet = collection[worksheetIndex];

                int rows = worksheet.Cells.MaxDataRow;
                int cols = worksheet.Cells.MaxDataColumn - 1;

                for (int i = 1; i < rows; i++)
                {
           
                    var student = new StudentHomework();
                    student.Name = worksheet.Cells[i, 0].Value.ToString();

                    student.PercentageHomework = int.Parse(worksheet.Cells[i, 19].Value.ToString());
                    StudentHomework.StudentsHomework.Add(student);
                }
            }
        }
        public List<StudentHomework> ReportSutedentHomework()
        {
            var list = new List<StudentHomework>();

            foreach(var item in StudentHomework.StudentsHomework)
            {
                if(item.PercentageHomework <=50)
                {
                    list.Add(item);
                }
            }
            return list;
        }
        public void FileExcelReadHomework(string filePath)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(filePath);

            WorksheetCollection collection = wb.Worksheets;

            for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
            {
                Aspose.Cells.Worksheet worksheet = collection[worksheetIndex];

                int rows = worksheet.Cells.MaxDataRow;
                int cols = worksheet.Cells.MaxDataColumn;

                for (int i = 1; i < rows; i++)
                {

                    var student = new Student();
                    student.NameStudent = worksheet.Cells[i, 0].Value.ToString();
                    student.Homework = int.Parse(worksheet.Cells[i, 15].Value.ToString());
                    student.Classroom = int.Parse(worksheet.Cells[i, 16].Value.ToString());
                    string attendance = worksheet.Cells[i, 16].Value.ToString();
                    if (attendance.StartsWith("-") != true)
                        student.Attendence = int.Parse(attendance);
                    StudentLists.Students.Add(student);
                }
            }
        }

        public List<Teacher> PercentageOfIssuaedCompletedMonth()
        {
            var list = new List<Teacher>();
            foreach (var item in TList.TeachersList)
            {
                if ((item.ValueTeacher[1] == 0) || (item.ValueTeacher[4]) == 0)
                {
                    list.Add(item);
                }
                else if (((item.ValueTeacher[1] / item.ValueTeacher[4]) * 100) <= 70)
                {

                    list.Add(item);
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


        public List<LessonTopic> ReturnTeacherAttendance()
        {
            var list = new List<LessonTopic>();

            foreach (var item in LessonTopics.LessonTopic)
            {
                if (double.TryParse(item.Topic, out double attendanceValueDouble))
                {
                    int attendanceValue = (int)Math.Floor(attendanceValueDouble);
                    if (attendanceValue < 65) 
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
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
                    list.Add(item);
                }
            }   
            return list;  
        }

        public List<Student> ReturnStudentHomework()
        {
            var list = new List<Student>();
            foreach(var item in StudentLists.Students)
            {
                if(item.Homework <=3 || item.Classroom <= 3 || item.Attendence <= 3 )
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
            StudentHomework.StudentsHomework.Clear();
            StudentLists.Students.Clear();
        }
    }
}
