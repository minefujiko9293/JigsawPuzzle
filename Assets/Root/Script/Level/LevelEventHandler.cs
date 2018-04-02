using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelEventHandler : MonoBehaviour {

    private RectTransform rectTransform;

    public Button btn_ok;

    /// <summary>
    /// 关卡名称
    /// </summary>
    public string levelName;

    /// <summary>
    /// 难度等级
    /// </summary>
    public int level=0;

    /// <summary>
    /// 点击消息处理委托
    /// </summary>
    /// <param name="str"></param>
    public delegate void OnClickEventHandler(string str);

    /// <summary>
    /// 点击事件
    /// </summary>
    public static event OnClickEventHandler OnClickMessage;

    /// <summary>
    /// 关卡UI被选择
    /// </summary>
    public void onClick() {
        btn_ok.interactable = true;

        if (OnClickMessage!=null) {
            OnClickMessage(levelName);
        }
    }

    /// <summary>
    /// 点击相应逻辑
    /// </summary>
    /// <param name="str"></param>
    void ReceiveClickMessage(string str) {
        if (str==levelName) {
            rectTransform.localScale = new Vector3(2, 2, 1);

            DataManager.Instance.Current_Level = level;
            Debug.Log("Current Level:"+DataManager.Instance.Current_Level);
        }
        else {
            rectTransform.localScale = new Vector3(1, 1,1);
        }
    }

    void Start() {

        rectTransform = gameObject.GetComponent<RectTransform>();

        //注册点击事件处理函数
        LevelEventHandler.OnClickMessage += ReceiveClickMessage;
    }

    void OnDestroy() {
        //解除点击事件处理函数
        LevelEventHandler.OnClickMessage -= ReceiveClickMessage;
    
    }
}
