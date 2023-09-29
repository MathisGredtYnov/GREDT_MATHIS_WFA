using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace Platform_Game_Tutorial_MOO_ICT
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight, jumping, isGameOver;

        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;
        bool canjump = true;

        int horizontalSpeed = 5;
        int verticalSpeed = 3;

        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;
        private readonly SoundPlayer backgroundMusic;
        private readonly SoundPlayer victorybackgroundMusic;
        private readonly SoundPlayer defeatbackgroundMusic;
        

        public Form1()
        {
            InitializeComponent();
            backgroundMusic = new SoundPlayer(Properties.Resources.Megalovania);
            victorybackgroundMusic = new SoundPlayer(Properties.Resources.Overwatch_Victory);
            defeatbackgroundMusic = new SoundPlayer(Properties.Resources.you_are_dead);
            backgroundMusic.PlayLooping();
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;

            player.Top += jumpSpeed;

            if (goLeft == true)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
            }

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 8;
            }

            foreach(Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            if (player.Top < x.Bottom + x.Height && player.Bottom > x.Bottom)

                            {
                                
                                player.Top = x.Top + x.Height;
                                force = 0;
                                jumping = false;
                            }

                            else if (player.Bottom > x.Top && player.Top < x.Top)
                            {
                                force = 8;
                                if (!jumping)
                                {
                                    jumpSpeed = 0;
                                }
                                else
                                {
                                    jumpSpeed = -8;
                                }
                                player.Top = x.Top - player.Height + 1;
                                jumping = false;
                                canjump = true;
                            } 
                           
                            else
                            {
                                if (player.Left < x.Left && player.Right > x.Left)
                                {
                                    player.Left = x.Left;
                                }
                                else if (player.Right > x.Right && player.Left < x.Right)
                                {
                                    player.Left = x.Right;
                                }
                            }
                        }
                    }

                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score++;
                        }
                    }


                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gameTimer.Stop();
                            isGameOver = true;
                            txtScore.Text = "Score: " + score + Environment.NewLine + "You were killed in your journey!!";
                            defeatbackgroundMusic.Play();
                        }
                    }
                }
            }


            horizontalPlatform.Left -= horizontalSpeed;

            if (horizontalPlatform.Left < 0 || horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            verticalPlatform.Top += verticalSpeed;

            if (verticalPlatform.Top < 195 || verticalPlatform.Top > 581)
            {
                verticalSpeed = -verticalSpeed;
            }


            enemyOne.Left -= enemyOneSpeed;

            if (enemyOne.Left < pictureBox5.Left || enemyOne.Left + enemyOne.Width > pictureBox5.Left + pictureBox5.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;
            }

            enemyTwo.Left += enemyTwoSpeed;

            if (enemyTwo.Left < pictureBox2.Left || enemyTwo.Left + enemyTwo.Width > pictureBox2.Left + pictureBox2.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
            }


            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "You fell to your death!";
                defeatbackgroundMusic.Play();
            }

            if (player.Bounds.IntersectsWith(door.Bounds) && score == 24)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "Your quest is complete!";
                victorybackgroundMusic.Play();
            }
            else
            {
                txtScore.Text = "Score: " + score + Environment.NewLine + "Collect all the coins";
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
                player.Image = Properties.Resources.personnage_gauche; 
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                player.Image = Properties.Resources.personnage_droite;
            }
            if (e.KeyCode == Keys.Space && canjump)
            {
                jumping = true;
                canjump = false;
                jumpSpeed = -8;
            }
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {

            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            score = 0;

            txtScore.Text = "Score: " + score;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                    backgroundMusic.Play();
                }
            }

            player.Left = 60;
            player.Top = 680;

            enemyOne.Left = 471;
            enemyTwo.Left = 360;

            horizontalPlatform.Left = 275;
            verticalPlatform.Top = 581;

            gameTimer.Start();


        }
    }
}
