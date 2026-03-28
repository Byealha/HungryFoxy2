using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StageControl : MonoBehaviour {
    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    
    private bool playerMoveToggle = true;

    public AudioMixer bgmMixer;
    public AudioMixer sfxMixer;

    public FadeIn fadeIn;

    public GameObject BlackScreen;
    public GameObject stage1Info;
    public GameObject stage2Info;

    public int stage = 1;
    public int maxStage = 2;

    public float moveSpeed = 4f;

    public bool portalReady = false;
    public bool selectLevel = false;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start() {
        bgmMixer.SetFloat("BGM", PlayerPrefs.GetFloat("bgmSound", 0));
        sfxMixer.SetFloat("SFX", PlayerPrefs.GetFloat("sfxSound", 0));
    }

    private void Update() {
        //오른쪽 키로 다음 포탈까지 이동
        if (Input.GetKeyDown(KeyCode.RightArrow) && !playerMoveToggle && !selectLevel) {
            if (stage < 2) {
                AllInfoClose();
                portalReady = false;
                spriteRenderer.flipX = false;
                playerMoveToggle = true;
                stage++;
            }
        }
        //왼쪽 키로 이전 포탈까지 이동
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !playerMoveToggle && !selectLevel) {
            if (stage > 1) {
                AllInfoClose();
                portalReady = false;
                spriteRenderer.flipX = true;
                playerMoveToggle = true;
                stage--;
            }
        }
    }

    private void FixedUpdate() {
        //playerMoveToggle이 활성화되면 이동로직 활성화
        if (playerMoveToggle) {
            anim.SetBool("isWalk", true);
            if (spriteRenderer.flipX)
                rigid.AddForce(Vector2.left, ForceMode2D.Impulse);
            else
                rigid.AddForce(Vector2.right, ForceMode2D.Impulse);

            if (rigid.linearVelocity.x > moveSpeed)
                rigid.linearVelocity = new Vector2(moveSpeed, rigid.linearVelocity.y);
            else if (rigid.linearVelocity.x < moveSpeed * (-1))
                rigid.linearVelocity = new Vector2(moveSpeed * (-1), rigid.linearVelocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Portal태그와 충돌하면 playerMoveToggle 비활성화
        if (collision.gameObject.CompareTag("Portal")) {
            playerMoveToggle = false;
            portalReady = true;
            anim.SetBool("isWalk", false);
            Portal portal = collision.gameObject.GetComponent<Portal>();
            if(portal.stagePortal == 1) {
                stage1Info.SetActive(true);
            }
            else if (portal.stagePortal == 2) {
                stage2Info.SetActive(true);
            }
        }
    }

    private void AllInfoClose() {
        stage1Info.SetActive(false);
        stage2Info.SetActive(false);
    }

    public void GameQuit() {
        Application.Quit();
    }

    public void GotoTitle() {
        BlackScreen.SetActive(true);
        fadeIn.ShowBlackScreen();
        StartCoroutine(GotoTitleScene());
    }

    IEnumerator GotoTitleScene() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Title");
    }
}
