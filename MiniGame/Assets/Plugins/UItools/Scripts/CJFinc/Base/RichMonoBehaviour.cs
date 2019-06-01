using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace CJFinc {

/*
	[DisallowMultipleComponent]
	// class

	void Start() {
		if (initOnStart) Init (true);
	}

	protected override void RichInit() {
		base.RichInit(); // as first line - call base initialization

		// your initialization here
	}

	// if needed
	protected override void Update() {
		base.Update();

		// your update functions here
	}

	// if needed - add UpdateInEditor implementation
	// also add for class [ExecuteInEditMode]
	// and exclude needless calls from Update
	// 	if (!(Application.isEditor && !Application.isPlaying)) {
	//		// place all Update calls here to avoid duplication call at runtime
	//	}
	protected override void UpdateInEditor() {
		base.UpdateInEditor();

		// your update in editor functions here
	}
*/

/*
	Class: RichMonoBehaviour
	<RichMonoBehaviour> is a base class for all UItools components.

	Details:
	It contains base methods and variables that can be used for any UItools component.

	In editor view there is a two general options for each component.
	Event <OnInitFinish> and <initOnStart> option.

	(see RichMonoBehaviour-editor.png)
*/

public class RichMonoBehaviour : MonoBehaviour {
	bool isInitialized = false;
	/*
		Prop: IsInitialized
		Is current component already initialized or not
	*/
	public bool IsInitialized { get { return isInitialized; } }

	/*
		Func: Init ()
		Initialize current component if it's not initialized yet

		(start code)
		GetComponent<UIItem>().Init();
		(end code)

		You can force component re-init by passing true as a parameter.

		(start code)
		GetComponent<UIItem>().Init(true);
		(end code)

		But it's better to use special helper method for that <InitForce ()>

		Parameters:
			force - bool, defines should initialization be forced or not. Default - false
	*/
	public void Init(bool force=false) {
		if (!enabled) return; // skip for disabled component
		if (IsInitialized && !force) return; // skip if already initialized and not force

		RichInit(); // rich component initialization

		isInitialized = true;

		if (OnInitFinish != null) OnInitFinish.Invoke(); // call OnInitFinish event
	}

	/*
		Func: InitForce ()
		Force component initialization

		It means that <IsInitialized> value will be ignored.

		(start code)
		GetComponent<UIItem>().InitForce();
		(end code)
	*/
	public void InitForce() {
		Init(true);
	}

	protected virtual void RichInit() {}

	public void Reset() {
		Init(true);
	}

	protected virtual void Update() {}

	void OnRenderObject() {
		if (Application.isEditor && !Application.isPlaying) {
			UpdateInEditor();
		}
	}

	protected virtual void UpdateInEditor() {}



	 // editor
	[SerializeField] bool isEditorCallbacksExpanded;
	[SerializeField] bool isEditorGeneralExpanded;



	// init
	/*
		Var: initOnStart
		Should this component be initialized at <Start at https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html> or not
	*/
	public bool initOnStart = true;



	// callbacks
	/*
		Var: OnInitFinish
		This <UnityEvent at https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html> is called when initialization is finished

		You can subscribe any of your functions to this event.

		(start code)
		GetComponent<UIItem>().OnInitFinish.AddListener(ItemInitFinished);

		...

		void ItemInitFinished() {
			Debug.Log("Item initialized");
		}
		(end code)

	*/
	public UnityEvent OnInitFinish;
}

}
