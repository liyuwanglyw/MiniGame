using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CJFinc.UItools {

/*
	Class: UIGroup
	<UIGroup> component allows you to create a group of items and get access to any item by name or id.

	(start code)
	UIGroup group = GetComponent<UIGroup>();

	// get by id
	UIItem firstItem = group.GetItem(0);

	// get by name
	UIItem itemByName = group.GetItem("item (2)");
	(end code)

	There is a two items detection modes available
	- Automatic (default)
	- Manual

	(see UIGroup-editor.png)

	In "Automatic" mode <UIGroup> detects items from its children.

	NOTE: Disabled game objects and game objects with disabled <UIItem> component are ignores.

	(see UIGroup-editor-and-hierarchy-items-automatic.gif)

	"Manual" mode allows to define items for group from anywhere in hierarchy.

	(see UIGroup-editor-and-hierarchy-items-manual.gif)

	Details:
	For each group item the <UIItem.group> parameter will be linked to current group.

	(see UIGroup-editor-and-hierarchy-items-group.gif)

	You may need to force re-init group when items count has been changed in runtime in hierarchy (item added/removed, enabled/disabled)
	(start code)
	UIGroup group = GetComponent<UIGroup>();
	group.InitForce();
	(end code)

	Supported items classes are <UIItem> and all it descendant.
*/

[DisallowMultipleComponent]
public class UIGroup : RichMonoBehaviour {
	void Start() {
		if (initOnStart) Init (true);
	}

	protected override void RichInit() {
		base.RichInit(); // as first line - call base initialization

		// your initialization here
		InitGroupItems();
	}



	 // editor
	[SerializeField] bool isGroupExpanded;



	// items
	[SerializeField] int itemsMode; // 0 - automatic, 1 - manual

	/*
		Var: items
		Array of <UIItem> items assigned to current group

		(start code)
		Debug.Log("UIGroup items count = " + GetComponent<UIGroup>().items.Length);
		(end code)
	*/
	public UIItem [] items;

	[SerializeField] Dictionary <string, UIItem> itemsByName;

	protected string itemType;

	void InitGroupItems() {
		if (itemType == null || itemType == "") itemType = "UIItem";

		int i;
		itemsByName = new Dictionary <string, UIItem>(); // flush itemsByName dictionary

		// Init from children
		if (itemsMode == 0) {
			for (i=0; i<transform.childCount; i++) {
				UIItem item = transform.GetChild(i).gameObject.GetComponent(itemType) as UIItem;
				// init and use only items with correct enabled component and active gameobject
				if (item != null && item.gameObject.activeSelf && item.enabled) {
					item.Init(); // force items initialization
					itemsByName[item.itemName] = item;
					item.group = this;
				}
			}

			// set items array
			items = new UIItem [itemsByName.Count];
			i=0;
			foreach (UIItem item in itemsByName.Values) {
				items[i] = item;
				i++;
			}
		}

		// Init items from array
		if (itemsMode == 1) {
			// remove items with wrong type
			List <UIItem> tempItems = new List <UIItem>();
			for (i=0; i<items.Length; i++) {
				UIItem item = items[i].transform.gameObject.GetComponent(itemType) as UIItem;
				if (item != null) {
					tempItems.Add(item);
				}
			}
			items = tempItems.ToArray();

			for (i=0; i<items.Length; i++) {
				// init and use only items with enabled component and active gameobject
				if (items[i].gameObject.activeSelf && items[i].enabled) {
					items[i].Init(); // force items initialization
					itemsByName[items[i].itemName] = items[i];
					items[i].group = this; // init item group
				}
			}
		}
	}

	/*
		Func: GetItem (id)
		Find item by id in items array

		(start code)
		UIItem item = GetComponent<UIGroup>().GetItem(0);
		(end code)

		Parameters:
			id - item id from items array

		Returns:
			Found <UIItem> or null
	*/
	public UIItem GetItem(int id) {
		if (id < 0 || id >= items.Length) return null;
		return items[id];
	}

	/*
		Func: GetItem (name)
		Find item by name in items array

		(start code)
		UIItem item = GetComponent<UIGroup>().GetItem("item 1");
		(end code)

		Parameters:
			name - item name

		Returns:
			Found <UIItem> or null
	*/
	public UIItem GetItem(string name) {
		if (!itemsByName.ContainsKey(name)) return null;
		return itemsByName[name];
	}
}
}
