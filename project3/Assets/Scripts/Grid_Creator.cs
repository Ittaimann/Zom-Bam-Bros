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

    //public static bool operator ==(Node obj1, Node obj2)
    //{
        
    //    return (obj1.x == obj2.x && obj1.y == obj2.y);
    //}
    //public static bool operator !=(Node obj1, Node obj2)
    //{
    //    return !(obj1.x == obj2.x && obj1.y == obj2.y);
    //}

    public static bool samePosition(Node obj1, Node obj2)
    {
        return (obj1.x == obj2.x && obj1.y == obj2.y);
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
    //public Transform f, t;

    //public GameObject visibleNodes;

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

    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.G))
    //    {
    //        List<Node> path = Get_Path(f, t);

    //        for (int i = 0; i < path.Count; ++i)
    //        {
    //            print(path[i].x + " " + path[i].y);
    //            Instantiate(visibleNodes, new Vector3(path[i].x, path[i].y, 0), Quaternion.identity);
    //        }
    //    }
    //}

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
        SortedList<int, Node> open = new SortedList<int, Node>(new DuplicateKeyComparer<int>());
        List<Node> closed = new List<Node>();

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
                    if (x < 0 || x >= width || y < 0 || y >= height || (x == x_index && y == y_index))
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

                    //Set the Path cost to 10 plus the path cost of the current Node
                    grid[x, y].g = testing.g + 10;

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

}
