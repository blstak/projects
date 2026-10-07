using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
        class DNA
        {
                string genes;
                float fitness;

                public DNA(string genes)
                {
                        this.genes = genes;
                }

                public string GetGenes()
                {
                        return genes;
                }
                
                public void SetGenes(string newGenes)
                {

                        genes = newGenes;
                }

                public float GetFitness()
                {
                        return fitness;
                }

                public void CalcFit(string target)
                {
                        fitness = 0;
                        int count = 0;
                        var array = target.ToCharArray();
                        for (int i = 0; i < array.Length; i++)
                        {
                                if (genes[i] == array[i])
                                        count++;
                        }

                        fitness = (float) count / (float) genes.Length;
                        fitness *= fitness * fitness * fitness;
                }

        }
}
