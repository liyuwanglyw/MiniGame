using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CJFinc.UItools {

/*
	Class: UIStateItemAnimation
	<UIStateItemAnimation> allows to animate <UIStateItem> state change transition

	It subscribes to <UIStateItem.OnStateChange> event and launches transition animation between two states.

	While animation process new values are calculated for enabled animation fields for each <Update at http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html>.


	Animation parameters:

	Duration parameter defines the total animation duration in seconds.

	"Start delay" parameter allows to add delay in seconds before animation start.

	(see UIStateItemAnimation-editor.png)

	The "Value change speed curve" is used to calculate transition parameter value over the animation time. It sounds weird, but next examples will show that it's simple.

	Let's imagine that we need to move object horizontally from left to right.

	This curve will animate our movement evenly

	(see UIStateItemAnimation-curve-evenly.png)

	(see UIStateItemAnimation-curve-evenly.gif)

	This one will make movement slowed down at the beginning and speeding up at the end of animation

	(see UIStateItemAnimation-curve-slow-fast.png)

	(see UIStateItemAnimation-curve-slow-fast.gif)

	And finally this curve will make movement fast at the beginning and slowing down at the end

	(see UIStateItemAnimation-curve-fast-slow.png)

	(see UIStateItemAnimation-curve-fast-slow.gif)

	<UIStateItemAnimation> contains a predefined custom curve that uses for almost all modern UI transitions

	(see UIStateItemAnimation-curve-predefined.gif)

	It produces the next animation

	(see UIStateItemAnimation-curve-predefined-transition.gif)


	Animation fields:

	Currently <UIStateItemAnimation> supports the next components with values

	* <RectTransform at http://docs.unity3d.com/ScriptReference/RectTransform.html> (offsetMin, offsetMax, anchorMin, anchorMax)
	* <CanvasGroup at http://docs.unity3d.com/ScriptReference/CanvasGroup.html> (alpha)
	* <LayoutElement at http://docs.unity3d.com/ScriptReference/UI.LayoutElement.html> (preferredHeight, preferredWidth)

	You can send request to add a new components and values to me by email <cjf.inc@gmail.com>

	To use component value in transition, open "Animation fields" group and simply select needed fields

	(see UIStateItemAnimation-editor-animation-fields.gif)

	It's also possible to change it at runtime <animationFields>

	(start code)
	UIStateItemAnimation anim = GetComponent<UIStateItemAnimation>();

	anim.animationFields.CanvasGroup_alpha = true;
	(end code)


	Animation states:

	<UIStateItemAnimation> displays all states from <UIStateItem> component with all enabled animation fields.

	(see UIStateItemAnimation-editor-animation-states.png)

	You can define each state field value in editor

	(see UIStateItemAnimation-editor-animation-states.gif)

	or at runtime through <AnimationStates>

	(start code)
	UIStateItemAnimation anim = GetComponent<UIStateItemAnimation>();

	anim.AnimationStates[UIStateItem.STATE_ACTIVE].CanvasGroup_alpha = 1;
	anim.AnimationStates[UIStateItem.STATE_INACTIVE].CanvasGroup_alpha = 0.3f;
	anim.AnimationStates[UIStateItem.STATE_DISABLED].CanvasGroup_alpha = 0;
	(end code)

	To set all <RectTransform at http://docs.unity3d.com/ScriptReference/RectTransform.html> values in one click from current object position there is a magic button presented in editor

	(see UIStateItemAnimation-editor-animation-states-rect-transform.gif)

	You also can do it at runtime

	(start code)
	// set RectTransform fields values for UIStateItem.STATE_ACTIVE state
	GetComponent<UIStateItemAnimation>().SetAnimationStateRectTransform(UIStateItem.STATE_ACTIVE);
	(end code)


	Animation transition testing:

	Animation testing is possible with help of <UIStateItem> states testing options in editor mode

	(see UIStateItemAnimation-editor-animation-testing.gif)

	as well as in player mode

	(see UIStateItemAnimation-player-animation-testing.gif)


	Animation examples:

	(TBD) Short video examples how to setup animation for different components
	* RectTransform
	* CanvasGroup
	* LayoutElement (vertical layout group)
	* LayoutElement (horizontal layout group)


	Callbacks:

	<UIStateItemAnimation> implements callback event <OnAnimationFinished> that called after animation is finished

	(see UIStateItemAnimation-editor-callbacks.png)


	Details:
	This component require the <UIStateItem> to be attached to the same game object
*/

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class UIStateItemAnimation : UIStateItemExtention {
	void Start() {
		if (initOnStart) Init (true);
	}

	protected override void RichInit() {
		base.RichInit(); // as first line - call base initialization

		// your initialization here
		if (StateItem == null) return; // no UIStateItem

		rectRransform = StateItem.GetComponent<RectTransform>();
		canvasGroup = StateItem.GetComponent<CanvasGroup>();
		layoutElement = StateItem.GetComponent<LayoutElement>();

		InitAnimationFields();
		InitAnimationStates();

		// if layout element animation is not enabled
		if (!(animationFields.LayoutElement_preferredHeight || animationFields.LayoutElement_preferredWidth)) {
			// call default state
			if (
				!(Application.isEditor && !Application.isPlaying) // except not playing editor
				&& StateItem.group == null // and single mode
			) StateItem.SetStateDefault(true);
		}
	}

	protected override void Update() {
		base.Update();

		// your update functions here
		// CalculateAnimationParameters();
		if (!(Application.isEditor && !Application.isPlaying)) {
			ProcessAnimation();
		}
	}

	protected override void UpdateInEditor() {
		base.UpdateInEditor();

		// your update in editor functions here
		ProcessAnimation();
	}

	public override void ApplyCurrentState() {
		StartAnimation(StateItem.PreviousState, StateItem.CurrentState);
	}



	// editor
	[SerializeField] bool isStateItemAnimationExpanded;
	[SerializeField] bool isStateItemAnimationFieldsExpanded;
	[SerializeField] bool isStateItemAnimationStatesExpanded;
	[SerializeField] bool isAnimationSettingsExpanded;



	// animations components
	[SerializeField] RectTransform rectRransform;
	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] LayoutElement layoutElement;



	// animation settings
	/*
		Var: animationDuration
		Animation transition duration in seconds
	*/
	public float animationDuration;
	/*
		Var: animationStartDelay
		Animation transition start delay in seconds
	*/
	public float animationStartDelay;
	/*
		Var: valueChangeSpeedCurve
		<AnimationCurve at http://docs.unity3d.com/ScriptReference/AnimationCurve.html> that defines value change speed for <animationDuration>
	*/
	public AnimationCurve valueChangeSpeedCurve;



	// animation fields
	/*
		Var: animationFields
		<AnimationFields> stores what fields are used to animate states transition
	*/
	[SerializeField] public AnimationFields animationFields;

	void InitAnimationFields() {
		if (animationFields == null)
			animationFields = new AnimationFields();
	}



	// animation state
	/*
		Prop: AnimationStates
		<AnimationState> array stores all enabled <animationFields> fields for each <UIStateItem> state

		Note: You can't change a <AnimationStates> array size. The <AnimationStates> array size is always equals to <UIStateItem.States> array size and controlling automatically.
	*/
	public AnimationState [] AnimationStates { get { return animationStates; }}
	[SerializeField] AnimationState [] animationStates;

	// Used to sync UIStateItem states count
	public void InitAnimationStates() {
		if (animationDuration <= 0) animationDuration=1;

		if (animationStates == null || animationStates.Length == 0) {
			animationStates = new AnimationState[ StateItem.States.Length ];
			for (int i=0; i<StateItem.States.Length; i++) {
				animationStates[i] = new AnimationState();
			}
		}

		if (StateItem.States.Length != animationStates.Length) {
			if (StateItem.States.Length > animationStates.Length) {
				int oldStatesLength = animationStates.Length;
				AnimationState [] newAnimationStates = new AnimationState[ StateItem.States.Length ];
				animationStates.CopyTo(newAnimationStates, 0);
				animationStates = new AnimationState[ StateItem.States.Length ];
				newAnimationStates.CopyTo(animationStates, 0);
				for (int i=oldStatesLength; i<StateItem.States.Length; i++)
					animationStates[i] = new AnimationState();
			}
			else {
				animationStates = animationStates.Where((obj, index) => index < animationStates.Length - 1).ToArray();
			}
		}
	}

	/*
		Func: SetAnimationStateRectTransform (state, rectTransform)
		Sets all <RectTransform at http://docs.unity3d.com/ScriptReference/RectTransform.html> related fields in <AnimationStates> for specified state

		If rectTransform parameter is not defined, the <RectTransform at http://docs.unity3d.com/ScriptReference/RectTransform.html> of current game object will be used.

		Parameters:
			state - state name to fill <RectTransform at http://docs.unity3d.com/ScriptReference/RectTransform.html> fields in <AnimationStates>
			rectTransform - object to gather <RectTransform at http://docs.unity3d.com/ScriptReference/RectTransform.html> fields from
	*/
	public void SetAnimationStateRectTransform(string state, RectTransform rectTransform = null) {
		if (rectTransform == null) rectTransform = rectRransform;

		int stateId = StateItem.GetStateId(state);
		if (stateId < 0) return; // no state found

		animationStates[stateId].RectTransform_offsetMin = rectTransform.offsetMin;
		animationStates[stateId].RectTransform_offsetMax = rectTransform.offsetMax;
		animationStates[stateId].RectTransform_anchorMin = rectTransform.anchorMin;
		animationStates[stateId].RectTransform_anchorMax = rectTransform.anchorMax;
	}



	// animations process
	int animationStartState, animationEndState;
	float animationCurrentDuration, animationCurrentStartDelay;
	/*
		Prop: IsAnimationActive
		Indicates is animation currently active or not
	*/
	public bool IsAnimationActive { get { return isAnimationActive; }}
	bool isAnimationActive;

	void StartAnimation(string startState, string endState) {
		isAnimationActive = false;
		animationStartState = StateItem.GetStateId(startState);
		animationEndState = StateItem.GetStateId(endState);
		animationCurrentDuration = 0;
		animationCurrentStartDelay = animationStartDelay;
		isAnimationActive = true;
	}

	void FinishAnimation() {
		isAnimationActive = false;
		animationCurrentDuration = animationDuration;
		SetAnimationFrame();
		if (OnAnimationFinished != null) OnAnimationFinished.Invoke(); // call OnAnimationFinished event
	}

	void ProcessAnimation() {
		if (isAnimationActive) {
			if (animationCurrentStartDelay > 0) {
				animationCurrentStartDelay -= Time.deltaTime;
			}
			else {
				animationCurrentDuration += Time.deltaTime;
				if (animationCurrentDuration < animationDuration) SetAnimationFrame();
				else FinishAnimation();
			}
		}
	}

	void SetAnimationFrame() {
		// RectTransform
		if (rectRransform && animationFields.RectTransform_offsetMin)
			rectRransform.offsetMin = Out(
				AnimationStates[animationStartState].RectTransform_offsetMin,
				AnimationStates[animationEndState].RectTransform_offsetMin,
				animationCurrentDuration,
				animationDuration
			);
		if (rectRransform && animationFields.RectTransform_offsetMax)
			rectRransform.offsetMax = Out(
				AnimationStates[animationStartState].RectTransform_offsetMax,
				AnimationStates[animationEndState].RectTransform_offsetMax,
				animationCurrentDuration,
				animationDuration
			);
		if (rectRransform && animationFields.RectTransform_anchorMin)
			rectRransform.anchorMin = Out(
				AnimationStates[animationStartState].RectTransform_anchorMin,
				AnimationStates[animationEndState].RectTransform_anchorMin,
				animationCurrentDuration,
				animationDuration
			);
		if (rectRransform && animationFields.RectTransform_anchorMax)
			rectRransform.anchorMax = Out(
				AnimationStates[animationStartState].RectTransform_anchorMax,
				AnimationStates[animationEndState].RectTransform_anchorMax,
				animationCurrentDuration,
				animationDuration
			);

		// CanvasGroup
		if (canvasGroup != null && animationFields.CanvasGroup_alpha) {
			canvasGroup.alpha = Out(
				AnimationStates[animationStartState].CanvasGroup_alpha,
				AnimationStates[animationEndState].CanvasGroup_alpha,
				animationCurrentDuration,
				animationDuration
			);
		}

		if (layoutElement && animationFields.LayoutElement_preferredHeight) {
			layoutElement.preferredHeight = Out(
				AnimationStates[animationStartState].LayoutElement_preferredHeight,
				AnimationStates[animationEndState].LayoutElement_preferredHeight,
				// (AnimationStates[animationStartState].LayoutElement_preferredHeight == -1) ? _real_ph : AnimationStates[animationStartState].LayoutElement_preferredHeight,
				// (AnimationStates[animationEndState].LayoutElement_preferredHeight == -1) ? _real_ph : AnimationStates[animationEndState].LayoutElement_preferredHeight,
				animationCurrentDuration,
				animationDuration
			);
		}

		if (layoutElement && animationFields.LayoutElement_preferredWidth) {
			layoutElement.preferredWidth = Out(
				AnimationStates[animationStartState].LayoutElement_preferredWidth,
				AnimationStates[animationEndState].LayoutElement_preferredWidth,
				animationCurrentDuration,
				animationDuration
			);
		}

	}



	// animation transition functions
	float Out(float startValue, float endValue, float time, float duration) {
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp(time, 0f, duration);
		time /= duration;

		if (time == 0f) return startValue;
		if (time == 1f) return endValue;

		return differenceValue * valueChangeSpeedCurve.Evaluate(time) + startValue;
	}

	Vector2 Out(Vector2 startValue, Vector2 endValue, float time, float duration) {
		Vector2 tempVector = startValue;
		tempVector.x = Out(startValue.x, endValue.x, time, duration);
		tempVector.y = Out(startValue.y, endValue.y, time, duration);
		return tempVector;
	}

	// float _real_ph = -1, _real_pw = -1;
	// bool _is_preferred_height_calculated, _is_preferred_width_calculated;

	// void CalculateAnimationParameters() {
	// 	// if animations enabled and layout element is present
	// 	if (layoutElement != null && (animationFields.LayoutElement_preferredHeight || animationFields.LayoutElement_preferredWidth)) {

	// 		// if height is enabled and not calculated yet
	// 		if (animationFields.LayoutElement_preferredHeight && !_is_preferred_height_calculated) {
	// 			layoutElement.preferredHeight = -1; // set layout height to auto size

	// 			// if preferred_height is not calculated (still changing)
	// 			if (_real_ph != LayoutUtility.GetPreferredHeight(rectRransform)) {
	// 				_real_ph = LayoutUtility.GetPreferredHeight(rectRransform);
	// 			}
	// 			// preferred_height calculated
	// 			else {
	// 				_real_ph = LayoutUtility.GetPreferredHeight(rectRransform);
	// 				_is_preferred_height_calculated = true;

	// 				// call default state
	// 				if (
	// 					!(Application.isEditor && !Application.isPlaying) // except not playing editor
	// 					&& StateItem.group == null // and single mode
	// 					// and preferred_width also calculated or didn't enabled
	// 					&& ((animationFields.LayoutElement_preferredWidth && _is_preferred_width_calculated) || !animationFields.LayoutElement_preferredWidth)
	// 				) StateItem.SetStateDefault(true);
	// 			}
	// 		}


	// 		// if width is enabled and not calculated yet
	// 		if (animationFields.LayoutElement_preferredWidth && !_is_preferred_width_calculated) {
	// 			layoutElement.preferredWidth = -1; // set layout width to auto size

	// 			// if preferred_width is not calculated (still changing)
	// 			if (_real_pw != LayoutUtility.GetPreferredWidth(rectRransform)) {
	// 				_real_pw = LayoutUtility.GetPreferredWidth(rectRransform);
	// 			}
	// 			// preferred_width calculated
	// 			else {
	// 				_real_pw = LayoutUtility.GetPreferredWidth(rectRransform);
	// 				_is_preferred_width_calculated = true;

	// 				// call default state
	// 				if (
	// 					!(Application.isEditor && !Application.isPlaying) // except not playing editor
	// 					&& StateItem.group == null // and single mode
	// 					// and preferred_height also calculated or didn't enabled
	// 					&& ((animationFields.LayoutElement_preferredHeight && _is_preferred_height_calculated) || !animationFields.LayoutElement_preferredHeight)
	// 				) StateItem.SetStateDefault(true);
	// 			}
	// 		}

	// 	}
	// }



	// callbacks
	/*
		Var: OnAnimationFinished
		This <UnityEvent at https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html> is called when animation transition is finished

		You can subscribe any of your functions to this event.

		(start code)
		GetComponent<UIStateItem>().OnAnimationFinished.AddListener(OnAnimationFinished);

		...

		void OnAnimationFinished() {
			Debug.Log("Animation finished!");
		}
		(end code)

	*/
	public UnityEvent OnAnimationFinished;
}
}

