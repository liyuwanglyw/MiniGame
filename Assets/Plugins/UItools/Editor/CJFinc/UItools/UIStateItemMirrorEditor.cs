using UnityEditor;
using UnityEngine;
using System.Collections;

namespace CJFinc.UItools {

[CustomEditor(typeof(UIStateItemMirror), true)]
[CanEditMultipleObjects]

public class UIStateItemMirrorEditor : UIStateItemExtentionEditor {
	UIStateItemMirror scriptStateItemMirror;
	SerializedProperty mirrorItems
	;

	protected override void InitInspector() {
		base.InitInspector();

		// do initialization here
		scriptStateItemMirror = (UIStateItemMirror)target;
		if (!Application.isPlaying) scriptStateItemMirror.Init(true);

		mirrorItems = serializedObject.FindProperty ("mirrorItems");
	}

	protected override void DrawInspector() {
		// do draw here
		EditorGUI.indentLevel++;
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(mirrorItems, new GUIContent("Mirror items"), true);
		EditorGUI.indentLevel--;

		base.DrawInspector(); // call base draw inspector as last line
		EditorUtility.SetDirty (scriptStateItemMirror.gameObject); // redraw game object
	}

	protected override void DrawCallbacks() {
		base.DrawCallbacks();

		// your callbacks
	}

}
}
