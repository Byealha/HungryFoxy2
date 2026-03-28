using UnityEngine;

public class ScrollObject : MonoBehaviour {
    public float scrollSpeed = 10f;

    private void Update() {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }
}
