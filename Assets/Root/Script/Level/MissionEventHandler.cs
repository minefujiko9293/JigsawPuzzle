using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 关卡UI 的事件处理器
/// </summary>
public class MissionEventHandler : MonoBehaviour {

	/// <summary>
	/// 事件触发器 用于动态绑定事件
	/// </summary>
    private EventTrigger eventTrigger;

	/// <summary>
	/// 关卡UI的数据处理器
	/// </summary>
    private MissionDataHandler missionDataHandler;

	/// <summary>
	/// 关卡UI的RectTransform
	/// </summary>
	private RectTransform rectTransform;

	/// <summary>
	/// 关卡选择确认按钮
	/// </summary>
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
		if (name == gameObject.name) {	//通过name判断当前接受事件的对象是否为目标对象
			Debug.Log(gameObject.name);

			if (missionDataHandler.IsUnlock) {	//如果点击中的关卡已经解锁
				btn_ok.interactable = true;		//确认按钮设置为可用
				rectTransform.localScale = new Vector3(1.5f, 1.5f, 1);	//设置关卡UI的缩放

				DataManager.Instance.Current_Mission = missionDataHandler.ID;	//
			}
			else {
				btn_ok.interactable = false;
			}
		}
		else {
			rectTransform.localScale = Vector3.one;
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
		rectTransform = gameObject.GetComponent<RectTransform>();
	}
	

    void OnDestroy() {
        //解除点击事件处理函数
        MissionEventHandler.OnClickMessage -= ReceiveClickMessage;
    }

	// Update is called once per frame
	void Update() {

	}
}
