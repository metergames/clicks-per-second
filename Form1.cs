using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Clicks_Per_Second
{
    public partial class Form1 : Form
    {
        bool firstClickDone = false;
        int score = 0;
        double timeLeft;
        string scoresPath;

        public Form1()
        {
            InitializeComponent();

            scoresPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Game Scores - Saves\\CPS-Scores.sav";

            try
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Game Scores - Saves"))
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Game Scores - Saves");

                if (!File.Exists(scoresPath))
                    File.Create(scoresPath, 10).Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Creating score file has failed: " + "\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                throw;
            }

            numericUpDown1.Value = Properties.Settings.Default.Seconds;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (firstClickDone)
                {
                    score++;
                }
                else
                {
                    firstClickDone = true;
                    score++;
                    timeLeft = Convert.ToInt32(numericUpDown1.Value);
                    timer_Count.Enabled = true;
                    numericUpDown1.Enabled = false;
                    numericUpDown1.ForeColor = Color.Black;
                    button1.Enabled = true;
                }
                label1.Text = "Score: " + score.ToString();
            }
        }

        private void timer1_Count_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 1)
            {
                timeLeft -= 1;
                label2.Text = timeLeft.ToString() + " Seconds";
                SecondSpelling();
            }
            else
            {
                label2.Text = "0 Seconds";
                timer_Count.Enabled = false;
                button1.Enabled = false;

                if (numericUpDown1.Value != 1)
                {
                    if (score != 1)
                        MessageBox.Show("You clicked " + score.ToString() + " times for " + numericUpDown1.Value.ToString() + " seconds\nYour click speed is " + Math.Round(score / numericUpDown1.Value, 1) + " CPS", "Time's Up!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("You clicked " + score.ToString() + " time for " + numericUpDown1.Value.ToString() + " seconds\nYour click speed is " + Math.Round(score / numericUpDown1.Value, 1) + " CPS", "Time's Up!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (score != 1)
                        MessageBox.Show("You clicked " + score.ToString() + " times for " + numericUpDown1.Value.ToString() + " second\nYour click speed is " + Math.Round(score / numericUpDown1.Value, 1) + " CPS", "Time's Up!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("You clicked " + score.ToString() + " time for " + numericUpDown1.Value.ToString() + " second\nYour click speed is " + Math.Round(score / numericUpDown1.Value, 1) + " CPS", "Time's Up!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (score > Properties.Settings.Default.CPS_Record)
                {
                    Properties.Settings.Default.CPS_Record = Math.Round(score / numericUpDown1.Value, 1);
                    Properties.Settings.Default.Save();
                }

                timer_WaitEnd.Enabled = true;

                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(DateTime.Now.ToString() + " - " + Math.Round(score / numericUpDown1.Value, 1) + " CPS\n");
                    File.AppendAllText(scoresPath, sb.ToString());
                    sb.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save CPS score to file: " + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Game Scores - Saves\\CPS-Scores.sav\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    throw;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetTest();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Seconds = numericUpDown1.Value;
            Properties.Settings.Default.Save();
            label2.Text = numericUpDown1.Value.ToString() + " Seconds";
            SecondSpelling();
        }

        private void timer_WaitEnd_Tick(object sender, EventArgs e)
        {
            timer_WaitEnd.Enabled = false;
            ResetTest();
        }

        private void ResetTest()
        {
            timer_Count.Enabled = false;
            button1.Enabled = true;
            score = 0;
            label1.Text = "Score: 0";
            firstClickDone = false;
            numericUpDown1.Enabled = true;
            label2.Text = numericUpDown1.Value.ToString() + " Seconds";
            SecondSpelling();
        }

        private void SecondSpelling()
        {
            if (label2.Text == "1 Seconds")
                label2.Text = "1 Second";
        }

        private void all_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                Form_About abtFrm = new Form_About();
                abtFrm.ShowDialog();
            }
        }

        private void timer_WaitAbout_Tick(object sender, EventArgs e)
        {
            timer_WaitAbout.Stop();
            Form_About abtFrm = new Form_About();
            abtFrm.ShowDialog();
        }

        private void all_MouseMove(object sender, MouseEventArgs e)
        {
            timer_WaitAbout.Start();
        }

        private void all_MouseLeave(object sender, EventArgs e)
        {
            timer_WaitAbout.Stop();
        }
    }
}
