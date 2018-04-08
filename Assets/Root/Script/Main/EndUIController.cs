using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 拼图完成UI控制器
/// </summary>
public class EndUIController : MonoBehaviour {

    /// <summary>
    /// 步数统计文字
    /// </summary>
	public Text StepText;

    /// <summary>
    /// 得分文字
    /// </summary>
	public Text StartText;

    /// <summary>
    /// 设置得分
    /// </summary>
    /// <param name="step"></param>
    /// <param name="start"></param>
	public void SetScore(int step,int start) {
		StepText.text = "总共用了" + step + "步完成了拼图，获得";

        //根据start设置得分文字
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
