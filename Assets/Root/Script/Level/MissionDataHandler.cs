using UnityEngine;
using System.Collections;

public class MissionDataHandler : MonoBehaviour {

	public int ID;
	public bool IsUnlock = false;
	public int Score = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DataBind(Mission m) {
		ID = m.ID;
		IsUnlock = m.UnLock;
		Score = m.Score;

	}
}
