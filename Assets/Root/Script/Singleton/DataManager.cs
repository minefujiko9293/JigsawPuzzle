using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

/// <summary>
/// DataManager 数据管理器
/// 单例模式
/// </summary>
public class DataManager : MonoBehaviour {
    private static DataManager _instance = null;
    public static DataManager Instance {
        get { return GetInstance(); }
    }

    public static DataManager GetInstance() {
        if (_instance == null) {
            GameObject obj = new GameObject("DataManager");
            _instance = obj.AddComponent<DataManager>();
            DontDestroyOnLoad(obj);
        }
        return _instance;
    }

    /// <summary>
    /// 加载Xml文件 
    /// </summary>
    /// <returns>The levels.</returns>
    public List<Mission> LoadLevels() {
        //创建Xml对象
        XmlDocument xmlDoc = new XmlDocument();
        //如果本地存在配置文件则读取配置文件
        //否则在本地创建配置文件的副本
        //为了跨平台及可读可写，需要使用Application.persistentDataPath
        string filePath = Application.persistentDataPath + "/levels.xml";
        if (!IOUntility.isFileExists(filePath)) {
            xmlDoc.LoadXml(((TextAsset)Resources.Load("levels")).text);
            IOUntility.CreateFile(filePath, xmlDoc.InnerXml);
        }
        else {
            xmlDoc.Load(filePath);
        }
        XmlElement root = xmlDoc.DocumentElement;
        XmlNodeList levelsNode = root.SelectNodes("/levels/level");
        //初始化关卡列表
        List<Mission> levels = new List<Mission>();
        foreach (XmlElement xe in levelsNode) {
            Mission l = new Mission();
            l.ID = xe.GetAttribute("id");
            l.Score = int.Parse(xe.GetAttribute("score"));
            //使用unlock属性来标识当前关卡是否解锁
            if (xe.GetAttribute("unlock") == "1") {
                l.UnLock = true;
            }
            else {
                l.UnLock = false;
            }

            levels.Add(l);
        }

        return levels;
    }


}
