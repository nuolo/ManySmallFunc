using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[InitializeOnLoad]
public  class Tool_Build  {

 
     static Tool_Build()
    {
        EditorApplication.hierarchyChanged += projectChanged;
    }//

    private static void projectChanged()
    {
        SaveBuildSeting();
    }

  static void  SaveBuildSeting()
    {

         PlayerSettings.companyName = "趣多多";
        //PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        //PlayerSettings.Android.androidTVCompatibility = false;
        //PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel18;
        //PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        //PlayerSettings.Android.keystoreName = @"C:\Users\Admin\Desktop\资料\签名\Sg3.keystore";
        //PlayerSettings.Android.keyaliasName = "Sg3.keystore";
        //PlayerSettings.Android.keystorePass = "morefunte";
        //PlayerSettings.Android.keyaliasPass = "morefunte";//
    }
}
