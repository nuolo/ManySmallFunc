using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Obj3DClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PointerClickHandler.m_UnityEvent_Click.AddListener((str,pointerevent)=> { Debug.Log("Click:"+str+pointerevent.ToString()); });
        PointerClickHandler.m_UnityEvent_Enter.AddListener((str, pointerevent) => { Debug.Log("Enter:" + str + pointerevent.ToString()); });
        PointerClickHandler.m_UnityEvent_Down.AddListener((str, pointerevent) => { Debug.Log("Down:" + str + pointerevent.ToString()); });
        PointerClickHandler.m_UnityEvent_Exit.AddListener((str, pointerevent) => { Debug.Log("Exit:" + str + pointerevent.ToString()); });
        PointerClickHandler.m_UnityEvent_Up.AddListener((str, pointerevent) => { Debug.Log("Up:" + str + pointerevent.ToString()); });
    }
	
	
}
