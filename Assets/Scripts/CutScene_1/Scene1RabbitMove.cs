using UnityEngine;

public class Scene1RabbitMove : MonoBehaviour {
    private Rigidbody2D rigid;

    private float nextJumpTime = 0;

    public Scene1CherryEat cherryEat;

    public float jumpPower = 10f;
    public float setNextJumpTime = 1;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        //토끼의 이동 로직
        if(nextJumpTime != 0) {
            nextJumpTime -= Time.deltaTime;
            if (nextJumpTime <= 0) {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                rigid.AddForce(Vector2.left * 10f, ForceMode2D.Impulse);
                RabbitJumpStart();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //체리 충돌시 체리 획득
        if (collision.gameObject.CompareTag("Cherry")) {
            cherryEat.OnCherry();
        }
    }

    //체리 이동 로직 트리거
    public void RabbitJumpStart() {
        nextJumpTime = setNextJumpTime;
    }
}
