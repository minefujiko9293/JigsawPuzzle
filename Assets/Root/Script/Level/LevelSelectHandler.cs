using UnityEngine;
using System.Collections;

public class LevelSelectHandler : MonoBehaviour {

	private RectTransform rectTransform;
	private GameObject[] missionCollections;

	public void onMissionBack() {
		t = 0f;
		direction = 0;
		isMove = true;

		DataManager.Instance.Current_Mission=0;

		for (int i = 0; i < 10; i++) {
			var temp = missionCollections[i].GetComponent<RectTransform>();
			temp.localScale = Vector3.one;
		}

	}

	public void onLevelSelected() {
		t = 0f;
		direction = 1;

		var missionData = DataManager.Instance.LoadMissions();

		int current_level = DataManager.Instance.Current_Level;

		for (int i = 0; i < 10; i++) {
			var missionDataHandler = missionCollections[i].GetComponent<MissionDataHandler>();
			missionDataHandler.DataBind(missionData[10 * current_level+i]);
		}

		isMove = true;
	}

	bool isMove = false;
	float t = 0f;
	int direction = -1;
	Vector3 targetPosition;

	void Update() {
		if (isMove) {
			if (t < 1) {
				rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, direction * targetPosition, t);
				t += Time.deltaTime;
			}
			else {
				isMove = false;
			}
		}
	}

	void Start() {
		rectTransform = gameObject.GetComponent<RectTransform>();
		targetPosition = new Vector3(-1024, 0, 0);

		missionCollections = new GameObject[10];
		for (int i = 0; i < 10; i++) {
			string itemName = "MissionItem" + i.ToString();
			missionCollections[i] = GameObject.Find(itemName);
		}
	}
}
