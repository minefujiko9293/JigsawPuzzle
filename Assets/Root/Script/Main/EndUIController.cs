using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndUIController : MonoBehaviour {

	public Text StepText;
	public Text StartText;

	public void SetScore(int step,int start) {
		StepText.text = "总共用了" + step + "步完成了拼图，获得";

		if (start==3) {
			StartText.text = "★★★";
		}
		else if (start==2) {
			StartText.text = "★★☆";
		}
		else {
			StartText.text = "★☆☆";
		}
	}
}
