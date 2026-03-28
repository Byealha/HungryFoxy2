using UnityEngine;

public class RunningRespawn : MonoBehaviour {
    public RunningManager runningManager;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("RespawnPoint")) {
            Debug.Log("리스폰 저장");
        }

        if (collision.gameObject.CompareTag("RespawnPoint") && runningManager.playerHurt) {
            runningManager.SecondScene();
            runningManager.playerHurt = false;
        }
    }
}
