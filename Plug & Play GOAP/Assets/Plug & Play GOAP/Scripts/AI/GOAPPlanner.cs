using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using System;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GOAPAction action;

    public Node(Node parent, float cost, Dictionary<string, int> states, GOAPAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int> (states);
        this.action = action;
    }
}

public class GOAPPlanner 
{
    public Queue<GOAPAction> Plan(List<GOAPAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GOAPAction> usableActions = new List<GOAPAction>();
        foreach (GOAPAction action in actions)
        {
            if (action.IsAchievable())
            {
                usableActions.Add(action);
            }
        }

        List<Node> leaves = new List<Node> ();
        Node start = new Node(null, 0, GOAPWorld.Instance.GetWorld().GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        Node cheapest = null;

        foreach(Node leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }

            else if(leaf.cost < cheapest.cost)
            {
                cheapest = leaf;
            }
        }

        List<GOAPAction> result = new List<GOAPAction> ();
        Node n = cheapest;

        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GOAPAction> queue = new Queue<GOAPAction> ();

        foreach(GOAPAction action in result)
        {
            queue.Enqueue(action);
        }

        Debug.Log("The Plan is :");
        foreach(GOAPAction action in queue)
        {
            Debug.Log ("Q: " +action.actionName);
        }
        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GOAPAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GOAPAction action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string,int>effects in action.effects)
                {
                    if (!currentState.ContainsKey(effects.Key))
                    {
                        currentState.Add(effects.Key,effects.Value);
                    }
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GOAPAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;
    }

    private List<GOAPAction> ActionSubset(List<GOAPAction> usableActions, GOAPAction removeAction)
    {
        List<GOAPAction> subset = new List<GOAPAction>();
        foreach(GOAPAction action in usableActions)
        {
            if (!action.Equals(removeAction))
            {
                subset.Add(action);
            }
        }
        return subset;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
       foreach(KeyValuePair<string, int>effects in goal)
        {
            if (!state.ContainsKey(effects.Key))
            {
                return false;
            }
        }
       return true;
    }
}
