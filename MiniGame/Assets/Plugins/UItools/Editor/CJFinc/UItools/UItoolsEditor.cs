using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CJFinc.UItools {

public static class UItoolsEditor {
	private static GameObject selectedObject;

	// helper
	private static void GetSelectedObject() {
		selectedObject = Selection.activeGameObject;
		if (!selectedObject) {
			selectedObject = null;
			return;
		}
		if (!GameObject.Find(selectedObject.name)) {
			selectedObject = null;
			return;
		}
	}



	// components
	[MenuItem("Component/UI tools/UIItem")]
	// [MenuItem("GameObject/UI tools/Add component/UIItem", false, 20)]
	private static void AddUIItem() {
		GetSelectedObject();
		if (selectedObject != null) Undo.AddComponent(selectedObject, typeof(UIItem));
	}

	[MenuItem("Component/UI tools/UIStateItem")]
	// [MenuItem("GameObject/UI tools/Add component/UIStateItem", false, 20)]
	private static void AddUIStateItem() {
		GetSelectedObject();
		if (selectedObject != null) Undo.AddComponent(selectedObject, typeof(UIStateItem));
	}

	[MenuItem("Component/UI tools/UIStateItemMirror")]
	// [MenuItem("GameObject/UI tools/Add component/UIStateItemMirror", false, 20)]
	private static void AddUIStateItemMirror() {
		GetSelectedObject();
		if (selectedObject != null) Undo.AddComponent(selectedObject, typeof(UIStateItemMirror));
	}

	[MenuItem("Component/UI tools/UIStateItemAnimation")]
	// [MenuItem("GameObject/UI tools/Add component/UIStateItemAnimation", false, 20)]
	private static void AddUIStateItemAnimation() {
		GetSelectedObject();
		if (selectedObject != null) Undo.AddComponent(selectedObject, typeof(UIStateItemAnimation));
	}

	[MenuItem("Component/UI tools/UIGroup")]
	// [MenuItem("GameObject/UI tools/Add component/UIGroup", false, 20)]
	private static void AddUIGroup() {
		GetSelectedObject();
		if (selectedObject != null) Undo.AddComponent(selectedObject, typeof(UIGroup));
	}

	[MenuItem("Component/UI tools/UIStateGroup")]
	// [MenuItem("GameObject/UI tools/Add component/UIStateGroup", false, 20)]
	private static void AddUIStateGroup() {
		GetSelectedObject();
		if (selectedObject != null) Undo.AddComponent(selectedObject, typeof(UIStateGroup));
	}

	[MenuItem("Component/UI tools/UIStateGroupControl")]
	// [MenuItem("GameObject/UI tools/Add component/UIStateGroupControl", false, 20)]
	private static void AddUIStateGroupControl() {
		GetSelectedObject();
		if (selectedObject != null) Undo.AddComponent(selectedObject, typeof(UIStateGroupControl));
	}



	// prefab helper
	private static void AddPrefab(MenuCommand menuCommand, string prefabName) {
		string[] assetGuids = AssetDatabase.FindAssets (prefabName);
		if (assetGuids.Length < 1) return;

		Object asset = AssetDatabase.LoadAssetAtPath(
			AssetDatabase.GUIDToAssetPath(assetGuids[0]),
			typeof (GameObject)
		);
		GameObject go = GameObject.Instantiate(asset) as GameObject;
		go.name = asset.name;

		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
		Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
		Selection.activeObject = go;
	}



	// prefabs
	[MenuItem("GameObject/UI tools/UIItem", false, 9)]
	private static void AddPrefabUIItem(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-prefab UIItem");
	}

	[MenuItem("GameObject/UI tools/UIStateItem", false, 9)]
	private static void AddPrefabUIStateItem(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-prefab UIStateItem");
	}

	[MenuItem("GameObject/UI tools/UIStateItemMirror", false, 9)]
	private static void AddPrefabUIStateItemMirror(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-prefab UIStateItemMirror");
	}

	[MenuItem("GameObject/UI tools/UIStateItemAnimation", false, 9)]
	private static void AddPrefabUIStateItemAnimation(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-prefab UIStateItemAnimation");
	}

	[MenuItem("GameObject/UI tools/UIGroup", false, 9)]
	private static void AddPrefabUIGroup(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-prefab UIGroup");
	}

	[MenuItem("GameObject/UI tools/UIStateGroup", false, 9)]
	private static void AddPrefabUIStateGroup(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-prefab UIStateGroup");
	}

	[MenuItem("GameObject/UI tools/UIStateGroupControl", false, 9)]
	private static void AddPrefabUIStateGroupControl(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-prefab UIStateGroupControl");
	}



	// use cases
	// Buttons
	[MenuItem("GameObject/UI tools/Use cases/On off button", false, 20)]
	private static void AddUsecase1(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-usecase On off button");
	}

	[MenuItem("GameObject/UI tools/Use cases/Multi-state button", false, 20)]
	private static void AddUsecase2(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-usecase Multi-state button");
	}

	[MenuItem("GameObject/UI tools/Use cases/Radio buttons group", false, 20)]
	private static void AddUsecase3(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-usecase Radio buttons group");
	}

	// UI elements
	[MenuItem("GameObject/UI tools/Use cases/Multi-state element", false, 40)]
	private static void AddUsecase40(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-usecase Multi-state element");
	}

	[MenuItem("GameObject/UI tools/Use cases/UI element with position change", false, 41)]
	private static void AddUsecase41(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-usecase UI element with position change");
	}

	// Complex cases
	[MenuItem("GameObject/UI tools/Use cases/Accordion", false, 60)]
	private static void AddUsecase60(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-usecase Accordion");
	}

	[MenuItem("GameObject/UI tools/Use cases/Sub-menu", false, 61)]
	private static void AddUsecase61(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-usecase Sub-menu");
	}

	[MenuItem("GameObject/UI tools/Use cases/Tabs with content", false, 62)]
	private static void AddUsecase62(MenuCommand menuCommand) {
		AddPrefab(menuCommand, "l:Uitools-usecase Tabs with content");
	}

}

}
