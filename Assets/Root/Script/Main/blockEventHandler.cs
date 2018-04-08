using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Image))] //确保gameObject拥有Image组件
[RequireComponent(typeof(EventTrigger))] //确保gameObject拥有EventTrigger组件

/// <summary>
/// 拼图块事件处理器
/// </summary>
public class blockEventHandler : MonoBehaviour {

    /// <summary>
    /// 拼图块的Image组件
    /// </summary>
	Image image;

    /// <summary>
    /// 拼图块的EventTrigger组件
    /// </summary>
	EventTrigger eventTrigger;

	// Use this for initialization
	void Start () {
        //获取Image组件
		image = gameObject.GetComponent<Image>();

        //获取EventTrigger组件
		eventTrigger = gameObject.GetComponent<EventTrigger>();

        //实例化Entry类型触发器
		EventTrigger.Entry entry = new EventTrigger.Entry();

        //设置监听事件类型为点击
		entry.eventID = EventTriggerType.PointerClick;

        //使用匿名函数为entry触发器添加监听回调函数OnPointerDownDelegate
		entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });

        //把entry触发器添加到 EventTrigger组件上，动态为该gameobject添加点击事件监听
        eventTrigger.triggers.Add(entry);
	}

    /// <summary>
    /// 拼图块的点击响应逻辑
    /// </summary>
    /// <param name="data"></param>
	void OnPointerDownDelegate(PointerEventData data) {
		//to do onclick
		if (!GameManager.Instance.isHold) { //判断游戏管理器中 没有拼图块处于选中状态
			GameManager.Instance.isHold = true; //设置拼图选中标识为真
			GameManager.Instance.HoldingObject = gameObject;    //保存选中拼图块的引用
			Debug.Log("Pickup Block: " +gameObject.name);

			image.color = Color.green;  //通过设置颜色表示拼图块被选中
		}
		else {  //已有拼图块选中

            //取消选中拼图块的提示颜色
			GameManager.Instance.HoldingObject.GetComponent<Image>().color = Color.white;

			if (GameManager.Instance.HoldingObject == gameObject) {//判断已选中拼图块是不是自己
				Debug.Log("Putdown: Block" + gameObject.name);

			}
			else {  //已选中的拼图块不是自己
				int a = int.Parse(GameManager.Instance.HoldingObject.name);
				int b = int.Parse(gameObject.name);
				Debug.Log("Switch Block: " + a + " and " + b);
				GameManager.Instance.SwitchBlock(a,b);  //交换拼图块
				
			}

			GameManager.Instance.isHold = false;    //取消选择拼图块标识
			GameManager.Instance.HoldingObject = null;  //设置选中块为空
		}
	}
}
