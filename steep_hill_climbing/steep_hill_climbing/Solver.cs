using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace steep_hill_climbing
{
    public class Solver
    {
        

        public int[,] Solve(int[,] matrix, int[,] goalMatrix, int numberOfTries)
        {
            Node bestMoves = new Node(matrix);
            

            int[,] originalMatrix = CopyMatrix(matrix);
            while (numberOfTries > 0)
            {
                if (IsGoalReached(matrix, goalMatrix))
                {
                    Console.WriteLine("sucess");
                    break;
                }
                else
                {
                    //Start processing
                    int x, y = 0;
                    GetXLocation(matrix, out x, out y);
                    LocationType locType = GetLocationType(x, y);
                    Location currentXLocation = new Location() { x = x, y = y };
                    Location[] possibleMoves = GetPossibleMovingLocations(currentXLocation, locType);
                    int currentMessyCount = GetMessyCount(matrix, goalMatrix);
                    matrix = MakeBestMove(possibleMoves, matrix, goalMatrix, currentXLocation, currentMessyCount, bestMoves);
                    PrintMatrix(matrix);
                    numberOfTries--;
                }
            }
            return matrix;
        }

        private int[,] MakeBestMove(Location[] possibleMoves, int[,] matrix, int[,] goalMatrix, Location currentXLocation,int messyCount,Node bestMoves)
        {
            for (int i = 0; i < possibleMoves.Length; i++)
            {
                Location movingLocation = possibleMoves[i];
                int[,] trialMatrix = CopyMatrix(matrix);
                trialMatrix = Move(trialMatrix, movingLocation, currentXLocation);
                movingLocation.currentMessyCount = GetMessyCount(trialMatrix, goalMatrix);
            }
            Location bestMovingLocation = GetBestMovingLocation(possibleMoves);
            int[,] trialMatrix1 = CopyMatrix(matrix);
            int[,] bestMove = Move(trialMatrix1, GetBestMovingLocation(possibleMoves), currentXLocation);
            int currentMessyCount = GetMessyCount(bestMove, goalMatrix);
            if (currentMessyCount >= messyCount) //No point in going this route. go back to original state
            {
                return bestMoves.Parent.Move;
            }
                
            //Now check if the move resulted in already tried move
            if (IsMoveAlreadyTried(bestMove))
            {
                if (possibleMoves.Length == 1)
                    return bestMove;
                
                possibleMoves = RefinePossibleMoves(bestMovingLocation, possibleMoves);
                bestMove = MakeBestMove(possibleMoves, matrix, goalMatrix, currentXLocation,currentMessyCount, originalMatrix);
            }
            bestMoves.AddChild(new Node(bestMove));
            return bestMove;

        }

        private Location[] RefinePossibleMoves(Location location, Location[] possibleMoves)
        {
            if (possibleMoves.Length == 1)
                return null;
            Location[] result = new Location[possibleMoves.Length - 1];
            int j = 0;
            for(int i=0;i< possibleMoves.Length;i++)
            {
                if (possibleMoves[i].x == location.x && possibleMoves[i].y == location.y)
                    continue;
                result[j] = possibleMoves[i];
                j++;
            }
            return result;
        }

        private bool IsMoveAlreadyTried(Node move
            
            
            )
        {

        }
        /*private bool IsMoveAlreadyTried(int[,] bestMove)
        {
            foreach(int[,] move in bestMoves)
            {
                if (IsSame(move, bestMove))
                    return true;
            }
            return false;
        }*/

        private bool IsSame(int[,] move1, int[,] move2)
        {
            for (int i = 0; i < move1.GetLength(0); i++)
                for (int j = 0; j < move1.GetLength(1); j++)
                    if (move1[i, j] != move2[i, j])
                        return false;
            return true;
        }

        private Location GetBestMovingLocation(Location[] possibleMoves)
        {
            Location bestMoveSoFar = possibleMoves[0];
            for (int i = 0; i < possibleMoves.Length; i++)
            {
                if (possibleMoves[i].currentMessyCount < bestMoveSoFar.currentMessyCount)
                    bestMoveSoFar = possibleMoves[i];
            }
            return bestMoveSoFar;
        }

        private int[,] Move(int[,] matrix, Location movingLocation, Location currentXLocation)
        {

            matrix[currentXLocation.x, currentXLocation.y] = matrix[movingLocation.x, movingLocation.y];
            matrix[movingLocation.x, movingLocation.y] = -1;
            return matrix;
        }

        private int[,] CopyMatrix(int[,] matrix)
        {
            int[,] copy = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    copy[i, j] = matrix[i, j];
            return copy;
        }

        private Location[] GetPossibleMovingLocations(Location currentLocation, LocationType locationType)
        {
            Location[] possibleLocations = null;
            if (locationType == LocationType.Center)
            {
                possibleLocations = new Location[4];
                possibleLocations[0] = new Location() { x = 1, y = 0 };
                possibleLocations[1] = new Location() { x = 0, y = 1 };
                possibleLocations[2] = new Location() { x = 2, y = 1 };
                possibleLocations[3] = new Location() { x = 1, y = 2 };

            }
            if (locationType == LocationType.Corner)
            {
                int x = currentLocation.x;
                int y = currentLocation.y;
                possibleLocations = new Location[2];
                if (x == 0 && y == 0) //First Corner
                {
                    possibleLocations[0] = new Location() { x = 1, y = 0 };
                    possibleLocations[1] = new Location() { x = 0, y = 1 };
                }
                else if (x == 2 && y == 0)
                {
                    possibleLocations[0] = new Location() { x = 1, y = 0 };
                    possibleLocations[1] = new Location() { x = 2, y = 1 };
                }
                else if (x == 0 && y == 2)
                {
                    possibleLocations[0] = new Location() { x = 0, y = 1 };
                    possibleLocations[1] = new Location() { x = 1, y = 2 };
                }
                else if (x == 2 && y == 2)
                {
                    possibleLocations[0] = new Location() { x = 1, y = 2 };
                    possibleLocations[1] = new Location() { x = 2, y = 1 };
                }

            }

            if (locationType == LocationType.Side)
            {
                int x = currentLocation.x;
                int y = currentLocation.y;
                possibleLocations = new Location[3];
                possibleLocations[0] = new Location() { x = 1, y = 1 }; //Center

                if (x == 1) //Top or bottom side
                {
                    possibleLocations[1] = new Location() { x = x - 1, y = y };
                    possibleLocations[2] = new Location() { x = x + 1, y = y };
                }

                if (y == 1) //Left or right side
                {
                    possibleLocations[1] = new Location() { x = x, y = y - 1 };
                    possibleLocations[2] = new Location() { x = x, y = y + 1 };
                }

            }

            return possibleLocations;

        }

        private LocationType GetLocationType(int x, int y)
        {
            if (x == 1 && y == 1)
                return LocationType.Center;
            if ((x == 0 && y == 0) ||
               (x == 0 && y == 2) ||
               (x == 2 && y == 0) ||
               (x == 2 && y == 2))
                return LocationType.Corner;
            return LocationType.Side;
        }


        private int GetMessyCount(int[,] matrix, int[,] goalMatrix)
        {
            int count = 0;
            for (int i = 0; i < goalMatrix.GetLength(0); i++)
                for (int j = 0; j < goalMatrix.GetLength(1); j++)
                    if ((goalMatrix[i, j] != matrix[i, j])&& matrix[i,j]!=-1)
                        count++;
            return count;
        }

        private void GetXLocation(int[,] matrix, out int x, out int y)
        {
            x = 0; y = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] == -1)
                    {
                        x = i;
                        y = j;
                        return;
                    }

        }

        private bool IsGoalReached(int[,] matrix, int[,] goalMatrix)
        {
            for (int i = 0; i < goalMatrix.GetLength(0); i++)
                for (int j = 0; j < goalMatrix.GetLength(1); j++)
                    if (goalMatrix[i, j] != matrix[i, j])
                        return false;
            return true;
        }

        public void PrintMatrix(int[,] matrix)
        {
            
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                String line = "";
                for (int j = 0; j < matrix.GetLength(1); j++)
                    line = line + " " + matrix[i, j];
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
    }

    public enum LocationType
    {
        Center = 1,
        Side = 2,
        Corner = 3
    }
    

}
