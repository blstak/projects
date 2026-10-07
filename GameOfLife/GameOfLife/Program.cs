using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter width of the field:");
            int w = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("Enter height of the field:");
            int h = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("Enter the amount of living cells in the field:");
            int n = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            bool[,] field = Start(w, h, n);
            Draw(field, w, h);

            GameOfLife(field, w, h);
        }

        static bool[,] Start(int w, int h, int n)
        {
            bool[,] field = new bool[w, h];
            float chance = (float)n / (w * h);
            if (chance < 1)
            {
                chance *= 100000;
            }
            int sa = Convert.ToInt32(Math.Round(Convert.ToDouble(chance), MidpointRounding.AwayFromZero));
            /*
            string[] a = chance.ToString().Split(new char[] { '.' });
            int dec;
            try
            {
                 dec = Convert.ToInt32(a[1]);
            }
            catch
            {
                dec = Convert.ToInt32(Math.Round(Convert.ToDouble(Int32.MaxValue / 11), MidpointRounding.AwayFromZero));
            }
            */
            Random r = new Random();
            int s = 0;
            while (s < n)
            {
                for (int y = 0; y < h && s < n; y++)
                {
                    for (int x = 0; x < w && s < n; x++)
                    {
                        int sfa = r.Next(0, 10000000);
                        if (s < n && sfa <= sa && !field[x, y])
                        {
                            field[x, y] = true;
                            s++;
                        }
                    }
                }
            }
            return field;
        }

        static void GameOfLife(bool[,] field, int w, int h)
        {
            while (true)
            {
                bool[,] nextField = new bool[w, h];
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int n = 0;

                        if (x == 0)
                        {
                            if (y == 0)
                            {
                                if (field[x + 1, y])
                                    n++;
                                if (field[x + 1, y + 1])
                                    n++;
                                if (field[x, y + 1])
                                    n++;

                            }
                            else if (y == h - 1)
                            {
                                if (field[x + 1, y])
                                    n++;
                                if (field[x + 1, y - 1])
                                    n++;
                                if (field[x, y - 1])
                                    n++;
                            }
                            else
                            {
                                if (field[x + 1, y])
                                    n++;
                                if (field[x + 1, y + 1])
                                    n++;
                                if (field[x, y + 1])
                                    n++;
                                if (field[x + 1, y - 1])
                                    n++;
                                if (field[x, y - 1])
                                    n++;
                            }
                        }
                        else if (x == w - 1)
                        {
                            if (y == 0)
                            {
                                if (field[x - 1, y])
                                    n++;
                                if (field[x - 1, y + 1])
                                    n++;
                                if (field[x, y + 1])
                                    n++;

                            }
                            else if (y == h - 1)
                            {
                                if (field[x - 1, y])
                                    n++;
                                if (field[x - 1, y - 1])
                                    n++;
                                if (field[x, y - 1])
                                    n++;
                            }
                            else
                            {
                                if (field[x - 1, y])
                                    n++;
                                if (field[x - 1, y + 1])
                                    n++;
                                if (field[x, y + 1])
                                    n++;
                                if (field[x - 1, y - 1])
                                    n++;
                                if (field[x, y - 1])
                                    n++;
                            }
                        }
                        else
                        {
                            if (y == 0)
                            {
                                if (field[x + 1, y])
                                    n++;
                                if (field[x + 1, y + 1])
                                    n++;
                                if (field[x, y + 1])
                                    n++;
                                if (field[x - 1, y])
                                    n++;
                                if (field[x - 1, y + 1])
                                    n++;

                            }
                            else if (y == h - 1)
                            {
                                if (field[x + 1, y])
                                    n++;
                                if (field[x + 1, y - 1])
                                    n++;
                                if (field[x, y - 1])
                                    n++;
                                if (field[x - 1, y])
                                    n++;
                                if (field[x - 1, y - 1])
                                    n++;
                            }
                            else
                            {
                                if (field[x + 1, y])
                                    n++;
                                if (field[x + 1, y + 1])
                                    n++;
                                if (field[x, y + 1])
                                    n++;
                                if (field[x + 1, y - 1])
                                    n++;
                                if (field[x, y - 1])
                                    n++;
                                if (field[x - 1, y])
                                    n++;
                                if (field[x - 1, y + 1])
                                    n++;
                                if (field[x - 1, y - 1])
                                    n++;
                            }
                        }

                        if (field[x, y] && (n < 2 || n > 3))
                        {
                            nextField[x, y] = false;
                        }
                        else if (!field[x, y] && n == 3)
                        {
                            nextField[x, y] = true;
                        }
                        else
                        {
                            nextField[x, y] = field[x, y];
                        }
                    }
                }
                field = nextField;
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Draw(field, w, h);
            }
        }

        static void Draw(bool[,] field, int w, int h)
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (field[x, y])
                    {
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }
                Console.WriteLine("");
            }
        }

    }
}