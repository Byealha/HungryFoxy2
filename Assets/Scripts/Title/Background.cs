using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private int firstIndex = 1;

    public SpriteRenderer spriteRenderer;

    public PlayerMove playerMove;

    public GameObject playerObj;

    public float interval;
    public float speed = 1f;

    public bool minusSpeed = false;
    public bool isPlayerTarget = false;

    private void Awake() {
        // 원본 이미지를 하나 더 복제한다.
        var newSpriteRenderer = Instantiate<SpriteRenderer>(spriteRenderer);
        newSpriteRenderer.transform.SetParent(this.transform);
        spriteRenderers.Add(spriteRenderer);
        spriteRenderers.Add(newSpriteRenderer);
        SortImage();
        if (isPlayerTarget) {
            speed = 0f;
        }
    }

    private void SortImage() {
        for (int i = spriteRenderers.Count - 1; i >= 0; i--) {
            var spriteRenderer = spriteRenderers[i];
            if (!minusSpeed)
                spriteRenderer.transform.localPosition = Vector3.left * interval * i;
            if (minusSpeed)
                spriteRenderer.transform.localPosition = Vector3.right * interval * i;
        }
    }

    private void Update() {
        UpdateMoveImages();
        if (isPlayerTarget) {
            if (playerMove.GetComponent<Rigidbody2D>().linearVelocity.x > 0) {
                Debug.Log("오른쪽으로  이동");
                speed = playerMove.GetComponent<Rigidbody2D>().linearVelocity.x;
                minusSpeed = false;
            }
            else if (playerMove.GetComponent<Rigidbody2D>().linearVelocity.x < 0) {
                Debug.Log("왼쪽으로  이동");
                speed = playerMove.GetComponent<Rigidbody2D>().linearVelocity.x;
                minusSpeed = true;
            }
        }
    }

    private void UpdateMoveImages() {
        float move = Time.deltaTime * speed;
        for (int i = 0; i < spriteRenderers.Count; i++) {
            var spriteRenderer = spriteRenderers[i];
            spriteRenderer.transform.localPosition += Vector3.right * move;

            if (!minusSpeed) {
                if (spriteRenderer.transform.localPosition.x >= interval) {
                    spriteRenderer.transform.localPosition = new Vector3(spriteRenderers[firstIndex].transform.localPosition.x - interval, 0f, 0f);
                    firstIndex = spriteRenderers.IndexOf(spriteRenderer);
                }
            }
            else if (minusSpeed) {
                if (spriteRenderer.transform.localPosition.x * (-1) >= interval) {
                    Debug.Log("s");
                    spriteRenderer.transform.localPosition = new Vector3(spriteRenderers[firstIndex].transform.localPosition.x + interval, 0f, 0f);
                    firstIndex = spriteRenderers.IndexOf(spriteRenderer);
                }
            }
        }
    }
}
