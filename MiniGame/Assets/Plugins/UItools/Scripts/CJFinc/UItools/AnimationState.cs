using System;
using UnityEngine;

namespace CJFinc.UItools {
/*
	Class: AnimationState
	<AnimationState> is a helper class to store and serialize animation <UIStateItemAnimation> fields for each <UIStateItem> state
*/
	[Serializable]
	public class AnimationState {
		/*
			Var: RectTransform_offsetMin
			state value for <RectTransform.offsetMin at http://docs.unity3d.com/ScriptReference/RectTransform-offsetMin.html>
		*/
		public Vector2 RectTransform_offsetMin;
		/*
			Var: RectTransform_offsetMax
			state value for <RectTransform.offsetMax at http://docs.unity3d.com/ScriptReference/RectTransform-offsetMax.html>
		*/
		public Vector2 RectTransform_offsetMax;
		/*
			Var: RectTransform_anchorMin
			state value for <RectTransform.anchorMin at http://docs.unity3d.com/ScriptReference/RectTransform-anchorMin.html>
		*/
		public Vector2 RectTransform_anchorMin;
		/*
			Var: RectTransform_anchorMax
			state value for <RectTransform.anchorMax at http://docs.unity3d.com/ScriptReference/RectTransform-anchorMax.html>
		*/
		public Vector2 RectTransform_anchorMax;

		/*
			Var: CanvasGroup_alpha
			state value for <CanvasGroup.alpha at http://docs.unity3d.com/ScriptReference/CanvasGroup-alpha.html>
		*/
		public float CanvasGroup_alpha;

		/*
			Var: LayoutElement_preferredHeight
			state value for <LayoutElement.preferredHeight at http://docs.unity3d.com/ScriptReference/UI.LayoutElement-preferredHeight.html>
		*/
		public float LayoutElement_preferredHeight;
		/*
			Var: LayoutElement_preferredWidth
			state value for <LayoutElement.preferredWidth at http://docs.unity3d.com/ScriptReference/UI.LayoutElement-preferredWidth.html>
		*/
		public float LayoutElement_preferredWidth;
	};

}
