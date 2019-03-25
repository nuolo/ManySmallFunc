using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;

/// <summary>关于查找路径
/// 作者：小白熊。
/// QQ:2565190823
/// </summary>
public class FindPathEditor
{


    /// <summary>快捷键 Ctrl+Shift + C ===>复制选中两个游戏对象之间的查找路径x ：transform.FindChild("路径x")
    /// </summary>
    [MenuItem("GameObject/Create Other/Copy Find Child Path _%#_ C")]
    static void CopyFindChildPath()
    {

        Object[] objAry = Selection.objects;
        //Debug.Log(objAry.Length);

        if (objAry.Length == 2)
        {
            GameObject gmObj0 = (GameObject)objAry[0];
            GameObject gmObj1 = (GameObject)objAry[1];
            List<Transform> listGameParent0 = new List<Transform>(gmObj0.transform.GetComponentsInParent<Transform>(true));
            List<Transform> listGameParent1 = new List<Transform>(gmObj1.transform.GetComponentsInParent<Transform>(true));
            System.Text.StringBuilder strBd = new System.Text.StringBuilder("");
            //gmObj0.transform.FindChild("");
            //string findCode = "gmObj0"
            if (listGameParent0.Contains(gmObj1.transform))
            {
                int startIndex = listGameParent0.IndexOf(gmObj1.transform);
                Debug.Log(startIndex);
                for (int i = startIndex; i >= 0; i--)
                {
                    if (i != startIndex)
                    {
                        strBd.Append(listGameParent0[i].gameObject.name).Append(i != 0 ? "/" : "");
                    }

                }
            }

            if (listGameParent1.Contains(gmObj0.transform))
            {
                int startIndex = listGameParent1.IndexOf(gmObj0.transform);
                for (int i = startIndex; i >= 0; i--)
                {
                    if (i != startIndex)
                    {
                        strBd.Append(listGameParent1[i].gameObject.name).Append(i != 0 ? "/" : "");
                    }
                }
            }

            TextEditor textEditor = new TextEditor();
            textEditor.text = "\"" + strBd.ToString() + "\"";// "hello world";
            textEditor.OnFocus();
            textEditor.Copy();
            string colorStr = strBd.Length > 0 ? "<color=blue>" : "<color=red>";
            Debug.Log(colorStr + "复制：【\"" + strBd.ToString() + "\"】" + "</color>");
        }
    }


    private Button BPLocaChangeGold;
    private Button btn_Invite;
    private Button playerPokerInfo;
    private Button showGoldParent1;
    private Button layoutParent_BP1;

