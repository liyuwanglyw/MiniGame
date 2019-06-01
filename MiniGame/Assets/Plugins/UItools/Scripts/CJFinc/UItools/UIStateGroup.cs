using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace CJFinc.UItools {

/*
	Class: UIStateGroup
	<UIStateGroup> allows to manage group of <UIStateItem> items and perform bulk state change

	(see UIStateGroup-editor.png)

	In editor view <UIStateGroup> provides functionality to change all items to particular state

	(see UIStateGroup-bulk-state-change.gif)


	Callbacks:

	<UIStateGroup> implements callback event <OnStateChange> that called on each group state change

	(see UIStateGroup-editor-callbacks.png)


	Details:

	Scripting functions listed bellow allows to do more different group states changes.

*/

[DisallowMultipleComponent]
public class UIStateGroup : UIGroup {
	void Start() {
		if (initOnStart) Init (true);
	}

	protected override void RichInit() {
		if (itemType == "UIItem" || itemType == null) itemType = "UIStateItem";
		base.RichInit(); // as first line - call base initialization

		// your initialization here
		GrabStateItems();
		InitStates();
	}



	// editor
	[SerializeField] bool isStateGroupExpanded;



	/*
		Prop: StateItems
		Array of all detected <UIStateItem> items
	*/
	public UIStateItem [] StateItems { get { return stateItems; } }
	UIStateItem [] stateItems;

	void GrabStateItems() {
		stateItems = items.Cast<UIStateItem>().ToArray();
	}

	/*
		Prop: States
		String array of all states grabbed from <StateItems>
	*/
	public string [] States { get { return states; } }
	[SerializeField] string [] states = new string[] { UIStateItem.STATE_INACTIVE, UIStateItem.STATE_ACTIVE, UIStateItem.STATE_DISABLED };

	void InitStates() {

		List <string> allStates = new List <string>();
		for (int i=0; i<StateItems.Length; i++) {
			for (int j=0; j<StateItems[i].States.Length; j++) {
				allStates.Add(StateItems[i].States[j]);
			}
		}
		allStates = allStates.Select(x => x).Distinct().ToList();
		states = allStates.ToArray();
	}

	/*
		Func: GetStateItem (itemName)
		Gets <UIStateItem> by name from <StateItems> array

		(start code)
		UIStateItem StateItem = GetComponent<UIStateGroup>().GetStateItem("item (1)");

		if (StateItem != null) {
			Debug.Log("Found item : " + StateItem.itemName);
		}
		(end code)

		Parameters:
			itemName - <UIStateItem> item name

		Returns:
			Found <UIStateItem> or null
	*/
	public UIStateItem GetStateItem(string itemName) {
		return GetItem(itemName) as UIStateItem;
	}



	// states management
	/*
		Func: ItemStateChanged (itemName)
		Internal function. Called automatically from <UIStateItem> for each state change

		This function used as an internal callback between components <UIStateItem> and <UIStateGroup>

		Parameters:
			itemName - item name
	*/
	public virtual void ItemStateChanged(string itemName) {
		UIStateItem StateItem = GetStateItem(itemName);
		if (StateItem == null) return;
	}

	/*
		Func: SetStateForItem (state, itemName)
		Set state for item

		(start code)
		GetComponent<UIStateGroup>().SetStateForItem(UIStateItem.STATE_ACTIVE, "item (1)");
		GetComponent<UIStateGroup>().SetStateForItem("custom state", "item (1)");
		(end code)

		Parameters:
			state - state name
			itemName - <UIStateItem> item name
	*/
	public void SetStateForItem(string state, string itemName) {
		SetStateForItems( state, new string [] {itemName});
	}

	/*
		Func: SetStateForItems (state, itemsNames)
		Set state for several items

		(start code)
		GetComponent<UIStateGroup>().SetStateForItems(UIStateItem.STATE_ACTIVE, new string [] {"item (1)", "item (3)"});
		(end code)

		Parameters:
			state - state name
			itemsNames - string array of <UIStateItem> items names
	*/
	public void SetStateForItems(string state, string [] itemsNames) {
		if (OnStateChange != null) OnStateChange.Invoke(); // call OnStateChange event

		for (int i=0; i<itemsNames.Length; i++) {
			UIStateItem StateItem = GetItem(itemsNames[i]) as UIStateItem;
			if (StateItem != null) {
				StateItem.SetState(state);
			}
		}
	}

	/*
		Func: SetStateExceptItem (state, excludeItemName)
		Set state for all items excluding given item

		(start code)
		GetComponent<UIStateGroup>().SetStateExceptItem(UIStateItem.STATE_ACTIVE, "item (1)");
		(end code)

		Parameters:
			state - state name
			excludeItemName - <UIStateItem> item name to exclude
	*/
	public void SetStateExceptItem(string state, string excludeItemName) {
		SetStateExceptItems(state, new string[] {excludeItemName});
	}

	/*
		Func: SetStateExceptItems (state, excludeItemsNames)
		Set state for all items excluding given items names

		(start code)
		GetComponent<UIStateGroup>().SetStateExceptItems(UIStateItem.STATE_ACTIVE, new string [] {"item (1)", "item (3)"});
		(end code)

		Parameters:
			state - state name
			excludeItemsNames - string array of <UIStateItem> items names to exclude
	*/
	public void SetStateExceptItems(string state, string [] excludeItemsNames) {
		if (OnStateChange != null) OnStateChange.Invoke(); // call OnStateChange event

		for (int i=0; i<items.Length; i++) {
			bool isExclude = false;
			for (int j=0; j<excludeItemsNames.Length; j++) {
				if (excludeItemsNames[j] == items[i].itemName) {
					isExclude = true;
					break;
				}
			}
			if (isExclude) continue;
			(items[i] as UIStateItem).SetState(state);
		}
	}

	/*
		Func: SetStateForAllItems (state)
		Set state for all items

		(start code)
		GetComponent<UIStateGroup>().SetStateForAllItems(UIStateItem.STATE_INACTIVE);
		(end code)

		Parameters:
			state - state name
	*/
	public void SetStateForAllItems(string state) {
		if (OnStateChange != null) OnStateChange.Invoke(); // call OnStateChange event

		for (int i=0; i<items.Length; i++) {
			UIStateItem StateItem = items[i] as UIStateItem;
			if (StateItem != null) {
				StateItem.SetState(state);
			}
		}
	}



	// callbacks
	/*
		Var: OnStateChange
		This <UnityEvent at https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html> is called when animation transition is finished

		You can subscribe any of your functions to this event.

		(start code)
		GetComponent<UIStateGroup>().OnStateChange.AddListener(OnGroupStateChange);

		...

		void OnGroupStateChange() {
			Debug.Log("Group state changes!");
		}
		(end code)

	*/
	public UnityEvent OnStateChange;

}

}
