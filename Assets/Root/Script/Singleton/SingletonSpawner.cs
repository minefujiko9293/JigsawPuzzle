using UnityEngine;
using System.Collections;

public class SingletonSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DataManager dataManager = DataManager.GetInstance();

        var level=dataManager.LoadMissions();
	}
	
}
