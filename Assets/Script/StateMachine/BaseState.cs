using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class BaseState<EState> where EState: Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;
    
    void Start()
    {
        CurrentState.EnterState();
    }
    void Update()
    {
        CurrentState.UpdateState();
    }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
    public abstract void OnTriggerEnter();
    public abstract void OnTriggerStay();
    public abstract void OnTriggerExit();
}