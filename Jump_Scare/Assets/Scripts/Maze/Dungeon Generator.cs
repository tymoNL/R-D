using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        // 0 = Up, 1 = Down, 2 = Right, 3 = Left
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;
        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y)
        {
            // 0 - cannot spawn, 1 - can spawn, 2 - must spawn
            if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }

            return 0;
        }
    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset = new Vector2(10, 10); // make sure this matches room size

    private List<Cell> board;

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // Initialize board
        board = new List<Cell>();
        for (int i = 0; i < size.x * size.y; i++)
        {
            board.Add(new Cell());
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        board[currentCell].visited = true;

        int steps = 0;
        while (steps < 1000)
        {
            steps++;

            List<int> neighbors = GetUnvisitedNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0) break;
                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                // Determine direction and open walls
                // Maze generation, connecting two cells
                if (newCell == currentCell - size.x) // Up
                {
                    board[currentCell].status[0] = true; // current cell opens UP
                    board[newCell].status[1] = true;    // neighbor opens DOWN
                }
                else if (newCell == currentCell + size.x) // Down
                {
                    board[currentCell].status[1] = true; // current DOWN
                    board[newCell].status[0] = true;    // neighbor UP
                }
                else if (newCell == currentCell + 1) // Right
                {
                    board[currentCell].status[2] = true; // current RIGHT
                    board[newCell].status[3] = true;    // neighbor LEFT
                }
                else if (newCell == currentCell - 1) // Left
                {
                    board[currentCell].status[3] = true; // current LEFT
                    board[newCell].status[2] = true;    // neighbor RIGHT
                }

                currentCell = newCell;
                board[currentCell].visited = true;
            }
        }

        // Instantiate rooms after maze is generated
        GenerateDungeon();
    }

    List<int> GetUnvisitedNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        // Up
        if (cell - size.x >= 0 && !board[cell - size.x].visited)
            neighbors.Add(cell - size.x);
        // Down
        if (cell + size.x < board.Count && !board[cell + size.x].visited)
            neighbors.Add(cell + size.x);
        // Right
        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited)
            neighbors.Add(cell + 1);
        // Left
        if (cell % size.x != 0 && !board[cell - 1].visited)
            neighbors.Add(cell - 1);

        return neighbors;
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[i + j * size.x];
                if (!currentCell.visited) continue;

                int chosenRoom = -1;
                List<int> possibleRooms = new List<int>();

                for (int k = 0; k < rooms.Length; k++)
                {
                    int p = rooms[k].ProbabilityOfSpawning(i, j);
                    if (p == 2)
                    {
                        chosenRoom = k;
                        break;
                    }
                    else if (p == 1)
                    {
                        possibleRooms.Add(k);
                    }
                }

                if (chosenRoom == -1)
                {
                    if (possibleRooms.Count > 0)
                        chosenRoom = possibleRooms[Random.Range(0, possibleRooms.Count)];
                    else
                        chosenRoom = 0;
                }

                GameObject roomObj = Instantiate(
                    rooms[chosenRoom].room,
                    new Vector3(i * offset.x, 0, -j * offset.y),
                    Quaternion.identity,
                    transform
                );

                RoomBehaviour rb = roomObj.GetComponent<RoomBehaviour>();
                rb.UpdateRoom(currentCell.status);
                roomObj.name += $" {i}-{j}";
            }
        }
    }
}