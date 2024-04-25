using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace EnergyApplication
{
    
    public partial class apartmentRegisterForm : Form
    {
        [Test]
        public void TextBox_KeyPress_AllowDigitOrComma()
        {
            
            apartmentRegisterForm form = new apartmentRegisterForm();
            KeyPressEventArgs args = new KeyPressEventArgs('5');

            
            form.TextBox_KeyPress(null, args);

            Assert.IsFalse(args.Handled);
        }

        [Test]
        public void TextBox_KeyPress_BlockNonDigitOrComma()
        {
           
            apartmentRegisterForm form = new apartmentRegisterForm();
            KeyPressEventArgs args = new KeyPressEventArgs('a');

            form.TextBox_KeyPress(null, args);

            Assert.IsTrue(args.Handled);
        }

        [Test]
        public void TextBox_KeyPressValidation_AllowDigit()
        {
          
            apartmentRegisterForm form = new apartmentRegisterForm();
            KeyPressEventArgs args = new KeyPressEventArgs('5');

         
            form.TextBox_KeyPressValidation(null, args);

       
            Assert.IsFalse(args.Handled);
        }

        [Test]
        public void TextBox_KeyPressValidation_BlockNonDigit()
        {
            
            apartmentRegisterForm form = new apartmentRegisterForm();
            KeyPressEventArgs args = new KeyPressEventArgs('a');

            form.TextBox_KeyPressValidation(null, args);

          
            Assert.IsTrue(args.Handled);
        }

        [Test]
        public void isEmpty_AllFieldsFilled_ReturnsFalse()
        {
         
            apartmentRegisterForm form = new apartmentRegisterForm();
            form.comboBoxHouseID.Text = "1";
            form.comboBoxFloor.Text = "2";
            form.Rooms.Text = "3";
            form.Area.Text = "100";
            form.Electricity.Text = "50";
            form.ElectricityCost.Text = "132";
            form.Water.Text = "200";
            form.WaterCost.Text = "300";
            form.Gas.Text = "20";
            form.GasCost.Text = "159.8";
            form.Heating.Text = "1500";
            form.Residents.Text = "4";

          
            bool result = form.isEmpty();

           
            Assert.IsFalse(result);
        }
        private SqlConnection cnn;
        String[] s = {"Rooms", "Area", "Residents", "Electricity", "ElectricityCost",
                          "Water", "WaterCost", "Gas", "GasCost", "Heating",
                          "HeatingCost","HeatingType"};
        Boolean editText=false;

        public apartmentRegisterForm()
        {
            InitializeComponent();
        }

        private void apartmentRegisterForm_Load(object sender, EventArgs e)
        {
            cnn = new SqlConnection();
            cnn.ConnectionString = "Data Source=(local); Initial Catalog = EnergyConsumptionInApartments; Integrated Security=True";
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    if (control == WaterCost || control == GasCost || control == ElectricityCost || control == Heating || control == HeatingCost)
                    {
                        ((TextBox)control).KeyPress += TextBox_KeyPress;
                    }
                    else
                    {
                        ((TextBox)control).KeyPress += TextBox_KeyPressValidation;
                    }
                }
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("takeFlatID", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Flat", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            listBox1.DataSource = dt;
            listBox1.DisplayMember = dt.Columns[0].ColumnName;
        }

        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("searchFlat", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            DataRowView dr = (DataRowView)listBox1.SelectedItem;
            cmd.Parameters.Add("@search_Flat", SqlDbType.VarChar).Value =
                dr.Row.ItemArray[0].ToString();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da1.Fill(dt);
            FlatText.Text = "Flat " + dr.Row.ItemArray[0].ToString();
            fillComboBox();
            comboBoxHouseID.Text = dt.Rows[0].ItemArray[1].ToString().Trim();
            comboBoxFloor.Text = dt.Rows[0].ItemArray[2].ToString().Trim();

            for (int i = 3; i < 15; i++)
            {
                TextBox textBox = this.Controls.Find(s[i - 3], true).FirstOrDefault() as TextBox;
                textBox.Text = dt.Rows[0].ItemArray[i].ToString();
            }
            editButton.Enabled = true;
        }

        private void updateData()
        {
            SqlCommand cmd = new SqlCommand("takeFlatID", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Flat", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            listBox1.DataSource = dt;
            listBox1.DisplayMember = dt.Columns[0].ColumnName;

            SqlCommand cmd1 = new SqlCommand("searchFlat", cnn);
            cmd1.CommandType = CommandType.StoredProcedure;
            DataRowView dr = (DataRowView)listBox1.SelectedItem;
            cmd1.Parameters.Add("@search_Flat", SqlDbType.VarChar).Value =
                dr.Row.ItemArray[0].ToString();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            FlatText.Text = "Flat " + dr.Row.ItemArray[0].ToString();
            fillComboBox();
            comboBoxHouseID.Text = dt1.Rows[0].ItemArray[1].ToString().Trim();
            comboBoxFloor.Text = dt1.Rows[0].ItemArray[2].ToString().Trim();

            for (int i = 3; i < 15; i++)
            {
                TextBox textBox = this.Controls.Find(s[i - 3], true).FirstOrDefault() as TextBox;
                textBox.Text = dt1.Rows[0].ItemArray[i].ToString();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            buttonsEdit(true);
            editButton.Hide();
            changeDataButton.Show();
            newDataButton.Show();
            deleteDataButton.Show();
            editText = true;

        }

        private void buttonsEdit(bool condition)
        {
            comboBoxHouseID.Enabled = condition;
            comboBoxFloor.Enabled = condition;
            for (int i = 3; i < 14; i++)
            {
                TextBox textBox = this.Controls.Find(s[i - 3], true).FirstOrDefault() as TextBox;
                textBox.Enabled = condition;
            }
        }

        private void fillComboBox()
        {
            SqlCommand cmdHouseID = new SqlCommand("SELECT DISTINCT HouseID FROM Buildings", cnn);
            SqlDataAdapter daHouseID = new SqlDataAdapter(cmdHouseID);
            DataTable dtHouseID = new DataTable();
            daHouseID.Fill(dtHouseID);
            foreach (DataRow row in dtHouseID.Rows)
            {
                row["HouseID"] = row["HouseID"].ToString().Trim();
            }

            comboBoxHouseID.DataSource = dtHouseID;
            comboBoxHouseID.DisplayMember = "HouseID";

        }


        private void comboBoxHouseID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedHouseID = comboBoxHouseID.SelectedItem?.ToString();
            string type;
            using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog = EnergyConsumptionInApartments; Integrated Security=True"))
            {
                connection.Open();

                using (SqlCommand selectHeatingType = new SqlCommand("select HeatingSystems.Type from HeatingSystems where HeatingSystems.HeatingSystemID = (select HeatingSystemID from Buildings Where HouseID=@HouseID)", connection))
                {
                    selectHeatingType.Parameters.AddWithValue("@HouseID", comboBoxHouseID.Text);
                    type = Convert.ToString(selectHeatingType.ExecuteScalar());
                }
            }
            HeatingType.Text = type;
            if (type.Trim() == "Печі та каміни")
            {
                Heating.Text = "0";
                HeatingCost.Text = "0";
            }
            else if (type.Trim() == "Сонячні системи опалення")
            {
                Gas.Text = "0";
                GasCost.Text = "0";
            }

        }

        private void newDataButton_Click(object sender, EventArgs e)
        {
            newDataButton.Hide();
            deleteDataButton.Hide();
            changeDataButton.Hide();
            confirmDataButton.Show();
            textBox1.Enabled = false;
            listBox1.Enabled = false;
            FlatText.Text = "New Flat";
            comboBoxHouseID.Text = "";
            comboBoxFloor.Text = "";
            for (int i = 3; i < 14; i++)
            {
                TextBox textBox = this.Controls.Find(s[i - 3], true).FirstOrDefault() as TextBox;
                textBox.Text = "";
            }
        }

        private void confirmDataButton_Click(object sender, EventArgs e)
        {
            if (isEmpty())
            {
                MessageBox.Show("Fill in all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else{
                int flatId;
                using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog = EnergyConsumptionInApartments; Integrated Security=True"))
                {
                    connection.Open();

                    using (SqlCommand insertIntoFlats = new SqlCommand("INSERT INTO Flats ( HouseID, Floor, Rooms, Area, Residents) OUTPUT INSERTED.FlatID VALUES (@HouseID, @Floor, @Rooms, @Area, @Residents);", connection))
                    {
                        insertIntoFlats.Parameters.AddWithValue("@HouseID", comboBoxHouseID.Text.Trim());
                        insertIntoFlats.Parameters.AddWithValue("@Floor", Convert.ToInt32(comboBoxFloor.Text));
                        insertIntoFlats.Parameters.AddWithValue("@Rooms", Convert.ToInt32(Rooms.Text));
                        insertIntoFlats.Parameters.AddWithValue("@Area", Convert.ToInt32(Area.Text));
                        insertIntoFlats.Parameters.AddWithValue("@Residents", Convert.ToInt32(Residents.Text));
                        flatId = Convert.ToInt32(insertIntoFlats.ExecuteScalar());
                    }

                    int electricityUsageId;
                    using (SqlCommand insertIntoElectricityUsage = new SqlCommand("INSERT INTO ElectricityUsage (Electricity, ElectricityCost) OUTPUT INSERTED.UsageID VALUES (@Electricity, @ElectricityCost);", connection))
                    {
                        insertIntoElectricityUsage.Parameters.AddWithValue("@Electricity", Convert.ToInt32(Electricity.Text));
                        insertIntoElectricityUsage.Parameters.AddWithValue("@ElectricityCost", Convert.ToDouble(ElectricityCost.Text));
                        electricityUsageId = Convert.ToInt32(insertIntoElectricityUsage.ExecuteScalar());
                    }

                    int waterUsageid;
                    using (SqlCommand insertIntoWaterUsage = new SqlCommand("INSERT INTO WaterUsage (Water, WaterCost) OUTPUT INSERTED.UsageID VALUES (@Water, @WaterCost);", connection))
                    {
                        insertIntoWaterUsage.Parameters.AddWithValue("@Water", Convert.ToInt32(Water.Text));
                        insertIntoWaterUsage.Parameters.AddWithValue("@WaterCost", Convert.ToDouble(WaterCost.Text));
                        waterUsageid = Convert.ToInt32(insertIntoWaterUsage.ExecuteScalar());
                    }
                  

                    int heatingUsageid;
                    using (SqlCommand insertIntoHeatingUsage = new SqlCommand("INSERT INTO HeatingUsage (Heating, HeatingCost) OUTPUT INSERTED.UsageID VALUES (@Heating, @HeatingCost);", connection))
                    {
                        insertIntoHeatingUsage.Parameters.AddWithValue("@Heating", Convert.ToDouble(Heating.Text));
                        insertIntoHeatingUsage.Parameters.AddWithValue("@HeatingCost", Convert.ToDouble(HeatingCost.Text));
                        heatingUsageid = Convert.ToInt32(insertIntoHeatingUsage.ExecuteScalar());
                    }

                    int gasUsageid;
                    using (SqlCommand insertIntoGasUsage = new SqlCommand("INSERT INTO GasUsage (Gas, GasCost) OUTPUT INSERTED.UsageID VALUES (@Gas, @GasCost);", connection))
                    {
                        insertIntoGasUsage.Parameters.AddWithValue("@Gas", Convert.ToInt32(Gas.Text));
                        insertIntoGasUsage.Parameters.AddWithValue("@GasCost", Convert.ToDouble(GasCost.Text));
                        gasUsageid = Convert.ToInt32(insertIntoGasUsage.ExecuteScalar());
                    }

                    using (SqlCommand insertIntoExpenses = new SqlCommand("INSERT INTO Expenses (FlatID, ElectricityUsageID, WaterUsageID, HeatingUsageID, GasUsageID, LastUpdate) VALUES (@FlatID, @ElectricityUsageID, @WaterUsageID, @HeatingUsageID, @GasUsageID, GETDATE());", connection))
                    {
                        insertIntoExpenses.Parameters.AddWithValue("@FlatID", flatId);
                        insertIntoExpenses.Parameters.AddWithValue("@ElectricityUsageID", electricityUsageId);
                        insertIntoExpenses.Parameters.AddWithValue("@WaterUsageID", waterUsageid);
                        insertIntoExpenses.Parameters.AddWithValue("@HeatingUsageID", heatingUsageid);
                        insertIntoExpenses.Parameters.AddWithValue("@GasUsageID", gasUsageid);
                        insertIntoExpenses.ExecuteNonQuery();
                    }

                }
                MessageBox.Show("A new flat has been created!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FlatText.Text = "Flat "+ flatId;
                textBox1.Text = Convert.ToString(flatId);
                textBox1.Enabled = true;
                listBox1.Enabled = true;
                newDataButton.Show();
                deleteDataButton.Show();
                changeDataButton.Show();
                confirmDataButton.Hide();
            }
        }

        private bool isEmpty()
        {
            return (string.IsNullOrWhiteSpace(comboBoxHouseID.Text) ||
                string.IsNullOrWhiteSpace(comboBoxFloor.Text) ||
                string.IsNullOrWhiteSpace(Rooms.Text) ||
                string.IsNullOrWhiteSpace(Area.Text) ||
                string.IsNullOrWhiteSpace(Electricity.Text) ||
                string.IsNullOrWhiteSpace(ElectricityCost.Text) ||
                string.IsNullOrWhiteSpace(Water.Text) ||
                string.IsNullOrWhiteSpace(WaterCost.Text) ||
                string.IsNullOrWhiteSpace(Heating.Text) ||
                string.IsNullOrWhiteSpace(HeatingCost.Text) ||
                string.IsNullOrWhiteSpace(Gas.Text) ||
                string.IsNullOrWhiteSpace(GasCost.Text));
        }

        private void changeDataButton_Click(object sender, EventArgs e)
        {
            if (isEmpty())
            {
                MessageBox.Show("Fill in all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DataRowView dr = (DataRowView)listBox1.SelectedItem;
                int flatID = Convert.ToInt32(dr.Row.ItemArray[0]);

                using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog = EnergyConsumptionInApartments; Integrated Security=True"))
                {
                    connection.Open();

                    using (SqlCommand updateFlatsCmd = new SqlCommand("UPDATE Flats SET HouseID = @HouseID, Floor = @Floor, Rooms = @Rooms, Area = @Area, Residents = @Residents WHERE FlatID = @FlatID", connection))
                    {
                        updateFlatsCmd.Parameters.AddWithValue("@FlatID", flatID);
                        updateFlatsCmd.Parameters.AddWithValue("@HouseID", comboBoxHouseID.Text.Trim());
                        updateFlatsCmd.Parameters.AddWithValue("@Floor", Convert.ToInt32(comboBoxFloor.Text));
                        updateFlatsCmd.Parameters.AddWithValue("@Rooms", Convert.ToInt32(Rooms.Text));
                        updateFlatsCmd.Parameters.AddWithValue("@Area", Convert.ToInt32(Area.Text));
                        updateFlatsCmd.Parameters.AddWithValue("@Residents", Convert.ToInt32(Residents.Text));

                        updateFlatsCmd.ExecuteNonQuery();
                    }
                    using (SqlCommand updateWaterUsageCmd = new SqlCommand("UPDATE WaterUsage SET Water = @Water, WaterCost = @WaterCost WHERE UsageID = (Select WaterUsageID from Expenses Where FlatID = @FlatID)", connection))
                    {
                        updateWaterUsageCmd.Parameters.AddWithValue("@FlatID", flatID);
                        updateWaterUsageCmd.Parameters.AddWithValue("@Water", Convert.ToInt32(Water.Text));
                        updateWaterUsageCmd.Parameters.AddWithValue("@WaterCost", Convert.ToDouble(WaterCost.Text));

                        updateWaterUsageCmd.ExecuteNonQuery();
                    }
                    using (SqlCommand updateGasUsageCmd = new SqlCommand("UPDATE GasUsage SET Gas = @Gas, GasCost = @GasCost WHERE UsageID = (Select GasUsageID from Expenses where FlatID = @FlatID)", connection))
                    {
                        updateGasUsageCmd.Parameters.AddWithValue("@FlatID", flatID);
                        updateGasUsageCmd.Parameters.AddWithValue("@Gas", Convert.ToInt32(Gas.Text));
                        updateGasUsageCmd.Parameters.AddWithValue("@GasCost", Convert.ToDouble(GasCost.Text));

                        updateGasUsageCmd.ExecuteNonQuery();
                    }
                    using (SqlCommand updateHeatingUsageCmd = new SqlCommand("UPDATE HeatingUsage SET Heating = @Heating, HeatingCost  = @HeatingCost WHERE UsageID = (Select HeatingUsageID from Expenses where FlatID = @FlatID)", connection))
                    {
                        updateHeatingUsageCmd.Parameters.AddWithValue("@FlatID", flatID);
                        updateHeatingUsageCmd.Parameters.AddWithValue("@Heating", Convert.ToDouble(Heating.Text));
                        updateHeatingUsageCmd.Parameters.AddWithValue("@HeatingCost", Convert.ToDouble(HeatingCost.Text));

                        updateHeatingUsageCmd.ExecuteNonQuery();
                    }
                    using (SqlCommand updateElectricityUsageCmd = new SqlCommand("UPDATE ElectricityUsage SET Electricity = @Electricity, ElectricityCost = @ElectricityCost WHERE UsageID = (Select ElectricityUsageID from Expenses where FlatID = @FlatID)", connection))
                    {
                        updateElectricityUsageCmd.Parameters.AddWithValue("@FlatID", flatID);
                        updateElectricityUsageCmd.Parameters.AddWithValue("@Electricity", Convert.ToInt32(Electricity.Text));
                        updateElectricityUsageCmd.Parameters.AddWithValue("@ElectricityCost", Convert.ToDouble(ElectricityCost.Text));

                        updateElectricityUsageCmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Data changed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void deleteDataButton_Click(object sender, EventArgs e)
        {
            if (isEmpty()) {
                MessageBox.Show("Fill in all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Do you want to delete " + FlatText.Text + "?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataRowView dr = (DataRowView)listBox1.SelectedItem;
                    int flatID = Convert.ToInt32(dr.Row.ItemArray[0]);
                    using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog = EnergyConsumptionInApartments; Integrated Security=True"))
                    {
                        connection.Open();

                        int expensesid;
                        using (SqlCommand SelectExpensesCmd = new SqlCommand("Select ExpenseID FROM Expenses WHERE FlatID = @FlatID", connection))
                        {
                            SelectExpensesCmd.Parameters.AddWithValue("@FlatID", flatID);
                            expensesid = Convert.ToInt32(SelectExpensesCmd.ExecuteScalar());
                        }

                        using (SqlCommand deleteExpensesCmd = new SqlCommand("DELETE FROM Expenses WHERE FlatID = @FlatID", connection))
                        {
                            deleteExpensesCmd.Parameters.AddWithValue("@FlatID", flatID);
                            deleteExpensesCmd.ExecuteNonQuery();
                        }

                        using (SqlCommand deleteFlatsCmd = new SqlCommand("DELETE FROM Flats WHERE FlatID = @FlatID", connection))
                        {
                            deleteFlatsCmd.Parameters.AddWithValue("@FlatID", flatID);
                            deleteFlatsCmd.ExecuteNonQuery();
                        }
                        using (SqlCommand deleteElectricityUsageCmd = new SqlCommand("DELETE FROM ElectricityUsage Where UsageID = @FlatID", connection))
                        {
                            deleteElectricityUsageCmd.Parameters.AddWithValue("@FlatID", expensesid);
                            deleteElectricityUsageCmd.ExecuteNonQuery();
                        }
                        using (SqlCommand deleteWaterUsageCmd = new SqlCommand("DELETE FROM WaterUsage Where UsageID = @FlatID", connection))
                        {
                            deleteWaterUsageCmd.Parameters.AddWithValue("@FlatID", expensesid);
                            deleteWaterUsageCmd.ExecuteNonQuery();
                        }
                        using (SqlCommand deleteGasUsageCmd = new SqlCommand("DELETE FROM GasUsage Where UsageID = @FlatID", connection))
                        {
                            deleteGasUsageCmd.Parameters.AddWithValue("@FlatID", expensesid);
                            deleteGasUsageCmd.ExecuteNonQuery();
                        }
                        using (SqlCommand deleteHeatingUsageCmd = new SqlCommand("DELETE FROM HeatingUsage Where UsageID = @FlatID", connection))
                        {
                            deleteHeatingUsageCmd.Parameters.AddWithValue("@FlatID", expensesid);
                            deleteHeatingUsageCmd.ExecuteNonQuery();
                        }

                    }
                    MessageBox.Show("Flat delete!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Text = "1";
                    updateData();
                }
            }

        }

        private void comboBoxHouseID_TextChanged(object sender, EventArgs e)
        {

            int numberOfFloor = 0;
            using (SqlConnection connection = new SqlConnection("Data Source=(local); Initial Catalog = EnergyConsumptionInApartments; Integrated Security=True"))
            {
                connection.Open();
                using (SqlCommand cmdHouseID = new SqlCommand("SELECT Floors FROM Buildings WHERE HouseID = @HouseID", connection))
                {
                    cmdHouseID.Parameters.AddWithValue("@HouseID", comboBoxHouseID.Text);
                    numberOfFloor = Convert.ToInt32(cmdHouseID.ExecuteScalar());

                }
            }
            comboBoxFloor.Items.Clear();
            for (int i = 1; i <= numberOfFloor; i++)
            {
                comboBoxFloor.Items.Add(i);

            }
        }

        private void Electricity_TextChanged(object sender, EventArgs e)
        {
            if (editText) {
                double Rate = 2.64;
                if (double.TryParse(Electricity.Text, out double electricityValue))
                {
                    double result = electricityValue * Rate;
                    ElectricityCost.Text = result.ToString();
                }
            }
        }

        private void Water_TextChanged(object sender, EventArgs e)
        {
            if (editText)
            {
                double Rate = 41.136;
                if (double.TryParse(Water.Text, out double waterValue))
                {
                    double result = waterValue * Rate;
                    WaterCost.Text = result.ToString();
                }
            }
        }

        private void Gas_TextChanged(object sender, EventArgs e)
        {
            if (editText)
            {
                double Rate = 7.99;
                if (double.TryParse(Gas.Text, out double gasValue))
                {
                    double result = gasValue * Rate;
                    GasCost.Text = result.ToString();
                }
            }
        }

        private void Heating_TextChanged(object sender, EventArgs e)
        {
            if (editText)
            {
                double Rate = 1813.94;
                if (double.TryParse(Heating.Text, out double heatingValue))
                {
                    double result = heatingValue * Rate;
                    HeatingCost.Text = result.ToString();
                }
            }
        }


        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == ',') && (((TextBox)sender).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }
        private void TextBox_KeyPressValidation(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }
    }
}