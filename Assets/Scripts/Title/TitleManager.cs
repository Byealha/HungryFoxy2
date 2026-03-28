using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    public GameObject AlertWindow;
    public GameObject blackScreen;
    public GameObject optionScreen;
    public GameObject title;

    private void Start() {
        PlayerPrefs.SetInt("Stage", 0);
        optionScreen.SetActive(false);
    }

    public void GameStart() {
        blackScreen.SetActive(true);
        blackScreen.GetComponent<FadeIn>().StartGame();
        Invoke("GotoLoading", 1.7f);
    }

    public void GameOptionOpen() {
        title.SetActive(false);
        optionScreen.SetActive(true);
    }

    public void GameOptionClose() {
        title.SetActive(true);
        optionScreen.SetActive(false);
    }

    public void GameQuit() {
        Application.Quit();
    }

    private void GotoLoading() {
        SceneManager.LoadScene("Loading");
    }

    public void AlertClose() {
        AlertWindow.SetActive(false);
    }
}
