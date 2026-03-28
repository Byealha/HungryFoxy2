using System.Collections;
using UnityEngine;

public class DebugManager : MonoBehaviour {
    [SerializeField] private bool debugEnabled = false;

    public GameObject DebugText;
    public GameObject Map;

    private void Update() {
        if (debugEnabled) {
            if (Input.GetKeyDown(KeyCode.M)) {
                DebugText.SetActive(true);
                Map.transform.position = new Vector3(-249, 0, 0);
                StartCoroutine(OffDebugText());
            }
        }
    }

    IEnumerator OffDebugText() {
        yield return new WaitForSeconds(2f);
        DebugText.SetActive(false);
    }
}
