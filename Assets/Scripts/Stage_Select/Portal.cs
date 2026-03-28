using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {
    public FadeIn fadeIn;
    public StageControl stageControl;

    public GameObject blackScreen;

    public int stagePortal;

    private void Update() {
        //스페이스 바 키를 통해 스테이지 이동
        if (stageControl.portalReady) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (!stageControl.selectLevel) {
                    blackScreen.SetActive(true);
                    fadeIn.ShowBlackScreen();
                }
                stageControl.selectLevel = true;
                if (stagePortal == stageControl.stage) {
                    StartCoroutine(GotoStage());
                }
            }
        }
    }

    //stagePortal값을 확인하고 해당 값의 맵으로 이동
    IEnumerator GotoStage() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Loading");
        PlayerPrefs.SetInt("Stage", stagePortal);
    }
}
