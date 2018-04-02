using UnityEngine;
using System.Collections;

public class SingletonSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {

		var missions = DataManager.Instance.LoadMissions();
	}
	
}
