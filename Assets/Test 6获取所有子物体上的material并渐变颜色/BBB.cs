using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBB : MonoBehaviour {

    public float sp = 0.4f;
    Material mat;
    // Update is called once per frame
    void Update () {
        foreach (Transform child in gameObject.transform)
        {
            //Debug.Log("所有该脚本的物体下的子物体名称:"+child.name);  
            mat = child.GetComponent<MeshRenderer>().material;
            //mat.color = Color.blue;
            Color col = mat.GetColor("_Color");
            col.a = Mathf.Lerp(col.a, 0, Time.deltaTime * sp);
            mat.SetColor("_Color", col);
        }
    }
}
