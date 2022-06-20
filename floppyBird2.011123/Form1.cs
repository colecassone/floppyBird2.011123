﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;


namespace floppyBird2._011123
{
    public partial class FloppyBird : Form
    {
        System.Windows.Media.MediaPlayer backgroundMedia = new System.Windows.Media.MediaPlayer();
        SoundPlayer player = new SoundPlayer(Properties.Resources.jumpSound);

        Image PGfloppy = Properties.Resources.PGfloppy;
        Image backround = Properties.Resources.backround;
        Image cloud = Properties.Resources.cloud;

        List<Rectangle> movingBackroud = new List<Rectangle>();
        List<Rectangle> clouds = new List<Rectangle>();
        Rectangle floppy = new Rectangle(50, 223, 25, 25);
        List<Rectangle> topTunnel = new List<Rectangle>();
        List<Rectangle> bottomTunnel = new List<Rectangle>();
        List<Rectangle> INscore = new List<Rectangle>();

        int backroundSpeed = 2;
        int backroundX = 334;
        int backroundY = 243;
        int backRoundCounter = 148;
        int cloudsSpeed = 1;
        int cloudX = 47;
        int cloudY = 30;
        int cloudCounter = 0;
        int gameState = 1;
        int tunnelCount = 0;
        int speed = 4;
        int score = 0;
        int stateJump = 0;
        int highScore = 0;

        bool spaceBar = false;
        bool pause = false;
        bool quit = false;

        Pen drawPen = new Pen(Color.Black, 5);
        SolidBrush backFill = new SolidBrush(Color.Green);

        Random randGen = new Random();

        public FloppyBird()
        {
            InitializeComponent();
            Cursor.Hide(); 

            backgroundMedia.Open(new Uri(Application.StartupPath + "/Resources/Kahoot Lobby Music.mp3"));
            backgroundMedia.MediaEnded += new EventHandler(Media_Ended);

            backgroundMedia.Play();
        }
        private void Media_Ended(object sender, EventArgs e)
        {
            backgroundMedia.Stop();
            backgroundMedia.Play();
        }
        public void GameInitialize()
        {
            gameState = 2;
            score = 0;
            tunnelCount = 0;
            backRoundCounter = 148;
            titleLabel.Visible = false;
            subTitleLabel.Visible = false;

            INscore.Clear();

            floppy.Y = 223;

            topTunnel.Clear();
            bottomTunnel.Clear();


            gameTimer.Start();
            movingBackroud.Clear();
            clouds.Clear();

            movingBackroud.Add(new Rectangle(0, 333, backroundX, backroundY));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void FloppyBird_Paint(object sender, PaintEventArgs e)
        {
            // Stating screen
            if (gameState == 1)
            {
                subTitleLabel.Visible = true;
                subTitleLabel.Text = $"Press Space To Start The Game Or Press Escape To Leave";
            }
            //game plaing screen
            else if (gameState == 2)
            {
                for (int i = 0; i < movingBackroud.Count; i++)
                {
                    e.Graphics.DrawImage(backround, movingBackroud[i]);
                }
                for (int i = 0; i < clouds.Count; i++)
                {
                    e.Graphics.DrawImage(cloud, clouds[i]);
                }
                e.Graphics.DrawImage(PGfloppy, floppy);

                scoreLabel.Text = $"Score is {score}";

                for (int i = 0; i < topTunnel.Count; i++)
                {
                    e.Graphics.FillRectangle(backFill, topTunnel[i]);
                }
                for (int i = 0; i < bottomTunnel.Count; i++)
                {
                    e.Graphics.FillRectangle(backFill, bottomTunnel[i]);
                }
            }
            //after you loses
            else
            {

                subTitleLabel.Visible = true;
                titleLabel.Visible = true;
                if (score > highScore)
                {
                    highScore = score;
                }
                subTitleLabel.Text = $"Your Score was {score}";

                subTitleLabel.Text += $"\nYour High Score is {highScore}";

            }
        }

        private void FloppyBird_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    quit = true;

                    if (gameState == 1 || gameState == 3)
                    {
                        this.Close();
                    }
                    break;
                case Keys.Space:
                    if (gameState == 1 || gameState == 3)
                    {
                        GameInitialize();

                    }
                    spaceBar = true;
                    break;
                case Keys.P:
                    pause = true;
                    break;
            }
        }

