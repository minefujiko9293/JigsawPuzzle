using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private static GameManager _instance = null;
	public static GameManager Instance {
		get {
			return _instance;
		}
	}

	public GameObject blockPrefab;
	public GridLayoutGroup gamePanel;

	public int[] CurrentIndex;
	public GameObject[] blockCollections;

	public bool isHold = false;
	public GameObject HoldingObject = null;

	public int StepCount = 0;


	// Use this for initialization
	void Start () {
		_instance = this;

		StepCount = 0;

		int side=DataManager.Instance.Current_Level+4;
		float spacing = 5 * (2 + side - 1);
		float cellSize = (500 - spacing) / side;

		gamePanel.cellSize = new Vector2(cellSize, cellSize);
		gamePanel.constraintCount = side;

		int length=side*side;
		CurrentIndex = new int[length];
		blockCollections = new GameObject[length];
		for (int i = 0; i < length; i++) {
			CurrentIndex[i] = i;

			GameObject newBlock = Instantiate(blockPrefab, gamePanel.transform,false) as GameObject;
			newBlock.name = i.ToString();
			blockCollections[i] = newBlock;

		}
		CurrentIndex = RandArray(CurrentIndex);

		string spriteName = "Images/missions/mission" + DataManager.Instance.Current_Level + "_" + DataManager.Instance.Current_Mission;
		//Debug.Log(spriteName);
		var spriteBlocks = Resources.LoadAll<Sprite>(spriteName);
		MatchSprite(blockCollections, spriteBlocks, CurrentIndex);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	int[] RandArray(int[] arr) {
		int[] newarr = new int[arr.Length];
		int k = arr.Length;
		for (int i = 0; i < arr.Length; i++) {
			int temp =  Random.Range(0, k);
			newarr[i] = arr[temp];
			//arr[temp]后面的数向前移一位  
			for (int j = temp; j < arr.Length - 1; j++) {
				arr[j] = arr[j + 1];
			}
			k--;
		}
		return newarr;
	}

	void MatchSprite(GameObject[] blockCollections, Sprite[] spriteBlocks, int[] indexs) {
		int length = indexs.Length;
		for (int i = 0; i < length; i++) {
			var image = blockCollections[i].GetComponent<Image>();
			image.sprite = spriteBlocks[indexs[i]];
		}
	}

	public void SwitchBlock(int block1, int block2) {
		int a = CurrentIndex[block1];
		CurrentIndex[block1] = CurrentIndex[block2];
		CurrentIndex[block2] = a;
		Debug.Log("Switch Real Index: " + a + " And " + CurrentIndex[block1]);

		var b=blockCollections[block1].GetComponent<Image>().sprite;
		blockCollections[block1].GetComponent<Image>().sprite = blockCollections[block2].GetComponent<Image>().sprite;
		blockCollections[block2].GetComponent<Image>().sprite = b;

		StepCount++;

	}

	void OnDestroy() {
		_instance = null;
	}
}
