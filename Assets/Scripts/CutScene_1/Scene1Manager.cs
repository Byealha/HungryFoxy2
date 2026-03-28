using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene1Manager : MonoBehaviour {
    private AudioSource audioSource;

    private int scenario = 1;

    private bool skipTab = false; //대화 스킵 판정

    public AudioClip itemEat;
    public AudioMixer bgmMixer;
    public AudioMixer sfxMixer;

    public FadeIn fadeIn;
    public Scene1PlayerMove player;
    public Scene1RabbitMove rabbit;
    public TextDelay textDelay;

    public GameObject BlackScreen;
    public GameObject infoText;
    public GameObject textBox;
    public Text playerText;

    public bool nextTab = false; //다음 대화로 넘어가는 판정

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        bgmMixer.SetFloat("BGM", PlayerPrefs.GetFloat("bgmSound", 0));
        sfxMixer.SetFloat("SFX", PlayerPrefs.GetFloat("sfxSound", 0));

        textBox.SetActive(false);
        //플레이어가 나무로 이동하는 시나리오
        player.PlayerMoveToggle();
        //1초 뒤 플레이어가 정지하는 시나리오 & 대화 스크립트
        StartCoroutine(Scenario1());
        //다시 캐릭터가 움직이다 1초 뒤 화면 밖에 있던 토끼가 점프하며 체리를 먹고 감
        //체리를 먹자마자 캐릭터의 움직임 정지 & x-flip 활성화를 통해 도망가는 토끼를 확인
    }

    private void Update() {
        //글자 출력이 덜 됐을 때 입력시 남은 글자를 딜레이 없이 출력
        if (skipTab) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) {
                textDelay.delay = 0.001f;
                skipTab = false;
                StartCoroutine(nextTabToggle());
            }
        }

        //글자 출력이 끝났을 때 다음 시나리오들
        if (nextTab && scenario == 2) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) {
                textBox.SetActive(false);
                textDelay.delay = 0.125f;
                StartCoroutine(Scenario2());
                nextTab = false;
            }
        }

        if (nextTab && scenario == 3) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) {
                textBox.SetActive(false);
                textDelay.delay = 0.125f;
                StartCoroutine(Scenario3());
                nextTab = false;
            }
        }
    }
    
    //글자 스킵 후 다음 대화로 넘어가기 위한 딜레이
    IEnumerator nextTabToggle() {
        yield return new WaitForSeconds(0.2f);
        nextTab = true;
        textDelay.delay = 0.125f;
        scenario++;
    }

    //시나리오 1
    IEnumerator Scenario1() {
        yield return new WaitForSeconds(1f);
        player.PlayerMoveToggle();
        textBox.SetActive(true);
        textDelay.TextStart();

        yield return new WaitForSeconds(0.2f);
        skipTab = true;
        yield return new WaitForSeconds(2f);
        //글자를 스킵하지 않고 끝까지 봤을 때
        if (scenario != 2) {
            skipTab = false;
            nextTab = true;
            scenario++;
        }
    }

    //시나리오 2
    IEnumerator Scenario2() {
        yield return new WaitForSeconds(1f);
        player.PlayerMoveToggle();

        yield return new WaitForSeconds(0.5f);
        rabbit.RabbitJumpStart();

        yield return new WaitForSeconds(2.5f);
        player.PlayerMoveToggle();

        yield return new WaitForSeconds(0.3f);
        player.PlayerFlipToggle();

        yield return new WaitForSeconds(1f);
        player.PlayerMoveToggle();
        textBox.SetActive(true);
        playerText.text = "ㄱ...거기서..!!! \n 내가 먼저 찜해 뒀단 말이야!!";
        textDelay.TextStart();
        textDelay.delay = 0.3f;

        yield return new WaitForSeconds(0.2f);
        textDelay.delay = 0.1f;

        yield return new WaitForSeconds(0.7f);
        textDelay.delay = 0.03f;
        skipTab = true;

        yield return new WaitForSeconds(1.7f);
        //글자를 스킵하지 않고 끝까지 봤을 때
        if (scenario != 3) {
            skipTab = false;
            nextTab = true;
            scenario++;
        }
    }

    //시나리오 3이라기 보단 다음 씬으로 넘어가기 위한 트리거
    IEnumerator Scenario3() {
        BlackScreen.SetActive(true);
        fadeIn.ShowBlackScreen();
        yield return new WaitForSeconds(1.5f);
        infoText.SetActive(false);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SelectLevel");
    }

    //사운드 관리
    public void PlaySound(string sound) {
        switch (sound) {
            case "itemEat":
                audioSource.clip = itemEat;
                break;
        }
        audioSource.Play();
    }
}
