using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource[] sounds;
    [SerializeField] AudioMixerGroup allSound;

    [SerializeField] Slider volumeSlider;

    void Awake()
    {
        Instance = this;
        foreach (AudioSource s in sounds)
        {
            s.outputAudioMixerGroup = allSound;
        }
        
    }
    private void Start()
    {
        LoadAllSound();
    }

    public void UpdateMixerVolume()
    {
        allSound.audioMixer.SetFloat("AllVolume", Mathf.Log10(PlayerDataHandle.Instance.soundVolume) * 20);
    }

    public void ChangeVolumeSliderValue(float valueToSet)
    {
        PlayerDataHandle.Instance.soundVolume = valueToSet;
        SaveVolume();
        UpdateMixerVolume();
    }

    void LoadAllSound()
    {
        string path = Application.persistentDataPath + "/savedvolume.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            PlayerDataHandle.Instance.soundVolume = data.LoadedVolumeValue;
        }
        else
            PlayerDataHandle.Instance.soundVolume = 1;

        UpdateMixerVolume();
        volumeSlider.value = PlayerDataHandle.Instance.soundVolume;
    }

    void SaveVolume()
    {
        SaveData data = new SaveData();

        data.LoadedVolumeValue = PlayerDataHandle.Instance.soundVolume;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedvolume.json", json);
    }

    [System.Serializable]
    class SaveData
    {
        public float LoadedVolumeValue;
    }
}