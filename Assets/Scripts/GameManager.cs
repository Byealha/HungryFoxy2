using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private float cherryAlertTimer = 0f;

    public AudioMixer bgmMixer;
    public AudioMixer sfxMixer;

    public FadeIn fadeIn;
    public FadeIn fadeIn2;
    public PauseManager pauseManager;
    public PlayerMove playerMove;
    public SoundManager soundManager;
    public StageManager stageManager;
    public TextDelay textDelay;

    public GameObject playerHeart1;
    public GameObject playerHeart2;
    public GameObject playerHeart3;
    public GameObject StageClear;
    public GameObject StageFail;
    public GameObject Buttons;
    public GameObject Explain;
    public GameObject Player;
    public GameObject CherryExplainObj;
    public GameObject blackScreen;
    public GameObject BlackScreen2;

    public Text cherryExplain;
    public Text stageExplain;

    public int cherryCount = 0;
    public int cherryEat = 0;
    public int playerHP = 3;

    public bool playerCantMove = false;

    private void Start() {
        bgmMixer.SetFloat("BGM", PlayerPrefs.GetFloat("bgmSound", 0));
        sfxMixer.SetFloat("SFX", PlayerPrefs.GetFloat("sfxSound", 0));
    }

    private void Update() {
        if (cherryAlertTimer > 0) {
            cherryAlertTimer -= Time.deltaTime;
            if (cherryAlertTimer <= 0) {
                cherryAlertTimer = 0;
            }
        }
    }

    private void CherryObjClose() {
        CherryExplainObj.SetActive(false);
    }

    public void CherryCheck() {
        if (cherryEat == 0) {
            return;
        }
        else if (cherryCount > cherryEat && cherryAlertTimer == 0) {
            CherryExplainObj.SetActive(true);
            cherryExplain.text = "아직 집에 갈 수는 없어! 체리가 " + (cherryCount - cherryEat) + "개 부족해!";
            cherryAlertTimer = 3f;
            textDelay.delay = 0.05f;
            textDelay.TextStart();
            Invoke("CherryObjClose", 3);
        }
        else if (cherryCount == cherryEat) {
            Debug.Log("다음맵 이동");
            playerMove.StageClear();
            playerCantMove = true;
            stageManager.PlaySound("Clear");
            StageClear.SetActive(true);
            StartCoroutine(MapClear());
        }
    }

    public void ScanHealth() {
        if(playerHP == 3) {
            playerHeart1.SetActive(true);
            playerHeart2.SetActive(true);
            playerHeart3.SetActive(true);
        }
        else if (playerHP == 2) {
            playerHeart1.SetActive(true);
            playerHeart2.SetActive(true);
            playerHeart3.SetActive(false);
        }
        else if (playerHP == 1) {
            playerHeart1.SetActive(true);
            playerHeart2.SetActive(false);
            playerHeart3.SetActive(false);
        }
        else if (playerHP == 0) {
            playerHeart1.SetActive(false);
            playerHeart2.SetActive(false);
            playerHeart3.SetActive(false);
            StartCoroutine(ShowBlackScreen());
        }
    }

    IEnumerator MapClear() {
        soundManager.BgmChangeClear();
        yield return new WaitForSeconds(0.8f);
        stageExplain.text = "정말 멋져요!";
        Buttons.SetActive(true);
    }

    IEnumerator ShowBlackScreen() {
        yield return new WaitForSeconds(0.8f);
        blackScreen.SetActive(true);
        fadeIn.ShowBlackScreen();
        yield return new WaitForSeconds(1f);
        StageFail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        stageExplain.text = "다음엔 더 잘할 수 있을거에요!";
        Buttons.SetActive(true);
    }

    public void CloseExplain() {
        Explain.SetActive(false);
        playerCantMove = false;
        pauseManager.cantPause = false;
    }

    IEnumerator DelayLoadScene(string stage) {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(stage);
    }

    public void Restart() {
        BlackScreen2.SetActive(true);
        fadeIn2.ShowBlackScreen();
        StartCoroutine(DelayLoadScene(SceneManager.GetActiveScene().name));
    }

    public void Quit() {
        BlackScreen2.SetActive(true);
        fadeIn2.ShowBlackScreen();
        StartCoroutine(DelayLoadScene("SelectLevel"));
    }
}
