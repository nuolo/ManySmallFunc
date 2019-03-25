using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextRead : MonoBehaviour {

    string[] strs;
    string m_Str;
    void Start()
    {
        ReadFile(Application.dataPath+"/1.text");

    }
    void ReadFile(string FileName)    {        strs = File.ReadAllLines(FileName);//读取制定文件路径的所有行，并将数据读取到定义好的字符数组strs中，一行存一个单元
        for (int i = 0; i < strs.Length; i++)        {            m_Str += strs[i];
            m_Str += "\n";//每一行末尾换行
        }
        //Debug.Log(strs.ToString());
        Debug.Log(m_Str);    }
}
