using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
    private float inputDelay = 1f;

    private bool openPause = false;

    public FadeIn fadeIn;
    public RunningManager runningManager;

    public GameObject BlackScreen;
    public GameObject Border;
    public GameObject PauseWindow;
    public GameObject RestartWindow;
    public GameObject SettingWindow;
    public GameObject QuitWindow;

    public bool cantPause = true;
    public bool runningMode = false;

    private void Update() {
        if (inputDelay > 0) {
            inputDelay -= Time.deltaTime;
            if (inputDelay <= 0) {
                inputDelay = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !openPause && inputDelay == 0 && !cantPause) {
            PauseWindow.SetActive(true);
            openPause = true;
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && openPause) {
            if (!runningMode) {
                Border.SetActive(false);
                SettingWindow.SetActive(false);
                PauseWindow.SetActive(false);
                openPause = false;
                inputDelay = 1f;
                Time.timeScale = 1f;
            }
            else {
                Border.SetActive(false);
                SettingWindow.SetActive(false);
                PauseWindow.SetActive(false);
                openPause = false;
                inputDelay = 1f;
                runningManager.StartRestart();
                runningManager.startCountDown = 3f;
                StartCoroutine(RunningOffFreeze());
            }
        }
    }

    IEnumerator RunningOffFreeze() {
        yield return new WaitForSeconds(runningManager.startCountDown);
        Time.timeScale = 1f;
    }

    public void GameResume() {
        if (!runningMode) {
            PauseWindow.SetActive(false);
            openPause = false;
            inputDelay = 1f;
            Time.timeScale = 1f;
        }
        else {
            PauseWindow.SetActive(false);
            openPause = false;
            inputDelay = 1f;
            runningManager.StartRestart();
            runningManager.startCountDown = 3f;
            StartCoroutine(RunningOffFreeze());
        }
    }

    public void GameRestartCheck() {
        RestartWindow.SetActive(true);
        Border.SetActive(true);
    }

    public void GameRestartDeny() {
        RestartWindow.SetActive(false);
        Border.SetActive(false);
    }

    public void OnGameSetting() {
        SettingWindow.SetActive(true);
        Border.SetActive(true);
    }

    public void OffGameSetting() {
        SettingWindow.SetActive(false);
        Border.SetActive(false);
    }

    public void GameQuitCheck() {
        QuitWindow.SetActive(true);
        Border.SetActive(true);
    }

    public void GameQuitDeny() {
        QuitWindow.SetActive(false);
        Border.SetActive(false);
    }

    public void GameRestartAccept() {
        Time.timeScale = 1f;
        BlackScreen.SetActive(true);
        fadeIn.ShowBlackScreen();
        StartCoroutine(DelayLoadScene(SceneManager.GetActiveScene().name));
    }

    public void GameQuitAccept() {
        Time.timeScale = 1f;
        BlackScreen.SetActive(true);
        fadeIn.ShowBlackScreen();
        StartCoroutine(DelayLoadScene("SelectLevel"));
    }

    IEnumerator DelayLoadScene(string stage) {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(stage);
    }
}
