# Genetic Algorithm (C#)

A console application that uses a genetic algorithm to evolve a population of random strings until one matches a target phrase. Everything is written from scratch in C#, with no external libraries.

## What it does
The program starts with 1,500 random strings of the same length as the target phrase. Each generation, the fittest strings are more likely to be chosen as parents, their genes are combined, and random mutations are added. This repeats until a string equals the target. The console shows the progress live: the first 20 individuals, the average fitness, the best individual, the generation number and the elapsed time.

The demo target is `genes`, with a population of 1,500 and a mutation rate of 1.5%.

## Run it

To change the experiment, edit the line in `Program.cs`:

```csharp
Population pop = new Population(1500, "genes", 0.015f);
//                               size   target   mutation rate
```

The target may only contain letters, spaces and the characters `! ' , . ?`. Any other character is outside the gene alphabet and could never be produced. Use a console window at least 80 columns wide so the live display does not overlap.

## How it works
| Step | Implementation |
|------|----------------|
| **Genes** | Each individual (`DNA`) is a string. The alphabet is A-Z, a-z, space and `! ' , . ?` |
| **Fitness** | The fraction of characters that match the target, raised to the fourth power. This rewards near-matches much more strongly than a linear score |
| **Selection** | Fitness-proportionate (roulette wheel): an individual's chance of being picked as a parent is proportional to its fitness |
| **Crossover** | The child takes even-position genes from the first parent and odd-position genes from the second |
| **Mutation** | Each gene has a small chance of being replaced by a random character |
| **Replacement** | The whole population is replaced every generation |
| **Termination** | The search stops when an individual equals the target phrase |

## Things to try
- Lower or raise the mutation rate and watch how it changes the number of generations
- Use a longer target phrase and see how quickly the search time grows
- Change the population size

## Limitations
- No elitism, so the best individual can be lost between generations
- Only strings with a fixed alphabet are supported
- The demo problem (matching a known phrase) is for illustration. The algorithm is the point, not the task
