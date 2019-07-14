using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class Pools : MonoBehaviour {
    public Transform testPrefab;
    private SpawnPool pool;
	// Use this for initialization
	void Start () {
        pool=PoolManager.Pools.Create("test");

        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
