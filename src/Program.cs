using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    static class Program
    {
        //Setup needed to prevent, resizing, minimizing and maximizing console window 
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();


        //Needed variable declarations
        static int _gameState = 1;
        static int _currentDirection;

        /// <summary>
        /// Main program loop
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //*****************Setup Stuff*****************
            Console.CursorVisible = false;
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }

            var origWidth = Console.WindowWidth;
            var origHeight = Console.WindowHeight;

            System.Console.SetWindowSize(origWidth, origHeight + 6);
            System.Console.SetBufferSize(origWidth, origHeight + 6);

        
            //*****************Animation Start*****************
            Animate.StartupAnim();

            //*****************Starts the main loop*****************
            while (_gameState != 2)
            {
                //Animate main menu
                Animate.MainMenuAnim();
                //Get menu selection
                _gameState = Animate.MenuSelector();

                //Selector states. 1 - Start, 2 - Exit, 3 - Settings
                if (_gameState == 1)
                {
                    //PreGame start setup
                    _currentDirection = 3;
                    RenderEngine.SetSnakeHead(1, 1);
                    RenderEngine.StartGame(true);
                }
                else if(_gameState == 2)
                {
                    RenderEngine.StartGame(false);
                }
                else
                {
                    RenderEngine.StartGame(false);
                    Animate.SettingsAnim();
                    Animate.SettingsSelector();
                }
                
                //Start game if it isn't over
                while (!RenderEngine.GameOver())
                {
                    _readKeys();
                    RenderEngine.Game(_currentDirection);

                    if(RenderEngine.GameOver())
                    {
                        Animate.GameOverAnim();
                    }
                }
            }
        }

        /// <summary>
        /// Reads the current pressed key.
        /// </summary>
        static void _readKeys()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = default;

                //Flush out all but last, pressed keys
                while (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true).Key;
                }

                //W = 0, A = 1, S = 2, D = 3
                switch (key)
                {
                    case var x when x == ConsoleKey.W || x == ConsoleKey.UpArrow:
                        if (_currentDirection != 2)
                        {
                            _currentDirection = 0;
                        }
                        break;
                    case var x when x == ConsoleKey.A || x == ConsoleKey.LeftArrow:
                        if (_currentDirection != 3)
                        {
                            _currentDirection = 1;
                        }
                        break;
                    case var x when x == ConsoleKey.S || x == ConsoleKey.DownArrow:
                        if (_currentDirection != 0)
                        {
                            _currentDirection = 2;
                        }
                        break;
                    case var x when x == ConsoleKey.D || x == ConsoleKey.RightArrow:
                        if (_currentDirection != 1)
                        {
                            _currentDirection = 3;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
