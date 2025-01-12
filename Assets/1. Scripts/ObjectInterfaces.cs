using System;
using System.Collections.Generic;
using UnityEngine;

public interface IStateGetable
{
    public string GetState();
}

public interface IStateSetable
{
    public void SetState(string new_state);
}

public interface IInteractable
{
    public void Interact(Agent agent, string content);
}