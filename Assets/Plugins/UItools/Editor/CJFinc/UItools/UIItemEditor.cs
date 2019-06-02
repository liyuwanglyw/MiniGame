using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEditor.AnimatedValues;

namespace CJFinc.UItools {

[CustomEditor(typeof(UIItem), true)]
[CanEditMultipleObjects]

public class UIItemEditor : RichEditor {

	UIItem scriptItem;
	SerializedProperty group, itemName, isItemExpanded;

	protected override void InitInspector() {
		base.InitInspector();

		// do initialization here
		scriptItem = (UIItem)target;

		if (!Application.isPlaying) scriptItem.Init(true);

		itemName = serializedObject.FindProperty ("itemName");
		group = serializedObject.FindProperty ("group");
		isItemExpanded = serializedObject.FindProperty ("isItemExpanded");
	}

	protected override void DrawInspector() {
		// do draw here
		EditorGUI.indentLevel++;
		isItemExpanded.boolValue = DrawMainFold(isItemExpanded.boolValue, "Item", DrawItemParameters);
		EditorGUI.indentLevel--;

		base.DrawInspector(); // call base draw inspector as last line
		EditorUtility.SetDirty (scriptItem.gameObject); // redraw game object
	}

	protected override void DrawCallbacks() {
		base.DrawCallbacks();

		// your callbacks
	}

	void DrawItemParameters () {
		EditorGUILayout.LabelField("Name", itemName.stringValue);
		EditorGUILayout.PropertyField(group, new GUIContent("Group"));
	}

}

}
