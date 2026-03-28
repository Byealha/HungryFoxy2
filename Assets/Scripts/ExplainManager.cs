using System.Collections;
using UnityEngine;

public class ExplainManager : MonoBehaviour {
    public GameObject ExplainWindow;
    public GameObject Page1;
    public GameObject Page2;
    public GameObject Page3;
    public GameObject Page4;
    public GameObject Page5;

    public GameObject NextBtn;
    public GameObject PreviousBtn;

    public int page = 1;

    public void OpenExplain() {
        ExplainWindow.SetActive(true);
    }

    public void CloseExplain() {
        ExplainWindow.SetActive(false);
    }

    public void NextPage() {
        PreviousBtn.SetActive(true);
        if (page < 5)
            page++;
        ExplainPage(page);
        if (page == 5)
            NextBtn.SetActive(false);
        else
            NextBtn.SetActive(true);
    }

    public void PrevPage() {
        NextBtn.SetActive(true);
        if (page > 1)
            page--;
        ExplainPage(page);
        if (page == 1)
            PreviousBtn.SetActive(false);
        else
            PreviousBtn.SetActive(true);

    }

    public void ExplainPage(int explainPage) {
        switch(explainPage){
            case 1:
                Page1.SetActive(true);
                Page2.SetActive(false);
                break;
            case 2:
                Page1.SetActive(false);
                Page2.SetActive(true);
                Page3.SetActive(false);
                break;
            case 3:
                Page2.SetActive(false);
                Page3.SetActive(true);
                Page4.SetActive(false);
                break;
            case 4:
                Page3.SetActive(false);
                Page4.SetActive(true);
                Page5.SetActive(false);
                break;
            case 5:
                Page4.SetActive(false);
                Page5.SetActive(true);
                break;
        }
    }
}
