using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour{
    public Image image;

    private float CorutineTimer = 0;

    private bool noCount = false;

    private void Start() {
        gameObject.SetActive(true);
        StartCoroutine(TitleShow());
    }

    private void Update() {
        if (CorutineTimer < 1.4 && !noCount) {
            CorutineTimer += Time.deltaTime;
        }
        else if (CorutineTimer >= 1.4 && !noCount) {
            gameObject.SetActive(false);
            CorutineTimer = 0;
            noCount = true;
        }
    }

    IEnumerator TitleShow() {
        float fadeCount = 1;
        while (fadeCount > 0) {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }
    }

    IEnumerator TitleHide() {
        float fadeCount = 0;
        while (fadeCount < 1f) {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }
    }

    public void StartGame() {
        StartCoroutine(TitleHide());
    }

    public void ShowBlackScreen() {
        StartCoroutine(TitleHide());
    }
}
