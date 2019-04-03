using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutTest : MonoBehaviour
{
  
    void Start()
    {

    }
    [ContextMenu("Chang")]
    void ChangLayout()
    {
        GetComponent<GridLayoutGroup>().constraintCount = 3;
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }
}
