Author: Bradley Morrison
Contact: bradley_morrison@outlook.com
LinkedIn: https://www.linkedin.com/in/bradleymorrison173/
Github: https://github.com/brad-morrison
Website: bradmorrison.dev

--------------

Program: MostIsolated.cs
Language: C#
Framework: .NET Core 2.2

Problem:

Given a list of millions of node coordinates, I developed a system that analyses all points and finds the most isolated point in seconds. The program accepts .txt files with coordinates and outputs the result.

Instructions:

The program folder contains the given text files (problem_small.txt, problem_big.txt), to run another file just move the desired text file to the same directory as the 'MostIsolated.cs' program file.


To use the program - 

	- navigate to the program directory in the console
	- type "dotnet run mostisolated" followed by the filename of desired data file
		
		example:

		dotnet run mostisolated problem_big.txt



Approach:

I began the program by creating functions to retrieve the data from a text file, create objects from that data and then store the objects in a list that would be used to sort through. 

I began the core purpose of the program (finding the most isolated point) by creating a 'brute-force' function that would iterate through all Node objects, find their nearest neighbour and then return the Node with the largest distance between itself and it's nearest neighbour. This returned the correct value each time, but, as expected, was painfully slow and expensive.

The second purpose of the program is to find the most isolated point in better than O(n^2) complexity. To achieve this I decided to implement a KdTree (https://en.wikipedia.org/wiki/K-d_tree#High-dimensional_data) as a way of structuring the data into a searchable tree which makes the nearest neighbour search very fast. I utilised a 3rd party KdTree package (see LICENCE file) that allows my program to search for nearest neighbours in O(log n) search time.

After each nearest neighbour is found the distances are simply compared to find the Node with the furthest away nearest neighbour, therefore the most isolated.
