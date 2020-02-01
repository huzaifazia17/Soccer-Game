//Lleyton
//ICS 3U1
//Final game
//June 17, 2019

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Soccer_Game
{
    public partial class Form1 : Form
    {
        //declares timer
        Timer timer;
        //declares scoreboard rectangle
        Rectangle scoreboard;
        //declares player rectangle
        Rectangle player;
        //declares ball rectangle
        Rectangle ball;
        //declares net rectangle
        Rectangle net;
        //declares enemy rectangle
        Rectangle enemy;

        //declares and sets the soccerBall picture
        Image soccerBall = Image.FromFile(Application.StartupPath + @"\ball.png", true);
        //declares and sets the netImage picture
        Image netImage = Image.FromFile(Application.StartupPath + @"\net.png", true);
        //declares and sets the playerImage picture
        Image playerImage = Image.FromFile(Application.StartupPath + @"\player.png", true);
        //declares and sets the enemyImage picture
        Image enemyImage = Image.FromFile(Application.StartupPath + @"\enemy.png", true);

        //integer for moving playerx
        int playerdx = 0;
        //integer for moving bally
        int balldy = 0;
        //integer for moving enemyx
        int enemydx = 10;
        //integer for score
        int score = 0;
        //integer for lives
        int lives = 3;
        //integer for screen height
        const int screenH = 900;
        //integer for screen width
        const int screenW = 900;
        //string for message on screen
        string message = "Press 'Space' to Shoot";
        //boolean for keeping track of ball movement
        bool ballMove = false;

        public Form1()
        {
            InitializeComponent();
        }
        
        //loads form
        private void Form1_Load(object sender, EventArgs e)
        {
            //makes paint event
            this.Paint += Form1_Paint;
            //makes keydown event
            this.KeyDown += Form1_KeyDown;
            //makes keyup event
            this.KeyUp += Form1_KeyUp;

            //sets background colour to green
            this.BackColor = Color.Green;

            //Sets the text of the window to "Snake game"
            this.Text = "Snake Game";
            //Sets the height of the window to 900
            this.Height = screenH;
            //Sets the height of the window to 900
            this.Width = screenW;
            //sets minimizebox to false
            this.MaximizeBox = false;
            //centers form
            this.CenterToScreen();
            //events for drawing to screen and keydown events
            this.DoubleBuffered = true;

            //makes scoreboard rectangle
            scoreboard = new Rectangle(0, 0, ClientSize.Width, 50);
            //makes player rectangle
            player = new Rectangle(400, 700, 100, 150);
            //makes enemy rectangle
            enemy = new Rectangle(400, 125, 100, 125);
            //makes net rectangle
            net = new Rectangle(275, 50, 350, 150);
            //makes ball rectangle
            ball = new Rectangle(player.X + 75, player.Y + 80, 50, 50);

            //Creates a timer
            timer = new Timer();
            //Creates method for timer
            timer.Tick += Timer_Tick;
            //Sets the timer interval to 60fps
            timer.Interval = 1000 / 60;
            //Starts timer
            timer.Start();
        }

        //method for keyup event
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //sets playerdx to 0
            playerdx = 0;
        }

        //timer event
        private void Timer_Tick(object sender, EventArgs e)
        {
            //Checks if the left coordinate of the player is less than or equal to 0
            if (player.Left <= 0)
            {
                //Checks if the player is moving left
                if (playerdx == (-7))
                {
                    //Sets dx to 0
                    playerdx = 0;
                }
            }
            //Checks if the right coordinate of the player is greater than or equal to the width of the window
            if (player.Right >= this.ClientSize.Width)
            {
                //Checks if the player is moving right
                if (playerdx == 7)
                {
                    //Sets dx to 0
                    playerdx = 0;
                }
            }

            //gets enemydx value
            getEnemydx();
            //moves player
            player.X += playerdx;
            //moves enemy
            enemy.X += enemydx;

            //checks if ball isnt moving
            if (ballMove == false)
            {
                //moves ball x to player x
                ball.X = player.X + 75;
                //moves ball y to player y
                ball.Y = player.Y + 80;
            }

            //moves ball
            ball.Y += balldy;

            //checks if ball intersects net
            if (ball.IntersectsWith(net))
            {
                //goal method
                goal();
            }
            //checks if ball intersects enemy
            if (ball.IntersectsWith(enemy))
            {
                //blocked method
                blocked();
            }
            //checks if ball misses
            if (ball.Y <= 50)
            {
                //missed method
                missed();
            }
            //if lives is 0
            if (lives <= 0)
            {
                //stops timer
                timer.Stop();
                //shows gameover box
                MessageBox.Show("Game Over\nYou scored " + score + " points!");
                //ends program
                Application.Exit();
            }
            //refreshes screen
            this.Invalidate();
        }

        //method for keydown event
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //checks if right key is pressed
            if (e.KeyCode == Keys.Right)
            {
                //sets playerdx to 7
                playerdx = 7;
            }
            //checks if Left key is pressed
            if (e.KeyCode == Keys.Left)
            {
                //sets playerdx to -7
                playerdx = -7;
            }
            //checks if Space key is pressed
            if (e.KeyCode == Keys.Space)
            {
                //sets balldy to -10
                balldy = -10;
                //sets ballmove to true
                ballMove = true;
            }
        }

        //goal method
        private void goal()
        {
            //ballmove set to false
            ballMove = false;
            //adds to score
            score++;
            //changes screen message to "GOAL!"
            message = "GOAL!";
            //plays sound
            SystemSounds.Hand.Play();
        }
        //missed method
        private void missed()
        {
            //ballmove set to false
            ballMove = false;
            //changes screen message to "MISSED!"
            message = "MISSED!";
            //removes a live
            lives--;
            //plays sound
            SystemSounds.Beep.Play();
        }
        //blocked method
        private void blocked()
        {
            //ballmove set to false
            ballMove = false;
            //changes screen message to "BLOCKED!"
            message = "BLOCKED!";
            //removes a live
            lives--;
            //plays sound
            SystemSounds.Beep.Play();
        }

        //getenemydx method
        private void getEnemydx()
        {
            //Checks if the left coordinate of the enemy is less than or equal to left of net and enemyDx is less than 0
            if (enemy.Left <= net.Left && enemydx < 0)
            {
                //Reverses the direction of the enemy
                enemydx = enemydx * (-1);
            }

            //Checks if the right coordinate of the enemy is greater than or equal to right of net
            if (enemy.Right >= net.Right)
            {
                //Reverses the direction of the enemy
                enemydx = enemydx * (-1);
            }
        }

        //paint event
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //colours scoreboard to darkgreen
            e.Graphics.FillRectangle(Brushes.DarkGreen, scoreboard);
            //paints net with netImage
            e.Graphics.DrawImage(netImage, net);
            //paints player with playerImage
            e.Graphics.DrawImage(playerImage, player);
            //paints enemy with enemyImage
            e.Graphics.DrawImage(enemyImage, enemy);
            //paints ball with soccerBall image
            e.Graphics.DrawImage(soccerBall, ball);
            //draws the score on top right of screen
            e.Graphics.DrawString("Score: " + Convert.ToString(score), new Font("Arial", 16), new SolidBrush(Color.Black), 775, 12);
            //draws message on top of screen
            e.Graphics.DrawString(message, new Font("Arial", 16), new SolidBrush(Color.Black), 425, 12);
            //draws lives on top left of screen
            e.Graphics.DrawString("Lives: " + Convert.ToString(lives), new Font("Arial", 16), new SolidBrush(Color.Black), 0, 12);
        }
    }
}
