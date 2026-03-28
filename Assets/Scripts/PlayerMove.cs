using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour {
    private AudioSource audioSource;
    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float maxSpeed = 7.5f;
    [SerializeField] private float jumpPower = 15f;

    private bool onAttackCantJump = false;

    public AudioClip AudioJump;
    public AudioClip AudioDamage;
    public AudioClip AudioAttack;
    public AudioClip AudioDie;

    public GameManager gameManager;
    public StageManager stageManager;

    public delegate void OnPlayerMove(float moveAmount);
    public static event OnPlayerMove OnMove;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        //좌우 이동
        if (Input.GetButtonUp("Horizontal")) {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.normalized.x * 0.5f, rigid.linearVelocity.y);
        }

        //이동방향에 따른 플레이어 이미지 좌우 반전
        if (Input.GetButton("Horizontal")) {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //점프
        if (!gameManager.playerCantMove) {
            if (Input.GetKeyDown(KeyCode.Space) && !anim.GetBool("isJumping") && !onAttackCantJump && !anim.GetBool("isFalling")) {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
                PlaySound("Jump");
            }
        }

        //걷는 모션
        if (Mathf.Abs(rigid.linearVelocity.x) < 0.5f)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        //낙하 모션
        if (rigid.linearVelocity.y < -1.2f && !anim.GetBool("isJumping") && !anim.GetBool("isFalling")) {
            anim.SetBool("isFalling", true);
        }

        ///////////////////디버깅///////////////////
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("Stage1");
            Time.timeScale = 1;
        }
    }

    private void FixedUpdate() {
        //이동 로직
        
        float h = Input.GetAxisRaw("Horizontal");
        if (!gameManager.playerCantMove) {
            if (gameManager.playerHP != 0)
                rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        }

        if (rigid.linearVelocity.x > maxSpeed)
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        else if (rigid.linearVelocity.x < maxSpeed * (-1))
            rigid.linearVelocity = new Vector2(maxSpeed * (-1), rigid.linearVelocity.y);

        if (rigid.linearVelocity.x != 0 && OnMove != null) {
            OnMove(h);
        }

        //점프 모션 로직
        if (rigid.linearVelocity.y < 0) {
            Vector2 playerTransform = new Vector2(rigid.position.x, rigid.position.y - 0.4f);
            Debug.DrawRay(playerTransform, Vector3.down * 0.1f, new Color(0, 1, 0));
            
            Vector2 playerLeftCorrectionJumping = new Vector2(rigid.position.x - 4f * 0.1f, rigid.position.y - 0.4f);
            Debug.DrawRay(playerLeftCorrectionJumping, Vector3.down * 0.1f, new Color(0, 1, 0));

            Vector2 playerRightCorrectionJumping = new Vector2(rigid.position.x + 4f * 0.1f, rigid.position.y - 0.4f);
            Debug.DrawRay(playerRightCorrectionJumping, Vector3.down * 0.1f, new Color(0, 1, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(playerTransform, Vector3.down * 0.1f, 1, PlatformLayers());
            RaycastHit2D playerLeftCorrectionRayHit = Physics2D.Raycast(playerLeftCorrectionJumping, Vector3.down * 0.1f, 1, PlatformLayers());
            RaycastHit2D playerRightCorrectionRayHit = Physics2D.Raycast(playerRightCorrectionJumping, Vector3.down * 0.1f, 1, PlatformLayers());

            if (rayHit.collider != null || playerLeftCorrectionRayHit.collider != null || playerRightCorrectionRayHit.collider != null) {
                if (rayHit.distance < 6.4f) {
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isFalling", false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //적 충돌시 데미지
        if (collision.gameObject.CompareTag("Enemy")) {
            gameManager.playerHP--;
            gameManager.ScanHealth();
            OnDamaged(collision.transform.position);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //적 충돌시 처치
        if (collision.gameObject.CompareTag("Enemy")) {
            if (rigid.linearVelocity.y < 0 && transform.position.y > collision.transform.position.y) {
                OnAttack(collision.transform);
                PlaySound("Attack");
            }
        }
        //잼 충돌시 획득
        else if (collision.gameObject.CompareTag("Gem")) {
            OnGem(collision.transform);
            stageManager.PlaySound("Gem");
        }
        //체리 충돌시 획득
        else if (collision.gameObject.CompareTag("Cherry")) {
            OnCherry(collision.transform);
            stageManager.PlaySound("Cherry");
        }
        //집 충돌시 맵 이동 판단
        else if (collision.gameObject.CompareTag("House")) {
            gameManager.CherryCheck();
        }
    }

    //레이어 2개를 동시에 받기 위한 LayerMask
    public LayerMask PlatformLayers() {
        int layer1 = LayerMask.GetMask("Platforms");
        int layer2 = LayerMask.GetMask("PlatformsEffector");
        return layer1 | layer2;
    }

    private void OnDamaged(Vector2 targetPos) {
        //플레이어가 데미지 받았을 때
        if (gameManager.playerHP != 0) {
            gameObject.layer = 11;

            spriteRenderer.color = new Color(1, 1, 1, 0.4f);

            int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dir, 1) * 7, ForceMode2D.Impulse);

            Invoke("OffDamage", 2);
            PlaySound("Damage");
        }

        //플레이어가 사망했을 때
        if (gameManager.playerHP == 0) {
            gameObject.layer = 16;
            rigid.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
            anim.SetBool("isDying", true);
            PlaySound("Die");
        }
    }

    //데미지로 인한 무적 해제
    private void OffDamage() {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    //플레이어의 공격
    private void OnAttack(Transform enemy) { 
        EnemyDie enemyDie = enemy.GetComponent<EnemyDie>();
        enemyDie.OnDamaged();
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        onAttackCantJump = true;
        anim.SetBool("isJumping", false);
        Invoke("OnAttackLateJumpAnimation", 0.1f);
    }

    //드물게 일어나는 점프 오류 개선 및 조작감 개선
    private void OnAttackLateJumpAnimation() {
        anim.SetBool("isJumping", true);
        onAttackCantJump = false;
    }

    //잼 획득
    private void OnGem(Transform gem) {
        Gem gemEat = gem.GetComponent<Gem>();
        gemEat.GemEat();
        stageManager.AddGem();
    }

    //체리 획득
    private void OnCherry(Transform cherry) {
        Cherry cherryEat = cherry.GetComponent<Cherry>();
        cherryEat.CherryEat();
        stageManager.AddCherry();
    }

    public void StageClear() {
        anim.SetBool("isCrouching", true);
        rigid.linearVelocity = Vector3.zero;
    }

    //사운드 관리
    private void PlaySound(string sound) {
        switch (sound) {
            case "Jump":
                audioSource.clip = AudioJump;
                break;
            case "Damage":
                audioSource.clip = AudioDamage;
                break;
            case "Attack":
                audioSource.clip = AudioAttack;
                break;
            case "Die":
                audioSource.clip = AudioDie;
                break;
        }
        audioSource.Play();
    }
}
