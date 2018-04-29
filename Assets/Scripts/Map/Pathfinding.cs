using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Location 
{
    public readonly int x;
    public readonly int y;

    public Location(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class PriorityQueue<T>
{
    // From Red Blob: I'm using an unsorted array for this example, but ideally this
    // would be a binary heap. Find a binary heap class:
    // * https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/wiki/Home
    // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
    // * http://xfleury.github.io/graphsearch.html
    // * http://stackoverflow.com/questions/102398/priority-queue-in-net

    private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item,priority));
    }

    // Returns the Location that has the lowest priority
    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Value < elements[bestIndex].Value)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}

// Now that all of our classes are in place, we get get
// down to the business of actually finding a path.
public class Pathfinding 
{
    
    private static float Heuristic(Location a, Location b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // Conduct the A* search
    public static List<Location> FindPath(Location start, Location goal)
    {
        // frontier is a List of key-value pairs:
        // Location, (float) priority
        var frontier = new PriorityQueue<Location>();
        // Add the starting location to the frontier with a priority of 0
        frontier.Enqueue(start, 0f);

        var cameFrom = new Dictionary<Location, Location>();
        var costSoFar = new Dictionary<Location, int>();

        cameFrom.Add(start, start); // is set to start, None in example
        costSoFar.Add(start, 0);

        while (frontier.Count > 0f)
        {
            // Get the Location from the frontier that has the lowest
            // priority, then remove that Location from the frontier
            Location current = frontier.Dequeue();

            // If we're at the goal Location, stop looking.
            if (current.Equals(goal)) break;

            // Neighbors will return a List of valid tile Locations
            // that are next to, diagonal to, above or below current
            var neighbours = TileController.I.Neighbors(current);
            for (int i = 0; i < neighbours.Count; i++)
            {
                var neighbor = neighbours[i];
                var newCost = 1;

                // If there's no cost assigned to the neighbor yet, or if the new
                // cost is lower than the assigned one, add newCost for this neighbor
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor]) {

                    // If we're replacing the previous cost, remove it
                    if (costSoFar.ContainsKey(neighbor)) {
                        costSoFar.Remove(neighbor);
                        cameFrom.Remove(neighbor);
                    }

                    costSoFar.Add(neighbor, newCost);
                    cameFrom.Add(neighbor, current);
                    float priority = newCost + Heuristic(neighbor, goal);
                    frontier.Enqueue(neighbor, priority);
                }
            }
        }

        // TRACE IT BACK
        List<Location> path = new List<Location>();
        Location currentLocation = goal;
        // path.Add(current);

        while (!currentLocation.Equals(start))
        {
            if (!cameFrom.ContainsKey(currentLocation))
            {
                return new List<Location>();
            }
            path.Add(currentLocation);
            currentLocation = cameFrom[currentLocation];
        }
        // path.Add(start);
        path.Reverse();
        return path;
    }
}