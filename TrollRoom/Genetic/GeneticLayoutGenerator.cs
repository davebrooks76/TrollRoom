using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TrollRoom.Genetic.LayoutScorers;

namespace TrollRoom
{
    public class GeneticLayoutGenerator : ILayoutGenerator
    {
        private const int GenerationCountLimit = 1000000;
        private const int PopulationSize = 100; //even number
        private const double CrossoverRate = 0.7;
        private const double MutationRate = 0.01;

        private readonly Map map;
        private Random random = new Random();
        private int generationCounter = 0;
        private List<Layout> currentPopulation = new List<Layout>();
        private List<Layout> newPopulation = new List<Layout>();

        private const int bitsLength = 5; //8 is the max
        private int maxCoordinateValue = (int)Math.Pow(2, bitsLength);

        public GeneticLayoutGenerator(Map map)
        {
            this.map = map;
            currentPopulation = new List<Layout>();
        }

        public Layout GenerateLayout()
        {
            var overallBestLayout = new Layout(map) {FitnessScore = 0};

            currentPopulation = CreatePopulation();
            while (generationCounter < GenerationCountLimit)
            {
                newPopulation.Clear(); 
                newPopulation = BreedPopulation();
                var maxScore = newPopulation.Max(x => x.FitnessScore);

                if (maxScore > overallBestLayout.FitnessScore)
                    overallBestLayout = newPopulation.First(x => x.FitnessScore == maxScore);
                currentPopulation = new List<Layout>(newPopulation);
                //Debug.WriteLine(newPopulation.Max(x=>x.FitnessScore));
                 generationCounter++;
            }
            
            return overallBestLayout;
        }

        private List<Layout> BreedPopulation()
        {
            while (newPopulation.Count < PopulationSize)
            {
                var mate1 = SelectLayout();
                var mate2 = SelectLayout();
                var child1 = new Layout(map);
                var child2 = new Layout(map);
                double randomCrossover = random.NextDouble();
                if (randomCrossover < CrossoverRate)
                {
                    int randomIndex = random.Next(1, map.Rooms.Count - 1) * 2;
                    for (int i = 0; i < mate1.Coordinates.Length; i ++)
                    {
                        if (i < randomIndex)
                        {
                            child1.Coordinates[i] = mate1.Coordinates[i];
                            child2.Coordinates[i] = mate2.Coordinates[i];
                        }
                        else
                        {
                            child1.Coordinates[i] = mate2.Coordinates[i];
                            child2.Coordinates[i] = mate1.Coordinates[i];
                        }
                    }
                } 
                else
                {
                    //just make copies of mate1 and mate2
                    for (int i = 0; i < mate1.Coordinates.Length; i++)
                    {
                        child1.Coordinates[i] = mate1.Coordinates[i];
                        child2.Coordinates[i] = mate2.Coordinates[i];
                    }
                }
                MutateLayout(child1, MutationRate);
                MutateLayout(child2, MutationRate);
                ScoreLayout(child1);
                ScoreLayout(child2);

                newPopulation.Add(child1);
                newPopulation.Add(child2);
            }
            
            return newPopulation;
        }

        private void MutateLayout(Layout layout, double mutationRate)
        {
            for (int i = 0; i < layout.Coordinates.Length; i++)
            {
                var randomMutation = random.NextDouble();
                if (randomMutation <= mutationRate)
                {
                    var newCoordinate = Convert.ToInt32(layout.Coordinates[i]) + random.Next(-10, 10);
                    if (newCoordinate >= 0 && newCoordinate <= 63)
                        layout.Coordinates[i] = (byte)newCoordinate;
                }
            }
        }

        private Layout SelectLayout()
        {
            double total = currentPopulation.Sum(x => x.FitnessScore);
            double randomSelect = random.NextDouble() * total;
            double currentScore = 0.0;
            foreach (var layout in currentPopulation)
            {
                currentScore += layout.FitnessScore;
                if (currentScore >= randomSelect)
                    return layout;
            }
            // this should never happen
            throw new IndexOutOfRangeException();
        }

        private List<Layout> CreatePopulation()
        {
            var population = new List<Layout>();
            for (var i = 0; i < PopulationSize; i++)
            {
                var layout = new Layout(map);
                var coordinates = new List<byte>();
                for(var j = 0; j < map.Rooms.Count * 2; j++ )
                {
                    coordinates.Add((byte)random.Next(0, maxCoordinateValue));
                }
                layout.Coordinates = coordinates.ToArray();
                ScoreLayout(layout);
                population.Add(layout);
            }
            
            return population;
        }

        private void ScoreLayout(Layout layout)
        {
            //var generalDirectionLayoutScorer = new GeneralDirectionLayoutScorer();
            var angleLayoutScorer = new AngleLayoutScorer();
            //var collisionLayoutScorer = new CollisionLayoutScorer(bitsLength);
            var distanceLayoutScorer = new DistanceLayoutScorer(bitsLength);
            //var clusterLayoutScorer = new ClusterLayoutScorer(bitsLength);

            //var totalScore = angleLayoutScorer.Score(_map, layout)
            //                    + collisionLayoutScorer.Score(_map, layout)
            //                    + distanceLayoutScorer.Score(_map, layout);

            //var totalScore = distanceLayoutScorer.Score(map, layout);

            var totalScore = angleLayoutScorer.Score(map, layout);
            totalScore += distanceLayoutScorer.Score(map, layout);
            layout.FitnessScore = totalScore.Remap(0, 2, 0, 1);
            //Debug.WriteLine(layout.FitnessScore);
        }
    }
}
