using System;
using System.Data;
using NUnit.Framework;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EnergyApplication
{
   
    public partial class apartmentSearchForm : Form
    {
        private SqlConnection cnn = new SqlConnection();
        private DataTable dt2 = new DataTable();
        private BindingSource bs = new BindingSource();
        public apartmentSearchForm()
        {
            InitializeComponent();
        }
        private void apartmentSearchForm_Load(object sender, EventArgs e)
        {
            cnn.ConnectionString = "Data Source = (local); Initial Catalog = EnergyConsumptionInApartments; Integrated Security = true";
            SqlCommand cmd = new SqlCommand("select FlatID, Type from Flats inner join Buildings on Buildings.HouseID = Flats.HouseID inner join HeatingSystems on HeatingSystems.HeatingSystemID = Buildings.HeatingSystemID order by FlatID", cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt2);
            bs.DataSource = dt2;
            selectlistBox.DataSource = bs;
            selectlistBox.DisplayMember = dt2.Columns[0].ColumnName;
            selectlistBox.ValueMember = dt2.Columns[1].ColumnName;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                string s = "Type like '" + textBox1.Text + "%'";
                DataRow[] drs = dt2.Select(s, "FlatID");
                if (drs.Length != 0)
                {
                    string dep_name = System.Convert.ToString(drs[0].ItemArray[0]);
                    int target = bs.Find("FlatID", dep_name);
                    bs.Position = target;
                }
            }
            else
                bs.Position = 0;
        }
        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("searchFlat", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            DataRowView dr = (DataRowView)selectlistBox.SelectedItem;
            cmd.Parameters.Add("@search_Flat", SqlDbType.NVarChar).Value =
                dr.Row.ItemArray[0].ToString();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            label4.Text = "HeatingType:" + dt.Rows[0].ItemArray[14].ToString().Trim() + "  FlatID: " + dr.Row.ItemArray[0].ToString();
            GridView.DataSource = dt;
        }


    }
}
