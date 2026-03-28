using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RunningPlayerMove : MonoBehaviour {
    private AudioSource audioSource;
    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    
    public AudioClip AudioDamage;
    public AudioClip AudioDie;
    public AudioClip AudioJump;

    public Background background;
    public Background middle;
    public RunningManager runningManager;
    public ScrollObject scrollObject;
    public SoundManager soundManager;
    public StageManager stageManager;

    public GameObject StageClear;
    public GameObject Buttons;

    public Text stageExplain;

    public float jumpChance = 0;
    public float jumpPower = 15f;

    public bool isJumpState = false;
    public bool playerCantMove = false;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        //점프
        if (Input.GetKeyDown(KeyCode.Space) && jumpChance < 2 && !anim.GetBool("isFalling") && !playerCantMove) {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJumpState = true;
            jumpChance++;
            anim.SetBool("isWalking", true);
            anim.SetBool("isJumping", false);
            Invoke("JumpAnim", 0.1f);
            PlaySound("Jump");
        }

        if (!isJumpState)
            anim.SetBool("isWalking", true);

        //낙하 모션
        if (rigid.linearVelocity.y < -1.2f && !anim.GetBool("isJumping") && !anim.GetBool("isFalling")) {
            anim.SetBool("isFalling", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (anim.GetBool("isJumping") || anim.GetBool("isFalling")){
            if (collision.gameObject.CompareTag("Platforms")) {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", false);
                isJumpState = false;
                jumpChance = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            playerCantMove = true;
            OnDamaged(collision.transform.position);
        }
        else if (collision.gameObject.CompareTag("DeadLine")) {
            Debug.Log("추락사 감지");
            playerCantMove = true;
            runningManager.playerHP--;
            runningManager.ScanHealth();
            runningManager.WaitGame();
            PlaySound("Damage");
        }
        else if (collision.gameObject.CompareTag("RunningClear")) {
            playerCantMove = true;
        }
        else if (collision.gameObject.CompareTag("RunningJump")) {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", true);
            PlaySound("Jump");
            StartCoroutine(StopPlayer());
        }
        else if (collision.gameObject.CompareTag("Cherry")) {
            OnCherry(collision.transform);
            stageManager.PlaySound("Cherry");
        }
        else if (collision.gameObject.CompareTag("Gem")) {
            OnGem(collision.transform);
            stageManager.PlaySound("Gem");
        }
    }

    private void OnGem(Transform gem) {
        Gem gemEat = gem.GetComponent<Gem>();
        gemEat.GemEat();
        stageManager.AddGem();
    }

    private void OnCherry(Transform cherry) {
        Cherry cherryEat = cherry.GetComponent<Cherry>();
        cherryEat.CherryEat();
        stageManager.AddCherry();
    }

    IEnumerator StopPlayer() {
        yield return new WaitForSeconds(0.8f);
        scrollObject.scrollSpeed = 0;
        background.speed = 0;
        middle.speed = 0;
        anim.SetBool("isJumping", false);
        yield return new WaitForSeconds(0.1f);
        isJumpState = true;
        anim.SetBool("isWalking", false);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isCrouching", true);
        StageClear.SetActive(true);
        soundManager.BgmChangeClear();
        yield return new WaitForSeconds(0.8f);
        stageExplain.text = "정말 멋져요!";
        Buttons.SetActive(true);
    }

    private void OnDamaged(Vector2 targetPos) {
        //플레이어가 데미지 받았을 때
        if (runningManager.playerHP != 0) {
            gameObject.layer = 16;

            runningManager.playerHP--;
            runningManager.ScanHealth();

            spriteRenderer.color = new Color(1, 1, 1, 0.4f);

            int dir = transform.position.y - targetPos.y > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dir, 1) * 2, ForceMode2D.Impulse);

            runningManager.WaitGame();
            PlaySound("Damage");
        }
        //플레이어가 사망했을 때
        else if (runningManager.playerHP == 0) {
            gameObject.layer = 16;
            rigid.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
            anim.SetBool("isDying", true);
            runningManager.WaitGame();
            PlaySound("Die");
        }
    }

    public void OffDamage() {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void JumpAnim() {
        anim.SetBool("isWalking", false);
        anim.SetBool("isJumping", true);
    }

    private void PlaySound(string sound) {
        switch (sound) {
            case "Jump":
                audioSource.clip = AudioJump;
                break;
            case "Damage":
                audioSource.clip = AudioDamage;
                break;
            case "Die":
                audioSource.clip = AudioDie;
                break;
        }
        audioSource.Play();
    }
}
