using UnityEngine;

public class ResolutionLock : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        Screen.SetResolution(1920, 1080, true);
    }

    private void FixedUpdate() {
        if (Screen.width != 1920 || Screen.height != 1080) {
            Screen.SetResolution(1920, 1080, true);
        }

    }
}
