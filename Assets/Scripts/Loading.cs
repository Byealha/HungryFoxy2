/*
해당 스크립트에서 사용하는 PlayerPrefs는 다음과 같습니다.

Stage = 불러올 스테이지 숫자
기본값 0
0 > 컷씬1
1 > Stage1
2 > Stage2
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    public GameObject charImage;
    public GameObject loadingText;

    private void Start() {
        RandomImage();
        StartCoroutine(GotoMain());
    }

    private void RandomImage() {
        int random = Random.Range(0, 100);
        if(random >= 62)
            charImage.GetComponent<Animator>().SetBool("isWalking", true);
        else if (random >= 34)
            charImage.GetComponent<Animator>().SetBool("isCrouching", true);
    }

    //화면 전환 & Stage값에 따라 변환되는 화면 결정
    IEnumerator GotoMain() {
        yield return new WaitForSeconds(3f);
        charImage.SetActive(false);
        loadingText.SetActive(false);
        yield return new WaitForSeconds(1f);

        StageLoad(PlayerPrefs.GetInt("Stage", 0));
    }

    private void StageLoad(int loading) {
        switch (loading) {
            case 0:
                SceneManager.LoadScene("CutScene1");
                break;
            case 1:
                SceneManager.LoadScene("Stage1");
                break;
            case 2:
                SceneManager.LoadScene("Stage2");
                break;
        }
        PlayerPrefs.SetInt("Stage", 0);
    }
}
