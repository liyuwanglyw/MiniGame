using UnityEngine;
using System.Collections;
using System.Linq;

namespace CJFinc.UItools {

/*
	Class: UIStateItemMirror
	<UIStateItemMirror> allows to synchronize main <UIStateItem> item state to another <UIStateItem> items

	(see UIStateItemMirror-editor.png)

	Once <mirrorItems> array is defined

	(see UIStateItemMirror-component-assign.gif)

	any state change of the main item will be translated to all <mirrorItems> items

	(see UIStateItemMirror-state-synchronization.gif)

	Details:
	This component require the <UIStateItem> to be attached to the same game object
*/

[DisallowMultipleComponent]
public class UIStateItemMirror : UIStateItemExtention {
	void Start() {
		if (initOnStart) Init (true);
	}

	protected override void RichInit() {
		base.RichInit(); // as first line - call base initialization

		// your initialization here
		if (StateItem == null) return; // no UIStateItem
	}



	/*
		Var: mirrorItems
		Array of <UIStateItem> items to sync current item state to
	*/
	public UIStateItem [] mirrorItems;



	/*
		Func: ApplyCurrentState ()
		Synchronize current <UIStateItem> state to all <mirrorItems>

		Called automatically on <UIStateItem> state change.
	*/
	public override void ApplyCurrentState() {
		if (mirrorItems == null) return;

		for (int i=0; i<mirrorItems.Length; i++)
			mirrorItems[i].SetState(StateItem.CurrentState);
	}

}
}

