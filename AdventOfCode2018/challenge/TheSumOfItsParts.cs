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

            string answer = "";
            while (nodes.Count > 0)
            {
                Node node = nodes.OrderBy(n => n.name).First(n => nodes.TrueForAll(nn => !nn.ancestors.Contains(n)));
                answer += node.name;
                nodes.Remove(node);
                nodes.ForEach(n => n.ancestors.Remove(node));
            }

            return answer;
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
