// author: Bradley Morrison
// date: 02/09/2022

using System;
using System.IO;
using System.Collections.Generic;
using KdTree.Math;
using KdTree;

namespace MostIsolatedFeatureSearch
{
    class MostIsolated
    {
        /// <summary>
        /// an object that is used to store the values of each individual 'feature', each feature 
        /// has an x value, y value and name.
        /// </summary>
        public class Node
        {
            public string name;
            public double x;
            public double y;
        }

        static void Main(string[] args)
        {
            // get data from file
            string[] fileData = GetData(args);

            // create list of node objects from data
            List<Node> nodeList = CreateNodes(fileData);

            // build and populate kdtree from node objects
            var kdtree = Create_KDTree(nodeList);
            
            // calculate the most isolated node 
            Node mostIsolated = MostIsolated_KdTree(kdtree, nodeList.ToArray());

            // output result to console
            Console.WriteLine("\nmost isolated feature - " + mostIsolated.name +"\n");
        }

        /// <summary>
        /// this fuction uses a Node list to build and populate a KdTree.
        /// </summary>
        /// <param name="nodes"> a list of Node objects </param>
        /// <returns> returns the created KdTree</returns>
        public static KdTree<double, Node> Create_KDTree(List<Node> nodes)
        {
            // build a kdtree - will be sorted using two dimensions (node.x, node.y) and will store the node object as a value
            var kdtree = new KdTree<double, Node>(2, new DoubleMath());

            // add each node from list to kdtree
            foreach (Node node in nodes)
            {
                kdtree.Add(new[] { node.x, node.y }, node);
            }

            return kdtree;
        }

        /// <summary>
        /// this function uses the generated KdTree to find the nearest neighbour of each node. As
        /// the for loop continues, the node with the furthest distance from it's nearest neighbour
        /// is stored and eventually returned.
        /// </summary>
        /// <param name="kdtree"> the optimized data structure that allows for fast nearest neighbour search</param>
        /// <param name="nodeArray"> the array of nodes to be tested</param>
        /// <returns> the most isolated node object in terms of distance from any other node</returns>
        public static Node MostIsolated_KdTree(KdTree<double, Node> kdtree, Node[] nodeArray)
        {
            // variables for keeping track of most isolated node
            Node mostIsolated = null;
            double mostIsolated_double = 0; 

            // for each node
            for (int i=0; i < nodeArray.Length; i++)
            {
                // search through the KdTree to find the nearest neighbour of current node
                KdTree.KdTreeNode<double, Node>[] nearestNeighbour_KdNode = kdtree.GetNearestNeighbours(new[] {nodeArray[i].x, nodeArray[i].y}, 2);

                //extract node object from KdTree node object
                Node nearestNeighbour_Node = nearestNeighbour_KdNode[1].Value;

                //get euclidean distance between current point and nearest neighbour
                double distance = GetDist(nodeArray[i].x, nodeArray[i].y, nearestNeighbour_Node.x, nearestNeighbour_Node.y);

                //store distance if it is bigger than current furthest distance
                if (distance > mostIsolated_double)
                {
                    mostIsolated = nodeArray[i];
                    mostIsolated_double = distance;
                }
            }

            return mostIsolated;
        }
        
        /// <summary>
        /// this function checks that the input file exists, and returns the lines 
        /// of the text file as a string array.
        /// </summary>
        /// <param name="fileName"> holds the argument array from Main()</param>
        /// <returns> an array of strings which correspond to a line of text per array element</returns>
        public static string[] GetData(string[] fileName)
        {
            // get all filenames as string in project directory
            string[] fileEntries = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory());

            bool fileExists = false;

            foreach (string file in fileEntries)
            {
                if (file.Contains(fileName[1]))
                {
                    fileExists = true;
                }
            }

