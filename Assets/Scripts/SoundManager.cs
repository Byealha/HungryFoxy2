using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private AudioSource audioSource;

    public AudioClip BGM;
    public AudioClip Clear;
    public AudioClip GameOver;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void BgmChangeClear() {
        StartCoroutine(BgmChange("Clear"));
    }

    public void BgmChangeGameOver() {
        StartCoroutine(BgmChange("GameOver"));
    }

    IEnumerator BgmChange(string sound) {
        audioSource.Pause();
        yield return new WaitForSeconds(1f);
        PlaySound(sound);
    }

    public void PlaySound(string sound) {
        switch (sound) {
            case "BGM":
                audioSource.clip = BGM;
                break;
            case "Clear":
                audioSource.clip = Clear;
                break;
            case "GameOver":
                audioSource.clip = GameOver;
                break;
        }
        audioSource.Play();
    }
}
