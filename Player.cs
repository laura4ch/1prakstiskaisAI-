using System;
using System.Collections.Generic;
using System.Text;

namespace AI1praktiskais
{
    public class Player
    {

        struct GameTree
        {
            public GameField field;
            public List<Cell> cells;
            public int bestScore;
            public Player aiPlayer, realPlayer;
            


            public void Maximize()
            {
                bestScore = 10;
            }

            public void Minimize()
            {
                bestScore = -10;
            }

        }

        public void BuildGameTree(GameField field, Player aiPlayer, Player realPlayer)
        {

        }

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


        public void Minimax(GameField field, Player aiPlayer, Player realPlayer, Cell cells)
        {
       

            //Field ierobezojumi
            if (aiPlayer.x<=field.height || aiPlayer.y <= field.width || aiPlayer.x >= field.height || aiPlayer.y >= field.width)
            {
                //AI look for another way
            }

            

        }
        

    }


}
