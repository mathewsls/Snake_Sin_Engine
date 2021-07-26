﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_0_engine
{
    public partial class Form1 : Form
    {
        int cols = 50, rows = 25, score = 0, dx = 0, dy = 0, front = 0, back = 0;
        Piece[] snake = new Piece[1250];
        List<int> avaliable = new List<int>();
        bool[,] visit;
        Random rand = new Random();
        Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            Initial();
            launchTimer();
        }

        private void launchTimer()
        {
            timer.Interval = 50;
            timer.Tick += move;
            timer.Start();
        }

        private void move(object sender, EventArgs e)
        {
            int x = snake[front].Location.X, y = snake[front].Location.Y;
            if (dx == 0 && dy == 0) return;
            if (game_over(x + dx, y + dy)) {
                timer.Stop();
                MessageBox.Show("Andate Manco");
                return;
            }
            if (collisionFood(x + dx, y + dy))
            {
                score += 1;
                lblScore.Text = "Score: " + score.ToString();
                if (hits((y + dy) / 20, (x + dx) / 20)) return;
                Piece head = new Piece(x + dx, y + dy);
                front = (front - 1 + 1250) % 1250;
                snake[front] = head;
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                Controls.Add(head);
                randomFood();

            }else{
                if (hits((y + dy) / 20, (x + dx) / 20)) return;
                visit[snake[back].Location.Y / 20, snake[back].Location.X / 20] = false;
                front = (front - 1 + 1250) % 1250;
                snake[front] = snake[back];
                snake[front].Location = new Point(x + dx, y + dy);
                back = (back - 1 + 1250) % 1250;
                visit[(y + dy) / 20, (x + dx) / 20] = true;
            }
        
        }

        private void Snake_Keydown(object sender, KeyEventArgs e)
        {
            dx = 0;
            dy = 0;
            switch (e.KeyCode)
            {
                case Keys.Right:
                    dx = 20;
                    break;
                case Keys.Left:
                    dx = -20;
                    break;
                case Keys.Up:
                    dy = -20;
                    break;
                case Keys.Down:
                    dy = 20;
                    break;
            }
        }

        private void randomFood()
        {
            avaliable.Clear();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (!visit[i, j]) avaliable.Add(i * cols + j);
            int idx = rand.Next(avaliable.Count) % avaliable.Count;
            lblFood.Left = (avaliable[idx] * 20) % Width;
            lblFood.Top = (avaliable[idx] * 20) / Width * 20;
        }

        private bool hits(int x, int y)
        {
            if (visit[x, y]) {
                timer.Stop();
                MessageBox.Show("la serpiente golpeo su cuerpo");
                return true;
            }
        return false;
        }

        private bool collisionFood(int x, int y)
        {
            return x == lblFood.Location.X && y == lblFood.Location.Y;
        }

        private bool game_over(int x, int y)
        {
            return x < 0 || y < 0 || x > 980|| y > 480;
        }

        private void Initial()
        {
            visit = new bool[rows, cols];
            Piece head = new Piece ((rand.Next() % cols  ) * 20, (rand.Next() % rows) * 20);
            lblFood.Location = new Point((rand.Next() % cols) * 20, (rand.Next() % rows) * 20);
            for (int i = 0; i < rows; i++)
             for (int j = 0; j < cols; j++) {
                    visit[i, j] = false;
                    avaliable.Add(i * cols + j);

            }
            visit[head.Location.Y / 20, head.Location.X / 20] = true;
            avaliable.Remove(head.Location.Y / 20 * cols + head.Location.X / 20 );
            Controls.Add(head); snake[front] = head;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
