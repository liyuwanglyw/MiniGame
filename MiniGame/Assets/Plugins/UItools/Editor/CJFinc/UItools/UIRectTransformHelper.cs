using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CJFinc.UItools {

/*
	Class: UIRectTransformHelper
	<UIRectTransformHelper> is an editor helper that allows to set <RectTransform at https://docs.unity3d.com/ScriptReference/RectTransform.html> anchors to object corners

	(see UIRectTransformHelper-editor.png)

	It's pretty annoying to set object anchors to its corners. This helper do it for you in a second :)

	(see UIRectTransformHelper-editor.gif)
*/
public class UIRectTransformHelper : MonoBehaviour {
	[MenuItem("GameObject/UItools/RectTransform/Set anchors to object corners", false, 100)]
	static void AnchorsToObjectSize() {
		Object [] objs = Selection.GetFiltered(typeof(RectTransform), SelectionMode.Editable);
		foreach (RectTransform t in objs) {
			SetAnchorsToObjectSize(t);
		}
	}

	[MenuItem ("CONTEXT/RectTransform/CJFinc - UItools: Set anchors to object corners")]
	static void ContextAnchorsToObjectSize(MenuCommand command) {
		RectTransform t = (RectTransform)command.context;
		SetAnchorsToObjectSize(t);
	}

	static void SetAnchorsToObjectSize(RectTransform t) {
		if (t == null) return;

		RectTransform pt = t.gameObject.transform.parent.GetComponent<RectTransform>();
		if (pt == null) return;

		Vector2 newAnchorsMin = new Vector2(
			t.anchorMin.x + t.offsetMin.x / pt.rect.width,
			t.anchorMin.y + t.offsetMin.y / pt.rect.height);

		Vector2 newAnchorsMax = new Vector2(
			t.anchorMax.x + t.offsetMax.x / pt.rect.width,
			t.anchorMax.y + t.offsetMax.y / pt.rect.height);

		t.anchorMin = newAnchorsMin;
		t.anchorMax = newAnchorsMax;
		t.offsetMin = t.offsetMax = new Vector2(0, 0);
	}
}

}
