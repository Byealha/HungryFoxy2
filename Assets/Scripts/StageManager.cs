using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour {
    private AudioSource audioSource;

    public AudioClip AudioGem;
    public AudioClip AudioCherry;
    public AudioClip AudioClear;

    public Text GemCount;
    public Text CherryCount;

    public int maxGem;
    public int gem = 0;
    public int maxCherry;
    public int cherry = 0;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        GemCount.text = gem + " / " + maxGem;
        CherryCount.text = cherry + " / " + maxCherry;
    }

    public void AddGem() {
        gem++;
    }

    public void AddCherry() {
        cherry++;
    }

    public void PlaySound(string sound) {
        switch (sound) {
            case "Gem":
                audioSource.clip = AudioGem;
                break;
            case "Cherry":
                audioSource.clip = AudioCherry;
                break;
            case "Clear":
                audioSource.clip = AudioClear;
                break;
        }
        audioSource.Play();
    }
}
