using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IntAgent_TicTacToe
{
    public partial class Form1 : Form
    {
        string[] board = new string[9]; // Index 0-8 untuk button1-button9
        bool isUserTurn = false;
        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        // --- LOGIKA UTAMA: TOMBOL START (button10) ---
        private void button10_Click(object sender, EventArgs e)
        {
            ResetGame();

            // Acak siapa yang mulai
            if (rand.Next(2) == 0)
            {
                isUserTurn = true;
                label1.Text = "Giliranmu (X)";
            }
            else
            {
                isUserTurn = false;
                label1.Text = "AI Sedang Berpikir...";
                AiMove();
            }
        }

        private void ResetGame()
        {
            for (int i = 0; i < 9; i++)
            {
                board[i] = "";
                // Mencari button1 sampai button9 secara otomatis
                Control btn = this.Controls.Find("button" + (i + 1), true).FirstOrDefault();
                if (btn != null)
                {
                    btn.Text = "";
                    btn.Enabled = true;
                    btn.BackColor = Color.White;
                }
            }
        }

        // --- EVENT HANDLER UNTUK GRID (button1 - button9) ---
        // Kamu bisa klik dua kali setiap tombol di Designer dan panggil fungsi ini
        private void HandlePlayerClick(int index, Button btn)
        {
            if (board[index] == "" && isUserTurn)
            {
                board[index] = "X";
                btn.Text = "X";
                btn.Enabled = false;

                if (CheckWinnerStatus()) return;

                isUserTurn = false;
                label1.Text = "AI Sedang Berpikir...";
                AiMove();
            }
        }

        // Hubungkan setiap button_Click ke index yang sesuai
        private void button1_Click(object sender, EventArgs e) => HandlePlayerClick(0, (Button)sender);
        private void button2_Click(object sender, EventArgs e) => HandlePlayerClick(1, (Button)sender);
        private void button3_Click(object sender, EventArgs e) => HandlePlayerClick(2, (Button)sender);
        private void button4_Click(object sender, EventArgs e) => HandlePlayerClick(3, (Button)sender);
        private void button5_Click(object sender, EventArgs e) => HandlePlayerClick(4, (Button)sender);
        private void button6_Click(object sender, EventArgs e) => HandlePlayerClick(5, (Button)sender);
        private void button7_Click(object sender, EventArgs e) => HandlePlayerClick(6, (Button)sender);
        private void button8_Click(object sender, EventArgs e) => HandlePlayerClick(7, (Button)sender);
        private void button9_Click(object sender, EventArgs e) => HandlePlayerClick(8, (Button)sender);

        // --- AI LOGIC (MINIMAX) ---
        private void AiMove()
        {
            int bestScore = -1000;
            int bestMove = -1;

            for (int i = 0; i < 9; i++)
            {
                if (board[i] == "")
                {
                    board[i] = "O";
                    int score = Minimax(board, false);
                    board[i] = "";
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = i;
                    }
                }
            }

            if (bestMove != -1)
            {
                board[bestMove] = "O";
                Control btn = this.Controls.Find("button" + (bestMove + 1), true).FirstOrDefault();
                btn.Text = "O";
                btn.Enabled = false;

                if (CheckWinnerStatus()) return;

                isUserTurn = true;
                label1.Text = "Giliranmu (X)";
            }
        }

        int Minimax(string[] b, bool isMaximizing)
        {
            string winner = Evaluate();
            if (winner == "O") return 10;
            if (winner == "X") return -10;
            if (winner == "Tie") return 0;

            if (isMaximizing)
            {
                int bestScore = -1000;
                for (int i = 0; i < 9; i++)
                {
                    if (b[i] == "")
                    {
                        b[i] = "O";
                        bestScore = Math.Max(bestScore, Minimax(b, false));
                        b[i] = "";
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = 1000;
                for (int i = 0; i < 9; i++)
                {
                    if (b[i] == "")
                    {
                        b[i] = "X";
                        bestScore = Math.Min(bestScore, Minimax(b, true));
                        b[i] = "";
                    }
                }
                return bestScore;
            }
        }

        string Evaluate()
        {
            int[][] winConditions = {
                new int[] {0,1,2}, new int[] {3,4,5}, new int[] {6,7,8},
                new int[] {0,3,6}, new int[] {1,4,7}, new int[] {2,5,8},
                new int[] {0,4,8}, new int[] {2,4,6}
            };

            foreach (var condition in winConditions)
            {
                if (board[condition[0]] != "" && board[condition[0]] == board[condition[1]] && board[condition[0]] == board[condition[2]])
                    return board[condition[0]];
            }

            if (board.All(s => s != "")) return "Tie";
            return null;
        }

        bool CheckWinnerStatus()
        {
            string winner = Evaluate();
            if (winner != null)
            {
                if (winner == "Tie") label1.Text = "Hasil Seri!";
                else label1.Text = (winner == "O" ? "AI Menang!" : "Kamu Menang!");

                for (int i = 1; i <= 9; i++)
                    this.Controls.Find("button" + i, true).FirstOrDefault().Enabled = false;
                return true;
            }
            return false;
        }

        private void label1_Click(object sender, EventArgs e) { }
    }
}