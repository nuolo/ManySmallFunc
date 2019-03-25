using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Transform : MonoBehaviour {
    private Transform[] AllChildren;
    // Use this for initialization
    void Start () {
        #region 1
        //AllChildren = gameObject.GetComponentsInChildren<Transform>();
        #endregion

        #region 2
        AllChildren = gameObject.GetComponentsInChildren<Transform>(true);
        #endregion



        foreach (var item in AllChildren)
        {
            Debug.Log(item.gameObject.name);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
