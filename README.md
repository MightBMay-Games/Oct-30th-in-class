I used a state machine to control the car simply. I did not have time to make left/right movement or a great scene, as I worked by myself, I chose to focus on functionality of the examples., but I implemented acceleration, a top speed and running out of gas using states.

The basic pattern of my states is as so:

public abstract Class State{
 StateMachine _stateMachine;
... (other variables needed on a state by state basis)

public State(Statemachine statemachine){_stateMachine = statemachine;}
public abstract OnStateEnter(); // called when changing TO a state.
public abstract OnStateExit(); // called when changing AWAY from a state
public abstract Update(); // called each frame the state remains unchanged.

}
if i want to change state, i do it from within update()  ( or i could make another method to seperate it from the update logic if i need).
i just create a new instance of whatever new state, then pass it as a parameter to _statemachine.ChangeState(), and it is changed.

i then have subclasses of this state which i can modify for whatever behaviours i desire.

I start in the accelerated state, where the behaviour is to increase the player velocity until reaching 40 units/second. Upon reaching this speed, we change to the maxSpeed state, which will check if the player ever slows to beneath the max speed, and will switch back to the acceleration state if this occurs.

Then there is the outofgas state, which after enough time, the car will run out of gas, causing it to slow down, and eventually stop.

I use a state machine for this as it allows simple transferal of behaviours for the car. I can switch between accelerating to max speed to running out of fuel simply and efficiently with one method, and the required information for each state to do its job.


For the observer pattern, I create an event in FuelManager.cs, a class where I use a coroutine to decrement the fuel level by 1 every 0.33 seconds, starting from 100 fuel.

When this reaches 0 fuel, it invokes the event, which notifies any methods subscribed to the event from elsewhere.
currently i only have 1 method subscribed, but this would allow simultaneous updates to any amount of other things that need to know when the fuel is empty.

I use this pattern here as we really don’t need to be checking the fuel level every single frame, only once the moment it reaches 0 fuel.
