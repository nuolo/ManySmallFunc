using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEditor.Callbacks;
using System;


public class BuildAndroid : EditorWindow
{


    string BuildToFolder;       //打包APK的输出路径
    Project tempProject; 

    bool ISFindConfig = false     ;
    bool ISaddConfig = false;
    bool ISChangeConfig = false;        bool IsCanShowBtn = true; Project ChangeProject;
    bool IsSign = false;
    bool IsInstallApk = true;

    [MenuItem("打包/多渠道打包窗口")]
    public static void Init()
    {
        //弹出窗口        
        EditorWindow.GetWindow(typeof(BuildAndroid));
       
    }



    BuildAndroid()
    {
        this.titleContent = new GUIContent("打包设置");
        tempProject = new Project();
    }
    private void OnEnable()
    {
        BuildToFolder = @"G:\项目打包\" + PlayerSettings.productName;

    }


    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        GUITitle();
        GUIConfig();
        GUIGameName();
        GUIHelp();
        GUIFindConfig();
        GUIAddConfing();
        GUIChangeConfig();
       

        GUIBuildFolder();
        EditorGUILayout.EndVertical();
    
    }
    #region GUI绘制界面

    //绘制标题
    void GUITitle()
    {
        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("自定义打包");
        GUILayout.Label(System.DateTime.Now.ToString("yyyy-MM-dd    HH:mm:ss"));

        GUILayout.EndVertical();
    }
    void GUIConfig()
    {
        GUILayout.BeginVertical();
        //绘制标题

        GUI.skin.label.fontSize = 17;
        GUI.skin.label.alignment = TextAnchor.UpperRight;
        GUILayout.Label("V1.7     ");
        GUILayout.EndVertical();
    }
    //绘制游戏名
    void GUIGameName()
    {
        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.TextField("游戏名：", PlayerSettings.productName);
        IsInstallApk = EditorGUILayout.Toggle("是否打包后安装", IsInstallApk);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }


    void GUIFindConfig()
    {
        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.Space(10);

        ISFindConfig = EditorGUILayout.Toggle("是否查找配置信息", ISFindConfig);
        if (ISFindConfig)
        {
            string SDKPluginspath = SDKPluginsPath.path;
            if (!Directory.Exists(SDKPluginspath)) { CreatEmtryBag(); this.ShowNotification(new GUIContent("暂无配置信息文件夹，以自动创建")); return; }
            if (Directory.GetDirectories(SDKPluginspath).Length == 0 || Directory.GetDirectories(SDKPluginspath) == null) { this.ShowNotification(new GUIContent("暂无配置信息，请添加配置2")); return; }
            else
            {
                string[] SDKNames = Directory.GetDirectories(SDKPluginspath);
                GUILayout.Space(10);
                for (int i = 0; i < SDKNames.Length; i++)
                {
                    if (GUILayout.Button(SDKNames[i], GUILayout.Width(200), GUILayout.ExpandWidth(true)))
                    {

                        ChooseAndroidProject(SDKNames[i]);

                    }
                }
            }

        }

        GUILayout.EndVertical();
    }
    void GUIAddConfing()
    {

        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.Space(10);
        //是否开启添加配置信息

        ISaddConfig = EditorGUILayout.Toggle("是否添加配置信息", ISaddConfig);

        if (ISaddConfig)
        {
            GUILayout.Label("配置信息");
            tempProject.Name = EditorGUILayout.TextField("配置信息名字：", tempProject.Name);
            tempProject.CompanyName = EditorGUILayout.TextField("公司名：", tempProject.CompanyName);
            tempProject.ProjectName = EditorGUILayout.TextField("软件名：", tempProject.ProjectName);
            tempProject.IconName = EditorGUILayout.TextField("Icon路径：", tempProject.IconName);
            tempProject.PackageName = EditorGUILayout.TextField("包名：", tempProject.PackageName);
            tempProject.Version = EditorGUILayout.TextField("版本：", tempProject.Version);
            tempProject.BundleVersionCode = EditorGUILayout.TextField("bundleVersionCode：", tempProject.BundleVersionCode);

            IsSign = EditorGUILayout.Toggle("是否签名：", IsSign);
            if (IsSign)
            {
                tempProject.IsSign = true;

                SaveKaystorePath(tempProject);
            }
            else
            {
                tempProject.IsSign = false;
                tempProject.KeystorePath = null;
                tempProject.KeyaliasName = null;
                tempProject.KeystorePass = null;
                tempProject.KeyaliasPass = null;
            }
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("保存配置信息", GUILayout.Height(20), GUILayout.Width(200)))
            {
                SavedPliginsPath path = new SavedPliginsPath(tempProject.Name);

                Project SaveProject = tempProject;
                UtilTHIS.CreatBag(path, SaveProject);
                ISaddConfig = false;
                ISFindConfig = true;
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
    void GUIChangeConfig()
    {
        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.Space(10);
        //是否开启添加配置信息
        ISChangeConfig = EditorGUILayout.Toggle("是否修改配置信息", ISChangeConfig);
        if (ISChangeConfig)
        {
            string SDKPluginspath = SDKPluginsPath.path;
            if (!Directory.Exists(SDKPluginspath)) { this.ShowNotification(new GUIContent("暂无配置信息文件夹，以自动创建")); Directory.CreateDirectory(SDKPluginspath); return; }
            if (Directory.GetDirectories(SDKPluginspath).Length == 0) { this.ShowNotification(new GUIContent("暂无配置信息，请添加配置2")); return; }

            string[] SDKNames = Directory.GetDirectories(SDKPluginspath);
            GUILayout.Space(10);
            if (IsCanShowBtn)
            {
                for (int i = 0; i < SDKNames.Length; i++)
                {
                    if (GUILayout.Button(SDKNames[i], GUILayout.Width(20), GUILayout.ExpandWidth(true)))
                    {
                        IsCanShowBtn = false;
                        if (BuildToFolder == null) { ShowNotification(new GUIContent("请选择打包的路径")); return; }
                        string PluginsName = new DirectoryInfo(SDKNames[i]).Name;   //找到plugins文件夹的名字
                        SavedPliginsPath PliginsPath = new SavedPliginsPath(PluginsName);
                        string txt = File.ReadAllText(PliginsPath.JsonPath, Encoding.UTF8);
                        ChangeProject = JsonUtility.FromJson<Project>(txt);
                    }
                }
            }
            else
            {
                if (ChangeProject != null)
                {

                    GUILayout.Label(ChangeProject.Name);
                    ChangeProject.ProjectName = EditorGUILayout.TextField("软件名：", ChangeProject.ProjectName);
                    ChangeProject.IconName = EditorGUILayout.TextField("Icon路径：", ChangeProject.IconName);
                    ChangeProject.PackageName = EditorGUILayout.TextField("包名：", ChangeProject.PackageName);
                    ChangeProject.Version = EditorGUILayout.TextField("版本：", ChangeProject.Version);
                    ChangeProject.BundleVersionCode = EditorGUILayout.TextField("bundleVersionCode：", ChangeProject.BundleVersionCode);

                    if (GUILayout.Button("保存配置信息", GUILayout.Height(20), GUILayout.Width(200)))
                    {

                        SavedPliginsPath path = new SavedPliginsPath(ChangeProject.Name);
                        if (File.Exists(path.JsonPath)) { File.Delete(path.JsonPath); }
                        AssetDatabase.Refresh();
                        AssetDatabase.SaveAssets();

                        string str = ChangeProject.SaveToString();
                        StreamWriter sw = new StreamWriter(path.JsonPath);
                        sw.WriteLine(str);
                        sw.Close();
                        AssetDatabase.Refresh();
                        AssetDatabase.SaveAssets();

                        string TitleTxt = UtilTHIS.ProjectToString(ChangeProject);
                        EditorUtility.DisplayDialog("配置信息更改完毕", TitleTxt, "确认");
                        ChangeProject = null;
                        ISChangeConfig = false;
                    }
                }
            }
        }
        else
        {
            IsCanShowBtn = true;
        }
        GUILayout.EndVertical();
    }
    void GUIBuildFolder()
    {
        GUILayout.BeginVertical();
        //BuildToFolder += PlayerSettings.productName;
        //绘制打包路径
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUIStyle style = new GUIStyle();
        style.fontSize = 17;
        GUILayout.Label("请选择要打包的路径：" + BuildToFolder, style);

        if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
        {
            string path = EditorUtility.OpenFolderPanel("Path to Save Images", @"C:\Users\administered\Desktop\Build", null);   //打开保存文件夹面板 
            if (path != null)
            {
                BuildToFolder = path;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    

    void GUIHelp()
    {
        GUILayout.BeginVertical();
        //辅助功能，默认开启
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 10;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("辅助功能，默认开启");
        GUILayout.BeginHorizontal();
        //辅助功能按钮
        if (GUILayout.Button("创建空包", GUILayout.ExpandWidth(false)))
        {
            CreatEmtryBag();
        }
        if (GUILayout.Button("配置oppo游戏中心", GUILayout.ExpandWidth(false)))
        {
            SetSomeBag("oppo游戏中心");
        }
        if (GUILayout.Button("配置oppo广告", GUILayout.ExpandWidth(false)))
        {
            SetSomeBag("oppo广告");
        }
        if (GUILayout.Button("配置vivo支付", GUILayout.ExpandWidth(false)))
        {
            SetSomeBag("vivo支付");
        }
        if (GUILayout.Button("配置vivo广告", GUILayout.ExpandWidth(false)))
        {
            SetSomeBag("vivo广告");
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    #endregion

    void ChooseAndroidProject(string jsonpath)
    {
        if (BuildToFolder == null) { ShowNotification(new GUIContent("请选择打包的路径")); return; }
        string PluginsName = new DirectoryInfo(jsonpath).Name;   //找到plugins文件夹的名字
        SavedPliginsPath CopyPliginsPath = new SavedPliginsPath(PluginsName);

        string txt = File.ReadAllText(CopyPliginsPath.JsonPath, Encoding.UTF8);
        Project ChoosedSetting = JsonUtility.FromJson<Project>(txt);

        string TitleTxt = UtilTHIS.ProjectToString(ChoosedSetting);
        bool SetPlayerSetting = EditorUtility.DisplayDialog("配置信息", TitleTxt+ "\n自动设置AndroidAPI等级为18-22", "打包","关闭");
        if (SetPlayerSetting)
        {
            SetProjectSetting(ChoosedSetting);
            if (Directory.Exists(SDKPluginsPath.MeAndroidDir))
            {
                Directory.Delete(SDKPluginsPath.MeAndroidDir, true);
            }
           
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            UtilTHIS. CopyOldLabFilesToNewLab(CopyPliginsPath.PluginsPath, SDKPluginsPath.MeAndroidDir);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Build(ChoosedSetting);
            OpenFolder(BuildToFolder);

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
        }
        
       
    }
    void SetProjectSetting(Project project)
    {
        PlayerSettings.companyName = project.CompanyName;
        PlayerSettings.productName = project.ProjectName;

        Texture2D tx = AssetDatabase.LoadAssetAtPath<Texture2D>(project.IconName);///tx = ("Assets/ArtWork/app_icon_360");
        int[] iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Android);
        Texture2D[] textureArray = new Texture2D[iconSize.Length];
        for (int i = 0; i < textureArray.Length; i++)
        {
            textureArray[i] = tx;
        }
        textureArray[0] = tx;
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, textureArray);
        AssetDatabase.SaveAssets();
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android,project.PackageName);
        //PlayerSettings.applicationIdentifier = project.PackageName;
        PlayerSettings.bundleVersion = project.Version;
        PlayerSettings.Android.bundleVersionCode = int.Parse(project.BundleVersionCode);

        //PlayerSettings.Android.keystoreName = project.KeystorePath;
        //PlayerSettings.Android.keyaliasName = project.KeyaliasName;
        //PlayerSettings.Android.keystorePass = project.KeystorePass;
        //PlayerSettings.Android.keyaliasPass = project.KeyaliasPass;

        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel18;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel22;

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

   

    void Build(Project project)
    {
        if (BuildToFolder == null) { ShowNotification(new GUIContent("请选择打包的路径")); return; }
        List<string> pathList = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                pathList.Add(scene.path);
            }
        }
        string BuildPath = string.Format("{0}/{1}_{2}.apk", BuildToFolder, project.ProjectName, project.Name);
        //if (File.Exists (BuildPath))
        //{
        //    //如果 软件名+配置名的安装包已经有了一个，则加上时间信息再次设置
       //     string str = project.Name + System.DateTime.Now.ToString("MM_dd_HH_mm_ss");
       //     BuildPath = string.Format("{0}/{1}_{2}.apk", BuildToFolder, project.ProjectName, str);
       // }

        BuildPipeline.BuildPlayer(pathList.ToArray(), BuildPath, BuildTarget.Android, BuildOptions.None);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

        if (IsInstallApk == true )
        {
            string packageName = project.PackageName;
            string cmd = string.Format("/k {0} {1}", @"F:&cd F:\Work\E\GLWork\android-sdk-windows\platform-tools& adb install", BuildPath);
            cmdInstallApk(cmd);
        }
       


    }
    void OpenFolder(string path)
    {
        if (string.IsNullOrEmpty(path)) return;

        path = path.Replace("/", "\\");
        if (!Directory.Exists(path))
        {
            Debug.LogError("No Directory: " + path);
            return;
        }

        System.Diagnostics.Process.Start("explorer.exe", path);
    }

    #region 辅助功能

    /// <summary>
    /// 创建空包。不包含sdk
    /// </summary>
    void CreatEmtryBag()
    {
        ISFindConfig = false;
        SavedPliginsPath path = new SavedPliginsPath("空包");
      
        Project SaveProject = new Project() ;
        SaveProject.Name = path.Name;
        SaveProject.CompanyName = "趣多多";
        SaveProject.ProjectName = PlayerSettings.productName;
        SaveProject.IconName = "无";
        SaveProject.PackageName = PlayerSettings.applicationIdentifier;
        SaveProject.Version = "1.0";
        SaveProject.BundleVersionCode = "10";
        SaveProject.IsSign = true;

        SaveKaystorePath(SaveProject);
        UtilTHIS.CreatBag(path ,SaveProject);
        ISFindConfig = true ;
    }
 

    void SetSomeBag(string Name)
    {
        tempProject.Name = EditorGUILayout.TextField("配置信息名字：", Name);
        tempProject.CompanyName = EditorGUILayout.TextField("公司名：", "趣多多");
        tempProject.ProjectName = EditorGUILayout.TextField("软件名：", PlayerSettings.productName);
        tempProject.IconName = EditorGUILayout.TextField("Icon路径：", "无");
        tempProject.PackageName = EditorGUILayout.TextField("包名：", PlayerSettings.applicationIdentifier);
        tempProject.Version = EditorGUILayout.TextField("版本：", "1.1");
        tempProject.BundleVersionCode = EditorGUILayout.TextField("bundleVersionCode：", "10");

        IsSign = EditorGUILayout.Toggle("是否签名：", true);
        if (IsSign)
        {
            tempProject.IsSign = true;
            SaveKaystorePath(tempProject);
        }
        else
        {
            tempProject.IsSign = false;
            tempProject.KeystorePath = null;
            tempProject.KeyaliasName = null;
            tempProject.KeystorePass = null;
            tempProject.KeyaliasPass = null;
        }
        ISaddConfig = true;
    }

    #endregion
    #region 配置Keystore

    public void SaveKaystorePath(Project project)
    {
        project.KeystorePath = EditorGUILayout.TextField("keystorePath：", @"C:\Users\administered\Desktop\签名\Sg3.keystore");
        project.KeyaliasName = EditorGUILayout.TextField("keyaliasName：", "Sg3.keystore");
        project.KeystorePass = EditorGUILayout.TextField("keystorePass：", "morefunte");
        project.KeyaliasPass = EditorGUILayout.TextField("keyaliasPass：", "morefunte");
    }

    #endregion

    #region cmd命令安装apk

    public void cmdInstallApk(string cmdInfo)
    {
        if (IsInstallApk ==false )
        {
            return;
        }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR

        cmdInfo = cmdInfo.Trim().TrimEnd('&') + "&exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状
        System.Diagnostics.ProcessStartInfo mCmdProcess = new System.Diagnostics.ProcessStartInfo("C:\\Windows\\system32\\cmd.exe");
        mCmdProcess.Arguments = cmdInfo;
        mCmdProcess.CreateNoWindow = false ;         // 不创建新窗口    
        mCmdProcess.UseShellExecute = true;       //不启用shell启动进程   

        mCmdProcess.RedirectStandardOutput = false; // 重定向标准输出 
        mCmdProcess.RedirectStandardInput = false; // 重定向标准输出    
        mCmdProcess.RedirectStandardError = false;  // 重定向错误输出  


        System.Diagnostics.Process mProcess = System.Diagnostics.Process.Start(mCmdProcess);


        mCmdProcess.StandardOutputEncoding = System.Text.Encoding.GetEncoding("GBK");
        mCmdProcess.StandardErrorEncoding = System.Text.Encoding.GetEncoding("GBK");
        try
        {
            String myReadInfo = mProcess.StandardOutput.ReadToEnd();
            if (myReadInfo != null)
            {
                UnityEngine.Debug.Log("Cmd执行.... " + myReadInfo);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("Cmd执行错误 " + e.Message);
        }
        mProcess.WaitForExit();

     
#endif
    }
    #endregion
    //监听打包完成
    [PostProcessBuildAttribute(1)]
    public static void OnAfterBuildPlayer(BuildTarget target, string pathToBuildProject)
    {
        Debug.Log("自定义打包完成");
        Debug.Log("Target：" + target + " Path:" + pathToBuildProject);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
       
    }

 
}

public  class Project
{
    public string Name ;
    public string CompanyName ;
    public string ProjectName ;
    public string IconName ;
    public string PackageName ;
    public string Version ;
    public string BundleVersionCode ;
    public   bool IsSign ;
    public string KeystorePath ;
    public string KeyaliasName ;
    public string KeystorePass ;
    public string KeyaliasPass ;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
   
}
class SavedPliginsPath
{
    public  string Name;//SDKPlugins/而此时/plguins，Json plugins的名字
    public string PluginsPath { get { return SDKPluginsPath.path +"/"+ Name+ "/Plugins/Android"; } }
    public string JsonPath { get { return SDKPluginsPath.path + "/" + Name + "/"+Name+".json"; } }

    public SavedPliginsPath(string name)
    {
        Name = name;
    }
}
public  class SDKPluginsPath
{   //SDKPlugins的地址
    public static string path = Application.dataPath + "/../" + "SDKPlugins";
    //Asset下的Plugins/Android文件夹
    public static string MeAndroidDir = Application.dataPath + "/Plugins/Android";
  
}
class UtilTHIS
{
    public static string ProjectToString(Project project)
    {
        string txt = "";
        txt += "\n"              + project.Name;
        txt += "\n";
        txt += "\n公司名："      + project.CompanyName;
        txt += "\n软件名："      + project.ProjectName;
        txt += "\nIcon路径："   +  project.IconName + "[暂无效]";
        txt += "\n包名：" + project.PackageName;
        txt += "\n版本：" + project.Version;
        txt += "\nbundleVersionCode:" + project.BundleVersionCode;
        txt += "\n是否签名：" + project.IsSign;
        txt += "\nkeystorePath：" + project.KeystorePath ;
        txt += "\nkeyaliasName：" + project.KeyaliasName ;
        txt += "\nkeystorePass：" + project.KeystorePass ;
        txt += "\nkeyaliasPass：" + project.KeyaliasPass;
        return txt;
    }

   
   public static  void CopyOldLabFilesToNewLab(string sourcePath, string savePath)
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        string[] labDirs = Directory.GetDirectories(sourcePath);//目录
        string[] labFiles = Directory.GetFiles(sourcePath);//文件
        if (labFiles.Length > 0)
        {
            for (int i = 0; i < labFiles.Length; i++)
            {
                if (Path.GetExtension(labFiles[i]) != ".meta")//排除.lab文件
                {
                    File.Copy(sourcePath + "\\" + Path.GetFileName(labFiles[i]), savePath + "\\" + Path.GetFileName(labFiles[i]), true);
                }
            }
        }
        if (labDirs.Length > 0)
        {
            for (int j = 0; j < labDirs.Length; j++)
            {
                Directory.GetDirectories(sourcePath + "\\" + Path.GetFileName(labDirs[j]));

                //递归调用
                CopyOldLabFilesToNewLab(sourcePath + "\\" + Path.GetFileName(labDirs[j]), savePath + "\\" + Path.GetFileName(labDirs[j]));
            }
        }

    }

    public static void CreatBag(SavedPliginsPath path, Project SaveProject)
    {
        
        if (!Directory.Exists(path.PluginsPath)) { Directory.CreateDirectory(path.PluginsPath); } else {  return; }
        string str = SaveProject.SaveToString();
        StreamWriter sw = new StreamWriter(path.JsonPath);
        sw.WriteLine(str);
        sw.Close();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}




