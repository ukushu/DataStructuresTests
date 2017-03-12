using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterestingTestsProject
{
    public partial class Form1 : Form
    {
        bool _progresIsGoing;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnAsync_Click(object sender, EventArgs e)
        {
            if (progressBar.Value == progressBar.Maximum)
            {
                progressBar.Value = 0;
            }

            btnAsync.Text = _progresIsGoing ? "Start" : "Pause";
            
            _progresIsGoing = !_progresIsGoing;

            await StartProgressBarGoing();

            if (progressBar.Value == progressBar.Maximum)
            {
                btnAsync.Text = "ReStart";
                _progresIsGoing = false;
            }
        }

        public async Task<int> StartProgressBarGoing()
        {
            while (progressBar.Value < progressBar.Maximum  && _progresIsGoing)
            {
                await Task.Delay(10);

                progressBar.Value++;
            }

            return 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar.Maximum = 1000;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataStructsTests gt = new DataStructsTests();

            textBox.Text = gt.RunTests();
        }
    }
}
