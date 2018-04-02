using UnityEngine;
using System.Collections;

public class LevelSelectHandler : MonoBehaviour {

	private RectTransform rectTransform;

	public void onMissionBack() {
		t = 0f;
		direction = 0;
		isMove = true;
	}

	public void onLevelSelected() {
		t = 0f;
		direction = 1;

		var missionData = DataManager.Instance.LoadMissions();
        switch (DataManager.Instance.Current_Level) {
            case 0:
                Debug.Log("0-9");
                break;
            case 1:
                Debug.Log("10-19");

                break;
            case 2:
                Debug.Log("20-29");

                break;
            default:
                break;
        }
        //to do bind data


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
	}
}
