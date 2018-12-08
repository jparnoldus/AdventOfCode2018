using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class TheSumOfItsParts : Challenge
    {
        public static string GetOrder()
        {
            List<Instruction> instructions = GetList();
            List<Node> order = GetOrder(instructions);

            return String.Join("", order.Select(o => o.name));
        }

        private static List<Node> GetOrder(List<Instruction> instructions)
        {
            List<Node> nodes = SortIntoNodes(instructions);
            List<Node> order = new List<Node>();

            while (nodes.Any())
            {
                Node node = nodes.OrderBy(n => n.name).First(n => nodes.TrueForAll(nn => !nn.ancestors.Contains(n)));
                order.Add(node);
                nodes.Remove(node);
                nodes.ForEach(n => n.ancestors.Remove(node));
            }

            return order;
        }

        public static int GetWorkTime()
        {
            List<Instruction> instructions = GetList();

            List<Node> order = GetOrder(instructions);
            List<Node> done = new List<Node>();

            Dictionary<string, int> timeLookupTable = GetTimeLookupTable();

            List<Worker> workers = new List<Worker>();
            for (int i = 0; i < 5; i++)
            {
                workers.Add(new Worker());
            }

            int seconds = 0;
            Dictionary<string, int> waitingRoom = new Dictionary<string, int>();
            while (done.Count != 26)
            {
                List<Node> waitingNodes = order.Where(n => order.TrueForAll(o => !o.ancestors.Contains(n)) && !done.Contains(n)).ToList();

                if (done.Count == 0)
                {
                    foreach (Node waitingFirst in waitingNodes)
                    {
                        waitingRoom.Add(waitingFirst.name, 0);
                    }
                }

                foreach (Node waitingNode in waitingNodes)
                {
                    if (seconds == waitingRoom[waitingNode.name])
                    {
                        Worker assignedWorker = workers.OrderBy(w => w.sum).First();
                        assignedWorker.Schedule.Add(waitingNode);
                        assignedWorker.sum = seconds + timeLookupTable[waitingNode.name];

                        done.Add(waitingNode);
                        order.Remove(waitingNode);

                        foreach (Node ancestor in waitingNode.ancestors)
                        {
                            if (!(waitingRoom.ContainsKey(ancestor.name)))
                            {
                                waitingRoom.Add(ancestor.name, seconds + timeLookupTable[waitingNode.name]);
                            }
                            else if (waitingRoom[ancestor.name] < seconds + timeLookupTable[waitingNode.name])
                            {
                                waitingRoom[ancestor.name] = seconds + timeLookupTable[waitingNode.name];
                            }
                        }
                    }
                }

                seconds++;
            }

            return 0;
        }

        public static Dictionary<string, int> GetTimeLookupTable()
        {
            int counter = 0;
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Dictionary<string, int> lookupTimes = new Dictionary<string, int>();
            foreach (char letter in alphabet)
            {
                counter++;
                lookupTimes.Add(letter.ToString(), 60 + counter);
            }

            return lookupTimes;
        }

        public static List<Node> SortIntoNodes(List<Instruction> instructions)
        {
            Dictionary<string, Node> nodesDictionary = new Dictionary<string, Node>();

            foreach (Instruction instruction in instructions)
            {
                if (!nodesDictionary.ContainsKey(instruction.name))
                {
                    nodesDictionary.Add(instruction.name, new Node(instruction.name));
                }

                if (!nodesDictionary.ContainsKey(instruction.before))
                {
                    nodesDictionary.Add(instruction.before, new Node(instruction.before));
                }

                nodesDictionary[instruction.name].ancestors.Add(nodesDictionary[instruction.before]);
            }

            List<Node> nodes = nodesDictionary.Select(c => c.Value).ToList();
            nodes.ForEach(n => n.ancestors = n.ancestors.ToList());

            return nodes;
        }

        private static List<Instruction> GetList()
        {
            List<Instruction> list = new List<Instruction>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(7)))
                {
                    while (!sr.EndOfStream)
                    {
                        list.Add(Instruction.Parse(sr.ReadLine()));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return list;
        }
    }

    class Worker
    {
        public List<Node> Schedule = new List<Node>();
        public int sum = 0;
    }

    class Instruction
    {
        public string name;
        public string before;

        public static Instruction Parse(string line)
        {
            string[] words = line.Split(' ');
            return new Instruction()
            {
                name = words[1],
                before = words[7]
            };
        }
    }

    class Node
    {
        public string name;
        public List<Node> ancestors;

        public Node(string name)
        {
            this.ancestors = new List<Node>();
            this.name = name;
        }
    }
}
