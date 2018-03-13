using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node(float _x, float _y, int _g = 0, int _f = 0)
    {
        x = _x;
        y = _y;
        g = _g;
        f = _f;
        parent = null;
    }

    public static bool samePosition(Node obj1, Node obj2)
    {
        return (obj1.x == obj2.x && obj1.y == obj2.y);
    }
    public Vector2 position()
    {
        return new Vector2(x, y);
    }

    public float x;
    public float y;
    public int g;
    public int f;
    public Node parent;
}

//Pulled this from the internet, solves for duplicate keys for the A* algorithm
public class DuplicateKeyComparer<TKey>
                :
             IComparer<TKey> where TKey : IComparable
{
    #region IComparer<TKey> Members

    public int Compare(TKey x, TKey y)
    {
        int result = x.CompareTo(y);

        if (result == 0)
            return 1;   // Handle equality as beeing greater
        else
            return result;
    }

    #endregion
}


public class Grid_Creator : MonoBehaviour {

    /// <summary>
    /// Creates the A* Grid and Can Return Paths
    /// </summary>

    [Header("Grid")]
    public Vector2 top_left;
    public int width, height;
    private Node[,] grid;

    [Header("Testing")]
    public GameObject visibleNodes;
    public bool seeNodes;
    public Transform f, t;




    SortedList<int, Node> open = new SortedList<int, Node>(new DuplicateKeyComparer<int>());
    List<Node> closed = new List<Node>();

    //***************************************************************Setting up the Grid*************************************************************

