using System;
using System.Collections.Generic;
using System.Text;

namespace AI1praktiskais
{
    public class GameField
    {
        public int height = 5, width = 5;
        public List<Cell> cells;

        public GameField()
        {

            cells = new List<Cell>();
            for(int i = 0; i < height; i++) {
                for(int j=0;j< width;j++) cells.Add(new Cell(i,j));

            }
        }

    }

    public class Cell
    {
        public int x, y;
        public bool isEmpty;
        public Player owner;


        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            isEmpty = true;
        }
    } 
}
