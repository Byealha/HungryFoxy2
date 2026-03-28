using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunningManager : MonoBehaviour {
    private float previousScrollSpeed;
    private float previousBackgroundSpeed;
    private float previousMiddleSpeed;

    public AudioMixer bgmMixer;
    public AudioMixer sfxMixer;

    public Background background;
    public Background middle;
    public FadeIn fadeIn;
    public FadeIn fadeIn2;
    public PauseManager pauseManager;
    public RunningPlayerMove runningPlayerMove;
    public ScrollObject scrollObject;

    public GameObject Buttons;
    public GameObject blackScreen;
    public GameObject BlackScreen2;
    public GameObject CountDownObj;
    public GameObject Explain;
    public GameObject player;
    public GameObject playerHeart1;
    public GameObject playerHeart2;
    public GameObject playerHeart3;
    public GameObject StageFail;

    public Text CountDown;

    public int playerHP = 3;

    public float respawnY = 0f;
    public float startCountDown = 3f;

    public bool playerHurt = false;

    private void Start() {
        bgmMixer.SetFloat("BGM", PlayerPrefs.GetFloat("bgmSound", 0));
        sfxMixer.SetFloat("SFX", PlayerPrefs.GetFloat("sfxSound", 0));
        previousScrollSpeed = scrollObject.scrollSpeed;
        previousBackgroundSpeed = background.speed;
        previousMiddleSpeed = middle.speed;
        //////////////////////
        scrollObject.scrollSpeed = 0;
        background.speed = 0;
        middle.speed = 0;
        runningPlayerMove.isJumpState = true;
        runningPlayerMove.playerCantMove = true;
        player.GetComponent<Animator>().SetBool("isWalking", false);
    }

    public void CloseExplain() {
        Explain.SetActive(false);
        Invoke("StartRestart", 1f);
    }

    public void StartRestart() {
        StartCoroutine(RestartCountdown());
    }

    IEnumerator RestartCountdown() {
        CountDownObj.SetActive(true);
        float count = startCountDown;
        while (count > 0) {
            CountDown.text = "" + (int)count;
            yield return new WaitForSecondsRealtime(1f);
            count--;
        }
        if (count <= 0) {
            Time.timeScale = 1;
            CountDown.text = "Start!";
            runningPlayerMove.isJumpState = false;
            player.GetComponent<Animator>().SetBool("isWalking", true);
            runningPlayerMove.playerCantMove = false;
            scrollObject.scrollSpeed = 8;
            background.speed = -5;
            middle.speed = -7;
            pauseManager.cantPause = false;
            Invoke("TextClose", 1);
        }
    }

    private void TextClose() {
        CountDownObj.SetActive(false);
    }

    public void WaitGame() {
        scrollObject.scrollSpeed = 0;
        background.speed = 0;
        middle.speed = 0;
        playerHurt = true;

        if (playerHP > 0)
            StartCoroutine(FirstScene());
        else
            StartCoroutine(DieScene());
    }

    IEnumerator DieScene() {
        yield return new WaitForSeconds(1f);
        blackScreen.SetActive(true);
        fadeIn.ShowBlackScreen();
        yield return new WaitForSeconds(1.5f);
        StageFail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Buttons.SetActive(true);
    }

    IEnumerator FirstScene() {
        yield return new WaitForSeconds(1f);
        Debug.Log("첫번째 연출 시작");
        scrollObject.scrollSpeed = -8;
        background.speed = 0.5f;
        middle.speed = 0.7f;
    }

    public void SecondScene() {
        Debug.Log("두번째 연출 시착");
        scrollObject.scrollSpeed = previousScrollSpeed;
        background.speed = previousBackgroundSpeed;
        middle.speed = previousMiddleSpeed;

        runningPlayerMove.OffDamage();
        player.transform.position = new Vector3(-8, respawnY, 0);
        runningPlayerMove.playerCantMove = false;
    }

    public void ScanHealth() {
        if (playerHP == 3) {
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
        }
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

    IEnumerator DelayLoadScene(string stage) {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(stage);
    }
}