    #region/*——关于选中物体，右键创建 变量的 代码——*/

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/GameObject")]
    static void CreateCode_GameObject()
    {
        CreateCodeSum("GameObject");
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/Transform")]
    static void CreateCode_Transform()
    {
        CreateCodeSum<Transform>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/RectTransform")]
    static void CreateCode_RectTransform()
    {
        CreateCodeSum<RectTransform>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/Text")]
    static void CreateCode_Text()
    {
        CreateCodeSum<Text>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/Image")]
    static void CreateCode_Image()
    {
        CreateCodeSum<Image>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/RawImage")]
    static void CreateCode_RawImage()
    {
        CreateCodeSum<RawImage>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/Button")]
    static void CreateCode_Button()
    {
        CreateCodeSum<Button>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/Toggle")]
    static void CreateCode_Toggle()
    {
        CreateCodeSum<Toggle>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/Slider")]
    static void CreateCode_Slider()
    {
        CreateCodeSum<Slider>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/Scrollbar")]
    static void CreateCode_Scrollbar()
    {
        CreateCodeSum<Scrollbar>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/Dropdown")]
    static void CreateCode_Dropdown()
    {
        CreateCodeSum<Dropdown>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/InputField")]
    static void CreateCode_InputField()
    {
        CreateCodeSum<InputField>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/Canvas")]
    static void CreateCode_Canvas()
    {
        CreateCodeSum<Canvas>();
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    [MenuItem("GameObject/Create Other/Create Var Code/ScrollRect")]
    static void CreateCode_ScrollRect()
    {
        CreateCodeSum<ScrollRect>();
    }

    #region /*——逻辑部分——*/
    static string CreateCodeSum<Type>() where Type : Component
    {
        string[] strAry = typeof(Type).ToString().Split('.');
        string strType = strAry[strAry.Length - 1];
        CreateCodeSum(strType);
        return "";
    }

    /// <summary>创建 变量的 代码
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    /// <returns></returns>
    static string CreateCodeSum(string strType)//<Type>() where Type : Component
    {
        Object[] objAry = Selection.objects;
        //Debug.Log(objAry.Length);
        System.Text.StringBuilder strBd = new System.Text.StringBuilder("");
        System.Text.StringBuilder strBdFindCode = new System.Text.StringBuilder("");

        ////string[] strAry = typeof(Type).ToString().Split('.');
        ////string strType = strAry[strAry.Length - 1];

        for (int i = 0; i < objAry.Length; i++)
        {
            GameObject gmObj = objAry[i] as GameObject;
            if (gmObj != null)
            {
                //这是得到连接的变量 ，格式： private Button showGoldParent3;
                strBd.Append(CreateCodeVar(gmObj, strType)).Append("\n");


                //这是得到连接的查找代码 。
                //格式：showGoldParent3 = transform.FindChild("Canvas/UINN28Table/Bg_BiPai/showGoldParent3").GetComponent<Button>();//（请检查路径）
                string strFindPath = GetParentPath(gmObj);
                strBdFindCode.Append("//").Append(gmObj.name).Append(" = transform.Find(\"")
                    .Append(strFindPath).Append("\")");
                if (strType == "GameObject")
                {
                    strBdFindCode.Append(".gameObject;").Append("//（请检查路径）").Append("\n");
                }
                else {
                    strBdFindCode.Append(".GetComponent<").Append(strType).Append(">();")
                         .Append("//（请检查路径）").Append("\n");
                }
            }
        }

        strBd.Append(strBdFindCode.ToString());

        //让系统复制 生成的 代码，  直接到 脚本里面取 Ctrl+V  粘贴就可以了
        TextEditor textEditor = new TextEditor();
        textEditor.text = strBd.ToString();//
        textEditor.OnFocus();
        textEditor.Copy();
        string colorStr = strBd.Length > 0 ? "<color=blue>" : "<color=red>";
        Debug.Log(colorStr + "复制：【\"" + strBd.ToString() + "\"】" + "</color>");
        return "";
    }

    /// <summary>创建变量 返回string 。格式：private Button showGoldParent3;
    /// </summary>
    /// <param name="gmObj"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    static string CreateCodeVar(GameObject gmObj, string type)
    {
        System.Text.StringBuilder strBd = new System.Text.StringBuilder("");
        if (gmObj == null) return strBd.ToString();
        //Debug.Log(typeof(Type).ToString());

        string gmName = gmObj.name;
        for (int i = 0; i < gmObj.name.Length; i++)
        {//移除空格
            gmName = gmObj.name.Replace(" ", "");
        }
        strBd.Append("private ").Append(type).Append(" ").Append(gmName).Append(";");
        return strBd.ToString();
    }

    /// <summary>创建变量GameObject 返回string 。格式：private GameObject showGoldParent3;//
    /// </summary>
    /// <param name="gmObj"></param>
    /// <returns></returns>
    static string CreateCodeGameObject(GameObject gmObj)
    {
        System.Text.StringBuilder strBd = new System.Text.StringBuilder("");
        if (gmObj == null) return strBd.ToString();

        string gmName = gmObj.name;
        for (int i = 0; i < gmObj.name.Length; i++)
        {//移除空格
            gmName = gmObj.name.Replace(" ", "");
        }
        strBd.Append("private ").Append("GameObject").Append(" ").Append(gmName).Append(";").Append("//");
        return strBd.ToString();
    }

    /// <summary>获取GameObject 的父对象 路径。  //路径格式：Canvas/UINN28Table/Bg_BiPai/showGoldParent0
    /// </summary>
    /// <param name="gmObj"></param>
    /// <returns></returns>
    static string GetParentPath(GameObject gmObj)
    {
        System.Text.StringBuilder strBd = new System.Text.StringBuilder("");
        List<Transform> listGameParent0 = new List<Transform>(gmObj.transform.GetComponentsInParent<Transform>(true));

        for (int i = (listGameParent0.Count - 1); i >= 0; i--)
        {
            strBd.Append(listGameParent0[i].gameObject.name).Append(i != 0 ? "/" : "");
        }
        Debug.Log(strBd.ToString());
        return strBd.ToString();
    }
    #endregion
    #endregion

}


