using UnityEngine;
using System.Collections;

public class TimerController : MonoBehaviour {

    public RectTransform backgroundRectTransform;
    public RectTransform timerRectTransform;
    public float time=10;

    public Vector3 init_scale;
    void Start() {

        init_scale = backgroundRectTransform.localScale;
        Init();
    }


    void Init() {
        timerRectTransform.localScale = init_scale;
    }

    public void StartTimerWork() {
        StartCoroutine(DoTimerWork());

    }

    IEnumerator DoTimerWork() {
        float t = time;
        while (t>0) {

            float temp_scaleX = t / time;
            Debug.Log(temp_scaleX);
            timerRectTransform.localScale=new Vector3(temp_scaleX,init_scale.y,init_scale.z);

            t -= Time.deltaTime;

            yield return null;
        }

        GameManager.Instance.StartGame();
    }
}
