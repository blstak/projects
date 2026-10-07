using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
        class Program
        {
                static void Main(string[] args)
                {
                        Population pop = new Population(1500, "genes", 0.015f);
                        pop.Start();
                        Console.ReadLine();
                }
        }
}
