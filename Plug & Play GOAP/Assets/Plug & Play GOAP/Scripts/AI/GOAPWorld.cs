using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Sealed Class:  a class that cannot be inherited by any class but can be instantiated*/

public sealed class GOAPWorld 
{
   private static readonly GOAPWorld instance = new GOAPWorld();
    public static WorldStates world;

    static GOAPWorld()
    {
        world = new WorldStates();
    }

    private GOAPWorld()
    {
    }

    public static GOAPWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
