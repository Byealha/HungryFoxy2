using UnityEngine;

public class Gem : MonoBehaviour {
    Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void GemEat() {
        gameObject.layer = 13;
        anim.SetBool("isEat", true);
        Invoke("DeActive", 0.3f);
    }

    void DeActive() {
        gameObject.SetActive(false);
    }
}
