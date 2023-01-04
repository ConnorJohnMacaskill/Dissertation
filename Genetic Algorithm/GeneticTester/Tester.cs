using ArtificialNeuralNetwork;
using GeneticAlgorithm;
using NaughtsAndCrosses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticTester
{
    class Tester
    {
        private Population population;
        private Breeder breeder;
        private NeuralNetwork network;

        private Player player;
        private Player opponentPlayer;

        private const int gamesToPlay = 1000;
        private int gamesWon = 0;
        private bool stopGame = false;

        public Tester(int populationSize)
        {
            network = NetworkBuilder.CreateNeuralNetwork(9, 1, 2, 4, ActivationFunction);
            GeneInfo geneInfo = new GeneInfo(GeneType.DOUBLE, network.WeightCount, -2, 2);
            population = PopulationBuilder.CreatePopulation(populationSize, geneInfo, FitnessFunction);
            breeder = new Breeder(mutationRate:0.3, tournamentSize:10, elitism:true);

            opponentPlayer = new Player(Playstyle.Random, Shape.Cross, "Infailable Machine");
        }

        public void RunTests()
        {
            Individual fittest;

            while (true)
            {
                population = breeder.Evolve(population);

                fittest = population.GetFittestIndividual();
                Console.WriteLine(fittest.GetFitness());
            }
        }

        public int FitnessFunction(double[] genes)
        {
            network.SetWeights(genes);

            player = new Player(Playstyle.External, Shape.Circle, "Training Network");
            gamesWon = 0;

            for(int i = 0; i < gamesToPlay; i++)
            {
                if(stopGame)
                {
                    //Network is terrible, discard it.
                    //gamesWon = -99999;
                    stopGame = false;
                   // break;
                }

                Game game = new Game(player, opponentPlayer);

                game.GameOver += GameOverEvent;
                game.PlayersTurn += PlayerTurn;

                game.StartGame(true);
            }
            

            return gamesWon;
        }

        public int FitnessSum(double[] genes)
        {
            return (int)genes.Sum();
        }

        public double ActivationFunction(double value)
        {
            //Sigmoid function.
            return 1 / (1 + Math.Exp(-value));
        }

        private void GameOverEvent(object sender, EventArgs e)
        {
            GameOverEventArgs gameOverArgs = (GameOverEventArgs)e;

            if(gameOverArgs.Winner == Winner.Player)
            {
                gamesWon += 2;
            }
            else if(gameOverArgs.Winner == Winner.Draw)
            {
                gamesWon += 1;
            }
        }

        private void PlayerTurn(object sender, EventArgs e)
        {
            Game game = (Game)sender;
            bool moveValid = false;
            Move move;

            while (!moveValid)
            {
                network.SetInput(game.Board.ToArray(player));
                network.Run();

                int index = Convert.ToInt32(network.OutputLayer.Neurons[0].Value * 8);

                int x = index % 3;
                int y = index / 3;

                move = new Move(y, x);

                if (game.MoveValid(move))
                {
                    moveValid = true;
                    game.MakeMove(move);
                }
                else
                {
                    stopGame = true;
                    break;
                }
            }
        }
    }
}
