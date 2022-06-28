# Maze-Navigation
A simple console OOP maze navigation program made in C# using informed and uninformed search methods on a 2D matrix.
The program returns the shortest possible path and the directions taken to get from the initial state to the goal state.

###### Uninformed Searches
* Depth-first search
* Breadth-first search

###### Informed Searches
* Greedy Best-First search
* A-Star search

# Testcase Format
To change test cases, place files in: "MazeNavigation\bin\Debug". The formatting of the test cases can be seen below:
```
[5,11]
(0,1) 
(7,0) | (10,3) 
(2,0,2,2) 
(8,0,1,2)
(10,0,1,1)
(2,3,1,2)
(3,4,3,1)
(9,3,1,1)
(8,4,2,1)
```
The first line represents the rows and columns of the NxM grid.
```
[5,11] // 5 rows, 11 columns
```
The second line represents the initial starting position on the grid.
```
(0,1)
```
The third line represents the goal states (destination we want to find).
```
(7,0) | (10,3) 
```
All the lines below the goal states are wall configurations.
```
(2,0,2,2) // the square wall has the leftmost top corner occupies cell (2,0) and is 2 cells wide and 2 cell high
(8,0,1,2)
(10,0,1,1)
(2,3,1,2)
(3,4,3,1)
(9,3,1,1)
(8,4,2,1)
```
