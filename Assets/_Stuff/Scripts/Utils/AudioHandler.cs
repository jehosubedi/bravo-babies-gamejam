using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler instance;

    [Header("Mixers")]
    public AudioMixerGroup BGM;
    public AudioMixerGroup SFX;

    [Header("BGM")]
    public AudioSource BGMSource;
    public AudioClip MenuScore;
    public AudioClip GameScore;
    public AudioClip PrepScore;

    [Header("SFX")]
    public AudioSource SFXSource;
    public AudioClip hover;
    public AudioClip click;
    public AudioClip end;
    public AudioClip coin;
    public AudioClip open;
    public AudioClip close;
    public AudioClip start;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            BGM.audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM"));
            SFX.audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));

            SceneManager.activeSceneChanged += SwitchBGM;
        }
        else
            Destroy(gameObject);
    }

    private void SwitchBGM(Scene oldScene, Scene newScene)
    {
        if(newScene.name.Contains("Game"))
            StartCoroutine(FadeBGM(GameScore));
        else if(newScene.name.Contains("MainMenu"))
            StartCoroutine(FadeBGM(MenuScore));


        IEnumerator FadeBGM(AudioClip newBGM)
        {
            while(BGMSource.volume > 0)
            {
                BGMSource.volume -= Time.deltaTime / .002f;
                yield return null;
            }

            BGMSource.clip = newBGM;
            BGMSource.Play();

            while (BGMSource.volume < .5f)
            {
                BGMSource.volume += Time.deltaTime / .002f;
                yield return null;
            }
            BGMSource.volume = .5f;

            yield return null;
        }
    }

    public void PlaySFX(string clipName)
    {
        AudioClip selected = null;

        switch (clipName)
        {
            case "Hover":
                selected = hover;
                break;
            case "Click":
                selected = click;
                break;
            case "End":
                selected = end;
                break;
            case "Coin":
                selected = coin;
                break;
            case "Open":
                selected = open;
                break;
            case "Close":
                selected = close;
                break;
            case "Start":
                selected = start;
                break;
        }

        SFXSource.PlayOneShot(selected);
    }
}
