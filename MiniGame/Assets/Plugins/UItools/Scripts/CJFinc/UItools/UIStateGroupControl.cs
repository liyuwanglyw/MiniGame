using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CJFinc.UItools {

/*
	Class: UIStateGroupControl
	<UIStateGroupControl> extends <UIStateGroup> into the group of items with only one item selected.

	(see UIStateGroupControl-editor.png)

	<UIStateGroupControl> works with active and inactive states of <UIStateItem>.
	It reacts to any <UIStateItem> state change and controls that only one item should be selected at the same time.

	(see UIStateGroupControl-editor-hierarchy.gif)


	There are two modes available
	- one active
	- one inactive

	In "one active" <UIStateGroupControl> controls that only one item is active.

	And "one inactive" has an inverse behavior when only one item could be inactive.
*/

[DisallowMultipleComponent]
public class UIStateGroupControl : UIStateGroup {

	void Start() {
		if (initOnStart) Init (true);
	}

	protected override void RichInit() {
		base.RichInit(); // as first line - call base initialization

		// your initialization here
		Flush();
	}



	// editor
	[SerializeField] bool isStateGroupControlExpanded;



	/*
		Enum: STATE_GROUP_CONTROL_MODE
		Enumeration of possible group modes

		- OneActive
		- OneInActive
	*/
	public enum STATE_GROUP_CONTROL_MODE {
		OneActive = 0,
		OneInActive = 1
	}



	/*
		Prop: Mode
		Current group mode <STATE_GROUP_CONTROL_MODE>
	*/
	public STATE_GROUP_CONTROL_MODE Mode { get { return mode; } }
	[SerializeField] STATE_GROUP_CONTROL_MODE mode;


	/*
		Prop: SelectedItem
		Currently selected <UIStateItem> item
	*/
	public UIStateItem SelectedItem { get { return selectedItem; } }
	/*
		Prop: SelectedItemName
		Currently selected <UIStateItem> item name
	*/
	public string SelectedItemName { get { return (selectedItem == null) ? "" : selectedItem.itemName; } }
	[SerializeField] UIStateItem selectedItem;



	// state group mode
	/*
		Func: SetMode (newMode)
		Change group mode and set items to initial states

		(start code)
		GetComponent<UIStateGroupControl>().SetMode(STATE_GROUP_CONTROL_MODE.OneInActive);
		(end code)

		Parameters:
			newMode - <STATE_GROUP_CONTROL_MODE> new group mode
	*/
	public void SetMode(STATE_GROUP_CONTROL_MODE newMode) {
		mode = newMode;
		Flush();
	}


	/*
		Func: ItemStateChanged (itemName)
		Internal function. Called automatically from <UIStateItem> for each state change

		See <UIStateGroup.ItemStateChanged (itemName)> for details.

		Parameters:
			itemName - item name
	*/
	public override void ItemStateChanged(string itemName) {
		UIStateItem StateItem = GetStateItem(itemName);
		if (StateItem == null) return;

		//		if Mode - one active
		//			if item active
		//				set others to inactive
		if (Mode == STATE_GROUP_CONTROL_MODE.OneActive) {
			if (StateItem.CurrentState == UIStateItem.STATE_ACTIVE) {
				selectedItem = StateItem;

				for (int i=0; i<StateItems.Length; i++) {
					if (StateItems[i].itemName != itemName)
						StateItems[i].SetState(UIStateItem.STATE_INACTIVE);
				}
			}
			else {
				if (SelectedItemName == itemName) selectedItem = null;
			}
		}

		//		if Mode - one inactive
		//			if item inactive
		//				set others to active
		if (Mode == STATE_GROUP_CONTROL_MODE.OneInActive) {
			if (StateItem.CurrentState == UIStateItem.STATE_INACTIVE) {
				selectedItem = StateItem;

				for (int i=0; i<StateItems.Length; i++) {
					if (StateItems[i].itemName != itemName)
						StateItems[i].SetState(UIStateItem.STATE_ACTIVE);
				}
			}
			else {
				if (SelectedItemName == itemName) selectedItem = null;
			}
		}
	}

	/*
		Func: SetItemActive(itemName)
		Change item with given name to state UIStateItem.STATE_ACTIVE

		(start code)
		GetComponent<UIStateGroupControl>().SetItemActive("item (2)");
		(end code)

		Parameters:
			itemName - <UIStateItem> item name
	*/
	public void SetItemActive(string itemName) {
		UIStateItem StateItem = GetStateItem(itemName);
		if (StateItem == null) return;
		StateItem.SetStateActive();
	}

	/*
		Func: SetItemInactive(itemName)
		Change item with given name to state UIStateItem.STATE_INACTIVE

		(start code)
		GetComponent<UIStateGroupControl>().SetItemInactive("item (2)");
		(end code)

		Parameters:
			itemName - <UIStateItem> item name
	*/
	public void SetItemInactive(string itemName) {
		UIStateItem StateItem = GetStateItem(itemName);
		if (StateItem == null) return;
		StateItem.SetStateInactive();
	}

	/*
		Func: Flush()
		Flush all items for default state according to current <Mode>

		(start code)
		GetComponent<UIStateGroupControl>().Flush();
		(end code)
	*/
	public void Flush() {
		switch (Mode) {
			case STATE_GROUP_CONTROL_MODE.OneActive:
				selectedItem = null;
				for (int i=0; i<StateItems.Length; i++)
					StateItems[i].SetStateInactive();
				break;

			case STATE_GROUP_CONTROL_MODE.OneInActive:
				selectedItem = null;
				for (int i=0; i<StateItems.Length; i++)
					StateItems[i].SetStateActive();
				break;
		}
	}

}
}
