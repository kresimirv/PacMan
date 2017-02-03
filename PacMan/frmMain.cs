using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace PacMan
{
    public partial class frmMain : Form
    {
        //FPS
        private const int fps = 30;

        //timer
        HiResTimer timer = new HiResTimer();
        long startTime;
        long interval = (long)TimeSpan.FromSeconds(1.0 / fps).TotalMilliseconds;

        //graphics
        Graphics graphics;
        Graphics bufferGraphics;
        Image backBuffer;

        int clientWidth;
        int clientHeight;

        //game engine
        private GameEngine gameEngine;

        //back color
        private Color FormBackColor = Color.Black;

        public frmMain()
        {
            InitializeComponent();

            //form config
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ClientSize = GameEngine.GameFieldSize();

            clientWidth = this.ClientRectangle.Width;
            clientHeight = this.ClientRectangle.Height;

            //buffer
            backBuffer = (Image)new Bitmap(clientWidth, clientHeight);
            bufferGraphics = Graphics.FromImage(backBuffer);

            graphics = this.CreateGraphics();
        }
        

        public void GameLoop()
        {
            gameEngine = new GameEngine();
            GameEngine.BackgroundColor = Color.Black;
            gameEngine.CreateGameField();
    
            //LOOP
            timer.Start();
            while (this.Created)
            {
                startTime = timer.ElapsedMilliseconds;

                RenderAll();
                
                Application.DoEvents();
                while (timer.ElapsedMilliseconds - startTime < interval) ;
            }

            
        }

        private void RenderAll()
        {
            //black screen
            bufferGraphics.FillRectangle(new SolidBrush(FormBackColor), this.ClientRectangle);

            //draw on buffer
            gameEngine.Render(bufferGraphics); 

            //draw on surface
            this.BackgroundImage = backBuffer;
            this.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.W) keyData = Keys.Up;
            if (keyData == Keys.S) keyData = Keys.Down;
            if (keyData == Keys.A) keyData = Keys.Left;
            if (keyData == Keys.D) keyData = Keys.Right;

            switch (keyData)
            {
                case Keys.Right:
                    if (gameEngine.isRunning == true && gameEngine.isPaused == false)
                    {
                        gameEngine.pacMan.NextDirection = PacMan.PacManDirection.Right;
                    }
                    break;

                case Keys.Left:
                    if (gameEngine.isRunning == true && gameEngine.isPaused == false)
                    {
                        gameEngine.pacMan.NextDirection = PacMan.PacManDirection.Left;
                    }
                    break;

                case Keys.Down:
                    if (gameEngine.isRunning == true && gameEngine.isPaused == false)
                    {
                        gameEngine.pacMan.NextDirection = PacMan.PacManDirection.Down;
                    }
                    break;

                case Keys.Up:
                    if (gameEngine.isRunning == true && gameEngine.isPaused == false)
                    {
                        gameEngine.pacMan.NextDirection = PacMan.PacManDirection.Up;
                    }
                    break;

                case Keys.Escape:
                    if (gameEngine.isRunning == true)
                    {
                        gameEngine.isPaused = !gameEngine.isPaused;
                        if (gameEngine.isPaused)
                        {
                            this.Text = "Main - PAUSED";
                        }
                        else
                        {
                            this.Text = "Main";
                        }
                    }
                    break;

                case Keys.Enter:
                    if (gameEngine.isRunning == false && gameEngine.isGameOver == true)
                    {
                        gameEngine.CreateGameField();
                    }
                    break;

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


    }
}
