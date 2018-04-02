using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionEventHandler : MonoBehaviour {
    private EventTrigger eventTrigger;
    private MissionDataHandler missionDataHandler;

    public Button btn_ok;

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
    /// 点击相应逻辑
    /// </summary>
    /// <param name="str"></param>
    void ReceiveClickMessage(string name) {
        if (name==gameObject.name) {
            Debug.Log(gameObject.name);

            if (missionDataHandler.IsUnlock) {
                btn_ok.interactable = true;
            }
            else {
                btn_ok.interactable = false;
            }
        }

        
    }

    void OnPointerDownDelegate(PointerEventData data) {
        if (OnClickMessage != null) {
            OnClickMessage(gameObject.name);
        }
    }

	// Use this for initialization
	void Start () {
        //注册点击事件处理函数
        MissionEventHandler.OnClickMessage += ReceiveClickMessage;

        //动态添加点击触发器
        eventTrigger = gameObject.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        eventTrigger.triggers.Add(entry);

        missionDataHandler = gameObject.GetComponent<MissionDataHandler>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy() {
        //解除点击事件处理函数
        MissionEventHandler.OnClickMessage -= ReceiveClickMessage;

    }
}
