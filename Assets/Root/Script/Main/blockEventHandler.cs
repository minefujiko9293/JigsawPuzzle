using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(EventTrigger))]

public class blockEventHandler : MonoBehaviour {

	Image image;
	EventTrigger eventTrigger;

	// Use this for initialization
	void Start () {
		image = gameObject.GetComponent<Image>();
		eventTrigger = gameObject.GetComponent<EventTrigger>();

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
		eventTrigger.triggers.Add(entry);
	}

	void OnPointerDownDelegate(PointerEventData data) {
		//to do onclick
		if (!GameManager.Instance.isHold) {
			GameManager.Instance.isHold = true;
			GameManager.Instance.HoldingObject = gameObject;
			Debug.Log("Pickup Block: " +gameObject.name);

			image.color = Color.green;
		}
		else {
			GameManager.Instance.HoldingObject.GetComponent<Image>().color = Color.white;
			if (GameManager.Instance.HoldingObject == gameObject) {
				Debug.Log("Putdown: Block" + gameObject.name);

			}
			else {
				int a = int.Parse(GameManager.Instance.HoldingObject.name);
				int b = int.Parse(gameObject.name);
				Debug.Log("Switch Block: " + a + " and " + b);
				GameManager.Instance.SwitchBlock(a,b);
				
			}

			GameManager.Instance.isHold = false;
			GameManager.Instance.HoldingObject = null;
		}
	}
}
