using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace steep_hill_climbing
{
    class Program
    {
        public static void Main()
        {
            String[] lines = File.ReadAllLines("in.txt");
            int[,] matrix = getMatrix(lines,0,2);
            int[,] goalMatrix = getMatrix(lines,4,6);
            new Solver().Solve(matrix, goalMatrix, 1000000);
            Console.ReadKey();

        }







        private static int[,] getMatrix(String [] lines,int startIndex,int endIndex)
        {
            int[,] matrix = new int[3, 3];
            int i = 0;
            for(int lineNo = startIndex; lineNo <= endIndex; lineNo++)
            {
                String[] line = lines[lineNo].Replace("x", "-1").Split(' ');
                matrix = getMatrix(matrix, line, i);
                i++;
            }
            return matrix;
        }

        private static int[,] getMatrix(int[,] matrix,String[] line,int lineNo)
        {
            for(int i=0;i<line.Length;i++)
            {
                matrix[lineNo, i] = Convert.ToInt32(line[i]);
            }
            return matrix;
        }
       
    }
}

