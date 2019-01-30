using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineObject : AnimatedObject
{
    protected Action[] States;
    protected Action[] StatesStarts;
    public int State = 0;

    // Update is called once per frame
    public void Update ()
    {
        if (States != null && States.Length > State &&
            States[State] != null)
            States[State]();
	}

    public void StartState(int state)
    {
        if (StatesStarts != null && StatesStarts.Length > state && 
            StatesStarts[state] != null)
            StatesStarts[state]();
        State = state;
    }
}
