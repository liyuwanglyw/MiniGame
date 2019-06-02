using UnityEditor;
using System.Collections;
using UnityEngine;
using System;
using System.Reflection;

/*
[CustomEditor(typeof(CLASSNAME), true)]
[CanEditMultipleObjects]
public class CLASSNAMEEditor : BASECLASSNAMEEditor {
	// public void OnEnable () {
	// 	base.OnEnable();
	// 	InitInspector();
	// }

	// public override void OnInspectorGUI() {
	// 	serializedObject.Update ();
	// 	DrawInspector();
	// 	this.Repaint();
	// 	serializedObject.ApplyModifiedProperties ();
	// 	base.OnInspectorGUI();
	// }

	YOUR_CLASS_HERE script;
	// SerializedProperty PROPERTY_NAME;

	protected override void InitInspector() {
		base.InitInspector();

		// do initialization here
		script = (YOUR_CLASS_HERE)target;
		if (!Application.isPlaying) script.Init(true);

		// PROPERTY_NAME = serializedObject.FindProperty ("PROPERTY_NAME");
	}

	protected override void DrawInspector() {
		// do draw here

		base.DrawInspector(); // call base draw inspector as last line
		EditorUtility.SetDirty (script.gameObject); // redraw game object
	}

	protected override void DrawCallbacks() {
		base.DrawCallbacks();

		// your callbacks
	}
}
*/

namespace CJFinc {

[CustomEditor(typeof(RichMonoBehaviour), true)]
[CanEditMultipleObjects]

public class RichEditor : Editor {
	public void OnEnable () {
		InitInspector();
	}

	public override void OnInspectorGUI() {
		serializedObject.Update ();
		DrawInspector();
		this.Repaint();
		serializedObject.ApplyModifiedProperties ();
	}



	SerializedProperty isEditorCallbacksExpanded, OnInitFinish, initOnStart, isEditorGeneralExpanded;


	protected virtual void InitInspector() {
		guistyleItalic = new GUIStyle();
		guistyleItalic.fontStyle = FontStyle.Italic;

		guistyleBold = new GUIStyle();
		guistyleBold.fontStyle = FontStyle.Bold;

		guistyleBoldItalic = new GUIStyle();
		guistyleBoldItalic.fontStyle = FontStyle.BoldAndItalic;

		guistylePlayer = new GUIStyle();
		guistylePlayer.normal.textColor = Color.blue;
		guistylePlayer.fontStyle = FontStyle.Bold;
//			guistylePlayer.fontSize = 12;

		guistyleHeader = new GUIStyle();
		guistyleHeader.fontStyle = FontStyle.Bold;
//		guistyleHeader.fontSize = 10;

		guistyleWarning = new GUIStyle();
		guistyleWarning.normal.textColor = Color.red;
		guistyleWarning.fontStyle = FontStyle.Italic;

		try {
			guistyleMainFoldout = new GUIStyle(EditorStyles.foldout);
			guistyleMainFoldout.fontStyle = FontStyle.Bold;
			guistyleMainFoldout.margin.top = 10;
		}
		catch {
			guistyleMainFoldout = new GUIStyle();
		}

		// vars
		isEditorCallbacksExpanded = serializedObject.FindProperty ("isEditorCallbacksExpanded");
		isEditorGeneralExpanded = serializedObject.FindProperty ("isEditorGeneralExpanded");
		OnInitFinish = serializedObject.FindProperty ("OnInitFinish");
		initOnStart = serializedObject.FindProperty ("initOnStart");
	}

	protected virtual void DrawInspector() {
		EditorGUI.indentLevel++;
		isEditorCallbacksExpanded.boolValue = DrawMainFold(
			isEditorCallbacksExpanded.boolValue, "Callbacks", DrawCallbacks);
		EditorGUI.indentLevel--;
		EditorGUI.indentLevel++;
		isEditorGeneralExpanded.boolValue = DrawMainFold(
			isEditorGeneralExpanded.boolValue, "General", DrawGeneral);
		EditorGUI.indentLevel--;
	}

	protected virtual void DrawCallbacks() {
		EditorGUILayout.PropertyField(OnInitFinish);
	}

	protected virtual void DrawGeneral() {
		initOnStart.boolValue = EditorGUILayout.ToggleLeft("Init on start?", initOnStart.boolValue);
	}

	// draw helpers
	protected GUIStyle guistyleItalic, guistyleBold, guistyleBoldItalic, guistylePlayer, guistyleHeader, guistyleMainFoldout, guistyleWarning;

	protected void DrawSeparator() {
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
	}

	protected void DrawHeader(string text) {
//		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField (text, guistyleHeader);
//		EditorGUILayout.Space();
	}

	protected void DrawPlayerModeHeader() {
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField ("DEBUG", guistylePlayer);
		EditorGUILayout.Space();
//		DrawSeparator();
	}

	protected bool DrawMainFold(bool fold, string label, Action callback) {
		fold = EditorGUILayout.Foldout(fold, label, guistyleMainFoldout);
		if (fold) {
			EditorGUI.indentLevel++;
			callback();
			EditorGUI.indentLevel--;
		}
		return fold;
	}

	protected void DrawWarning(string text) {
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField (text, guistyleWarning);
	}

}
}
