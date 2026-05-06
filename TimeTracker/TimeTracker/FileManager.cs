using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    public static class FileManager
    {
        // Сохранить все записи в CSV-файл
        public static void SaveToCsv(List<WorkRecord> records, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.WriteLine("ID;Сотрудник;Дата;Часы;Проект;Описание");
                foreach (var rec in records)
                    sw.WriteLine($"{rec.Id};{rec.EmployeeName};{rec.Date:dd.MM.yyyy};{rec.Hours};{rec.Project};{rec.Description}");
            }
        }

        // Загрузить записи из CSV-файла
        public static List<WorkRecord> LoadFromCsv(string path)
        {
            List<WorkRecord> records = new List<WorkRecord>();
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++) // пропустить заголовок
            {
                string[] parts = lines[i].Split(';');
                records.Add(new WorkRecord
                {
                    Id = int.Parse(parts[0]),
                    EmployeeName = parts[1],
                    Date = DateTime.Parse(parts[2]),
                    Hours = double.Parse(parts[3]),
                    Project = parts[4],
                    Description = parts[5]
                });
            }
            return records;
        }

        // Сохранить частотный словарь в файл
        public static void SaveDictionary(Dictionary<string, int> dict, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.WriteLine("Слово;Частота");
                foreach (var kvp in dict.OrderByDescending(x => x.Value))
                    sw.WriteLine($"{kvp.Key};{kvp.Value}");
            }
        }
    }
}
