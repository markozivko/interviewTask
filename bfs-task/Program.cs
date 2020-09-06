using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace bfs_task
{
    class Program
    {

        class Node
        {
            public int number, column;
            public Node parent;

            public Node(int number, int column, Node parent)
            {
                this.number = number;
                this.column = column;
                this.parent = parent;
            }
        }

        private static int[,] ReadPyramid(string path)
        {
            var lines = new List<string>();
            string line;
            using (var reader = new System.IO.StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null && line != "")
                {
                    lines.Add(line);

                }
            }

            int[,] matrix = new int[lines.Count, lines.Count];
            for (int i = 0; i < lines.Count; i++)
            {
                line = lines.ElementAt(i);
                string[] splitLine = line.Split(' ');
                for (int j = 0; j < splitLine.Length; j++)
                {
                    matrix[i, j] = Int32.Parse(splitLine[j]);
                }
            }
            return matrix;
        }

        private static List<Node> CalculateAllPossiblePaths(int[,] matrix)
        {
            var currentRow = new List<Node>();
            currentRow.Add(new Node(matrix[0, 0], 0, null));

            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                var nextRow = new List<Node>();
                foreach (var currNode in currentRow)
                {
                    int numberBelow = matrix[i, currNode.column];
                    int numberRight = matrix[i, currNode.column + 1];

                    if ((currNode.number + numberBelow) % 2 != 0)
                    {
                        nextRow.Add(new Node(numberBelow, currNode.column, currNode));
                    }
                    if ((currNode.number + numberRight) % 2 != 0)
                    {
                        nextRow.Add(new Node(numberRight, currNode.column + 1, currNode));
                    }
                }
                currentRow = nextRow;
            }

            return currentRow;
        }

        private static List<Node> CaluculateBestPath(List<Node> currentRow)
        {
            int maxSum = 0;
            Node finalNode = null;
            foreach (var currNode in currentRow)
            {
                int sum = currNode.number;
                var parent = currNode?.parent;
                while (parent != null)
                {
                    sum += parent.number;
                    parent = parent.parent;
                }

                if (sum > maxSum)
                {
                    maxSum = sum;
                    finalNode = currNode;
                }
            }

            var path = new List<Node>();
            path.Add(finalNode);
            Node paren = finalNode?.parent;
            while (paren != null)
            {
                path.Add(paren);
                paren = paren.parent;
            }

            return path;
        }

        private static void PrintOutput(List<Node> path)
        {
            int maxSum = 0;
            string outputPath = "Path: ";
            for (int i = path.Count - 1; i >= 0; i--)
            {
                maxSum += path.ElementAt(i).number;
                outputPath += path.ElementAt(i).number + ", ";
            }
            outputPath = outputPath.Remove(outputPath.Length - 1);
            outputPath = outputPath.Remove(outputPath.Length - 1);

            Console.WriteLine("Max sum: " + maxSum);
            Console.WriteLine(outputPath);
        }


        static void Main(string[] args)
        {
            int[,] matrix = ReadPyramid(@"pyramid.txt");
            var lastRow = CalculateAllPossiblePaths(matrix);
            var path = CaluculateBestPath(lastRow);
            PrintOutput(path);
        }
    }
}
