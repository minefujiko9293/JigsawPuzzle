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

	public GameSceneManager gameSceneManager;

    public GameObject StartUI;
    public GameObject MainUI;
    public GameObject EndUI;

	public Text StepCounterText;

	public GameObject blockPrefab;
	public GridLayoutGroup gamePanel;
	public float gamePanelSize;

	public int[] CurrentIndex;
	public GameObject[] blockCollections;

	public bool isHold = false;
	public GameObject HoldingObject = null;

	public int StepCount = 0;


	// Use this for initialization
	void Start () {
		_instance = this;

		StartUI.SetActive(true);
		MainUI.SetActive(false);
		EndUI.SetActive(false);

        initTip();
        initGame();
	}

    void initTip() {
        string spriteName = "Images/missions/mission_tip" + DataManager.Instance.Current_Mission;
        var sprite= Resources.Load<Sprite>(spriteName);

        StartUI.GetComponent<StartUIController>().SetTipImage(sprite);
    }

    public void StartGame() {
        StartUI.SetActive(false);
        MainUI.SetActive(true);
    }

    void initGame() {
        StepCount = 0;

        int side = DataManager.Instance.Current_Level + 4;
        float spacing = 5 * (2 + side - 1);
		float cellSize = (gamePanelSize - spacing) / side;

        gamePanel.cellSize = new Vector2(cellSize, cellSize);
        gamePanel.constraintCount = side;

        int length = side * side;
        CurrentIndex = new int[length];
        blockCollections = new GameObject[length];
        for (int i = 0; i < length; i++) {
            CurrentIndex[i] = i;

            GameObject newBlock = Instantiate(blockPrefab, gamePanel.transform, false) as GameObject;
            newBlock.name = i.ToString();
            blockCollections[i] = newBlock;

        }
        CurrentIndex = RandArray(CurrentIndex);

        string spriteName = "Images/missions/mission" + DataManager.Instance.Current_Mission;
        //Debug.Log(spriteName);
        var spriteBlocks = Resources.LoadAll<Sprite>(spriteName);
        MatchSprite(blockCollections, spriteBlocks, CurrentIndex);
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
		StepCounterText.text = "已用步数：" + StepCount;

		if (CheckComplete()) {
			GameComplete();
		}

	}

    public bool CheckComplete() {
        bool isComplete = true;
        var length = CurrentIndex.Length;
        for (int i = 0; i < length; i++) {
            if (i!=CurrentIndex[i]) {
                isComplete = false;
                break;
            }
        }
        return isComplete;
    }

	void GameComplete() {
		MainUI.SetActive(false);
		EndUI.SetActive(true);

		var level = DataManager.Instance.Current_Level;
		int threeStart = (level + 4) * (level + 4);
		int twoStart = (level + 4) * (level + 4 + 2);
		int oneStart = (level + 4) * (level + 4 + 4);
		int start = 1;

		if (StepCount <= threeStart) {
			start = 3;
		}
		else if (StepCount <= twoStart) {
			start = 2;
		}
		else {
			start = 1;
		}
		DataManager.Instance.CompleteMission(DataManager.Instance.Current_Mission, start);

		EndUI.GetComponent<EndUIController>().SetScore(StepCount, start);
	}

	public void NextMission() {
		DataManager.Instance.Current_Mission++;
		if (DataManager.Instance.Current_Mission<10) {
			DataManager.Instance.Current_Level = 0;
		}
		else if (DataManager.Instance.Current_Mission < 20) {
			DataManager.Instance.Current_Level = 1;
		}
		else {
			DataManager.Instance.Current_Level = 2;
		}
		gameSceneManager.GoMainScene();
	}

	void OnDestroy() {
		_instance = null;
	}


    // Update is called once per frame
    void Update() {

    }
}
