using UnityEditor;
using UnityEngine;
using System.Collections;

namespace CJFinc.UItools {

[CustomEditor(typeof(UIStateGroup), true)]
[CanEditMultipleObjects]

public class UIStateGroupEditor : UIGroupEditor {

	UIStateGroup scriptStateGroup;
	SerializedProperty isStateGroupExpanded, OnStateChange;

	protected override void InitInspector() {
		base.InitInspector();

		// do initialization here
		scriptStateGroup = (UIStateGroup)target;
		if (!Application.isPlaying) scriptStateGroup.Init(true);

		isStateGroupExpanded = serializedObject.FindProperty ("isStateGroupExpanded");
		OnStateChange = serializedObject.FindProperty ("OnStateChange");
	}

	protected override void DrawInspector() {
		// do draw here
		EditorGUI.indentLevel++;
		isStateGroupExpanded.boolValue = DrawMainFold(isStateGroupExpanded.boolValue, "State group", DrawStateGroup);
		EditorGUI.indentLevel--;

		base.DrawInspector(); // call base draw inspector as last line
		EditorUtility.SetDirty (scriptStateGroup.gameObject); // redraw game object
	}

	protected override void DrawCallbacks() {
		base.DrawCallbacks();

		// your callbacks
		EditorGUILayout.PropertyField(OnStateChange);
	}

	void DrawStateGroup() {
		EditorGUILayout.LabelField ("Set all items to state: ");

		EditorGUILayout.BeginHorizontal ();
		for (int i=0; i<scriptStateGroup.States.Length; i++) {
			if (i%3 == 0) {
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
			}
			if (GUILayout.Button (scriptStateGroup.States[i])) {
				scriptStateGroup.SetStateForAllItems( scriptStateGroup.States[i] );
			}
		}
		EditorGUILayout.EndHorizontal ();
	}
}

}
