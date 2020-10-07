using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Workout
{
    public partial class Form1 : Form
    {
        int countdownTimer;
        bool isRunning;
        bool isPaused;
        bool onBreak;
        bool workoutDone;
        int breakTime; 
        int exerciseTime;
        
        Queue<String> exerciseQueue = new Queue<string>();

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 1000;
          

            FetchWorkoutInfo();
            Setup();
            SetFormFullscreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (onBreak && countdownTimer > 0)
            { // During break
                countdownTimer--;
                UpdateLabel(countdownTimer);
            } else if(onBreak)
            { // break ending, exercise starting
                onBreak = false;
                label2.Text = exerciseQueue.Dequeue();
                countdownTimer = exerciseTime;
                UpdateLabel(countdownTimer);
            } else
            { // not on break
                if (countdownTimer > 0)
                { // During exercise
                    countdownTimer--;
                    UpdateLabel(countdownTimer);
                } else if(exerciseQueue.Count > 0)
                { // exercise ending, break starting
                    onBreak = true;
                    label2.Text = "break, next: " + exerciseQueue.Peek();
                    countdownTimer = breakTime;
                    UpdateLabel(countdownTimer);
                } else
                { // workout Over
                    label1.Text = "Yay you did it!";
                    label2.Text = "you sweaty bastard!";
                    button2.Text = "Do another?";
                    isRunning = false;
                    workoutDone = true;
                    timer1.Stop();
                }
            }
        }

        private void UpdateLabel(int countdownTimer)
        {
            label1.Text = countdownTimer.ToString();
        }

        private void FetchWorkoutInfo()
        {
            string[] lines = System.IO.File.ReadAllLines("WorkoutInfo.txt");

            exerciseTime = Int32.Parse(lines[1]);
            breakTime = Int32.Parse(lines[3]);
            string[] exerciseNames = lines[5].Split(',');

            foreach (string name in exerciseNames)
            {
                exerciseQueue.Enqueue(name);
            }
        }

        private void Setup()
        {
            isRunning = false;
            isPaused = false;
            onBreak = true;
            workoutDone = false;

            button2.Text = "Start Exercise";
            label1.Text = "Get Ready!";
            label2.Text = "next: " + exerciseQueue.Peek();
        }

        private void SetFormFullscreen()
        {
            this.TopMost = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (workoutDone)
            { // do another set
                FetchWorkoutInfo();
                Setup();
                timer1.Start();
                button2.Text = "Pause";
            }
            if (isRunning && !isPaused)
            { // Pause
                isPaused = true;
                timer1.Stop();
                button2.Text = "UnPause";

            } else
            {
                if (isPaused)
                {  // Unpause
                    isPaused = false;
                } else
                { // start Workout
                    isRunning = true;
                    countdownTimer = breakTime;
                    UpdateLabel(countdownTimer);
                }
                button2.Text = "Pause";
                timer1.Start();
            }
        }
    }
}
