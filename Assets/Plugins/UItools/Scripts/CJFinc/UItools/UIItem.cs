using UnityEngine;
using System.Collections;
using System.Linq;

namespace CJFinc.UItools {

/*
	Class: UIItem
	<UIItem> is one of the base components that implements core item properties.

	Item name is a copy from game object name

	(see UIItem-editor.png)

	Details:
	Usually you will not use <UIItem> component directly but one of it descendant components like: <UIStateItem>
*/

[DisallowMultipleComponent]
public class UIItem : RichMonoBehaviour {
	void Start() {
		if (initOnStart) Init (true);
	}

	protected override void RichInit() {
		base.RichInit(); // as first line - call base initialization

		// your initialization here
		InitName();
	}



	// editor
	[SerializeField] bool isItemExpanded;



	/*
		Var: itemName
		Item name filled automatically from <GameObject.name at https://docs.unity3d.com/ScriptReference/Object-name.html>
	*/
	public string itemName;
	void InitName() {
		itemName = gameObject.name;
	}



	/*
		Var: group
		Link to <UIGroup> object this item belongs to

		Defined at <UIGroup> initialization.
	*/
	public UIGroup group;
}

}

