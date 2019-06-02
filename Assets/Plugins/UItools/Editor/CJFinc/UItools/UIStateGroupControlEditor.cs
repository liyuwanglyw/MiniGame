using UnityEditor;
using UnityEngine;
using System.Collections;

namespace CJFinc.UItools {

[CustomEditor(typeof(UIStateGroupControl), true)]
[CanEditMultipleObjects]

public class UIStateGroupControlEditor : UIStateGroupEditor {

	UIStateGroupControl scriptStateGroupControl;
	SerializedProperty isStateGroupControlExpanded;

	protected override void InitInspector() {
		base.InitInspector();

		// do initialization here
		scriptStateGroupControl = (UIStateGroupControl)target;
		if (!Application.isPlaying) scriptStateGroupControl.Init(true);

		isStateGroupControlExpanded = serializedObject.FindProperty ("isStateGroupControlExpanded");
	}

	protected override void DrawInspector() {
		// do draw here
		EditorGUI.indentLevel++;
		isStateGroupControlExpanded.boolValue = DrawMainFold(isStateGroupControlExpanded.boolValue, "State group control", DrawStateGroupControl);
		EditorGUI.indentLevel--;

		base.DrawInspector(); // call base draw inspector as last line
		EditorUtility.SetDirty (scriptStateGroupControl.gameObject); // redraw game object
	}

	protected override void DrawCallbacks() {
		base.DrawCallbacks();

		// your callbacks
	}

	void DrawStateGroupControl() {
		if (!scriptStateGroupControl.enabled) return;
		if (scriptStateGroupControl.StateItems == null) scriptStateGroupControl.Init(true);

		DrawMode();
		DrawActiveItemsControl();
		DrawSelectedItem();
	}

	void DrawMode() {
		UIStateGroupControl.STATE_GROUP_CONTROL_MODE newMode = (UIStateGroupControl.STATE_GROUP_CONTROL_MODE) EditorGUILayout.EnumPopup("Group mode", scriptStateGroupControl.Mode);
		if (newMode != scriptStateGroupControl.Mode) scriptStateGroupControl.SetMode(newMode);
	}

	void DrawActiveItemsControl() {
		DrawHeader("Active items");
		EditorGUI.indentLevel++;

		if (scriptStateGroupControl.StateItems.Length == 0) {
			EditorGUILayout.LabelField("No UIStateItems found", guistyleWarning);
		}

		for (int i=0; i<scriptStateGroupControl.StateItems.Length; i++) {
			bool isActive = (scriptStateGroupControl.StateItems[i].CurrentState == UIStateItem.STATE_ACTIVE);
			bool newIsActive = EditorGUILayout.ToggleLeft(scriptStateGroupControl.StateItems[i].itemName, isActive);

			if (newIsActive == isActive) continue; // do nothing if state wasn't changed

			if (newIsActive)
				scriptStateGroupControl.StateItems[i].SetStateActive();
			else
				scriptStateGroupControl.StateItems[i].SetStateInactive();
		}

		EditorGUI.indentLevel--;
	}

	void DrawSelectedItem() {
		EditorGUILayout.Space();
		EditorGUILayout.LabelField ("Selected item", guistyleBold);
		EditorGUI.indentLevel++;
		if (scriptStateGroupControl.SelectedItemName == "")
			EditorGUILayout.LabelField ("(none)", guistyleItalic);
		else
			EditorGUILayout.LabelField (scriptStateGroupControl.SelectedItemName);
		EditorGUI.indentLevel--;
	}
}

}
