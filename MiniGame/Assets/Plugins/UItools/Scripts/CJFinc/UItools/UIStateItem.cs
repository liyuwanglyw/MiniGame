using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CJFinc.UItools {

/*
	Class: UIStateItem
	<UIStateItem> implements different UI item states and provides API to switch between them.

	States:

	<UIStateItem> contains three built-in states: active, inactive and disabled, that ideally fits most of UI elements use cases.

	(see UIStateItem-editor-builtin-states.png)

	Besides of that you can add any additional states that you need.

	(see UIStateItem-editor-custom-states.gif)

	Usually at initialization it's required to set <UIStateItem> to default state.

	You can set default state to one of your state. Use "Flush" button if you don't need a default state.

	(see UIStateItem-default-state.gif)

	States UI game objects:

	<UIStateItem> is automatically uses nested gameobject for each state with exact name.

	(see UIStateItem-editor-ui-gameobjects-automatic-mode.gif)

	Alternatively you can switch to "Manual" mode. It will allow you to link each state game object manually to any game object from hierarchy.

	(see UIStateItem-editor-ui-gameobjects-manual-mode.gif)

	On state change <UIStateItem> uses <GameObject.SetActive at https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html> to enable new state UI game object and to disable other states.

	You can easily test state change directly in editor as well as in player mode.

	(see UIStateItem-ui-gameobjects-state-change.gif)

	Callbacks:

	<UIStateItem> implements callback event <OnStateChange> that called at any state change

	(see UIStateItem-editor-callbacks.png)

	Details:

	Check the next additional components to add extra features to <UIStateItem>
	* <UIStateItemMirror>
	* <UIStateItemAnimation>
*/

[DisallowMultipleComponent]
public class UIStateItem : UIItem {
	void Start() {
		if (initOnStart) Init (true);
	}

	protected override void RichInit() {
		base.RichInit(); // as first line - call base initialization

		stateItemMirror = GetComponent<UIStateItemMirror>();
		if (stateItemMirror != null) stateItemMirror.Init();

		stateItemAnimation = GetComponent<UIStateItemAnimation>();
		if (stateItemAnimation != null) stateItemAnimation.Init();

		InitUIObjects();

		if (!IsInitialized) SetStateDefault(true);
	}



	// editor
	[SerializeField] bool isStateItemExpanded;
	[SerializeField] bool isStateItemStatesExpanded;
	[SerializeField] bool isStateItemUiExpanded;
	[SerializeField] bool isStateItemTestingExpanded;



	// extensions
	UIStateItemAnimation stateItemAnimation;
	UIStateItemMirror stateItemMirror;



	// states
	/*
		Const: STATE_INACTIVE
		Built-in state "inactive"
	*/
	public const string STATE_INACTIVE = "inactive";
	/*
		Const: STATE_ACTIVE
		Built-in state "active"
	*/
	public const string STATE_ACTIVE = "active";
	/*
		Const: STATE_DISABLED
		Built-in state "disabled"
	*/
	public const string STATE_DISABLED = "disabled";

	/*
		Prop: States
		String array of states names

		Default tree states are:
		- inactive
		- active
		- disabled

		(start code)
		Debug.Log("UIStateItem states count = " + GetComponent<UIStateItem>().States.Length);
		(end code)
	*/
	public string [] States { get { return states; } }
	[SerializeField] string [] states = new string[] { STATE_INACTIVE, STATE_ACTIVE, STATE_DISABLED };

	/*
		Prop: StatesUi
		<GameObject at https://docs.unity3d.com/ScriptReference/GameObject.html> array of <States> UI game objects

		You can assign game objects in run time. Just make sure that "UI game objects" mode is set to "manual" in editor. See <UIStateItem> editor manual for details.

		Note: You can't change a <StatesUi> array size. The <StatesUi> array size is always equals to <States> array size and controlling automatically.
	*/
	public GameObject [] StatesUi { get { return statesUi; }}
	[SerializeField] GameObject [] statesUi = new GameObject[ 3 ];

	/*
		Prop: CurrentState
		Current state name
	*/
	public string CurrentState { get { return GetStateName(currentStateId); } }
	[SerializeField] int currentStateId;

	/*
		Prop: DefaultState
		Default state name
	*/
	public string DefaultState { get { return GetStateName(defaultStateId); } }
	[SerializeField] int defaultStateId; // default state is - inactive

	/*
		Prop: PreviousState
		Previous state name before last state change
	*/
	public string PreviousState { get { return GetStateName(previousStateId); } }
	[SerializeField] int previousStateId;

	/*
		Prop: StateGroup
		Link to <UIStateGroup> object this item belongs to

		If group defined then <UIStateGroup.ItemStateChanged (itemName)> function will be called for each state change.
	*/
	public UIStateGroup StateGroup { get { return group as UIStateGroup; }}

	/*
		Func: SetDefaultStateTo (state)
		Set default state to new state by name

		(start code)
		GetComponent<UIStateItem>().SetDefaultStateTo(UIStateItem.STATE_ACTIVE);
		GetComponent<UIStateItem>().SetStateDefault();
		(end code)

		Parameters:
			state - state name from <States> array
	*/
	public void SetDefaultStateTo(string state) {
		int stateId = GetStateId(state);
		if (stateId < 0) return; // no state found
		SetDefaultStateTo(stateId);
	}
	void SetDefaultStateTo(int state) {
		if (state < 0 || state >= states.Length) return; // state id is exceed states array size
		defaultStateId = state;
	}

	/*
		Func: FlushDefaultState ()
		Flush default state
	*/
	public void FlushDefaultState() {
		defaultStateId = -1;
	}

	/*
		Func: GetStateName (stateId)
		Get state name by its id from <States> array

		Parameters:
			stateId - state id from <States> array

		Returns:
			Found state name or empty string
	*/
	public string GetStateName(int stateId) {
		return (stateId >= 0 && stateId < states.Length) ? states[stateId] : "";
	}

	/*
		Func: GetStateId (state)
		Get state id by its name from <States> array

		Parameters:
			state - state name from <States> array

		Returns:
			Found state id or -1
	*/
	public int GetStateId(string state) {
		return Array.FindIndex(states, s => s == state);
	}

	/*
		Func: AddState (state)
		Add new state with given name

		Parameters:
			state - new state name

		Returns:
			new state id from <States> array or -1 in case of duplicate state name
	*/
	public int AddState (string state) {
		if (states.Any(state.Equals)) return -1;

		string [] newStates = new string[ states.Length + 1 ];
		states.CopyTo(newStates, 0);
		newStates[newStates.Length - 1] = state;
		states = new string[ newStates.Length ];
		newStates.CopyTo(states, 0);

		AddStateUI();
		if (stateItemAnimation != null) stateItemAnimation.InitAnimationStates();

		return states.Length - 1;
	}

	/*
		Func: RemoveState (state)
		Remove state by given name

		Note: Built-in states could not be removed!

		Parameters:
			state - state name to remove
	*/
	public void RemoveState (string state) {
		int stateId = GetStateId(state);
		if (stateId < 0) return; // no state found
		RemoveState(stateId);
	}
	void RemoveState (int stateId) {
		if (stateId < 3) return; // do not allow to delete three built-in states
		if (stateId >= states.Length) return; // state id is exceed states array size

		states = (from element in states where element != states[stateId] select element).ToArray();

		if (currentStateId == stateId) currentStateId = 0;
		if (defaultStateId == stateId) defaultStateId = 0;

		RemoveStateUI(stateId);
		if (stateItemAnimation != null) stateItemAnimation.InitAnimationStates();
	}



	// states control
	/*
		Func: SetState (state, force)
		Change <CurrentState> to given state

		For force = true - all state change actions will be processed even if <CurrentState> is already equals to given state.

		For force = false - all actions will be skipped if <CurrentState> is already equals to given state.

		Parameters:
			state - state name from <States> array
			force - bool, should state change be forced?
	*/
	public void SetState(string state, bool force) {
		int stateId = GetStateId(state);
		if (stateId < 0) return; // no state found
		SetStateById (stateId, force);
	}
	void SetStateById(int stateId, bool force) {
		if (stateId < 0 || stateId >= states.Length) return;
		if (!force && currentStateId == stateId) return; // skip - item is already in this state

		previousStateId = currentStateId;
		currentStateId = stateId;

		if (OnStateChange != null) OnStateChange.Invoke(); // call OnStateChange event
		if (StateGroup != null && StateGroup.IsInitialized) StateGroup.ItemStateChanged(itemName); // call ItemStateChanged for exist and initialized group

		// update state UI
		for (int i=0; i<states.Length; i++) StateOffForce(i);
		StateOn(currentStateId);
	}

	/*
		Func: SetState (state)
		Change <CurrentState> to given state if it's not in given state yet

		This function will skip all actions if <CurrentState> is already equals to given state.

		See <SetState (state, force)> for details.

		Parameters:
			state - state name from <States> array
	*/
	public void SetState(string state) {
		SetState(state, false);
	}
	/*
		Func: ForceSetState (state)
		Force change <CurrentState> to given state

		This function will set <CurrentState> to given state even if <CurrentState> is already equals to given state.

		See <SetState (state, force)> for details.

		Parameters:
			state - state name from <States> array
	*/
	public void ForceSetState(string state) {
		SetState(state, true);
	}

	/*
		Func: SetStateDefault (force)
		Shortcut function to set <CurrentState> to <DefaultState>

		Use force parameter to control state change behavior. See <SetState (state, force)> for details.
	*/
	public void SetStateDefault(bool force = false) { SetState(DefaultState, force); }

	/*
		Func: SetStateActive (force)
		Shortcut function to set <CurrentState> to built-in state UIStateItem.STATE_ACTIVE

		Use force parameter to control state change behavior. See <SetState (state, force)> for details.

		Parameters:
			force - bool, should state change be forced?
	*/
	public void SetStateActive(bool force = false) { SetState(STATE_ACTIVE, force); }

	/*
		Func: SetStateInactive (force)
		Shortcut function to set <CurrentState> to built-in state UIStateItem.STATE_INACTIVE

		Use force parameter to control state change behavior. See <SetState (state, force)> for details.

		Parameters:
			force - bool, should state change be forced?
	*/
	public void SetStateInactive(bool force = false) { SetState(STATE_INACTIVE, force); }

	/*
		Func: SetStateDisabled (force)
		Shortcut function to set <CurrentState> to built-in state STATE_DISABLED

		Use force parameter to control state change behavior. See <SetState (state, force)> for details.

		Parameters:
			force - bool, should state change be forced?
	*/
	public void SetStateDisabled(bool force = false) { SetState(STATE_DISABLED, force); }





	// states UI
	[SerializeField] int statesUiMode; // 0 - automatic, 1 - manual
	[SerializeField] bool isEditorStatesUiExpanded; // for editor inspector

	void InitUIObjects() {
		if (statesUiMode == 1) return; // skip init ui for manual mode

		for (int i=0; i<states.Length; i++) {
			statesUi[i] = GetStateUIObject(i);
		}
	}

	void AddStateUI () {
		GameObject [] newStatesUi = new GameObject[ states.Length ];
		statesUi.CopyTo(newStatesUi, 0);
		statesUi = new GameObject[ states.Length ];
		newStatesUi.CopyTo(statesUi, 0);
	}

	void RemoveStateUI(int i) {
		statesUi = statesUi.Where((obj, index) => index != i).ToArray();
	}

	GameObject GetStateUIObject(int state) {
		Transform rt = transform.Find(states[state]);
		if (rt != null) return rt.gameObject;
		return null;
	}

	// states UI control
	void StateUiOn(int state, bool force=false) {
		GameObject stateUi = statesUi[state];
		if (stateUi == null && Application.isEditor) stateUi = GetStateUIObject(state);

		if (stateUi != null) {
			stateUi.SetActive(true);
		}

		// TODO: skip animation for force
		if (force) {
		}
		else {
		}
	}

	void StateUiOff(int state, bool force=false) {
		GameObject stateUi = statesUi[state];
		if (stateUi == null && Application.isEditor) stateUi = GetStateUIObject(state);

		if (stateUi != null) {
			stateUi.SetActive(false);
		}

		// TODO: skip animation for force
		if (force) {
		}
		else {
		}
	}

	void StateOnForce(int state) { StateOn(state, true); }
	void StateOffForce(int state) { StateOff(state, true); }
	void StateOn(int state, bool force=false) { StateUiOn(state, force); }
	void StateOff(int state, bool force=false) { StateUiOff(state, force); }



	// callbacks
	/*
		Var: OnStateChange
		This <UnityEvent at https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html> is called on each state change

		You can subscribe any of your functions to this event.

		(start code)
		GetComponent<UIStateItem>().OnStateChange.AddListener(StateChanged);

		...

		void StateChanged() {
			Debug.Log("State has been changed!");
		}
		(end code)

	*/
	public UnityEvent OnStateChange;

}
}
