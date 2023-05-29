using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Kino
{
    public partial class Teatr : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        public Teatr()
        {
            InitializeComponent();
        }
        string queryAll = "SELECT Teatr.teatrID, Teatr.Nomi, Teatr.Joylashuvi FROM Teatr ";
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
        private void Teatr_Load(object sender, EventArgs e)
        {
            dbConnection();
            showAllData(queryAll);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dbConnection();
                int newKinoId = int.Parse(textBox1.Text);
                string newNomi = textBox2.Text;
                string newDoza = textBox3.Text;
                cmd = new SqlCommand("insert into Teatr values (@TeatrID, @Nomi, @Joylashuvi)", con);
                cmd.Parameters.AddWithValue("@TeatrID", newKinoId);
                cmd.Parameters.AddWithValue("@Nomi", newNomi);
                cmd.Parameters.AddWithValue("@Joylashuvi", newDoza);
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
                dtRow++;
            }
            con.Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            dbConnection();
            int Id = int.Parse(textBox1.Text);
            string query = "Delete Teatr where TeatrID =@Id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Ma'lumot o'chirildi!");
            cleardata();
            showAllData(queryAll);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            textBox1.Text = dataGridView1.Rows[row].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[row].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[row].Cells[2].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbConnection();
            string query = "Update Teatr set TeatrID=@TeatrID,Nomi=@Nomi,Joylashuvi= @Joylashuvi where TeatrID=@TeatrID";
            cmd = new SqlCommand(query, con);
            int newKinoId = int.Parse(textBox1.Text);
            string newNomi = textBox2.Text;
            string newDoza = textBox3.Text;
            cmd.Parameters.AddWithValue("@TeatrID", newKinoId);
            cmd.Parameters.AddWithValue("@Nomi", newNomi);
            cmd.Parameters.AddWithValue("@Joylashuvi", newDoza);
            cmd.ExecuteNonQuery();
            showAllData(queryAll);
            con.Open();
            con.Close();
            cleardata();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }
    }
}
