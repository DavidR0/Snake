using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace Snake
{
    class RenderEngine
    {
        static bool _isStarting = true;
        static bool _noApple = true;
        static Random _random = new Random();
        static bool _gameOver;
        static int _scoreCount;
        static int _stepCount;

        //Points in which we hold the coordinants for the snake head and apple
        static Point _head;
        static Point _apple;

        //Settings variables 
        static int _boardSizeX = 30;
        static int _boardSizeY = 26;
        static int _snakeSpeed = 200;

        //Linked list holds the snake
        static LinkedList<Point> _snake = new LinkedList<Point>();

        /// <summary>
        /// Set the initial snake position on bord.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetSnakeHead(int x, int y)
        {
            _head.X = x;
            _head.Y = y;
            _addBlock(_head.X, _head.Y);
        }

        /// <summary>
        /// Move snake in the set direction. W - 0, A - 1, S - 2, D - 3
        /// </summary>
        /// <param name="setDirection"></param>
        private static void _snakeHead(int setDirection)
        {
            //Set direction in which the snake head to move
            switch (setDirection)
            {
                //W = 0, A = 1, S = 2, D = 3

                case 0:
                    //Making sure we aren't trying to move in the oppsite direction in which we are going
                    if (setDirection != 2)
                    {
                        _head.Y -= 1;
                    }
                    break;
                case 1:
                    if (setDirection != 3)
                    {

                        _head.X -= 1;
                    }
                    break;
                case 2:
                    if (setDirection != 0)
                    {

                        _head.Y += 1;
                    }
                    break;
                case 3:
                    if (setDirection != 1)
                    {
                        _head.X += 1;
                    }
                    break;

                default:
                    break;
            }
            //Check if snake hit a wall or ate itself
            if (_head.X < 1 || _head.Y < 1 || _head.Y > _boardSizeY - 1 || _head.X > _boardSizeX || _isSnake(_head.X, _head.Y))
            {
                _resetBord();
                //End current game
                _gameOver = true;
                //Return snake inside the board
                _head.X = 1;
                _head.Y = 1;
            }

            //Check if snake ate an apple
            if (_head.X == _apple.X && _head.Y == _apple.Y)
            {
                _addBlock(_head.X, _head.Y);
                //Set the head to the new position
                Console.SetCursorPosition(_head.X, _head.Y);
                //Making sure we know we don't have a apple on the board
                _noApple = true;
                //No need to clean the snake tail because we just ate a apple
                ++_scoreCount;
                _inGameStats();
            }
            else
            {
                _addBlock(_head.X, _head.Y);
                //Set the head to the new position
                Console.SetCursorPosition(_head.X, _head.Y);
                _cleanBoard();
            }

            //Set the snake speed
            Thread.Sleep(_snakeSpeed);
        }

        /// <summary>
        /// Reset the bord for the next game.
        /// </summary>
        private static void _resetBord()
        {
            _isStarting = true;
            //Deletes the _snake
            _snake.Clear();
        }

        /// <summary>
        /// Add new block to the snake and print it in the given (x,y) coordinants.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void _addBlock(int x, int y)
        {
            Point def = new Point();
            def.X = x;
            def.Y = y;
            _snake.AddFirst(def);
            Console.SetCursorPosition(x, y);
            Console.Write("#");
        }

        /// <summary>
        /// Main game loop, to be called in program.
        /// </summary>
        /// <param name="currentDirection"></param>
        public static void Game(int currentDirection)
        {
            if (_isStarting)
            {
                Console.Clear();
                _arena();
                _addApple();
                //Making sure we don't get in this "if" again, in the current game
                _isStarting = false;
                _gameOver = false;
                _scoreCount = 0;
                _stepCount = 0;
                _inGameStats();
            }
            ++_stepCount;
            _snakeHead(currentDirection);

            //Checking if we have an apple on the board
            if (_noApple)
            {
                _addApple();
            }
        }

        /// <summary>
        /// Add apple in random open position.
        /// </summary>
        static void _addApple()
        {
            int x, y;

            //Create random coordinants
            x = _random.Next(1, _boardSizeX);
            y = _random.Next(1, _boardSizeY);

            //Check if created coordinants are available
            while (_isSnake(x, y))
            {
                x = _random.Next(1, _boardSizeX);
                y = _random.Next(1, _boardSizeY);
            }

            //Set apple in the created coordinants
            _apple.X = x;
            _apple.Y = y;

            Console.SetCursorPosition(x, y);
            Console.Write("*");

            //Set _noApple to false so we know there is a apple on the bord
            _noApple = false;
        }

        /// <summary>
        /// Check if there is a snake in the way.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if there is a snake in (x,y)</returns>
        private static bool _isSnake(int x, int y)
        {
            foreach (var node in _snake)
            {
                if (node.X == x && node.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Clean the leftover snake tail.
        /// </summary>
        private static void _cleanBoard()
        {
            Point tail = new Point();
            //Get coordinants of last item in _snake tail
            tail = _snake.Last();
            Console.SetCursorPosition(tail.X, tail.Y);
            //Remove it from board
            Console.Write(" ");
            //Remove it from linked list
            _snake.RemoveLast();
        }

        /// <summary>
        /// Build the arena to the set size.
        /// </summary>
        private static void _arena()
        {
            //Build the vertical walls
            for (int i = 0; i <= _boardSizeY; ++i)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("█");
                Console.SetCursorPosition(_boardSizeX + 1, i);
                Console.WriteLine("█");
            }
            //Build the horizontal walls
            for (int i = 1; i <= _boardSizeX; ++i)
            {
                Console.SetCursorPosition(i, _boardSizeY);
                Console.WriteLine("█");
                Console.SetCursorPosition(i, 0);
                Console.WriteLine("█");
            }
            //Print game info
            Console.SetCursorPosition(_boardSizeX + 5, 7);
            Console.WriteLine("Score      :  ");
            Console.SetCursorPosition(_boardSizeX + 5, 9);
            Console.WriteLine("Steps Made :  ");

            //Print animation
            using (var inGameAnim = new StreamReader("InGameAnim.txt"))
            {
                var i = 0;
                while (!inGameAnim.EndOfStream)
                {
                    Console.SetCursorPosition(_boardSizeX + 5, i);
                    Console.WriteLine(inGameAnim.ReadLine());
                    ++i;
                }
            }
            Console.SetCursorPosition(_boardSizeX + 5, 6);

            for (var i = _boardSizeX + 5; i < _boardSizeX + 29; ++i)
            {
                Console.Write("*");
            }
        }

        /// <summary>
        /// Check if game over.
        /// </summary>
        /// <returns>gameOver. true/false </returns>
        public static bool GameOver()
        {
            return _gameOver;
        }

        /// <summary>
        /// Start the game.
        /// </summary>
        /// <param name="start"></param>
        public static void StartGame(bool start)
        {
            _gameOver = !start;
        }

        /// <summary>
        /// Display in game properties.
        /// </summary>
        private static void _inGameStats()
        {
            var commonX = _boardSizeX + 19;

            Console.SetCursorPosition(commonX, 7);
            Console.WriteLine(_scoreCount);
            Console.SetCursorPosition(commonX, 9);
            Console.WriteLine(_stepCount);
        }

        /// <summary>
        /// Change element by given incrament.  1 - BoardSizeX, 1 - BoardSizeY, 2 - SnakeSpeed
        /// </summary>
        /// <param name="element"></param>
        /// <param name="incrament"></param>
        public static void Settings(int element, int incrament)
        {
            // 1 - BoardSizeX, 1 - BoardSizeY, 2 - SnakeSpeed
            switch(element)
            {
                case 1:
                    if(_boardSizeX + incrament > 5 && _boardSizeX + incrament <= 80)
                    {
                        _boardSizeX += incrament;
                    }
                    break;

                case 2:
                    if(_boardSizeY + incrament > 5 && _boardSizeY + incrament <= 35)
                    {
                        _boardSizeY += incrament;
                    }
                    break;

                case 3:
                    if(_snakeSpeed + incrament >= 20 && _snakeSpeed + incrament <= 3000)
                    {
                        _snakeSpeed += incrament;
                    }
                    break;
            }

        }

        /// <summary>
        /// Get current setting. 1 - BoardSizeX, 1 - BoardSizeY, 2 - SnakeSpeed
        /// </summary>
        public static int GetSetting(int element)
        {
            // 1 - BoardSizeX, 1 - BoardSizeY, 2 - SnakeSpeed
            switch (element)
            {
                case 1:
                   return _boardSizeX ;

                case 2:
                    return _boardSizeY ;

                case 3:
                   return  _snakeSpeed;  
            }

            return -1;
        }

        /// <summary>
        /// Get game stat. 1 - score, 2 - steps
        /// </summary>
        /// <param name="stat"></param>
        /// <returns>stat</returns>
        public static int GetGameStat(int stat)
        {
            switch(stat)
            {
                case 1:
                    return _scoreCount;
                case 2:
                    return _stepCount;
            }
            return -1;
        }
    }
}
