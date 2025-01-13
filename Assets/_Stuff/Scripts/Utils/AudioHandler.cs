using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler instance;

    [Header("Mixers")]
    public AudioMixerGroup BGM;
    public AudioMixerGroup SFX;

    [Header("Others")]
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
        }
        else
            Destroy(gameObject);
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
