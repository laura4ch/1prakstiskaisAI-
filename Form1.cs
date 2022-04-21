using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AI1praktiskais.Player;

namespace AI1praktiskais
{
    public partial class Form1 : Form
    {

        Player realPlayer = new Player(Color.Pink, 0,2);
        Player aiPlayer = new Player(Color.Blue, 4,2);
        GameField field = new GameField();
        public bool aiStartsGame;

        public void RenderField()
        {
            int cellSize = this.gameField.Width / field.width;
            System.Drawing.SolidBrush realBrush = new System.Drawing.SolidBrush(realPlayer.color);
            System.Drawing.SolidBrush aiBrush = new System.Drawing.SolidBrush(aiPlayer.color);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.gameField.CreateGraphics();
            Cell[] cells = field.cells.ToArray();
            for (int i = 0; i < cells.Length; i++)
            {
                Cell cell = cells[i];
                if (cell.owner != null)
                    if (cell.owner.color == realPlayer.color)
                        formGraphics.FillRectangle(realBrush, new Rectangle(cell.x * cellSize, cell.y * cellSize, cellSize, cellSize));
                if (cell.owner != null)
                    if (cell.owner.color == aiPlayer.color)
                        formGraphics.FillRectangle(aiBrush, new Rectangle(cell.x * cellSize, cell.y * cellSize, cellSize, cellSize));
            }

            Cell realPlayerCell = field.cells.Find(cell => cell.x == realPlayer.x && realPlayer.y == cell.y) ;
            Cell aiPlayerCell = field.cells.Find(cell => cell.x == aiPlayer.x && aiPlayer.y == cell.y);

            formGraphics.FillEllipse(aiBrush, new Rectangle(realPlayerCell.x * cellSize + cellSize / 4, realPlayerCell.y * cellSize + cellSize / 4, cellSize / 2, cellSize / 2));
            formGraphics.FillEllipse(realBrush, new Rectangle(aiPlayerCell.x * cellSize + cellSize / 4, aiPlayerCell.y * cellSize + cellSize / 4, cellSize/2, cellSize / 2));
            realBrush.Dispose();
            formGraphics.Dispose();
        }

        public void RenderTurn()
        {
            int cellSize = this.gameField.Width / field.width;
            this.Invalidate(); // Call onPaint
            System.Drawing.SolidBrush realBrush = new System.Drawing.SolidBrush(realPlayer.color);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.gameField.CreateGraphics();
            formGraphics.FillRectangle(realBrush, new Rectangle(realPlayer.x * cellSize, realPlayer.y * cellSize, cellSize, cellSize));
            realBrush.Dispose();
            formGraphics.Dispose();

            //score output
            scoreLabel.Text = " Score " + field.cells.Where(cells => cells.owner == realPlayer).Count().ToString() + " : " + field.cells.Where(cells => cells.owner == aiPlayer).Count().ToString();
        }

        public Form1()
        {
            InitializeComponent();
            // inital owners of cells
            field.cells.Find(cell => cell.x == realPlayer.x && cell.y == realPlayer.y).owner = realPlayer; 
            field.cells.Find(cell => cell.x == aiPlayer.x && cell.y == aiPlayer.y).owner = aiPlayer;
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

            RenderField();
        }


        public void CheckWin()
        {
            if(field.cells.Where(cells => cells.owner == realPlayer).Count() == 14)
            {
                MessageBox.Show("Real user wins! ");
                ResetGame();
               
            }

            if(field.cells.Where(cells => cells.owner == aiPlayer).Count() == 14)
            {
                MessageBox.Show("AI wins! ");
                ResetGame();
            }

        }

        private void upButton_Click(object sender, EventArgs e)
        {
           if ( realPlayer.MoveUp(field, aiPlayer) ){
                RenderTurn();
                AIMove();
            }
            CheckWin();
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            // real player
            if( realPlayer.MoveLeft(field, aiPlayer) )
            {

                RenderTurn();
                AIMove();
            }

            CheckWin();
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            if (realPlayer.MoveRight(field, aiPlayer))
            {
                RenderTurn();
                AIMove();
            }

            CheckWin();
        }

        private void downButton_Click(object sender, EventArgs e)
        {
           if( realPlayer.MoveDown(field, aiPlayer))
            {
                RenderTurn();
                AIMove();
            }
            CheckWin();
        }


        protected void AIMove()
        {
            GameTree gameTree = aiPlayer.BuildGameTree(field, aiPlayer, realPlayer); 
            string bestMove = gameTree.bestMove;
            if (bestMove == Directions.Down) aiPlayer.MoveDown(field, realPlayer);
            if (bestMove == Directions.Up) aiPlayer.MoveUp(field, realPlayer);
            if (bestMove == Directions.Left) aiPlayer.MoveLeft(field, realPlayer);
            if (bestMove == Directions.Right) aiPlayer.MoveRight(field, realPlayer);
            this.Invalidate();
            RenderTurn();
        }

        public void ResetGame() {
            System.Drawing.Graphics formGraphics;
            formGraphics = this.gameField.CreateGraphics();
            formGraphics.Clear(this.gameField.BackColor);
            this.realPlayer = new Player(Color.Pink, 0, 2);
            this.aiPlayer = new Player(Color.Blue, 4,2);
            this.field = new GameField();
            field.cells.Find(cell => cell.x == realPlayer.x && cell.y == realPlayer.y).owner = realPlayer;
            field.cells.Find(cell => cell.x == aiPlayer.x && cell.y == aiPlayer.y).owner = aiPlayer;

            RenderField();
          

        }


        private void resetButton_Click(object sender, EventArgs e)
        {

            ResetGame();
            Splatoon_Load(sender, e);
        }


        
    }
}
