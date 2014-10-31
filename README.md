Software-Engineering-Project
============================

Game developed at Walla Walla University for Software Engineering

Movement:
For movement, we create an array of the entire map, and each unit keeps track of its position on the map.
When you click on a unit, it highlights the places it can move to. When you move the mouse onto grids, they turn colors to denote the predicted movement path. Going back the way you came with the mouse de-colors that square and allows you to move in another direction instead.

For now: 
We create an array of the entire map, holding information about each square. We use this to track distance between different squares and move our characters.

Idea:
Once your base is destroyed, you only have 1 lift left for all of your units, if it dies it's dead. Before that they'll respawn after a certain time.
