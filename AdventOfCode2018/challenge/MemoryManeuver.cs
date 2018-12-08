using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class MemoryManeuver : Challenge
    {
        public static int metadataSum = 0;

        public static int GetMetadataCount()
        {
            List<int> list = GetList();
            Node tree = BuildTree(list, 0);

            return metadataSum;
        }

        public static int GetRootNodeValue()
        {
            List<int> list = GetList();
            Node root = BuildTree(list, 0);

            return root.value;
        }

        private static Node BuildTree(List<int> list, int offset)
        {
            Node node = new Node(list.ElementAt(offset), list.ElementAt(offset + 1));

            int childOffset = 0;
            for (int i = 0; i < node.childCount; i++)
            {
                Node childNode = BuildTree(list, offset + 2 + childOffset);
                node.AddChild(childNode);
                childOffset += childNode.size;
            }
            
            for (int i = 0; i < node.metadataCount; i++)
            {
                node.AddMetadata(list.ElementAt(offset + 2 + childOffset + i));
            }

            if (node.childCount == 0)
            {
                node.value = node.metadata.Sum(m => m);
            }
            else
            {
                foreach (int metadata in node.metadata)
                {
                    if (metadata <= node.childCount)
                    {
                        node.value += node.children[metadata - 1].value;
                    }
                }
            }

            return node;
        }

        private static List<int> GetList()
        {
            List<int> list = new List<int>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(8)))
                {
                    while (!sr.EndOfStream)
                    {
                        list = sr.ReadLine().Split(' ').Select(s => int.Parse(s)).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return list;
        }

        public class Node
        {
            public int value;
            public int childCount;
            public int metadataCount;

            public List<Node> children = new List<Node>();
            public List<int> metadata = new List<int>();

            public int size;

            public Node(int childCount, int metadataCount)
            {
                this.childCount = childCount;
                this.metadataCount = metadataCount;
                this.size = 2;
            }

            public void AddChild(Node childNode)
            {
                this.children.Add(childNode);
                this.size += childNode.size;
            }

            public void AddMetadata(int metadata)
            {
                this.metadata.Add(metadata);
                metadataSum += metadata;
                this.size++;
            }
        }
    }
}
