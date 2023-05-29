using System;
using System.Windows.Forms;

namespace Kino
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.ShowDialog();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Chipta form1 = new Chipta();
            form1.ShowDialog();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Teatr form1 = new Teatr();
            form1.ShowDialog();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            KursatuvVaqt kursatuvVaqt = new KursatuvVaqt();
            kursatuvVaqt.ShowDialog();
            this.Hide();
        }
    }
}
