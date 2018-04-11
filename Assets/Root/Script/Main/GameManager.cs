using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : MonoBehaviour {

	#region 暴露Instance便于引用
	private static GameManager _instance = null;
	public static GameManager Instance {
		get {
			return _instance;
		}
	}
	#endregion

	/// <summary>
	/// 游戏场景管理器
	/// </summary>
	public GameSceneManager gameSceneManager;

	/// <summary>
	/// 开始UI
	/// </summary>
	public GameObject StartUI;

	/// <summary>
	/// 主游戏UI
	/// </summary>
	public GameObject MainUI;

	/// <summary>
	/// 结束UI
	/// </summary>
	public GameObject EndUI;

	/// <summary>
	/// 步数计数文本
	/// </summary>
	public Text StepCounterText;

	/// <summary>
	/// 拼图块预制体
	/// </summary>
	public GameObject blockPrefab;

	/// <summary>
	/// 拼图GridLayoutGroup
	/// </summary>
	public GridLayoutGroup gamePanel;

	/// <summary>
	/// 拼图Panel Size
	/// </summary>
	public float gamePanelSize;

	/// <summary>
	/// 拼图块Index映射数组
	/// </summary>
	public int[] CurrentIndex;

	/// <summary>
	/// 拼图块UI集合
	/// </summary>
	public GameObject[] blockCollections;

	/// <summary>
	/// 是否已选中拼图块
	/// </summary>
	public bool isHold = false;

	/// <summary>
	/// 选中的拼图块引用
	/// </summary>
	public GameObject HoldingObject = null;

	/// <summary>
	/// 统计步数
	/// </summary>
	public int StepCount = 0;


	// Use this for initialization
	void Start() {

		_instance = this;   //设置GameManager Instance引用

		StartUI.SetActive(true);    //显示开始UI
		MainUI.SetActive(false);    //隐藏游戏主UI
		EndUI.SetActive(false);     //隐藏结束UI

		initTip();  //初始化提示图片
		initGame(); //初始化主游戏逻辑
	}


	/// <summary>
	/// 初始化提示图片
	/// </summary>
	void initTip() {
		//设置关卡对应的拼图原图
		string spriteName = "Images/missions/mission_tip" + DataManager.Instance.Current_Mission;
		//加载图片
		var sprite = Resources.Load<Sprite>(spriteName);
		//设置提示图片
		StartUI.GetComponent<StartUIController>().SetTipImage(sprite);
	}

	/// <summary>
	/// 开始游戏
	/// </summary>
	public void StartGame() {
		StartUI.SetActive(false);   //隐藏开始UI
		MainUI.SetActive(true);     //显示主游戏UI
	}

	/// <summary>
	/// 初始化主游戏逻辑
	/// </summary>
	void initGame() {
		StepCount = 0;  //初始化步数统计


		int side = DataManager.Instance.Current_Level + 4;        //计算拼图分割数
		float spacing = 5 * (2 + side - 1);        //计算拼图块总间隔

		//计算拼图块单块Size=（拼图面板边长-拼图块总间隔）/分割数
		float cellSize = (gamePanelSize - spacing) / side;

		gamePanel.cellSize = new Vector2(cellSize, cellSize);   //设置gamePanel的块Size，即拼图块的size
		gamePanel.constraintCount = side;   //设置gamePanel的换行步长为拼图分割数

		int length = side * side;   //计算总拼图块块数
		CurrentIndex = new int[length]; //创建总块数等长的int型数组，用于映射拼图块序列
		blockCollections = new GameObject[length];  //创建总块数等长的GameObject数组,储存拼图块UI引用
		for (int i = 0; i < length; i++) {
			CurrentIndex[i] = i; //赋值拼图映射

			//实例化拼图块预制体
			GameObject newBlock = Instantiate(blockPrefab, gamePanel.transform, false) as GameObject;
			newBlock.name = i.ToString();   //命名拼图块
			blockCollections[i] = newBlock; //储存拼图块UI对象引用

		}
		CurrentIndex = RandArray(CurrentIndex); //打乱映射数组

		//设置关卡拼图块的图集
		string spriteName = "Images/missions/mission" + DataManager.Instance.Current_Mission;
		//Debug.Log(spriteName);
		//加载对应的图集
		var spriteBlocks = Resources.LoadAll<Sprite>(spriteName);
		//根据映射数组匹配拼图块显示内容
		MatchSprite(blockCollections, spriteBlocks, CurrentIndex);
	}

	/// <summary>
	/// 打乱数组函数
	/// </summary>
	/// <param name="arr"></param>
	/// <returns></returns>
	int[] RandArray(int[] arr) {
		int[] newarr = new int[arr.Length];
		int k = arr.Length;
		for (int i = 0; i < arr.Length; i++) {
			int temp = Random.Range(0, k);
			newarr[i] = arr[temp];
			//arr[temp]后面的数向前移一位  
			for (int j = temp; j < arr.Length - 1; j++) {
				arr[j] = arr[j + 1];
			}
			k--;
		}
		return newarr;
	}

	/// <summary>
	/// 匹配拼图块显示精灵
	/// </summary>
	/// <param name="blockCollections">拼图块GameObject集合</param>
	/// <param name="spriteBlocks">图集</param>
	/// <param name="indexs">映射数组</param>
	void MatchSprite(GameObject[] blockCollections, Sprite[] spriteBlocks, int[] indexs) {
		int length = indexs.Length;
		for (int i = 0; i < length; i++) {
			var image = blockCollections[i].GetComponent<Image>();
			image.sprite = spriteBlocks[indexs[i]];
		}
	}

	/// <summary>
	/// 交换拼图
	/// </summary>
	/// <param name="block1">拼图块1的序号</param>
	/// <param name="block2">拼图块2的序号</param>
	public void SwitchBlock(int block1, int block2) {
		#region 交换映射数据
		int a = CurrentIndex[block1];   //暂存拼图块1序号对应的映射数据
		CurrentIndex[block1] = CurrentIndex[block2];    //设置拼图块1序号对应的映射数据为块2对应的映射数据
		CurrentIndex[block2] = a;   //把暂存的映射数据赋值给块2对应的映射数据
		Debug.Log("Switch Real Index: " + a + " And " + CurrentIndex[block1]);
		#endregion

		#region 交换拼图块显示的图片精灵
		var b = blockCollections[block1].GetComponent<Image>().sprite;
		blockCollections[block1].GetComponent<Image>().sprite = blockCollections[block2].GetComponent<Image>().sprite;
		blockCollections[block2].GetComponent<Image>().sprite = b;
		#endregion

		StepCount++;    //累加步数
		StepCounterText.text = "已用步数：" + StepCount; //更新步数统计显示文字

		if (CheckComplete()) {  //判断拼图是否完成
			GameComplete(); //执行拼图完成函数
		}

	}

	/// <summary>
	/// 检查拼图是否完成
	/// </summary>
	/// <returns></returns>
	public bool CheckComplete() {
		bool isComplete = true;
		var length = CurrentIndex.Length;
		for (int i = 0; i < length; i++) {
			if (i != CurrentIndex[i]) {   //根据映射数组元素是否有序连贯来判断是否完成拼图
				isComplete = false;
				break;
			}
		}
		return isComplete;
	}

	/// <summary>
	/// 拼图完成函数
	/// </summary>
	void GameComplete() {
		MainUI.SetActive(false);    //隐藏游戏主UI
		EndUI.SetActive(true);  //显示结束UI

		var level = DataManager.Instance.Current_Level; //读取当前关卡难度
		int threeStart = (level + 4) * (level + 4);     //计算3星得分步数线
		int twoStart = (level + 4) * (level + 4 + 2);   //计算2星得分步数线
		int oneStart = (level + 4) * (level + 4 + 4);   //计算1星得分步数线
		int start = 1;

		if (StepCount <= threeStart) {  //根据步数设置得分
			start = 3;
		}
		else if (StepCount <= twoStart) {
			start = 2;
		}
		else {
			start = 1;
		}

		//通过数据管理器设置关卡完成，设置得分并解锁下一关卡
		DataManager.Instance.CompleteMission(DataManager.Instance.Current_Mission, start);

		//设置结束UI的分数，步数显示
		EndUI.GetComponent<EndUIController>().SetScore(StepCount, start);
	}

	/// <summary>
	/// 进入下一关卡
	/// </summary>
	public void NextMission() {
		//设置数据管理器当前关卡为下一关
		DataManager.Instance.Current_Mission++;

		#region 根据关卡ID设置数据管理器的当前难度
		if (DataManager.Instance.Current_Mission < 10) {
			DataManager.Instance.Current_Level = 0;
		}
		else if (DataManager.Instance.Current_Mission < 20) {
			DataManager.Instance.Current_Level = 1;
		}
		else {
			DataManager.Instance.Current_Level = 2;
		}
		#endregion

		//跳转新的游戏场景
		gameSceneManager.GoMainScene();
	}

	/// <summary>
	/// 销毁时调用
	/// </summary>
	void OnDestroy() {
		_instance = null;
	}
}
