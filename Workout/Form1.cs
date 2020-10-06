using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Workout
{
    public partial class Form1 : Form
    {
        int countdownTimer;
        bool isRunning;
        bool isPaused;
        bool onBreak;
        int breakTime = 3; // TODO pull these from txtfile
        int exerciseTime = 5;

        static List<String> exerciseList = new List<string> {"test1", "test2", "test3", "test4" };
        Queue<String> exerciseQueue = new Queue<string>(exerciseList);

        public Form1()
        {
            InitializeComponent();
            


            timer1.Interval = 1000;
            isRunning = false;
            isPaused = false;
            onBreak = true;
            
            button1.Text = "Start Exercise";
            label1.Text = "Get Ready!";
            label2.Text = "next: " + exerciseQueue.Peek();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (isRunning && !isPaused)
            { // Pause
                isPaused = true;
                timer1.Stop();
                //  TODO pause
            } else
            {
                if (isPaused)
                {  // Unpause
                    isPaused = false;
                    // TODO unPause
                } else
                { // start Workout
                    isRunning = true;
                    countdownTimer = breakTime;
                    updateLabel(countdownTimer);
                    // TODO start workout
                }
                timer1.Start();
            }          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (onBreak && countdownTimer > 0)
            { // on break
                countdownTimer--;
                updateLabel(countdownTimer);
            } else if(onBreak)
            { // break ending, exercise starting
                onBreak = false;
                label2.Text = exerciseQueue.Dequeue();
                countdownTimer = exerciseTime;
                updateLabel(countdownTimer);
                // TODO swap to next exercise
            } else
            { // not on break
                if (countdownTimer > 0)
                { // during exercise
                    countdownTimer--;
                    updateLabel(countdownTimer);
                } else if(exerciseQueue.Count > 0)
                { // exercise ending, break starting
                    onBreak = true;
                    label2.Text = "break, next: " + exerciseQueue.Peek();
                    countdownTimer = breakTime;
                    updateLabel(countdownTimer);
                } else
                { // workout Over
                    label1.Text = "Yay you did it!";
                    label2.Text = "you sweaty bastard!";
                }
            }
        }

        private void updateLabel(int countdownTimer)
        {
            label1.Text = countdownTimer.ToString();
        }
    }
}
