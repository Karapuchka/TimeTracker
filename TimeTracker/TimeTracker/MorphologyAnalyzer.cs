using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    public static class MorphologyAnalyzer
    {
        // Простейший анализатор: разбить текст на слова, удалить окончания, привести к нижнему регистру
        public static Dictionary<string, int> BuildFrequencyDictionary(List<WorkRecord> records)
        {
            Dictionary<string, int> freqDict = new Dictionary<string, int>();
            char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '-', '\n', '\r', '\t' };

            foreach (var rec in records)
            {
                if (string.IsNullOrWhiteSpace(rec.Description)) continue;
                string[] words = rec.Description.ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    string stemmed = SimpleStem(word);
                    if (freqDict.ContainsKey(stemmed))
                        freqDict[stemmed]++;
                    else
                        freqDict[stemmed] = 1;
                }
            }
            return freqDict;
        }

        // Простой стемминг: обрезаем распространенные окончания
        private static string SimpleStem(string word)
        {
            if (word.Length < 4) return word; // короткие не обрабатываем
            if (word.EndsWith("ами")) return word.Substring(0, word.Length - 3);
            if (word.EndsWith("ами")) return word.Substring(0, word.Length - 3);
            if (word.EndsWith("ами")) return word.Substring(0, word.Length - 3);
            if (word.EndsWith("ами")) return word.Substring(0, word.Length - 3);
            if (word.EndsWith("ами")) return word.Substring(0, word.Length - 3);
            if (word.EndsWith("ами")) return word.Substring(0, word.Length - 3);

            if (word.EndsWith("ами")) return word.Substring(0, word.Length - 3);
            if (word.EndsWith("ами")) return word.Substring(0, word.Length - 3);
            return word;
        }
    }
}
