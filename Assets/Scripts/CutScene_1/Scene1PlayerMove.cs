using UnityEngine;

public class Scene1PlayerMove : MonoBehaviour {
    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    private bool playerMove = false;

    public float moveSpeed = 4f;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        //playerMove가 활성화되면 이동 로직 시작
        if (playerMove) {
            if(spriteRenderer.flipX)
                rigid.AddForce(Vector2.left, ForceMode2D.Impulse);
            else
                rigid.AddForce(Vector2.right, ForceMode2D.Impulse);

            if (rigid.linearVelocity.x > moveSpeed)
                rigid.linearVelocity = new Vector2(moveSpeed, rigid.linearVelocity.y);
            else if (rigid.linearVelocity.x < moveSpeed * (-1))
                rigid.linearVelocity = new Vector2(moveSpeed * (-1), rigid.linearVelocity.y);
        }
    }

    //playerMove 활성화 여부 결정 & 애니메이션 출력
    public void PlayerMoveToggle() {
        if (playerMove) {
            playerMove = false;
            anim.SetBool("isIdle", true);
        }
        else {
            playerMove = true;
            anim.SetBool("isIdle", false);
        }
    }

    //스프라이트 반전 토글
    public void PlayerFlipToggle() {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
