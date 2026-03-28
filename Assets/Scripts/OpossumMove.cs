using UnityEngine;

public class OpossumMove : MonoBehaviour {
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    private int nextMoveDir = 1;

    private float waitNextMove = 0;

    private bool waitNextMoveTrigger = false;

    public int moveSpeed = 3;

    Vector3 frontVec;
    Vector3 underVec;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if(waitNextMoveTrigger)
            waitNextMove += Time.deltaTime;

        if (waitNextMove >= 1) {
            waitNextMove = 0;
            waitNextMoveTrigger = false;
        }
    }

    private void FixedUpdate() {
        rigid.linearVelocity = new Vector2(moveSpeed, rigid.linearVelocity.y);

        if (nextMoveDir > 0) {
            frontVec = new Vector2(rigid.position.x + 0.2f * nextMoveDir, rigid.position.y);
        }
        if (nextMoveDir < 0) {
            frontVec = new Vector2(rigid.position.x + 1.2f * nextMoveDir, rigid.position.y);
        }

        Debug.DrawRay(frontVec, Vector3.right, new Color(0, 1, 0));
        RaycastHit2D frontRayHit = Physics2D.Raycast(frontVec, Vector3.right, 1, LayerMask.GetMask("Platforms"));

        underVec = new Vector2(rigid.position.x + nextMoveDir, rigid.position.y);
        Debug.DrawRay(underVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D underRayHit = Physics2D.Raycast(underVec, Vector3.down, 1, LayerMask.GetMask("Platforms"));

        if (frontRayHit.collider != null && !waitNextMoveTrigger) {
            moveSpeed *= -1;
            nextMoveDir = moveSpeed > 0 ? 1 : -1;
            spriteRenderer.flipX = spriteRenderer.flipX ? false : true;
            waitNextMoveTrigger = true;
        }

        if (underRayHit.collider == null && !waitNextMoveTrigger) {
            moveSpeed *= -1;
            nextMoveDir = moveSpeed > 0 ? 1 : -1;
            spriteRenderer.flipX = spriteRenderer.flipX ? false : true;
            waitNextMoveTrigger = true;
        }
    }

}
