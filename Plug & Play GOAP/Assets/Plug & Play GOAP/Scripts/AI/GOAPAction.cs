using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GOAPAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    public NavMeshAgent agent;

    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;

    public bool running = false;

    public GOAPAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    private void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        if(preconditions != null)
        {
            foreach(WorldState state in preConditions)
            {
                preconditions.Add(state.key, state.value);
            }
        }

        if (afterEffects != null)
        {
            foreach (WorldState state in afterEffects)
            {
                effects.Add(state.key, state.value);
            }
        }
    }

    public bool IsAchievable()
    {

        return true;
    }

    public bool IsAchievableGiven(Dictionary<string,int> conditions)
    {
        foreach(KeyValuePair<string,int> condition in preconditions)
        {
            if(!conditions.ContainsKey(condition.Key))
                return false;
        }

        return true;
        
    }

    public abstract bool PrePerform();

    public abstract bool PostPerform();

}
