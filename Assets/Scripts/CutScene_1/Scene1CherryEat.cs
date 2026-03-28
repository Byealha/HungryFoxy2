using UnityEngine;

public class Scene1CherryEat : MonoBehaviour {
    private Animator anim;

    public Scene1Manager sceneManager;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    //√º∏Æ »πµÊΩ√ ¿Ã∫•∆Æ
    public void OnCherry() {
        sceneManager.PlaySound("itemEat");
        anim.SetBool("isEat", true);
        Invoke("DeActive", 0.3f);
    }

    private void DeActive() {
        gameObject.SetActive(false);
    }
    
}
