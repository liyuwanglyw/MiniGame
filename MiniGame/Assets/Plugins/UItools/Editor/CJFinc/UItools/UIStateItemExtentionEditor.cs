using UnityEditor;
using UnityEngine;
using System.Collections;

namespace CJFinc.UItools {

[CustomEditor(typeof(UIStateItemExtention), true)]
[CanEditMultipleObjects]

public class UIStateItemExtentionEditor : RichEditor {
	UIStateItemExtention scriptStateItemExtention;

	protected override void InitInspector() {
		base.InitInspector();

		// do initialization here
		scriptStateItemExtention = (UIStateItemExtention)target;
		if (!Application.isPlaying) scriptStateItemExtention.Init(true);
	}

	protected override void DrawInspector() {
		// do draw here
		if (scriptStateItemExtention.StateItem == null) {
			DrawWarning("UIStateItem should be attached to the same game object!");

			if (GUILayout.Button ("Fix")) {
				scriptStateItemExtention.gameObject.AddComponent<UIStateItem>();
			}
			EditorGUILayout.Space();
		}

		base.DrawInspector(); // call base draw inspector as last line
		EditorUtility.SetDirty (scriptStateItemExtention.gameObject); // redraw game object
	}

	protected override void DrawCallbacks() {
		base.DrawCallbacks();

		// your callbacks
	}

}
}
