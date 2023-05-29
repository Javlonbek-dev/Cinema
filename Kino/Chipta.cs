using System;


using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Kino
{
    public partial class Chipta : Form
    {
        SqlConnection con;
        SqlCommand cmd;

        public Chipta()
        {
            InitializeComponent();
        }

        string queryAll = "SELECT Chipta.ChiptaId, KursatuvVaqti.KursatuvVaqti, Chipta.UrinRaqami, Chipta.Narxi, Film.sarlavha FROM Chipta " +
            " INNER JOIN KursatuvVaqti ON Chipta.ChiptaId = KursatuvVaqti.KursatuvVaqtiId" +
            " INNER JOIN Film ON Chipta.KinoId = Film.KinoId";

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
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<DateTime>(1);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<Int32>(2);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<Int32>(3);
                dataGridView1.Rows[dtRow].Cells[column++].Value = row.Field<string>(4);
                dtRow++;
            }
            con.Close();
        }
        private void Chipta_Load(object sender, EventArgs e)
        {
            dbConnection();
            showAllData(queryAll);
            loadKursatuv();
            loadFilm();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dbConnection();
                int newchiptaId = int.Parse(textBox1.Text);
                int newKursa = int.Parse(comboBox1.SelectedValue.ToString());
                int newFilm = int.Parse(comboBox2.SelectedValue.ToString());
                string newUrin = textBox3.Text;
                string newNarxi = textBox4.Text;

                cmd = new SqlCommand("insert into Chipta values (@ChiptaId, @KursatuvVaqti, @UrinRaqami,@Narxi,@FilmNomi)", con);
                cmd.Parameters.AddWithValue("@ChiptaId", newchiptaId);
                cmd.Parameters.AddWithValue("@KursatuvVaqti", newKursa);
                cmd.Parameters.AddWithValue("@UrinRaqami", newUrin);
                cmd.Parameters.AddWithValue("@Narxi", newNarxi);
                cmd.Parameters.AddWithValue("@FilmNomi", newFilm);
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
        private void loadKursatuv()
        {

            string sqlCommand = "Select * from KursatuvVaqti";
            cmd = new SqlCommand(sqlCommand, con);
            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            data.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "KursatuvVaqti";
            comboBox1.ValueMember = "KursatuvVaqtiId";
            con.Close();
        }
        private void loadFilm()
        {

            string sqlCommand = "Select * from Film";
            cmd = new SqlCommand(sqlCommand, con);
            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            data.Fill(dt);
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "sarlavha";
            comboBox2.ValueMember = "KinoId";
            con.Close();
        }
        private void cleardata()
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbConnection();
            int Id = int.Parse(textBox1.Text);
            string query = "Delete Chipta where ChiptaId =@Id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Ma'lumot o'chirildi!");
            cleardata();
            showAllData(queryAll);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "Update Chipta set Chipta=@ChiptaID,KursatuvVaqtiId=@KursatuvVaqti,UrinRaqam= @UrinRaqam,Narxi=@Narxi, KinoId=@FilmNomi where Chipta=@ChiptaID";
            cmd = new SqlCommand(query, con);
            int newchiptaId = int.Parse(textBox1.Text);
            int newKursa = int.Parse(comboBox1.SelectedValue.ToString());
            int newFilm = int.Parse(comboBox2.SelectedValue.ToString());
            string newUrin = textBox3.Text;
            string newNarxi = textBox4.Text;
            cmd.Parameters.AddWithValue("@ChiptaID", newchiptaId);
            cmd.Parameters.AddWithValue("@KursatuvVaqti", newKursa);
            cmd.Parameters.AddWithValue("@UrinRaqam", newUrin);
            cmd.Parameters.AddWithValue("@Narxi", newNarxi);
            cmd.Parameters.AddWithValue("@FilmNomi", newFilm);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            textBox1.Text = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string kino = dataGridView1.Rows[row].Cells[1].Value.ToString();
            comboBox1.SelectedIndex = comboBox1.FindStringExact(kino);
            textBox3.Text = dataGridView1.Rows[row].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string film = dataGridView1.Rows[row].Cells[4].Value.ToString();
            comboBox2.SelectedIndex = comboBox2.FindStringExact(film);
        }
    }
}
