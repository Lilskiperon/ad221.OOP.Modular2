using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using word = Microsoft.Office.Interop.Word;

namespace EnergyApplication
{
    public partial class exportForm : Form
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection cnn = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        System.Data.DataTable dt = new System.Data.DataTable();

        public exportForm()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }
        private void InitializeDatabaseConnection()
        {
            cnn.ConnectionString = "Data Source=(local);Initial Catalog=EnergyConsumptionInApartments;Integrated Security=True";
            cmd.Connection = cnn;
            da.SelectCommand = cmd;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Вправа 3 Завдання 1");
            comboBox1.Items.Add("Вправа 3 Завдання 2");
            comboBox1.Items.Add("Вправа 3 Завдання 3");
            comboBox1.Items.Add("Вправа 4 Завдання 1");
            comboBox1.Items.Add("Вправа 4 Завдання 2");
            comboBox1.Items.Add("Вправа 4 Завдання 3");
            comboBox1.Items.Add("Вправа 5 Завдання 1");
            comboBox1.Items.Add("Вправа 5 Завдання 2");
            comboBox1.Items.Add("Вправа 5 Завдання 3");
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt.Rows.Clear();
            dt.Columns.Clear();
            switch (comboBox1.Text)
            {
                case "Вправа 3 Завдання 1":
                    textBox1.Text = "Виведення адрес даних про будинки, що знаходяться в Малиновському районі.";
                    cmd.CommandText = "SELECT * From Addresses Where District like '%Малиновський%';";
                    break;
                case "Вправа 3 Завдання 2":
                    textBox1.Text = "Виведення усіх даних про будинки, збудовані після 2000 року.";
                    cmd.CommandText = "SELECT * FROM Buildings WHERE Year > 2000;";
                    break;
                case "Вправа 3 Завдання 3":
                    textBox1.Text = "Отримати дані про будинки в яких є ліфт.";
                    cmd.CommandText = "SELECT Buildings.HouseID, Buildings.Year, Buildings.Material, Buildings.EnergyRating FROM Buildings WHERE Elevator=1;";
                    break;
                case "Вправа 4 Завдання 1":
                    textBox1.Text = "Показати кількість даних квартир на кожному поверсі будинку.";
                    cmd.CommandText = "SELECT HouseID, Floor, COUNT(FlatID) AS NumberOfFlats FROM Flats GROUP BY HouseID, Floor ORDER BY HouseID;";
                    break;
                case "Вправа 4 Завдання 2":
                    textBox1.Text = "Вивести інформацію про кількість квартир за кількістю мешканців, які в ній проживають.";
                    cmd.CommandText = "SELECT Residents, COUNT(FlatID) AS NumberOfFlats FROM Flats Where Residents is not Null GROUP BY Residents ORDER BY Residents;";
                    break;
                case "Вправа 4 Завдання 3":
                    textBox1.Text = "Підрахувати загальні витрати на енергоресурси для кожної квартири.";
                    cmd.CommandText = "SELECT Flats.FlatID, Flats.Rooms, Flats.Area, Flats.Residents, SUM(ElectricityUsage.ElectricityCost+WaterUsage.WaterCost+GasUsage.GasCost+HeatingUsage.HeatingCost) as TotalCost FROM Flats INNER JOIN Expenses ON Flats.FlatID = Expenses.FlatID LEFT JOIN ElectricityUsage ON Expenses.ElectricityUsageID = ElectricityUsage.UsageID LEFT JOIN WaterUsage ON Expenses.WaterUsageID = WaterUsage.UsageID LEFT JOIN GasUsage ON Expenses.GasUsageID = GasUsage.UsageID LEFT JOIN HeatingUsage ON Expenses.HeatingUsageID = HeatingUsage.UsageID GROUP BY Flats.FlatID, Flats.Rooms, Flats.Area, Flats.Residents;";
                    break;
                case "Вправа 5 Завдання 1":
                    textBox1.Text = "Додати до завдання 4.3 інформацію про тип системи опалення для кожної квартири.";
                    cmd.CommandText = "SELECT Flats.FlatID, Flats.Rooms, Flats.Area, Flats.Residents, SUM(ElectricityUsage.ElectricityCost + WaterUsage.WaterCost + GasUsage.GasCost + HeatingUsage.HeatingCost) as TotalCost, HeatingSystems.Type AS HeatingSystemType FROM Flats INNER JOIN Expenses ON Flats.FlatID = Expenses.FlatID LEFT JOIN ElectricityUsage ON Expenses.ElectricityUsageID = ElectricityUsage.UsageID LEFT JOIN WaterUsage ON Expenses.WaterUsageID = WaterUsage.UsageID LEFT JOIN GasUsage ON Expenses.GasUsageID = GasUsage.UsageID LEFT JOIN HeatingUsage ON Expenses.HeatingUsageID = HeatingUsage.UsageID LEFT JOIN Buildings ON Flats.HouseID = Buildings.HouseID LEFT JOIN HeatingSystems ON Buildings.HeatingSystemID = HeatingSystems.HeatingSystemID GROUP BY Flats.FlatID, Flats.Rooms, Flats.Area, Flats.Residents,Buildings.HouseID, HeatingSystems.Type;";
                    break;
                case "Вправа 5 Завдання 2":
                    textBox1.Text = "Отримати список всіх будинків разом з даними про адресу та про систему отоплення.";
                    cmd.CommandText = "SELECT Buildings.HouseID, Addresses.Address, Addresses.District,HeatingSystems.Type,Buildings.EnergyRating ,Buildings.Year, Buildings.Material FROM Buildings INNER JOIN Addresses ON Buildings.HouseID = Addresses.HouseID INNER JOIN HeatingSystems ON Buildings.HeatingSystemId = HeatingSystems.HeatingSystemID;";
                    break;
                case "Вправа 5 Завдання 3":
                    textBox1.Text = "Вивести інформацію про квартири, де використовують центральну систему опалення.";
                    cmd.CommandText = "SELECT Flats.FlatID, Flats.Rooms,  Flats.Area, Flats.Residents, Buildings.HouseID, HeatingSystems.Type AS HeatingSystemType FROM Flats LEFT JOIN Buildings ON Flats.HouseID = Buildings.HouseID LEFT JOIN HeatingSystems ON Buildings.HeatingSystemID = HeatingSystems.HeatingSystemID WHERE HeatingSystems.Type like '%Центральне%' GROUP BY Flats.FlatID, Flats.Rooms, Flats.Area, Flats.Residents, Buildings.HouseID, HeatingSystems.Type;";
                    break;
                default:
                    break;
            }
            try
            {
                cnn.Open();
                dt.Clear();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "Вправа 5 Завдання 2")
            {
                MessageBox.Show("Оберіть <Вправу 5 Завдання 2>");
                return;
            }
            else
            {
                PrintToWord();
            }

        }
        private void PrintToWord()
        {
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = null;
            string pathToExe = Application.StartupPath;
            doc = app.Documents.Open(pathToExe + "\\sampleTask5_2.docx");
            doc.Activate();

            Microsoft.Office.Interop.Word.Words wrds = doc.Words;
            Microsoft.Office.Interop.Word.Range wRange;
            wRange = wrds[11];
            wRange.InsertAfter(DateTime.Today.Date.ToString("d"));
            Microsoft.Office.Interop.Word.Bookmarks wBookmarks = doc.Bookmarks;
            Microsoft.Office.Interop.Word.Bookmark mark;
            mark = wBookmarks["kod_House"];
            wRange = mark.Range;
            wRange.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString().Trim();
            mark = wBookmarks["address_House"];
            wRange = mark.Range;
            wRange.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString().Trim();
            mark = wBookmarks["kod1_House"];
            wRange = mark.Range;
            wRange.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString().Trim();
            mark = wBookmarks["fillTable"];
            wRange = mark.Range;
            wRange.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            Microsoft.Office.Interop.Word.Table tab1 = doc.Tables[1];
            for (int i = 2; i < 6; ++i)
            {
                wRange = tab1.Cell(2, i).Range;
                wRange.Text = dataGridView1.CurrentRow.Cells[i+1].Value.ToString();
            }

            app.Visible = true;
        }


    }
}
