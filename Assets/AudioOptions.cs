using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider musicSlider;

    private void Start() { Initialize(); }

    private void Initialize()
    {
        masterSlider.value = PlayerPrefs.GetFloat("AudioMaster", 1f);
        SFXSlider.value = PlayerPrefs.GetFloat("AudioSFX", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("AudioMusic", 1f);
        AudioListener.volume = masterSlider.value;
        //Aca hay que agregar los audio listeners correspondientes a los SFX y a la musica, por ahora solamente se modifica el general.
    }

    public void SetMaster()
    {
        AudioListener.volume = masterSlider.value;
        PlayerPrefs.SetFloat("AudioMaster", masterSlider.value);
        PlayerPrefs.Save();
    }
    public void SetSFX()
    {
        //Aca se setea el audio listener correspondiente a los SFX
        PlayerPrefs.SetFloat("AudioSFX", SFXSlider.value);
        PlayerPrefs.Save();
    }
    public void SetMusic()
    {
        //Aca se setea el audio listener correspondiente a la musica
        PlayerPrefs.SetFloat("AudioMusic", musicSlider.value);
        PlayerPrefs.Save();
    }
}
