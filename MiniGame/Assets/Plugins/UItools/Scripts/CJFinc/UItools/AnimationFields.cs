using System;

namespace CJFinc.UItools {
/*
	Class: AnimationFields
	<AnimationFields> is a helper class to store and serialize enabled animation fields for <UIStateItemAnimation>
*/
	[Serializable]
	public class AnimationFields {
		/*
			Var: RectTransform_offsetMin
			animate <RectTransform.offsetMin at http://docs.unity3d.com/ScriptReference/RectTransform-offsetMin.html>
		*/
		public bool RectTransform_offsetMin;
		/*
			Var: RectTransform_offsetMax
			animate <RectTransform.offsetMax at http://docs.unity3d.com/ScriptReference/RectTransform-offsetMax.html>
		*/
		public bool RectTransform_offsetMax;
		/*
			Var: RectTransform_anchorMin
			animate <RectTransform.anchorMin at http://docs.unity3d.com/ScriptReference/RectTransform-anchorMin.html>
		*/
		public bool RectTransform_anchorMin;
		/*
			Var: RectTransform_anchorMax
			animate <RectTransform.anchorMax at http://docs.unity3d.com/ScriptReference/RectTransform-anchorMax.html>
		*/
		public bool RectTransform_anchorMax;

		/*
			Var: CanvasGroup_alpha
			animate <CanvasGroup.alpha at http://docs.unity3d.com/ScriptReference/CanvasGroup-alpha.html>
		*/
		public bool CanvasGroup_alpha;

		/*
			Var: LayoutElement_preferredHeight
			animate <LayoutElement.preferredHeight at http://docs.unity3d.com/ScriptReference/UI.LayoutElement-preferredHeight.html>
		*/
		public bool LayoutElement_preferredHeight;
		/*
			Var: LayoutElement_preferredWidth
			animate <LayoutElement.preferredWidth at http://docs.unity3d.com/ScriptReference/UI.LayoutElement-preferredWidth.html>
		*/
		public bool LayoutElement_preferredWidth;
	}

}
