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
        get {
			if (_instance == null) {
				GameObject obj = new GameObject("DataManager");
				_instance = obj.AddComponent<DataManager>();
				DontDestroyOnLoad(obj);
			}
			return _instance;
		}
    }


    /// <summary>
    /// 加载Xml文件 
    /// </summary>
    /// <returns>The levels.</returns>
	public List<Mission> LoadMissions() {
        //创建Xml对象
        XmlDocument xmlDoc = new XmlDocument();
        //如果本地存在配置文件则读取配置文件
        //否则在本地创建配置文件的副本
        //为了跨平台及可读可写，需要使用Application.persistentDataPath
		string filePath = Application.persistentDataPath + "/missions.xml";
		//Debug.Log(filePath);
        if (!IOUntility.isFileExists(filePath)) {
			xmlDoc.LoadXml(((TextAsset)Resources.Load("missions")).text);
            IOUntility.CreateFile(filePath, xmlDoc.InnerXml);
        }
        else {
            xmlDoc.Load(filePath);
        }
        XmlElement root = xmlDoc.DocumentElement;
		XmlNodeList missionNodes = root.SelectNodes("/missions/mission");
        //初始化关卡列表
        List<Mission> missions = new List<Mission>();
        foreach (XmlElement xe in missionNodes) {
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

            missions.Add(l);
        }

        return missions;
    }

	/// <summary>
	/// 设置关卡信息
	/// </summary>
	/// <param name="id"></param>
	/// <param name="unlock">是否解锁</param>
	/// <param name="score">分数</param>
	private void SetMission(int id, bool unlock,int score) {
		//创建Xml对象
		XmlDocument xmlDoc = new XmlDocument();
		string filePath = Application.persistentDataPath + "/missions.xml";
		xmlDoc.Load(filePath);
		XmlElement root = xmlDoc.DocumentElement;
		XmlNodeList missionNodes = root.SelectNodes("/missions/mission");
		foreach (XmlElement xe in missionNodes) {
			//根据id找到对应的关卡
			if (xe.GetAttribute("id") == id.ToString()) {
				//根据unlock重新为关卡赋值
				if (unlock) {
					xe.SetAttribute("unlock", "1");
				}
				else {
					xe.SetAttribute("unlock", "0");
				}

				xe.SetAttribute("score", score.ToString());

			}
		}

		//保存文件
		xmlDoc.Save(filePath);
	}

	/// <summary>
	/// 完成关卡
	/// </summary>
	/// <param name="id"></param>
	/// <param name="score">分数</param>
	/// <param name="unlockNext">是否解锁下一关</param>
	public void CompleteMission(int id, int score,bool unlockNext=true) {
		SetMission(id,true,score);	//更新本关卡信息
		if (unlockNext) {
			SetMission(id+1,true,0);	//更新本关卡信息
		}
	}

}
