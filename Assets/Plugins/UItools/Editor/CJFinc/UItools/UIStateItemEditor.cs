using UnityEditor;
using UnityEngine;
using System.Collections;

namespace CJFinc.UItools {

[CustomEditor(typeof(UIStateItem), true)]
[CanEditMultipleObjects]

public class UIStateItemEditor : UIItemEditor {

	UIStateItem scriptStateItem;
	SerializedProperty statesUi;
	SerializedProperty isStateItemExpanded
		, isStateItemStatesExpanded
		, isStateItemUiExpanded
		, isStateItemTestingExpanded

		, OnStateChange

		, statesUiMode

		, defaultStateId
	;

	protected override void InitInspector() {
		base.InitInspector();

		// do initialization here
		scriptStateItem = (UIStateItem)target;
		if (!Application.isPlaying) scriptStateItem.Init(true);

		statesUi = serializedObject.FindProperty ("statesUi");
		isStateItemExpanded = serializedObject.FindProperty ("isStateItemExpanded");
		isStateItemStatesExpanded = serializedObject.FindProperty ("isStateItemStatesExpanded");
		isStateItemUiExpanded = serializedObject.FindProperty ("isStateItemUiExpanded");
		isStateItemTestingExpanded = serializedObject.FindProperty ("isStateItemTestingExpanded");

		OnStateChange = serializedObject.FindProperty ("OnStateChange");

		statesUiMode = serializedObject.FindProperty ("statesUiMode");

		defaultStateId = serializedObject.FindProperty ("defaultStateId");
	}

	protected override void DrawInspector() {
		// do draw here
		EditorGUI.indentLevel++;
		isStateItemExpanded.boolValue = DrawMainFold(
			isStateItemExpanded.boolValue, "State item", DrawStateItem);
		EditorGUI.indentLevel--;

		base.DrawInspector(); // call base draw inspector as last line
		EditorUtility.SetDirty (scriptStateItem.gameObject); // redraw game object
	}

	protected override void DrawCallbacks() {
		base.DrawCallbacks();

		// your callbacks
		EditorGUILayout.PropertyField(OnStateChange);
	}


	void DrawStateItem() {
		EditorGUI.indentLevel++;
		isStateItemStatesExpanded.boolValue = DrawMainFold(
			isStateItemStatesExpanded.boolValue, "States", DrawStates);
		EditorGUI.indentLevel--;

		EditorGUI.indentLevel++;
		isStateItemUiExpanded.boolValue = DrawMainFold(
			isStateItemUiExpanded.boolValue, "UI game objects", DrawUiGameObjects);
		EditorGUI.indentLevel--;

		EditorGUI.indentLevel++;
		isStateItemTestingExpanded.boolValue = DrawMainFold(
			isStateItemTestingExpanded.boolValue, "Testing", DrawStateItemTesting);
		EditorGUI.indentLevel--;
	}

	string newState;
	void DrawStates () {
		// States
		for (int i = 0; i < scriptStateItem.States.Length; i++) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField (i.ToString () + ". " + scriptStateItem.States [i]);

			if (i > 2) {
				if (GUILayout.Button ("Remove")) {
					scriptStateItem.RemoveState(scriptStateItem.States[i]);
				}
			}

			EditorGUILayout.EndHorizontal ();
		}

		EditorGUILayout.Space();

		// add new state
		EditorGUILayout.BeginHorizontal ();
		newState = EditorGUILayout.TextField (newState);
		if (GUILayout.Button ("Add new")) {
			if (newState != null && newState != "") scriptStateItem.AddState (newState);
		}
		EditorGUILayout.EndHorizontal ();

		// States - end


		// default state
		DrawHeader("Default state");

		EditorGUILayout.BeginHorizontal ();
		int newDefaultState = EditorGUILayout.Popup (defaultStateId.intValue, scriptStateItem.States);
		if (newDefaultState >= 0 && newDefaultState < scriptStateItem.States.Length)
			scriptStateItem.SetDefaultStateTo(scriptStateItem.States[newDefaultState]);
		if (GUILayout.Button ("Flush"))
			scriptStateItem.FlushDefaultState();
		EditorGUILayout.EndHorizontal ();
		// default state - end
	}

	void DrawUiGameObjects () {
		int oldValue = statesUiMode.intValue;
		statesUiMode.intValue = EditorGUILayout.IntPopup(
			statesUiMode.intValue,
			new string[] {"Automatic", "Manual"},
			new int[] {0, 1}
		);

		// Automatic
		if (statesUiMode.intValue == 0) {
			EditorGUILayout.LabelField("(all game objects are read only)", guistyleItalic);

			// re init items on switch from manual to automatic
			if (oldValue == 1) {
				serializedObject.ApplyModifiedProperties ();
				scriptStateItem.Init(true);
			}

			if (statesUi.arraySize == scriptStateItem.States.Length) {
				for (int i = 0; i < statesUi.arraySize; i++) {
					EditorGUILayout.ObjectField(
						i.ToString () + ". " + scriptStateItem.States [i],
						statesUi.GetArrayElementAtIndex(i).objectReferenceValue,
						typeof(GameObject),
						true
					);
				}
			}
		}

		// Manual
		if (statesUiMode.intValue == 1) {
			EditorGUILayout.LabelField("(you can change any game object)", guistyleItalic);
			for (int i = 0; i < statesUi.arraySize; i++) {
				string stateName = (statesUi.arraySize == scriptStateItem.States.Length) ? scriptStateItem.States [i] : "?";
				EditorGUILayout.PropertyField(statesUi.GetArrayElementAtIndex(i),
					new GUIContent( i.ToString () + ". " + stateName )
				);
			}
		}
	}


	// TESTING
	void DrawStateItemTesting() {
		// current state
		EditorGUILayout.LabelField ("Current state: ", scriptStateItem.CurrentState, guistyleBold);

		EditorGUILayout.LabelField ("Set state to: ");

		if (GUILayout.Button ("Set to default")) { scriptStateItem.SetStateDefault(true); }
		EditorGUILayout.Space();

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button (UIStateItem.STATE_INACTIVE)) { scriptStateItem.SetStateInactive(true); }
		if (GUILayout.Button (UIStateItem.STATE_ACTIVE)) { scriptStateItem.SetStateActive(true); }
		if (GUILayout.Button (UIStateItem.STATE_DISABLED)) { scriptStateItem.SetStateDisabled(true); }
		GUILayout.EndHorizontal ();

		// custom States
		if (scriptStateItem.States.Length > 3) {
			EditorGUILayout.BeginHorizontal ();
			for (int i=3; i<scriptStateItem.States.Length; i++) {
				if (i%3 == 0) {
					EditorGUILayout.EndHorizontal ();
					EditorGUILayout.BeginHorizontal ();
				}
				if (GUILayout.Button (scriptStateItem.States[i])) { scriptStateItem.SetState(scriptStateItem.States[i], true); }
			}
			EditorGUILayout.EndHorizontal ();
		}
	}


}

}
