using UnityEngine;

public class Cherry : MonoBehaviour {
    private Animator anim;

    public GameManager gameManager;
    public RunningManager runningManager;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void CherryEat() {
        gameObject.layer = 15;
        if (gameManager != null) {
            gameManager.cherryEat++;
        }
        anim.SetBool("isEat", true);
        Invoke("DeActive", 0.3f);
    }

    void DeActive() {
        gameObject.SetActive(false);
    }
}
