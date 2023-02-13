using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace saper
{
    public partial class Form1 : Form
    {

        private const int ROWS = 9;
        private const int COLS = 9;
        private const int MINES = 5;
        private int safeButtons;
        private Button[,] buttons;
        private bool[,] mines;
        private bool firstClick;
        Random random = new Random();


        public Form1()
        {
            InitializeComponent();

            Text = "Saper";
            Size = new Size(COLS * 40, ROWS * 40);
            CenterToScreen();
            firstClick = true;
            safeButtons = ROWS * COLS - MINES;

            buttons = new Button[ROWS, COLS];
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    buttons[row, col] = new Button();
                    buttons[row, col].Size = new Size(40, 40);
                    buttons[row, col].Location = new Point(col * 40, row * 40);
                    buttons[row, col].Click += new EventHandler(ButtonClick);
                    Controls.Add(buttons[row, col]);
                }
            }

            Random random = new Random();
            mines = new bool[ROWS, COLS];
            for (int i = 0; i < MINES; i++)
            {
                int row, col;
                do
                {
                    row = random.Next(ROWS);
                    col = random.Next(COLS);
                } while (mines[row, col]);
                mines[row, col] = true;
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int row = button.Top / 40;
            int col = button.Left / 40;

            if (firstClick)
            {
                firstClick = false;
                if (mines[row, col])
                {
                    
                    do
                    {
                        row = random.Next(ROWS);
                        col = random.Next(COLS);
                    } while (mines[row, col]);
                    mines[row, col] = true;
                }
                Expand(row, col);
            }

            
            if (mines[row, col])
            {
                button.Text = "X";
                MessageBox.Show("You hit a mine!");
            }
            else
            {
               
                int count = 0;
                for (int r = row - 1; r <= row + 1; r++)
                {
                    for (int c = col - 1; c <= col + 1; c++)
                    {
                        if (r >= 0 && r < ROWS && c >= 0 && c < COLS && mines[r, c])
                        {
                            count++;
                        }
                    }
                }

                
                button.Text = count.ToString();
                safeButtons--;
                if (safeButtons == 0)
                {
                    MessageBox.Show("You win!");
                }
            }
        }
        private void Expand(int row, int col)
        {
            int r, c;
            for (r = row - 1; r <= row + 1; r++)
            {
                for (c = col - 1; c <= col + 1; c++)
                {
                    if (r >= 0 && r < ROWS && c >= 0 && c < COLS && !mines[r, c])
                    {
                        ButtonClick(buttons[r, c], new EventArgs());
                    }
                }
            }
        }
}}

