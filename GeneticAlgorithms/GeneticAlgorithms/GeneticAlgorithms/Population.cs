using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Population
        {

                public List<DNA> population;
                List<int> ASCII = new List<int> { 32, 33, 39, 44, 46, 63 };
                Random rand = new Random();
                float mutationRate;
                int populationCount;
                int generation;
                string targetPhrase;
                DateTime start;

                public Population(int maxPop, string targetPhrase, float mutationRate)

                {
                        population = new List<DNA>();
                        this.mutationRate = mutationRate;
                        this.targetPhrase = targetPhrase;
                        this.populationCount = maxPop;
                        generation = 0;
                        start = DateTime.Now;

                        for (int i = 65; i <= 90; i++)
                                ASCII.Add(i);
                        for (int i = 97; i <= 122; i++)
                                ASCII.Add(i);

                        for (int i = 0; i < maxPop; i++)
                        {
                                string newGenes = "";
                                for (int j = 0; j < targetPhrase.Length; j++)
                                        newGenes += Convert.ToChar(ASCII[rand.Next(0,ASCII.Count)]);
                                population.Add(new DNA(newGenes));
                                
                        }

                        for (int i = 0; i < 20; i++)
                        {
                                population[i].CalcFit(targetPhrase);
                                Console.WriteLine(population[i].GetGenes() + "   " + population[i].GetFitness());
                        }
                }

                public int GetRandomDNABasedOnFitness()
                {
                        float sum = 0;
                        for (int i = 0; i < population.Count; i++)
                        {
                                sum += population[i].GetFitness();
                        }

                        Random rand = new Random();
                        double value = rand.NextDouble() * sum;
                        float tempSum = 0;

                        for (var i = 0; i < population.Count; i++)
                        {
                                float nextFit = population[i].GetFitness();
                                if (value >= tempSum && value < tempSum + nextFit)
                                        return i;
                                tempSum += nextFit;
                        }
                        return rand.Next(population.Count) ;
                }

                public string Crossover(string parent1, string parent2)
                {
                        string child = "";
                        for (int i = 0; i < parent1.Length; i++)
                        {

                                if (rand.NextDouble() <= mutationRate)
                                {
                                        child += Convert.ToChar(ASCII[rand.Next(ASCII.Count)]);
                                }
                                else
                                {
                                        if (i % 2 == 0)
                                                child += parent1[i];
                                        else
                                                child += parent2[i];
                                }

                        }
                        return child;
                }

                public void Start()
                {
                        while (true)
                        {

                                for (int i = 0; i < population.Count; i++)
                                        if (population[i].GetGenes().Equals(targetPhrase))
                                        {
                                                Console.Clear();
                                                Console.WriteLine(population[i].GetGenes());
                                                return;
                                        }
                                List<DNA> newPopulation = new List<DNA>();
                                for (int i = 0; i < population.Count; i++)
                                {
                                        string parent1 = population[GetRandomDNABasedOnFitness()].GetGenes();
                                        string parent2 = population[GetRandomDNABasedOnFitness()].GetGenes();
                                        newPopulation.Add(new DNA(Crossover(parent1, parent2)));
                                        
                                }
                                generation++;
                                population = newPopulation;

                                float sum = 0;
                                float avarage = 0;
                                float bestVlue = -1;
                                int bestIndex = 0;

                                for (int i = 0; i < population.Count; i++)
                                {
                                        population[i].CalcFit(targetPhrase);
                                        sum += population[i].GetFitness();
                                        if (population[i].GetFitness() > bestVlue)
                                                bestIndex = i;
                                }

                                avarage = sum / population.Count;
                                Console.SetCursorPosition(40, 5);
                                Console.Write("Avarage: " + avarage);
                                Console.SetCursorPosition(40, 6);
                                Console.Write("Best gene: " + population[bestIndex].GetGenes() + " " + population[bestIndex].GetFitness());
                                Console.SetCursorPosition(40, 7);
                                Console.Write("Mutation rate: " + mutationRate);
                                Console.SetCursorPosition(40, 8);
                                Console.Write("Population: " + populationCount);
                                Console.SetCursorPosition(40, 9);
                                Console.Write("Generation: " + generation);
                                Console.SetCursorPosition(40, 10);
                                Console.Write("Target phrase: " + targetPhrase);
                                Console.SetCursorPosition(40, 11);
                                Console.Write("Time: " + (DateTime.Now - start).ToString());


                                for (int i = 0; i < 20; i++)
                                {
                                        if(i == population.Count)
                                                break;

                                        Console.SetCursorPosition(0, i);
                                        Console.WriteLine(population[i].GetGenes() + "   " + population[i].GetFitness());

                                }
                        }
                }
        }
}
