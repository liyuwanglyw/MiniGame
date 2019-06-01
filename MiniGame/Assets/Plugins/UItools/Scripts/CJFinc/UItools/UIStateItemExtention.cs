using UnityEngine;
using System.Collections;
using System.Linq;

namespace CJFinc.UItools {

/*
	public override void ApplyCurrentState() {
		// Debug.Log("previous state: " + StateItem.PreviousState);
		// Debug.Log("current state: " + StateItem.CurrentState);
	}
*/

/*
	Class: UIStateItemExtention
	<UIStateItemExtention> is a mediator class of all <UIStateItem> extension classes

	You have to use one of the extension listed bellow instead of including <UIStateItemExtention> component directly.

	See <UIStateItemMirror>, <UIStateItemAnimation>

	Details:
	This component and all its descendants require the <UIStateItem> to be attached to the same game object.
*/

public class UIStateItemExtention : RichMonoBehaviour {

	protected override void RichInit() {
		base.RichInit(); // as first line - call base initialization

		// your initialization here
		stateItem = GetComponent<UIStateItem>();
		if (stateItem == null) {
			Debug.LogWarning("UIStateItem component is missed!");
			return;
		}

		SubscribeToStateChange();
	}


	/*
		Prop: StateItem
		<UIStateItem> component attached to the same game object this extension extends
	*/
	public UIStateItem StateItem { get { return stateItem; }}
	UIStateItem stateItem;

	void SubscribeToStateChange() {
		if (stateItem.OnStateChange != null) {
			stateItem.OnStateChange.RemoveListener(ApplyCurrentState);
			stateItem.OnStateChange.AddListener(ApplyCurrentState);
		}
	}

	/*
		Func: ApplyCurrentState ()
		called automatically on <UIStateItem.OnStateChange> event

		Should be implemented in descendant classes.
	*/
	public virtual void ApplyCurrentState() {}
}
}