    void Awake () {
        
        //Creates the grid from the given dimensions
        CreateGrid(width, height);

        //Goes through and marks any places that are unpassable with an f values of -1
        RaycastHit2D rh;
		for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                rh = Physics2D.Raycast(new Vector2(top_left.x + (x * 5), top_left.y - (y * 5)), Vector2.zero);
                if(rh.collider)
                {
                    //print("hit at " + x + " " + y);
                    grid[x, y].f = -1;
                }
                else if(seeNodes)
                    Instantiate(visibleNodes, new Vector2(top_left.x + (x * 5), top_left.y - (y * 5)), Quaternion.identity);
            }
        }
    }

    private void CreateGrid(int sizex, int sizey)
    {
        grid = new Node[sizex, sizey];
        for (int y = 0; y < sizey; ++y)
        {
            for (int x = 0; x < sizex; ++x)
            {
                //Puts the position of each node at the center of each box of the grid
                grid[x, y] = new Node(top_left.x + (x * 5), top_left.y - (y * 5));
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            List<Node> path = Get_Path(f, t);

            for (int i = 0; i < path.Count; ++i)
            {
                print(path[i].x + " " + path[i].y);
                Instantiate(visibleNodes, new Vector3(path[i].x, path[i].y, 0), Quaternion.identity);
            }
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            print(GetClosestNode(f).x + " " + GetClosestNode(f).y);
        }
    }

    //********************************************************Creating the Paths*****************************************************************

    public List<Node> Get_Path(Transform from, Transform to)
    {
        //Gets the path and returns a list of nodes to go through to get to end
        List<Node> path = new List<Node>();

        Node curNode = Create_Path(from, to);

        while(curNode.parent != null)
        {
            path.Add(curNode);
            curNode = curNode.parent;
        }
        return path;
    }


    Node Create_Path(Transform from, Transform to)
    {
        //Creates the path and return the end Node which has the path backtracked through it's parent to the start

        //Transforms the from and to positions to their closests Nodes on the grid
        Node fromNode = GetClosestNode(from);
        Node toNode = GetClosestNode(to);

        int toNode_x = (int)Mathf.Round((toNode.x - top_left.x) / 5f);
        int toNode_y = (int)Mathf.Round((top_left.y - toNode.y) / 5f);

        //The Open list is the nodes to be searched and the closed list are nodes that have already been explored
        //Acts as a priority queue, using the f values of the node as the key and the Node as the value
        open.Clear();
        closed.Clear();

        //Get the first Node (the start node)
        open.Add(fromNode.f, fromNode);

        //Go through all the Nodes until it reaches the goal or runs out of Nodes to Search
        while (open.Count > 0)
        {
            //This pops the top node (lowest f value)
            Node testing = open.Values[0];
            open.RemoveAt(0);

            //Add this node to the closed list since we visited it
            closed.Add(testing);

            //Gets the x and y index of the node you are testing
            int x_index = (int)Mathf.Round((testing.x - top_left.x) / 5f);
            int y_index = (int)Mathf.Round((top_left.y - testing.y) / 5f);
            

            //Loop through all adjacent nodes
            for (int x = x_index - 1; x < x_index + 2; ++x)
            {
                for (int y = y_index - 1; y < y_index + 2; ++y)
                {

                    //Checks if index is in bounds of the map and its not on the testing Node (if so then ignore)
                    if (x < 0 || x >= width || y < 0 || y >= height || (x == x_index && y == y_index) || (x != x_index && y != y_index))
                        continue;

                    //Checks to see if node being checked is a wall (if so then ignore)
                    if (grid[x, y].f == -1)
                        continue;

                    //If that grid spot is the target then add it and the algorithm is finished
                    if (Node.samePosition(toNode, grid[x, y]))
                    {
                        grid[x, y].parent = testing;
                        return grid[x, y];
                    }


                    //Check to see if the Node is already on the closed list (meanings its already been explored)
                    if (closed.Contains(grid[x, y]))
                        continue;

                    //Set the Path cost to 1 plus the path cost of the current Node
                    grid[x, y].g = testing.g + 1;

                    //Calculate the new F value of the Node as the G + H values (the h is the heaurstic of this node's distance to the goal)
                    grid[x, y].f = Calculate_Manhattan(x, y, toNode_x, toNode_y) + grid[x, y].g;

                    //Check to see if the Node is already on the open list and if it is to see if the one on the open list is cheaper to get to (if so then ignore this one)
                    if (open.ContainsValue(grid[x, y]))
                    {
                        int index = open.IndexOfValue(grid[x, y]);
                        if (open.Values[index].g <= grid[x, y].g)
                            continue;
                        else
                        {
                            open.Values[index].f = grid[x, y].f;
                            open.Values[index].g = grid[x, y].g;
                            continue;
                        }
                    }

                    //Set that Node's parent to the current Node
                    grid[x, y].parent = testing;

                    //Add this node to the open list if it passes all the criteria
                    open.Add(grid[x, y].f, grid[x, y]);
                }
            }
        }

        //Returns start Node if no path is found
        return fromNode;
    }

    private int Calculate_Manhattan(int x_pos, int y_pos, int target_x, int target_y)
    {
        //Calculates the Manhattan distance (difference in x plus the difference in y)
        return Mathf.Abs(x_pos + target_x) + Mathf.Abs(y_pos + target_y);
    }

    private Node GetClosestNode(Transform pos)
    {
        //Gets the closest Node to the position given in the Transform
        return grid[(int) Mathf.Round((pos.position.x - top_left.x) / 5f), (int) Mathf.Round((top_left.y - pos.position.y) / 5f)];
    }


    //**********************************************************Jump Point Search Pruning**************************************************************

    void identifySuccessors(Node current, Node start, Node end) {
        //This function finds all the successor of the Node by jumping past uninteresting Nodes

        int x_index = (int)Mathf.Round((current.x - top_left.x) / 5f);
        int y_index = (int)Mathf.Round((top_left.y - current.y) / 5f);

        //Loop through all adjacent nodes
        for (int x = x_index - 1; x < x_index + 2; ++x)
        {
            for (int y = y_index - 1; y < y_index + 2; ++y)
            {
                // Direction from current node to neighbor:
                int dx = x_index - x;
                int dy = y_index - y;

                // Try to find a node to jump to:
                Node jumpPoint = jump(x_index, y_index, dx, dy, start, end);


                // If found add it to the list:
                if (jumpPoint != null)
                    open.Add(jumpPoint.f, jumpPoint);
            }
        }
    }


    Node jump(int x_pos, int y_pos, int dx, int dy, Node start, Node end) {
        //This function returns a Node after trying to go in one direction as far as it can go (if it hits a wall or if it hits a forced neighbor)
 
        // Position of new node we are going to consider:
        int nextX = x_pos + dx;
        int nextY = y_pos + dy;

        // If the position is outside the map then stop
        if (nextX < 0 || nextY < 0 || nextX >= width || nextY >= height)
            return null;
    
        // If it's blocked we can't jump here
        if (grid[nextX, nextY].f == -1) return null;
 
        // If the node is the goal return it
        if (grid[nextX, nextY] == end) return grid[nextX, nextY];

        // Diagonal Case   
        if (dx != 0 && dy != 0)
        {
            if (Diagonal_Forced_Neighbor(nextX, nextY, dx, dy))
            {
                return grid[nextX, nextY];
            }

            // Check in horizontal and vertical directions for forced neighbors
            // This is a special case for diagonal direction
            if (jump(nextX, nextY, dx, 0, start, end) != null ||
                jump(nextX, nextY, 0, dy, start, end) != null)
            {
                return grid[nextX, nextY];
            }
        }
        else
        {
            if (dx != 0)
            {
                // Horizontal case
                if (Horiz_Forced_Neighbor(nextX, nextY, dx))
                    return grid[nextX, nextY];
            }
            else if (dy != 0)
            {
                /// Vertical case
                if (Vert_Forced_Neighbor(nextX, nextY, dy))
                    return grid[nextX, nextY];
            }

        }



        /// If forced neighbor was not found try next jump point
        return jump(nextX, nextY, dx, dy, start, end);
    }


    private bool Horiz_Forced_Neighbor(int x_pos, int y_pos, int dx)
    {
        //This function check to see if any forced neighbors need to be set for jump points

        //dx is +1 for right and -1 for left

        bool foundNeighbor = false;
        if (grid[x_pos + dx, y_pos].f != -1)
        {
            if(grid[x_pos, y_pos+1].f == -1)
            {
                //Wall Underneath
                open.Add(grid[x_pos + dx, y_pos + 1].f, grid[x_pos + dx, y_pos + 1]);
                foundNeighbor = true;
            }
            if(grid[x_pos, y_pos-1].f == -1)
            {
                //Wall Above
                open.Add(grid[x_pos + dx, y_pos - 1].f, grid[x_pos + dx, y_pos - 1]);
                foundNeighbor = true;
            }
        }
        return foundNeighbor;
    }

    private bool Vert_Forced_Neighbor(int x_pos, int y_pos, int dy)
    {
        //This function check to see if any forced neighbors need to be set for jump points

        //dy is +1 for up and -1 for down
        bool foundNeighbor = false;
        if (grid[x_pos, y_pos + dy].f != -1)
        {
            if (grid[x_pos + 1, y_pos].f == -1)
            {
                //Wall to the Right
                open.Add(grid[x_pos + 1, y_pos + dy].f, grid[x_pos + 1, y_pos + dy]);
                foundNeighbor = true;
            }
            if (grid[x_pos - 1, y_pos].f == -1)
            {
                //Wall to the Left
                open.Add(grid[x_pos - 1, y_pos + dy].f, grid[x_pos - 1, y_pos + dy]);
                foundNeighbor = true;
            }
        }

        return foundNeighbor;
    }

    private bool Diagonal_Forced_Neighbor(int x_pos, int y_pos, int dx, int dy)
    {
        //This function checks to see if any forced neighbors need to be set for jump points
        bool foundNeighbor = false;

        if(dx > 0)
        {
            if(dy > 0)
            {
                //Path goind to upper right
                if (grid[x_pos, y_pos - 1].f == -1)
                    open.Add(grid[x_pos + 1, y_pos + 1].f, grid[x_pos + 1, y_pos + 1]);
                else if (grid[x_pos + 1, y_pos].f == -1)
                    open.Add(grid[x_pos + 1, y_pos + 1].f, grid[x_pos + 1, y_pos + 1]);
            }
            else
            {
                //Path going to lower right 
            }
        }
        else
        {
            if(dy > 0)
            {
                //Path going to upper left
            }
            else
            {
                //Path going to lower left
            }
        }


        return foundNeighbor;
    }
}
