using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour {

    public GameObject tipImage;
    public TimerController timerController;

    public void SetTipImage(Sprite sprite) {
        tipImage.GetComponent<Image>().sprite = sprite;
    }

    public void StartTip() {
        tipImage.SetActive( true);
        timerController.StartTimerWork();
    }
}
