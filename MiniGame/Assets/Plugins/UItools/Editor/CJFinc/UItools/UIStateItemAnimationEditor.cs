using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CJFinc.UItools {

[CustomEditor(typeof(UIStateItemAnimation), true)]
[CanEditMultipleObjects]

public class UIStateItemAnimationEditor : UIStateItemExtentionEditor {
	UIStateItemAnimation scriptStateItemAnimation;
	SerializedProperty isStateItemAnimationExpanded
		, isStateItemAnimationFieldsExpanded
		, isStateItemAnimationStatesExpanded

		, animationDuration
		, animationStartDelay
		, valueChangeSpeedCurve

		, OnAnimationFinished
	;

	protected override void InitInspector() {
		base.InitInspector();

		// do initialization here
		scriptStateItemAnimation = (UIStateItemAnimation)target;
		if (!Application.isPlaying) scriptStateItemAnimation.Init(true);

		isStateItemAnimationExpanded = serializedObject.FindProperty ("isStateItemAnimationExpanded");
		isStateItemAnimationFieldsExpanded = serializedObject.FindProperty ("isStateItemAnimationFieldsExpanded");
		isStateItemAnimationStatesExpanded = serializedObject.FindProperty ("isStateItemAnimationStatesExpanded");

		animationDuration = serializedObject.FindProperty ("animationDuration");
		animationStartDelay = serializedObject.FindProperty ("animationStartDelay");
		valueChangeSpeedCurve = serializedObject.FindProperty ("valueChangeSpeedCurve");

		OnAnimationFinished = serializedObject.FindProperty ("OnAnimationFinished");
	}

	protected override void DrawInspector() {
		// do draw here
		EditorGUI.indentLevel++;
		isStateItemAnimationExpanded.boolValue = DrawMainFold(
			isStateItemAnimationExpanded.boolValue, "State item animation", DrawStateItemAnimation);
		EditorGUI.indentLevel--;

		base.DrawInspector(); // call base draw inspector as last line
		EditorUtility.SetDirty (scriptStateItemAnimation.gameObject); // redraw game object
	}

	protected override void DrawCallbacks() {
		base.DrawCallbacks();

		// your callbacks
		EditorGUILayout.PropertyField(OnAnimationFinished);
	}

	void DrawStateItemAnimation() {

		EditorGUILayout.LabelField("Duration, sec");
		animationDuration.floatValue = EditorGUILayout.Slider(animationDuration.floatValue, 0, 60f);

		EditorGUILayout.LabelField("Start delay, sec");
		animationStartDelay.floatValue = EditorGUILayout.Slider(animationStartDelay.floatValue, 0, 60f);

		EditorGUILayout.LabelField("Value change speed curve");
		valueChangeSpeedCurve.animationCurveValue = EditorGUILayout.CurveField("", valueChangeSpeedCurve.animationCurveValue);

		if (valueChangeSpeedCurve.animationCurveValue.keys.Length == 0) {
			EditorGUILayout.LabelField("Select predefined curve or create new one!", guistyleWarning);
		}

		EditorGUILayout.Space();

		isStateItemAnimationFieldsExpanded.boolValue = DrawMainFold(
			isStateItemAnimationFieldsExpanded.boolValue, "Animation fields", DrawAnimationFields);

		isStateItemAnimationStatesExpanded.boolValue = DrawMainFold(
			isStateItemAnimationStatesExpanded.boolValue, "Animation states", DrawAnimationStates);
	}

	void DrawAnimationFields() {
		// RectTransform
		EditorGUILayout.LabelField("RectTransform", guistyleBold);

		EditorGUI.indentLevel++;
		EditorGUI.indentLevel++;

		scriptStateItemAnimation.animationFields.RectTransform_offsetMin = EditorGUILayout.ToggleLeft(
			"offsetMin", scriptStateItemAnimation.animationFields.RectTransform_offsetMin);

		scriptStateItemAnimation.animationFields.RectTransform_offsetMax = EditorGUILayout.ToggleLeft(
			"offsetMax", scriptStateItemAnimation.animationFields.RectTransform_offsetMax);

		scriptStateItemAnimation.animationFields.RectTransform_anchorMin = EditorGUILayout.ToggleLeft(
			"anchorMin", scriptStateItemAnimation.animationFields.RectTransform_anchorMin);

		scriptStateItemAnimation.animationFields.RectTransform_anchorMax = EditorGUILayout.ToggleLeft(
			"anchorMax", scriptStateItemAnimation.animationFields.RectTransform_anchorMax);

		EditorGUI.indentLevel--;
		EditorGUI.indentLevel--;
		// RectTransform - END

		// CanvasGroup
		EditorGUILayout.LabelField("CanvasGroup", guistyleBold);

		EditorGUI.indentLevel++;
		EditorGUI.indentLevel++;

		if (scriptStateItemAnimation.animationFields.CanvasGroup_alpha && scriptStateItemAnimation.gameObject.GetComponent<CanvasGroup>() == null) {
			EditorGUILayout.LabelField("CanvasGroup component is missing!", guistyleWarning);

			if (GUILayout.Button ("Fix")) {
				scriptStateItemAnimation.gameObject.AddComponent<CanvasGroup>();
			}
		}
		scriptStateItemAnimation.animationFields.CanvasGroup_alpha = EditorGUILayout.ToggleLeft(
			"alpha", scriptStateItemAnimation.animationFields.CanvasGroup_alpha);

		EditorGUI.indentLevel--;
		EditorGUI.indentLevel--;
		// CanvasGroup - END

		// LayoutElement
		EditorGUILayout.LabelField("LayoutElement", guistyleBold);

		EditorGUI.indentLevel++;
		EditorGUI.indentLevel++;

		if (
			(scriptStateItemAnimation.animationFields.LayoutElement_preferredHeight || scriptStateItemAnimation.animationFields.LayoutElement_preferredWidth)
			&& scriptStateItemAnimation.gameObject.GetComponent<LayoutElement>() == null
		) {
			EditorGUILayout.LabelField("LayoutElement component is missing!", guistyleWarning);

			if (GUILayout.Button ("Fix")) {
				scriptStateItemAnimation.gameObject.AddComponent<LayoutElement>();
			}
		}
		scriptStateItemAnimation.animationFields.LayoutElement_preferredHeight = EditorGUILayout.ToggleLeft(
			"preferredHeight", scriptStateItemAnimation.animationFields.LayoutElement_preferredHeight);
		scriptStateItemAnimation.animationFields.LayoutElement_preferredWidth = EditorGUILayout.ToggleLeft(
			"preferredWidth", scriptStateItemAnimation.animationFields.LayoutElement_preferredWidth);

		EditorGUI.indentLevel--;
		EditorGUI.indentLevel--;
		// LayoutElement - END
	}

	void DrawAnimationStates() {
		if (scriptStateItemAnimation.StateItem == null) return;
		for (int i = 0; i < scriptStateItemAnimation.StateItem.States.Length; i++) {
			if (i >= scriptStateItemAnimation.AnimationStates.Length || scriptStateItemAnimation.AnimationStates[i] == null) continue;

			EditorGUILayout.Space();
			DrawHeader(scriptStateItemAnimation.StateItem.States[i]);
			EditorGUI.indentLevel++;

			// RectTransform
			if (
				scriptStateItemAnimation.animationFields.RectTransform_offsetMin
				|| scriptStateItemAnimation.animationFields.RectTransform_offsetMax
				|| scriptStateItemAnimation.animationFields.RectTransform_anchorMin
				|| scriptStateItemAnimation.animationFields.RectTransform_anchorMax
			) {
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField("RectTransform", guistyleBold);
				if (GUILayout.Button ("set from current")) {
					scriptStateItemAnimation.SetAnimationStateRectTransform(scriptStateItemAnimation.StateItem.States[i]);
				}
				GUILayout.EndHorizontal ();
			}

			if (scriptStateItemAnimation.animationFields.RectTransform_offsetMin)
				scriptStateItemAnimation.AnimationStates[i].RectTransform_offsetMin =
					EditorGUILayout.Vector2Field("offsetMin", scriptStateItemAnimation.AnimationStates[i].RectTransform_offsetMin);
			if (scriptStateItemAnimation.animationFields.RectTransform_offsetMax)
				scriptStateItemAnimation.AnimationStates[i].RectTransform_offsetMax =
					EditorGUILayout.Vector2Field("offsetMax", scriptStateItemAnimation.AnimationStates[i].RectTransform_offsetMax);
			if (scriptStateItemAnimation.animationFields.RectTransform_anchorMin)
				scriptStateItemAnimation.AnimationStates[i].RectTransform_anchorMin =
					EditorGUILayout.Vector2Field("anchorMin", scriptStateItemAnimation.AnimationStates[i].RectTransform_anchorMin);
			if (scriptStateItemAnimation.animationFields.RectTransform_anchorMax)
				scriptStateItemAnimation.AnimationStates[i].RectTransform_anchorMax =
					EditorGUILayout.Vector2Field("anchorMax", scriptStateItemAnimation.AnimationStates[i].RectTransform_anchorMax);

			// RectTransform - END

			// CanvasGroup
			if (scriptStateItemAnimation.animationFields.CanvasGroup_alpha) {
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("CanvasGroup", guistyleBold);

				scriptStateItemAnimation.AnimationStates[i].CanvasGroup_alpha =
					EditorGUILayout.Slider("alpha", scriptStateItemAnimation.AnimationStates[i].CanvasGroup_alpha, 0, 1);
			}
			// CanvasGroup - END

			// LayoutElement
			if (scriptStateItemAnimation.animationFields.LayoutElement_preferredHeight || scriptStateItemAnimation.animationFields.LayoutElement_preferredWidth) {
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("LayoutElement", guistyleBold);
			}

			if (scriptStateItemAnimation.animationFields.LayoutElement_preferredHeight)
				scriptStateItemAnimation.AnimationStates[i].LayoutElement_preferredHeight =
					EditorGUILayout.FloatField("preferredHeight", scriptStateItemAnimation.AnimationStates[i].LayoutElement_preferredHeight);
			if (scriptStateItemAnimation.animationFields.LayoutElement_preferredWidth)
				scriptStateItemAnimation.AnimationStates[i].LayoutElement_preferredWidth =
					EditorGUILayout.FloatField("preferredWidth", scriptStateItemAnimation.AnimationStates[i].LayoutElement_preferredWidth);
			// LayoutElement - END

			EditorGUI.indentLevel--;
		}

	}

}
}
