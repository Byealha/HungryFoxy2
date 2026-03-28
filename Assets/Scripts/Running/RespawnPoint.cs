using UnityEngine;

public class RespawnPoint : MonoBehaviour {
    public RunningManager runningManager;

    public float respawnPointY = 4f;
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == 31) {
            runningManager.respawnY = respawnPointY;
        }
    }
}
