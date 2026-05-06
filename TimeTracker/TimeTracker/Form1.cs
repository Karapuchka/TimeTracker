using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTracker
{
    public partial class MainForm : Form
    {
        private List<WorkRecord> records = new List<WorkRecord>();
        private int nextId = 1;

        public MainForm()
        {
            InitializeComponent();
            cmbProject.Items.AddRange(new string[] { "Проект А", "Проект Б", "Проект В", "Общее" });
            cmbProject.SelectedIndex = 0;
        }

        // Обновить таблицу
        private void RefreshGrid()
        {
            dgvRecords.DataSource = null;
            dgvRecords.DataSource = records;
        }

        // Кнопка "Добавить"
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFIO.Text))
            {
                MessageBox.Show("Введите ФИО сотрудника!");
                return;
            }
            records.Add(new WorkRecord
            {
                Id = nextId++,
                EmployeeName = txtFIO.Text,
                Date = dtpDate.Value,
                Hours = double.TryParse(txtHours.Text, out double h) ? h : 0,
                Project = cmbProject.SelectedItem.ToString(),
                Description = txtDescription.Text
            });
            RefreshGrid();
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtFIO.Clear();
            txtHours.Clear();
            txtDescription.Clear();
        }

        //Кнопка "Удалить"
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRecords.CurrentRow != null)
            {
                var rec = dgvRecords.CurrentRow.DataBoundItem as WorkRecord;
                records.Remove(rec);
                RefreshGrid();
            }
        }

        // Кнопка "Сохранить в файл"
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileManager.SaveToCsv(records, sfd.FileName);
                MessageBox.Show("Файл сохранен!");
            }
        }

        // Кнопка "Загрузить из файла"
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "CSV (*.csv)|*.csv" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                records = FileManager.LoadFromCsv(ofd.FileName);
                nextId = records.Count > 0 ? records.Max(r => r.Id) + 1 : 1;
                RefreshGrid();
            }
        }

        // Кнопка "Анализ текста" (морфология)
        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (records.Count == 0)
            {
                MessageBox.Show("Нет данных для анализа!");
                return;
            }

            Dictionary<string, int> freqDict = MorphologyAnalyzer.BuildFrequencyDictionary(records);

            SaveFileDialog sfd = new SaveFileDialog { Filter = "Словарь (*.csv)|*.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileManager.SaveDictionary(freqDict, sfd.FileName);
                MessageBox.Show($"Словарь сохранен! Слов: {freqDict.Count}");
            }
        }

        // Кнопка "Отчет"
        private void btnReport_Click(object sender, EventArgs e)
        {
            string report = "ОТЧЕТ ПО СОТРУДНИКАМ:\n\n";
            var groups = records.GroupBy(r => r.EmployeeName);
            foreach (var g in groups)
            {
                double total = g.Sum(r => r.Hours);
                report += $"{g.Key}: {total} часов, записей: {g.Count()}\n";
            }
            MessageBox.Show(report, "Отчет");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void menuLoad_Click(object sender, EventArgs e)
        {
            btnLoad_Click(sender, e);
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuShowReport_Click(object sender, EventArgs e)
        {
            btnReport_Click(sender, e);
        }

        private void menuDictionary_Click(object sender, EventArgs e)
        {
            btnAnalyze_Click(sender, e);
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "TimeTracker v1.0\n" +
                "Система учета рабочего времени\n\n" +
                "Разработано в рамках контрольной работы\n" +
                "по дисциплине ТППОИС, 2026 г.",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
