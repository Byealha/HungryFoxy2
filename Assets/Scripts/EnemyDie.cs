using UnityEngine;

public class EnemyDie : MonoBehaviour {
    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void OnDamaged() {
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.layer = 9;
        anim.SetBool("isDie", true);
        Invoke("DeActive", 0.4f);
    }

    void DeActive() {
        gameObject.SetActive(false);
    }
}
