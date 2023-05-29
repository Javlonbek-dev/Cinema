using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Kino
{
    public partial class KursatuvVaqt : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        public KursatuvVaqt()
        {
            InitializeComponent();
        }
        string queryAll = "SELECT KursatuvVaqti.KursatuvVaqtiId, Film.sarlavha,Teatr.Nomi,KursatuvVaqti.KursatuvVaqti, KursatuvVaqti.KursatuvSoati From KursatuvVaqti " +
            " INNER JOIN Film ON KursatuvVaqti.KinoId= Film.KinoId" +
            " INNER JOIN Teatr ON KursatuvVaqti.TeatrID = Teatr.TeatrID";
        private void dbConnection()
        {
            string strConnection = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Kino;Integrated Security=True";

            try
            {
                con = new SqlConnection(strConnection);
                con.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void KursatuvVaqt_Load(object sender, EventArgs e)
        {
            dbConnection();
            showAllData(queryAll);
            loadKino();
            loadTeatr();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dbConnection();
                cmd = new SqlCommand("insert into KursatuvVaqti values (@KursatuvVaqti_Id, @KinoId, @TeatrId, @KursatuvVaqti, @KursatuvSoati)", con);
                int newkurstuvId = int.Parse(textBox1.Text);
                string newsoati = textBox5.Text;
                string newvaqt = dateTimePicker1.Value.Date.ToString("dd-MM-yyyy");
                int newkino = int.Parse(comboBox1.SelectedValue.ToString());
                int newteatr = int.Parse(comboBox2.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@KursatuvVaqti_Id", newkurstuvId);
                cmd.Parameters.AddWithValue("@KinoId", newkino);
                cmd.Parameters.AddWithValue("@TeatrId", newteatr);
                cmd.Parameters.AddWithValue("@KursatuvVaqti", newvaqt);
                cmd.Parameters.AddWithValue("@KursatuvSoati", newsoati);
                cmd.ExecuteNonQuery();
                showAllData(queryAll);
                con.Open();
                con.Close();
                cleardata();
                MessageBox.Show("New record added successfully");

            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input. Please enter a valid number for Narxi and Miqdor fields.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the new record: " + ex.Message);
            }
        }
        private void cleardata()
        {
            textBox1.Clear();
            textBox5.Clear();
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
        }
        private void showAllData(string sorov)
        {
            cmd = new SqlCommand(sorov, con);
            SqlDataAdapter data = new SqlDataAdapter();
            data.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dataGridView1.Rows.Clear();
            data.Fill(dt);
            int column, dtRow = 0;

            foreach (DataRow row in dt.Rows)
            {
                column = 0;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<Int32>(0);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<string>(1);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<string>(2);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<DateTime>(3);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<string>(4);
                dtRow++;
            }
            con.Close();
        }
        private void loadKino()
        {
            string sqlCommand = "Select * from Film";
            cmd = new SqlCommand(sqlCommand, con);
            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            data.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "sarlavha";
            comboBox1.ValueMember = "KinoId";
            con.Close();
        }
        private void loadTeatr()
        {
            string sqlCommand = "Select * from Teatr";
            cmd = new SqlCommand(sqlCommand, con);
            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            data.Fill(dt);
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Nomi";
            comboBox2.ValueMember = "TeatrID";
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbConnection();
            string query = "Update KursatuvVaqti set KursatuvVaqtiId=@KursatuvVaqti_Id, KinoId=@KinoId,TeatrId=@TeatrId,KursatuvVaqti= @KursatuvVaqti, KursatuvSoati=@KursatuvSoati where KursatuvVaqtiId=@KursatuvVaqti_Id";
            cmd = new SqlCommand(query, con);
            int newkurstuvId = int.Parse(textBox1.Text);
            string newsoati = textBox5.Text;
            string newvaqt = dateTimePicker1.Value.Date.ToString("dd-MM-yyyy");
            int newkino = int.Parse(comboBox1.SelectedValue.ToString());
            int newteatr = int.Parse(comboBox2.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@KursatuvVaqti_Id", newkurstuvId);
            cmd.Parameters.AddWithValue("@KinoId", newkino);
            cmd.Parameters.AddWithValue("@TeatrId", newteatr);
            cmd.Parameters.AddWithValue("@KursatuvVaqti", newvaqt);
            cmd.Parameters.AddWithValue("@KursatuvSoati", newsoati);
            cmd.ExecuteNonQuery();
            showAllData(queryAll);
            con.Open();
            con.Close();
            cleardata();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            textBox1.Text = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string kino = dataGridView1.Rows[row].Cells[1].Value.ToString();
            comboBox1.SelectedIndex = comboBox1.FindStringExact(kino);
            string teatr = dataGridView1.Rows[row].Cells[2].Value.ToString();
            comboBox2.SelectedIndex = comboBox2.FindStringExact(teatr);
            textBox5.Text = dataGridView1.Rows[row].Cells[4].Value.ToString();
            dataGridView1.Rows[row].Cells[3].Value = dateTimePicker1.Value.ToShortDateString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbConnection();
            int Id = int.Parse(textBox1.Text);
            string query = "Delete KursatuvVaqti where KursatuvVaqtiId=@Id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Ma'lumot o'chirildi!");
            cleardata();
            showAllData(queryAll);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

