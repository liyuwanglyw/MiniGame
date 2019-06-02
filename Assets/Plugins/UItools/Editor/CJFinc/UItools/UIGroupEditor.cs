using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace CJFinc.UItools {

[CustomEditor(typeof(UIGroup), true)]
[CanEditMultipleObjects]

public class UIGroupEditor : RichEditor {

	UIGroup scriptGroup;
	SerializedProperty items
		, itemsMode
		, isGroupExpanded
	;

	protected override void InitInspector() {
		base.InitInspector();

		// do initialization here
		scriptGroup = (UIGroup)target;
		if (!Application.isPlaying) scriptGroup.Init(true);

		items = serializedObject.FindProperty ("items");
		itemsMode = serializedObject.FindProperty ("itemsMode");
		isGroupExpanded = serializedObject.FindProperty ("isGroupExpanded");
	}

	protected override void DrawInspector() {
		// do draw here
		EditorGUI.indentLevel++;
		isGroupExpanded.boolValue = DrawMainFold(isGroupExpanded.boolValue, "Group", DrawItems);
		EditorGUI.indentLevel--;

		base.DrawInspector(); // call base draw inspector as last line
		EditorUtility.SetDirty (scriptGroup.gameObject); // redraw game object
	}

	protected override void DrawCallbacks() {
		base.DrawCallbacks();

		// your callbacks
	}

	void DrawItems() {
		DrawHeader("Items detection:");
		EditorGUI.indentLevel++;

		int oldValue = itemsMode.intValue;
		itemsMode.intValue = EditorGUILayout.IntPopup(
			itemsMode.intValue,
			new string[] {"Automatic", "Manual"},
			new int[] {0, 1}
		);


		// Automatic
		if (itemsMode.intValue == 0) {
			EditorGUILayout.LabelField("(all game objects are read only)", guistyleItalic);

			// re init items on switch from manual to automatic
			if (oldValue == 1) {
				serializedObject.ApplyModifiedProperties ();
				scriptGroup.Init(true);
			}

			EditorGUILayout.LabelField("Items", guistyleBold);
			for (int i=0; i<scriptGroup.items.Length; i++) {
				EditorGUILayout.ObjectField("", scriptGroup.items[i], typeof(UIItem), true);
			}
		}

		// Manual
		if (itemsMode.intValue == 1) {
			EditorGUILayout.LabelField("(you can change any game object)", guistyleItalic);
			EditorGUILayout.PropertyField(items, true);
		}

		EditorGUI.indentLevel--;
	}
}
}
