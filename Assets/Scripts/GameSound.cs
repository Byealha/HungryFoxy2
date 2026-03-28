/*
해당 스크립트에서 사용하는 PlayerPrefs는 다음과 같습니다.

bgmSound = 배경음 크기
기본값 0
sfxSound = 효과음 크기
기본값 0
*/

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSound : MonoBehaviour {
    public AudioMixer bgmMixer;
    public AudioMixer sfxMixer;

    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start() {
        bgmSlider.value = PlayerPrefs.GetFloat("bgmSound", 0);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxSound", 0);
    }

    public void BGMAudioControl() {
        float bgmSound = bgmSlider.value;
        PlayerPrefs.SetFloat("bgmSound", bgmSound);

        if (bgmSound == -40f)
            bgmMixer.SetFloat("BGM", -80);
        else
            bgmMixer.SetFloat("BGM", bgmSound);
    }

    public void SFXAudioControl() {
        float sfxSound = sfxSlider.value;
        PlayerPrefs.SetFloat("sfxSound", sfxSound);

        if (sfxSound == -40f)
            sfxMixer.SetFloat("SFX", -80);
        else
            sfxMixer.SetFloat("SFX", sfxSound);

    }
}
