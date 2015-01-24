using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TrollRoom
{
    public class GeneticLayout
    {  
        private Random random = new Random();
        private int bitsLength = 5;

        private List<Graph> CreateInitialPool(Graph graph, int size)
        {
            var pool = new List<Graph>();
            while (pool.Count < size)
            {
                var randomGraph = CreateRandomGraph(graph);
                if (randomGraph.IsValid)
                {
                    randomGraph.CalculateFitness(0);
                    pool.Add(randomGraph);
                }
            }

            
            return pool;
        }

        public Graph CreateRandomGraph(Graph graph)
        {
            var newGraph = graph.Copy();
            var bits = new List<BitArray>();
            for (int i = 0; i < newGraph.Nodes.Count; i++)
            {
                var rankBits = new BitArray(bitsLength);
                var orderBits = new BitArray(bitsLength);
                for (int j = 0; j < bitsLength; j++)
                {
                    rankBits[j] = random.NextDouble() > 0.5;
                    orderBits[j] = random.NextDouble() > 0.5;
                }
                bits.Add(rankBits);
                bits.Add(orderBits);
            }
            newGraph.Bits = bits;
            return newGraph;
        }

        public Graph FindBestGraphLayout(Graph graph)
        {
            var maxScore = graph.Edges.Count * 2;
            var generationLimit = 100000;
            var pool = CreateInitialPool(graph, 100);
            var currentGeneration = 0;
            double bestFitness = pool.Max(x => x.Fitness);
            var bestGraph = pool.First(x => x.Fitness == bestFitness);

            if (bestGraph.Fitness == maxScore)
                return bestGraph;

            while (currentGeneration < generationLimit)
            {
                if (bestGraph.Fitness == maxScore)
                    return bestGraph;
                pool = ProcessPool(graph, pool, currentGeneration);
                var poolBestFitness = pool.Max(x => x.Fitness);
                if (poolBestFitness > bestFitness)
                {
                    bestFitness = poolBestFitness;
                    bestGraph = pool.First(x => x.Fitness == bestFitness);
                    if (bestGraph.Fitness == 1)
                        return bestGraph;
                }
                currentGeneration++;
            }

            return bestGraph;
        }

        private List<Graph> ProcessPool(Graph graph, List<Graph> pool, int generation)
        {
            var newPool = new List<Graph>();
            double crossoverRate = 0.7;
            double mutationRate = 0.001;

            while (newPool.Count < 100)
            {
                var mate1 = SelectGraph(pool);
                var mate2 = SelectGraph(pool);

                var child1Bits = new List<BitArray>(mate1.Bits.Count);
                var child2Bits = new List<BitArray>(mate1.Bits.Count);
                var child1 = graph.Copy();
                var child2 = graph.Copy();

                double randomCrossover = random.NextDouble();

                if (randomCrossover < crossoverRate)
                {
                    //these next two values indicate the crossover point
                    int randomBitArray = random.Next(0, mate1.Bits.Count);
                    int randomIndex = random.Next(0, bitsLength);
                    //swap the bits at the crossover
                    for (int i = 0; i < mate1.Bits.Count; i++)
                    {
                        if (i < randomBitArray)
                        {
                            child1Bits.Add(new BitArray(mate1.Bits[i]));
                            child2Bits.Add(new BitArray(mate2.Bits[i]));
                        }
                        if (i == randomBitArray)
                        {
                            var splice1 = new BitArray(bitsLength);
                            var splice2 = new BitArray(bitsLength);
                            for (int j = 0; j < bitsLength; j++)
                            {
                                if (j < randomIndex)
                                {
                                    splice1[j] = mate1.Bits[i][j];
                                    splice2[j] = mate2.Bits[i][j];
                                }
                                else
                                {
                                    splice1[j] = mate2.Bits[i][j];
                                    splice2[j] = mate1.Bits[i][j];
                                }
                            }
                            child1Bits.Add(splice1);
                            child2Bits.Add(splice2);
                        }
                        if (i > randomBitArray)
                        {
                            child1Bits.Add(new BitArray(mate2.Bits[i]));
                            child2Bits.Add(new BitArray(mate1.Bits[i]));
                        }
                    }
                }
                else
                { 
                    //just make copies of mate1 and mate2
                    for (int i = 0; i < mate1.Bits.Count; i++)
                    {
                        child1Bits.Add(new BitArray(mate1.Bits[i]));
                        child2Bits.Add(new BitArray(mate2.Bits[i]));
                    }
                }

                MutateEncodedGraph(child1Bits, mutationRate);
                MutateEncodedGraph(child2Bits, mutationRate);

                child1.Bits = child1Bits;
                child2.Bits = child2Bits;

                if (child1.IsValid)
                {
                    child1.CalculateFitness(generation);
                    newPool.Add(child1);
                }


                if (child2.IsValid)
                {
                    child2.CalculateFitness(generation);
                    newPool.Add(child2);
                }
            }
            return newPool;
        }

        private void MutateEncodedGraph(List<BitArray> encodedGraph, double mutationRate)
        {
            for (int i = 0; i < encodedGraph.Count; i++)
            {
                for (int j = 0; j < bitsLength; j++)
                {
                    var randomMutation = random.NextDouble();
                    if (randomMutation <= mutationRate)
                    {
                        encodedGraph[i][j] = !encodedGraph[i][j];
                    }
                }
            }
        }

        private Graph SelectGraph(List<Graph> pool)
        {
            double minimum = pool.Min(x => x.Fitness);
            double maximum = pool.Sum(x => x.Fitness);
            double randomSelect = random.NextDouble() * (maximum - minimum) + minimum;
            double currentScore = 0.0;
            foreach (var graph in pool)
            {
                currentScore += graph.Fitness;
                if (currentScore >= randomSelect)
                    return graph;
            }
            return null;
        }
    }
}
