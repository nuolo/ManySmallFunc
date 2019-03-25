using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextCreate : MonoBehaviour {

    string m_FileName;
    string information = "哈哈哈";
    void Start()
    {
        m_FileName = Application.dataPath ;
        if (!File.Exists(m_FileName))//如果文件不存在，就创建
            Save("1.text", information);

    }
    void Save(string Path, string information)
    {
        FileStream aFile = new FileStream( m_FileName +@"\"+ Path, FileMode.OpenOrCreate);

        StreamWriter sw = new StreamWriter(aFile);
        sw.Write(information);
        sw.Close();
        sw.Dispose();
    }
}