            if (fileExists)
            {
                // if file exists then return the file lines as a string array
                string[] lines = System.IO.File.ReadAllLines(@fileName[1]);
                return lines;
            }
            else
            {
                // show error
                Console.WriteLine("error - " + fileName[1] + " does not exist in " + System.IO.Directory.GetCurrentDirectory());
                Environment.Exit(0);
            }
            
            // if fails return null
            return null;
        }

        /// <summary>
        /// this function iterates through the string array and creates a Node object from line of data, 
        /// splitting the string at spaces to retrieve the relevant data.
        /// </summary>
        /// <param name="data"> the string array taken from the input text file</param>
        /// <returns> a list of Node objects </returns>
        public static List<Node> CreateNodes(string[] data)
        {
            // list to be populated by nodes
            List<Node> nodeList = new List<Node>(); 
            
            // for each line in file data
            for (int i = 0; i < data.Length; i++)
            {
                // parse string
                string[] lineItems = data[i].Split(" ");
                
                // create node object and set parameters
                Node tempNode = new Node();
                tempNode.name = lineItems[0];
                tempNode.x = Convert.ToInt32(lineItems[1]);
                tempNode.y = Convert.ToInt32(lineItems[2]);

                // add to list of nodes
                nodeList.Add(tempNode);

            }

            // return list of node objects
            return nodeList;
        }

        /// <summary>
        /// this function was used in testing to find the nearest neighbours and most isolated feature while 
        /// implementing the KdTree method. It is a linear 'brute force' approach to finding the most isolated 
        /// point and is very slow but accurate.
        /// </summary>
        /// <param name="nodeArray"> an array conversion of the list of nodes </param>
        /// <returns> the most isolated Node object </returns>
        public static Node MostIsolated_Naive(Node[] nodeArray)
        {
            double shortestDist;
            double largestDist = 0;
            double distance = 0;

            Node nearestNeighbour = null;
            Node mostIsolated = null;
            

            // for each node
            for (int i=0; i<nodeArray.Length; i++)
            {
                Console.WriteLine("Node " + (i+1) + " of " + nodeArray.Length);

                shortestDist = 0;
            
                // find nearest neighbour to current node (node[i])
                for (int j=0; j<nodeArray.Length; j++)
                {
                    if (i != j) // to stop identical nodes being checked against themselves
                    {
                        // get distance between node(i) and node(j)
                        distance = GetDist(nodeArray[i].x, nodeArray[i].y, nodeArray[j].x, nodeArray[j].y);

                        // check for new nearest neighbour
                        if (distance < shortestDist || shortestDist == 0)
                        {
                            shortestDist = distance;
                            nearestNeighbour = nodeArray[j];
                        }
                    }
                }
                
                // get current nodes distance to nearest neighbour
                double distanceToNearestNeighbour = GetDist(nodeArray[i].x, nodeArray[i].y, nearestNeighbour.x, nearestNeighbour.y);
                Console.WriteLine("distance between " + nodeArray[i].name + " and " + nearestNeighbour.name + " is " + distanceToNearestNeighbour);
                
                // if the distance is bigger than the current largest distance in memory
                if (distanceToNearestNeighbour > largestDist || largestDist == 0)
                {
                    // update as most isolated feature
                    largestDist = distanceToNearestNeighbour;
                    mostIsolated = nodeArray[i];
                }
            }

            return mostIsolated;
        }

        /// <summary>
        /// this is a helper function that calculates the distance between 2 coordintes.
        /// </summary>
        /// <param name="x1"> the x value of the first coordinate </param>
        /// <param name="y1"> the x value of the first coordinate </param>
        /// <param name="x2"> the y value of the second coordinate </param>
        /// <param name="y2"> the y value of the second coordinate </param>
        /// <returns> the distance between the 2 coordinates as a double</returns>
        public static double GetDist(double x1, double y1, double x2, double y2)
        {
            // calculate the euclidean distance between (x1, y1) and (x2, y2)
            double xDiff = x1 - x2;
            double yDiff = y1 - y2;
            double distance = Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));

            return distance;
        }

    }
}

