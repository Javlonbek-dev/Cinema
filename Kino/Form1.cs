using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Kino
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        public Form1()
        {
            InitializeComponent();
        }
        string queryAll = "SELECT Film.KinoId, Film.sarlavha, Film.janr, Film.davomiyligi, Film.Direktor, Film.Ishlab_Sana FROM Film ";
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dbConnection();
                int newKinoId = int.Parse(textBox1.Text);
                string newNomi = textBox2.Text;
                string newDoza = textBox3.Text;
                string newMuddati = textBox4.Text;
                string newJoy = textBox5.Text;
                string newvaqt = dateTimePicker1.Value.Date.ToString("dd-MM-yyyy");
                cmd = new SqlCommand("insert into Film values (@KinoId, @Sarlavha, @Janr, @Davomiyligi, @Direktor, @Ishlab_Chiqarilgan)", con);
                cmd.Parameters.AddWithValue("@KinoId", newKinoId);
                cmd.Parameters.AddWithValue("@Sarlavha", newNomi);
                cmd.Parameters.AddWithValue("@Janr", newDoza);
                cmd.Parameters.AddWithValue("@Davomiyligi", newMuddati);
                cmd.Parameters.AddWithValue("@Direktor", newJoy);
                cmd.Parameters.AddWithValue("@Ishlab_Chiqarilgan", newvaqt);
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
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
        private void showAllData(string sorov)
        {
            cmd = new SqlCommand(sorov, con);
            SqlDataAdapter data = new SqlDataAdapter(cmd);
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
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<string>(3);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<string>(4);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<DateTime>(5);


                dtRow++;
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbConnection();
            int Id = int.Parse(textBox1.Text);
            string query = "Delete Film where KinoId=@Id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Ma'lumot o'chirildi!");
            cleardata();
            showAllData(queryAll);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbConnection();
            showAllData(queryAll);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbConnection();
            string query = "Update Film set KinoId=@KinoId, sarlavha=@Sarlavha, janr=@Janr, Davomiyligi= @Davomiyligi, Direktor=@Direktor ,Ishlab_Sana= @Ishlab_Chiqarilgan where KinoId=@KinoId";
            cmd = new SqlCommand(query, con);
            int newKinoId = int.Parse(textBox1.Text);
            string newNomi = textBox2.Text;
            string newDoza = textBox3.Text;
            string newMuddati = textBox4.Text;
            string newJoy = textBox5.Text;
            string newvaqt = dateTimePicker1.Value.Date.ToString("dd-MM-yyyy");
            cmd.Parameters.AddWithValue("@KinoId", newKinoId);
            cmd.Parameters.AddWithValue("@Sarlavha", newNomi);
            cmd.Parameters.AddWithValue("@Janr", newDoza);
            cmd.Parameters.AddWithValue("@Davomiyligi", newMuddati);
            cmd.Parameters.AddWithValue("@Direktor", newJoy);
            cmd.Parameters.AddWithValue("@Ishlab_Chiqarilgan", newvaqt);
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
            textBox2.Text = dataGridView1.Rows[row].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[row].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[row].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.Rows[row].Cells[4].Value.ToString();
            dataGridView1.Rows[row].Cells[5].Value = dateTimePicker1.Value.ToShortDateString();
        }
    }
}
