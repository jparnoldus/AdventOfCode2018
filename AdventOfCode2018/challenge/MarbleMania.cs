using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class MarbleMania : Challenge
    {
        public static uint PlayGame(int multiplier)
        {
            Game game = GetGameInfo();
            uint counter = 0;
            while (counter <= game.lastMarbleValue * multiplier)
            {
                foreach (Player elf in game.players)
                {
                    if (game.currentMarble == null)
                    {
                        game.currentMarble = new Marble(counter);
                        game.marbleCircle.AddFirst(game.currentMarble);
                    }
                    else
                    {
                        Marble newMarble = new Marble(counter);
                        if (counter % 23 != 0)
                        {
                            game.currentMarble.StepsForward(1).AddAfter(newMarble);
                            game.currentMarble = newMarble;
                        }
                        else
                        {
                            Marble counterClockWiseMarble = game.currentMarble.StepsBack(7);
                            game.currentMarble = counterClockWiseMarble.StepsForward(1);
                            counterClockWiseMarble.Remove();

                            elf.score += newMarble.value + counterClockWiseMarble.value;
                        }
                    }

                    counter++;
                }
            }

            return game.players.Max(p => p.score);
        }

        private static Game GetGameInfo()
        {
            Game gameInfo = null;

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(9)))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split(' ');
                        gameInfo = new Game(int.Parse(line[0]), int.Parse(line[6]));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return gameInfo;
        }

        public class Game
        {
            public int playerCount;
            public int lastMarbleValue;

            public List<Player> players = new List<Player>();
            public LinkedList<Marble> marbleCircle = new LinkedList<Marble>();
            public Marble currentMarble;

            public Game(int playerCount, int lastMarbleValue)
            {
                this.playerCount = playerCount;
                this.lastMarbleValue = lastMarbleValue;
                for (int i = 0; i < playerCount; i++)
                {
                    this.players.Add(new Player());
                }
            }
        }

        public class Marble
        {
            private Marble previous;
            public uint value = 0;
            private Marble next;

            public Marble(uint value)
            {
                this.previous = this;
                this.next = this;
                this.value = value;
            }

            public void AddAfter(Marble marble)
            {
                marble.next = this.next;
                marble.previous = this;

                this.next.previous = marble;
                this.next = marble;
            }

            public Marble StepsForward(int counter)
            {
                Marble marble = this;
                for (int i = 0; i < counter; i++)
                {
                    marble = marble.next;
                }

                return marble;
            }

            public Marble StepsBack(int counter)
            {
                Marble marble = this;
                for (int i = 0; i < counter; i++)
                {
                    marble = marble.previous;
                }

                return marble;
            }

            public Marble Remove()
            {
                this.next.previous = this.previous;
                this.previous.next = this.next;

                return this;
            }
        }

        public class Player
        {
            public uint score;
        }
    }
}
