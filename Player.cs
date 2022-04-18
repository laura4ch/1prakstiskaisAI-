using System;
using System.Collections.Generic;
using System.Text;

namespace AI1praktiskais
{
    public class Player
    {
        public Player(System.Drawing.Color color, int score, int x, int y)
        {
            this.color = color; 
            this.score = score;
            this.x = x;
            this.y = y;
        }

        
        public System.Drawing.Color color;
        public int x, y;
        public int score;
        

    }


}
