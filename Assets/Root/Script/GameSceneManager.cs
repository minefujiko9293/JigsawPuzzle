using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour {

    public string menuSceneName;
    public string levelSceneName;
    public string mainSceneName;


    public void GoStartScene() {
        SceneManager.LoadScene(menuSceneName);

    }
    public void GoMainScene() {
        SceneManager.LoadScene(mainSceneName);
    }

    public void GoLevelScene() {
        SceneManager.LoadScene(levelSceneName);
    }

    public void ExitGame() {
        //If we are running in a standalone build of the game
#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
