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
    class Animate
    {
        /// <summary>
        /// Display startup animation.
        /// </summary>
        public static void StartupAnim()
        {
            //Time between a new line gets printed in milliseconds
            int timeBetweenSlides = 100;

            //Animation margin
            int marginX = 0;
            int marginY = 8;

            Console.Clear();

            //Print animation from .txt file
            using (var startupAnim = new StreamReader("StartupAnim.txt"))
            {
                while (!startupAnim.EndOfStream)
                {
                    Console.SetCursorPosition(marginX, marginY);

                    //Print first part of animation 
                    for (var i = 0; i < 6; ++i)
                    {
                        Console.WriteLine("                             {0}", startupAnim.ReadLine());
                        Thread.Sleep(timeBetweenSlides);
                    }

                    Thread.Sleep(800);
                    Console.Clear();
                    Console.SetCursorPosition(marginX, marginY);

                    //Print second part of animation 
                    for (var i = 0; i < 6; ++i)
                    {
                        Console.WriteLine("                                                  {0}", startupAnim.ReadLine());
                        Thread.Sleep(timeBetweenSlides);
                    }
                }
            }

            Thread.Sleep(2000);
            Console.Clear();
        }

        /// <summary>
        /// Bring up the main menu.
        /// </summary>
        public static void MainMenuAnim()
        {
            Console.Clear();

            //Time between a new line gets printed in milliseconds
            int timeBetweenSnakeSlides = 100;

            //Print game title 
            using (var snakeAnim = new StreamReader("TitleAnim.txt"))
            {
                while (!snakeAnim.EndOfStream)
                {
                    Console.WriteLine("             {0}", snakeAnim.ReadLine());
                    Thread.Sleep(timeBetweenSnakeSlides);
                }
            }

            //Print title and underline animation

            Console.SetCursorPosition(14, 16);

            for (int i = 0; i <= 86; ++i)
            {
                Console.Write("#");
                Thread.Sleep(10);
            }

            Console.WriteLine("\n\n");

            //Print available options on home menu
            using (var optionsAnim = new StreamReader("HomeAnim.txt"))
            {
                while (!optionsAnim.EndOfStream)
                {
                    Console.WriteLine(optionsAnim.ReadLine());
                }
            }
        }

        /// <summary>
        /// Display Game Over animation.
        /// </summary>
        public static void GameOverAnim()
        {
            Console.Clear();
            var commonX = 55;

            //Print text, score and steps
            using (var gameOverAnim = new StreamReader("GameOverAnim.txt"))
            {
                while (!gameOverAnim.EndOfStream)
                {
                    Console.WriteLine("               {0}",gameOverAnim.ReadLine());
                    Thread.Sleep(40);
                }
            }

            Console.SetCursorPosition(commonX, 12);
            Console.WriteLine(" {0}", RenderEngine.GetGameStat(1));
            Console.SetCursorPosition(commonX , 13);
            Console.Write(" ^^^^^^");

            Console.SetCursorPosition(commonX, 17);
            Console.WriteLine(" {0}", RenderEngine.GetGameStat(2));
            Console.SetCursorPosition(commonX, 18);
            //Binary message
            Console.Write(" ^^^^^^\n\n\n\n\nBinary message : 01011001 01101111 01110101 01101000 01100001 01110110 01100101 01110100 01101111 01101101 01110101\n" +
                          "                 01100011 01101000 01110100 01101001 01101101 01100101 ");

            Thread.Sleep(5000);
        }

        /// <summary>
        /// Bring up the settings menu.
        /// </summary>
        public static void SettingsAnim()
        {
            Console.Clear();

            using (var settingsAnim = new StreamReader("SettingsAnim.txt"))
            {
                while (!settingsAnim.EndOfStream)
                {
                    Console.WriteLine(settingsAnim.ReadLine());
                }
            }
            //Write selector
            Console.SetCursorPosition(24, 11);
            Console.WriteLine(">");
            _getSettings();

        }

        /// <summary>
        /// Enter selected menu from main screen.
        /// 1 - Start, 2 - Exit, 3 - Settings
        /// </summary>
        /// <returns>1 - Start, 2 - Exit, 3 - Settings</returns>
        public static int MenuSelector()
        {
            //Button coordinants 
            int enterX = 0;
            int exitX = 57;
            int settingsX = 98;
            int commonY = 26;

            //Selector states, 1 - Start, 2 - Exit, 3 - Settings
            //Initiate selector to the first position
            int selector = 1;

            //Flush all pressed keys
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
            }

            while (true)
            {
                //Read pressed key
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    //Incrament selector on apropriat key pres
                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            return selector;

                        case var x when x == ConsoleKey.D || x == ConsoleKey.RightArrow:
                            selector %= 3;
                            ++selector;
                            break;

                        case var x when x == ConsoleKey.A || x == ConsoleKey.LeftArrow:

                            if ((selector - 1) < 1)
                            {
                                selector = 3;
                            }
                            else
                            {
                                --selector;
                            }
                            break;

                        default:
                            break;
                    }
                }

                //Reposition selector and clean the old one
                if (selector == 1)
                {
                    Console.SetCursorPosition(exitX, commonY);
                    Console.WriteLine("                               ");
                    Console.SetCursorPosition(settingsX, commonY);
                    Console.WriteLine("                         ");
                    Console.SetCursorPosition(enterX, commonY);
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
                }
                else if (selector == 2)
                {
                    Console.SetCursorPosition(enterX, commonY);
                    Console.WriteLine("                                           ");
                    Console.SetCursorPosition(settingsX, commonY);
                    Console.WriteLine("                         ");
                    Console.SetCursorPosition(exitX, commonY);
                    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
                }
                else if (selector == 3)
                {
                    Console.SetCursorPosition(exitX, commonY);
                    Console.WriteLine("                               ");
                    Console.SetCursorPosition(enterX, commonY);
                    Console.WriteLine("                                           ");
                    Console.SetCursorPosition(settingsX, commonY);
                    Console.WriteLine("^^^^^^^^^^^^^^^^^");
                }
            }
        }

        /// <summary>
        /// Select and change options in the settings menu.
        /// </summary>
        public static void SettingsSelector()
        {
            var selector = 1;
            var commonX = 24;
            var cursorMoved = false;

            while (selector != 0)
            {
                cursorMoved = false;

                //Read pressed key
                if (Console.KeyAvailable)
                {
                    cursorMoved = true;
                    var key = Console.ReadKey(true).Key;

                    //Incrament selector on apropriat key pres
                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            //Exit settings
                            if (selector == 4)
                            {
                                selector = 0;
                            }
                            else
                            {
                                _settingsChanger(selector);
                            }
                            break;

                        case var x when x == ConsoleKey.S || x == ConsoleKey.DownArrow:

                            selector = (selector + 1) % 5;

                            if (selector == 0)
                            {
                                selector = 1;
                            }

                            break;

                        case var x when x == ConsoleKey.W || x == ConsoleKey.UpArrow:


                            if (selector - 1 == 0)
                            {
                                selector = 4;
                            }
                            else
                            {
                                --selector;
                            }
                            break;

                        default:
                            break;
                    }
                }

                //Set the cursor position
                if (cursorMoved)
                {
                    Console.SetCursorPosition(commonX, 11);
                    Console.WriteLine(" ");
                    Console.SetCursorPosition(commonX, 13);
                    Console.WriteLine(" ");
                    Console.SetCursorPosition(commonX, 15);
                    Console.WriteLine(" ");
                    Console.SetCursorPosition(commonX, 17);
                    Console.WriteLine(" ");

                    if (selector == 1)
                    {
                        Console.SetCursorPosition(commonX, 11);
                    }
                    else if (selector == 2)
                    {
                        Console.SetCursorPosition(commonX, 13);
                    }
                    else if (selector == 3)
                    {
                        Console.SetCursorPosition(commonX, 15);
                    }
                    else if (selector == 4)
                    {
                        Console.SetCursorPosition(commonX, 17);
                    }

                    Console.WriteLine(">");
                }

            }
        }

        /// <summary>
        /// Change selected element in settings.
        /// </summary>
        /// <param name="element"></param>
        static void _settingsChanger(int element)
        {
            bool changing = true;
            int defaultIncramentCoordinants = 2;
            int defaultIncramentSnakeSpeed = 50;

            while (changing)
            {
                //Set to false, to not update console every loop
                bool changed = false;

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    //Set to true, to update console with changes
                    changed = true;

                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            //Exit current selection
                            changing = false;
                            break;

                            //Change selected item
                        case var x when x == ConsoleKey.S || x == ConsoleKey.DownArrow:
                            if (element == 1 || element == 2)
                            {
                                RenderEngine.Settings(element, -defaultIncramentCoordinants);
                            }
                            else
                            {
                                RenderEngine.Settings(element, +defaultIncramentSnakeSpeed);
                            }
                            break;

                        case var x when x == ConsoleKey.W || x == ConsoleKey.UpArrow:
                            if (element == 1 || element == 2)
                            {
                                RenderEngine.Settings(element, +defaultIncramentCoordinants);
                            }
                            else
                            {
                                RenderEngine.Settings(element, -defaultIncramentSnakeSpeed);
                            }
                            break;

                        default:
                            break;
                    }
                }

                if (changed)
                {
                    _getSettings();
                }
            }
        }

        /// <summary>
        /// Get current settings and print them.
        /// </summary>
        static void _getSettings()
        {
            var commonX = 25;

            Console.SetCursorPosition(commonX, 11);
            Console.WriteLine("       ");
            Console.SetCursorPosition(commonX, 11);
            Console.WriteLine(RenderEngine.GetSetting(1));

            Console.SetCursorPosition(commonX, 13);
            Console.WriteLine("       ");
            Console.SetCursorPosition(commonX, 13);
            Console.WriteLine(RenderEngine.GetSetting(2));

            Console.SetCursorPosition(commonX, 15);
            Console.WriteLine("       ");
            Console.SetCursorPosition(commonX, 15);
            Console.WriteLine(RenderEngine.GetSetting(3));

        }
    }
}
