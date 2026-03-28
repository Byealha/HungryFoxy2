using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextDelay : MonoBehaviour {
    public Text targetText;

    public float delay = 0.125f;

    public string text;

    //해당 텍스트를 저장하고 빈 텍스트로 변경
    public void TextStart() {
        text = targetText.text.ToString();
        targetText.text = " ";
        StartCoroutine(textPrint());
    }

    //저장한 텍스트를 코루틴을 활용하여 한글자씩 다시 입력
    IEnumerator textPrint() {
        int count = 0;

        while (count != text.Length) {
            if(count < text.Length) {
                targetText.text += text[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(delay);
        }
    }
}
