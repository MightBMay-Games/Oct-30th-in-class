using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class State
{
    protected Rigidbody _rb;
    protected StateMachine _stateMachine;
    public State(Rigidbody rb, StateMachine stateMachine) { 
        _rb = rb;
        _stateMachine = stateMachine;
    }
    public abstract void OnStateEnter();
    public abstract void Update();
    public abstract void OnStateExit();
}

[System.Serializable]
public class AccelerateState : State
{

    public AccelerateState(Rigidbody rb, StateMachine stateMachine) : base(rb, stateMachine)
    {

    }

    public override void OnStateEnter() { 
        // nothing needed to be done
    }

    public override void Update()
    {

        if (_rb.velocity.magnitude < 40)
        {
            _rb.velocity += Vector3.up * 0.5f * Time.fixedDeltaTime;
        }
        else
        {
            _stateMachine.ChangeState(new MaxSpeedState(_rb, _stateMachine));
        }

    }
    public override void OnStateExit() {
        // nothing needed
    }
}
[System.Serializable]
public class MaxSpeedState : State
{
    public MaxSpeedState(Rigidbody rb, StateMachine stateMachine) : base(rb, stateMachine) { }
    public override void OnStateEnter() { }
    public override void Update()
    {
        if (_rb.velocity.magnitude < 40)
        {
            _stateMachine.ChangeState(new AccelerateState(_rb, _stateMachine));
        }


    }
    public override void OnStateExit() { }
}

[System.Serializable]
public class OutOfGasState : State
{
    public OutOfGasState(Rigidbody rb, StateMachine stateMachine) : base(rb, stateMachine) { }
    public override void OnStateEnter() {
        Debug.Log("out of fuel. slowing down.");
    }
    public override void Update()
    {
        if (_rb.velocity.magnitude <= 0)
        {
            Debug.Log(" YOU LOSE");
            return;
        }
        _rb.velocity -= Vector3.up * 0.25f*Time.fixedDeltaTime;

       
    }
    public override void OnStateExit() { }
}


public class StateMachine : MonoBehaviour
{

    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;

    Rigidbody _rb;
    [SerializeField]State _currentState = null;

    [SerializeField]float _GasLevel = 100;
    public float GasLevel { get => _GasLevel; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _currentState = new AccelerateState(_rb, this);
        FuelManager.instance.FuelEmptyEvent.AddListener(OutOfFuel);

    }

    private void FixedUpdate()
    {
        _currentState?.Update();
    }

    public void OutOfFuel()
    {
        ChangeState(new OutOfGasState(_rb, this));
    }
    public void ChangeState(State newState)
    {
        _currentState.OnStateExit();
        _currentState = newState;
        _currentState.OnStateEnter();

    }

    public State curState { get => _currentState; }





}

