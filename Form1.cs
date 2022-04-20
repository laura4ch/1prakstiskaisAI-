using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI1praktiskais
{
    public partial class Form1 : Form
    {

        Player realPlayer = new Player(Color.Pink, 0, 0, 2);
        Player aiPlayer = new Player(Color.Blue, 0, 4, 2);
        GameField field = new GameField();
        public bool aiStartsGame;
        

        public void RenderTurn()
        {
            System.Drawing.SolidBrush realBrush = new System.Drawing.SolidBrush(realPlayer.color);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.gameField.CreateGraphics();
            formGraphics.FillRectangle(realBrush, new Rectangle(realPlayer.x * 100, realPlayer.y * 100, 100, 100));
            realBrush.Dispose();
            formGraphics.Dispose();

            //score output
            scoreLabel.Text = " Score " + field.cells.Where(cells => cells.owner == realPlayer).Count().ToString() + " : " + field.cells.Where(cells => cells.owner == aiPlayer).Count().ToString();
        }

        public Form1()
        {
            InitializeComponent();
        
        }

        private void Splatoon_Load(object sender, EventArgs e)
        {

            var result = MessageBox.Show("Will you start new game?( If no, then computer starts game.)", "Computer", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                AIMove();
                aiStartsGame = true;
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
                

              // initial coord for real player
               RenderTurn();

               //initial coord for ai player
               System.Drawing.Graphics formGraphics;
               System.Drawing.SolidBrush aiBrush = new System.Drawing.SolidBrush(aiPlayer.color);
               formGraphics = this.gameField.CreateGraphics();
               formGraphics.FillRectangle(aiBrush, new Rectangle(400, 200, 100, 100));
               formGraphics.FillRectangle(aiBrush, new Rectangle(aiPlayer.x * 100, aiPlayer.y * 100, 100, 100));
               aiBrush.Dispose();
               formGraphics.Dispose();


        }


     

        private void upButton_Click(object sender, EventArgs e)
        {
            realPlayer.y = realPlayer.y - 1;
            this.field.cells.Find(cell => cell.x == realPlayer.x && cell.y == realPlayer.y).owner = realPlayer;
            RenderTurn();
            AIMove();
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            // real player
            realPlayer.x = realPlayer.x - 1;
            this.field.cells.Find(cell => cell.x == realPlayer.x && cell.y == realPlayer.y).owner = realPlayer;
            RenderTurn();
            AIMove();
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            realPlayer.x = realPlayer.x + 1;
            this.field.cells.Find(cell => cell.x == realPlayer.x && cell.y == realPlayer.y).owner = realPlayer;
            //this.field.cells.Find(cell => cell.x == realPlayer.x && cell.y == realPlayer.y).isEmpty = false;
            RenderTurn();
            AIMove();
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            realPlayer.y = realPlayer.y + 1;
            this.field.cells.Find(cell => cell.x == realPlayer.x && cell.y == realPlayer.y).owner = realPlayer;
            RenderTurn();
            AIMove();
        }


        protected void AIMove()
        {
            if (aiPlayer.x != 0)
                aiPlayer.x = aiPlayer.x - 1;
            this.field.cells.Find(cell => cell.x == aiPlayer.x && cell.y == aiPlayer.y).owner = aiPlayer;
            System.Drawing.SolidBrush aiBrush = new System.Drawing.SolidBrush(aiPlayer.color);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.gameField.CreateGraphics();
            formGraphics.FillRectangle(aiBrush, new Rectangle(aiPlayer.x * 100, aiPlayer.y * 100, 100, 100));
            aiBrush.Dispose();
            formGraphics.Dispose();
        }

    }
}
