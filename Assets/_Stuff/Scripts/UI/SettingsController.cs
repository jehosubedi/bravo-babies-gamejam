using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] AudioMixerGroup BGM;
    [SerializeField] AudioMixerGroup SFX;

    public Slider BGMSlider;
    public Slider SFXSlider;

    private void Start()
    {
        BGM.audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM"));
        SFX.audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));

        if (BGM.audioMixer.GetFloat("BGM", out float bgm))
            BGMSlider.value = bgm;
        if(SFX.audioMixer.GetFloat("SFX", out float sfx))
            SFXSlider.value = sfx;
    }

    public void CloseSettings()
    {
        PlayerPrefs.SetFloat("BGM", BGMSlider.value);
        PlayerPrefs.SetFloat("SFX", SFXSlider.value);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Settings");
    }
    public void Hover() => AudioHandler.instance.PlaySFX("Hover");
    public void Click() => AudioHandler.instance.PlaySFX("Click");
    public void AdjustBGM(float value) => BGM.audioMixer.SetFloat("BGM", value);
    public void AdjustSFX(float value) => SFX.audioMixer.SetFloat("SFX", value);
}
