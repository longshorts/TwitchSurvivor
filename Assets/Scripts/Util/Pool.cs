using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool: Singleton<Pool> {

    public GameObject PoolObjectPrefab;
    private List<GameObject> pool;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*public T Get()
    {
        if(pool.Count <= 0)
        {

        }
    }*/

    private void AddNewObject()
    {

    }
}
