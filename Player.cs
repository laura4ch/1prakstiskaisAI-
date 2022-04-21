using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI1praktiskais
{
    static class Directions
    {
        public const string Up = "Top";
        public const string Left = "Left";
        public const string Right = "Right";
        public const string Down = "Down";
    }

    public class Player
    {
        
        public struct GameTree
        {
            public GameField field;
            public double bestScore;
            public string bestMove;
            public Player aiPlayer, realPlayer;
            public List<GameTree> childGameTrees;
            public string direction;
            public int level;
            public bool isAITurn; 

        }

        public GameTree BuildGameTree(GameField field, Player aiPlayer, Player realPlayer)
        {
            return CreateMinimax(field, aiPlayer, realPlayer, "", 0);
        }

        public Player(System.Drawing.Color color, int score, int x, int y)
        {
            this.color = color;
            this.score = score;
            this.x = x;
            this.y = y;
        }

        public Player(Player player )
        {
            this.color = player.color;
            this.score = player.score;
            this.x = player.x;
            this.y = player.y;
        }
        public System.Drawing.Color color;
        public int x, y;
        public int score;

        protected bool CanMoveDirection(GameField gameField, Player secondPlayer, int x, int y)
        {
            return x >= 0 && x < gameField.width && y >= 0 && y < gameField.height && !(y == secondPlayer.y && x == secondPlayer.x);
        }
        protected double GetPointFromMove(GameField gameField, int x, int y, Player player)
        {
            int cellsOwned = gameField.cells.Where(cell => cell.owner == player).Count();
            Cell cell = gameField.cells.Find(cell => cell.x == x && cell.y == y);
            if (cell.owner == null)
            {
                return 10 + cellsOwned;
            }
            if (cell.owner == player)
            {
                return -10 + cellsOwned;
            }
            return 0 + cellsOwned;
        }
        protected double GetPointFromDirection(GameField gameField, Player player, string direction)
        {
            if (direction == Directions.Left) return GetPointFromMove(gameField, player.x-1, player.y, player);
            if (direction == Directions.Up) return GetPointFromMove(gameField, player.x, player.y -1, player);
            if (direction == Directions.Down) return GetPointFromMove(gameField, player.x, player.y + 1, player);
            if (direction == Directions.Right) return GetPointFromMove(gameField, player.x + 1, player.y, player);
            return 0;
        }

        protected bool Move(GameField gameField, Player secondPlayer, int xOffset, int yOffset)
        {
            int nextMoveX = this.x + xOffset;
            int nextMoveY = this.y + yOffset;
            if (CanMoveDirection(gameField, secondPlayer, nextMoveX, nextMoveY))
            {
                this.x = nextMoveX;
                this.y = nextMoveY;
                gameField.cells.Find(cell => cell.x == this.x && cell.y == this.y).owner = this;
                return true;
            }
            return false;
        }

        public bool MoveLeft(GameField gameField, Player secondPlayer)
        {
            return this.Move(gameField, secondPlayer, -1, 0);
        }
        public bool MoveRight(GameField gameField, Player secondPlayer)
        {
            return this.Move(gameField, secondPlayer, 1, 0);
        }
        public bool MoveUp(GameField gameField, Player secondPlayer)
        {
            return this.Move(gameField, secondPlayer, 0 , -1);
        }
        public bool MoveDown(GameField gameField, Player secondPlayer)
        {
            return this.Move(gameField, secondPlayer, 0, 1);
        }

        private GameTree CreateMinimax(GameField field, Player aiPlayer, Player realPlayer, string direction, int level)
        {
            GameTree gameTree = new GameTree();
            gameTree.aiPlayer = aiPlayer;
            gameTree.realPlayer = realPlayer;
            gameTree.field = field;
            gameTree.childGameTrees = new List<GameTree>();
            gameTree.direction = direction;
            gameTree.level = level;
            Player firstPlayer;
            Player secondPlayer;
            // if level is even, then maximize and its pc turn
            if (!Convert.ToBoolean(level % 2))
            {
                firstPlayer = aiPlayer;
                secondPlayer = realPlayer;
            } else
            {
                firstPlayer = realPlayer;
                secondPlayer = aiPlayer;
                gameTree.isAITurn = true;
            }

            if (level == 7)
            {
                // get potential coordinates
                int newX = secondPlayer.x;
                int newY = secondPlayer.y;
                gameTree.bestScore = GetPointFromMove(field, newX, newY, aiPlayer);
                return gameTree;
            }
            List<string> directions = new List<string>();

            if (CanMoveDirection(field, secondPlayer, firstPlayer.x + 1, firstPlayer.y)) directions.Add(Directions.Right);
            if (CanMoveDirection(field, secondPlayer, firstPlayer.x - 1, firstPlayer.y)) directions.Add(Directions.Left);
            if (CanMoveDirection(field, secondPlayer, firstPlayer.x, firstPlayer.y - 1)) directions.Add(Directions.Up);
            if (CanMoveDirection(field, secondPlayer, firstPlayer.x, firstPlayer.y + 1)) directions.Add(Directions.Down);

            string[] directionsArray = directions.ToArray();

            int oldX = firstPlayer.x;
            int oldY = firstPlayer.y;
            int oldX2 = secondPlayer.x;
            int oldY2 = secondPlayer.y;
            for (int i = 0; i < directionsArray.Length; i++)
            {

                if (directions[i] == Directions.Up)
                {
                    GameField newGameField = new GameField(field);
                    firstPlayer.MoveUp(newGameField, firstPlayer);
                    gameTree.childGameTrees.Add(CreateMinimax(newGameField, aiPlayer, realPlayer, Directions.Up, level + 1));
                }
                firstPlayer.x = oldX;
                firstPlayer.y = oldY;
                secondPlayer.x = oldX2;
                secondPlayer.y = oldY2;

                if (directions[i] == Directions.Down)
                {
                    GameField newGameField = new GameField(field);
                    firstPlayer.MoveDown(newGameField, firstPlayer);
                    gameTree.childGameTrees.Add(CreateMinimax(newGameField, aiPlayer, realPlayer, Directions.Down, level + 1));
                }
                firstPlayer.x = oldX;
                firstPlayer.y = oldY;
                secondPlayer.x = oldX2;
                secondPlayer.y = oldY2;

                if (directions[i] == Directions.Left)
                {
                    GameField newGameField = new GameField(field);
                    firstPlayer.MoveLeft(newGameField, firstPlayer);
                    gameTree.childGameTrees.Add(CreateMinimax(newGameField, aiPlayer, realPlayer, Directions.Left, level + 1));
                }
                firstPlayer.x = oldX;
                firstPlayer.y = oldY;
                secondPlayer.x = oldX2;
                secondPlayer.y = oldY2;

                if (directions[i] == Directions.Right)
                {
                    GameField newGameField = new GameField(field);
                    firstPlayer.MoveRight(newGameField, firstPlayer);
                    gameTree.childGameTrees.Add(CreateMinimax(newGameField, aiPlayer, realPlayer, Directions.Right, level + 1));
                }
                firstPlayer.x = oldX;
                firstPlayer.y = oldY;
                secondPlayer.x = oldX2;
                secondPlayer.y = oldY2;

            }
            double bestScore;

            bestScore = gameTree.childGameTrees.Max(tree => tree.bestScore);
            if(level == 0)
            {
                level = 0;
            }
            gameTree.bestScore = bestScore;
            gameTree.bestMove = gameTree.childGameTrees.Find(tree=> tree.bestScore == bestScore).direction;
            return gameTree;
        }



    }


}