        private void FloppyBird_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    quit = false;
                    break;
                case Keys.Space:
                    spaceBar = false;
                    break;
                case Keys.P:
                    pause = false;
                    break;

            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Charcter Movement 
            if (spaceBar == true)
            {
                stateJump = 1;
                player.Stop();
                player.Play();
            }
            // jump anamtion 
            switch (stateJump)
            {
                case 1:
                    floppy.Y -= 10;
                    stateJump++;
                    break;
                case 2:
                    floppy.Y -= 5;
                    stateJump++;
                    break;
                case 3:
                    floppy.Y -= 2;
                    stateJump++;
                    break;
                case 4:
                    floppy.Y -= 0;
                    stateJump++;
                    break;
                case 5:
                    floppy.Y += 2;

                    stateJump++;
                    break;
                case 6:
                    floppy.Y += 6;
                    stateJump = 0;
                    break;
                default:
                    floppy.Y += 6;
                    stateJump = 0;
                    break;
            }
            // check if floppy hit the bottom 

            if (floppy.Y > 476)
            {
                gameState = 3;
            }

            // check if floppy hit the top of the roof
            if (floppy.Y < -13)
            {
                gameState = 3;
            }

            // Creating the tunnels if it is time and add a scoreing intersetor 

            tunnelCount++;

            if (tunnelCount == 70)
            {
                int Random = randGen.Next(1, 350);

                topTunnel.Add(new Rectangle(400, 0, 30, Random));

                INscore.Add(new Rectangle(400, Random + 5, 5, 100));

                int hight = 486 - Random - 100;

                Random = Random + 100;

                bottomTunnel.Add(new Rectangle(400, Random, 30, hight));

                tunnelCount = 0;
            }


            //Moving the tunnes 

            for (int i = 0; i < topTunnel.Count; i++)
            {
                int x = topTunnel[i].X - speed;
                topTunnel[i] = (new Rectangle(x, 0, 30, topTunnel[i].Height));
            }
            for (int i = 0; i < bottomTunnel.Count; i++)
            {
                int x = bottomTunnel[i].X - speed;
                bottomTunnel[i] = (new Rectangle(x, bottomTunnel[i].Y, 30, bottomTunnel[i].Height));
            }

            // move scoring interscetor 

            for (int i = 0; i < INscore.Count; i++)
            {
                int x = INscore[i].X - speed;
                INscore[i] = (new Rectangle(x, INscore[i].Y, 30, INscore[i].Height));
            }

            // removing the tunnels 

            for (int i = 0; i < topTunnel.Count; i++)
            {
                if (topTunnel[i].X <= -20)
                {
                    topTunnel.RemoveAt(i);

                }
                if (bottomTunnel[i].X <= -20)
                {
                    bottomTunnel.RemoveAt(i);
                }
            }

            // intercetion of the tunnels
            for (int i = 0; i < bottomTunnel.Count; i++)
            {
                if (floppy.IntersectsWith(bottomTunnel[i]))
                {
                    gameState = 3;
                }
                if (floppy.IntersectsWith(topTunnel[i]))
                {
                    gameState = 3;
                }
            }

            // scoring 
            for (int i = 0; i < INscore.Count; i++)
            {
                if (floppy.IntersectsWith(INscore[i]))
                {
                    score++;
                    INscore.RemoveAt(i);
                    scoreLabel.Text = $"score is {score}";

                }
            }

            // city moving backround 
            backRoundCounter++;
            if (backRoundCounter == 149)
            {
                movingBackroud.Add(new Rectangle(300, 333, backroundX, backroundY));
                backRoundCounter = 0;
            }
            for (int i = 0; i < movingBackroud.Count; i++)
            {
                int x = movingBackroud[i].X - backroundSpeed;
                movingBackroud[i] = new Rectangle(x, movingBackroud[i].Y, backroundX, backroundY);
            }

            // removing ctiys for the list. 
            for (int i = 0; i < movingBackroud.Count; i++)
            {
                if (movingBackroud[i].X <= -400)
                {
                    movingBackroud.RemoveAt(i);
                }
            }

            // clouds moving backround 
            cloudCounter++;
            if (cloudCounter == 100)
            {
                int Random = randGen.Next(1, 350);

                clouds.Add(new Rectangle(334, Random, cloudX, cloudY));
                cloudCounter = 0;
            }

            for (int i = 0; i < clouds.Count; i++)
            {
                int x = clouds[i].X - cloudsSpeed;
                clouds[i] = new Rectangle(x, clouds[i].Y, cloudX, cloudY);
            }

            // removing clouds 
            for (int i = 0; i < clouds.Count; i++)

            {
                if (clouds[i].X <= -40)
                {
                    clouds.RemoveAt(i);
                }
            }


            Refresh();
        }
    }
}
