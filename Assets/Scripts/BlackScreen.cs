using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BlackScreen : MonoBehaviour{
    public Image image;

    IEnumerator FadeOut() {
        float fadeCount = 0;
        while (fadeCount < 1f) {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }
    }

    public void Corutine() {
        gameObject.SetActive(true);
        StartCoroutine(FadeOut());
        StartCoroutine(GoToBlackScreen());
    }

    IEnumerator GoToBlackScreen() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("BlackScreen");
    }
}
