using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject settingsPanel;
    public UIAudioController UIAudioController;

    [Header("Audio")]
    public AudioMixer mainMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Pantalla")]
    public TMP_Dropdown resDropdown;
    public Toggle fullToggle;

    // Cambiamos el array por una Lista para manejar solo las resoluciones unicas
    List<Resolution> filteredResolutions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Limitamos los FPS del juego a 60 para que sea estable
        Application.targetFrameRate = 60;

        SetupResolution();
        LoadSettings();

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        fullToggle.onValueChanged.AddListener(SetFullscreen);
        resDropdown.onValueChanged.AddListener(SetResolution);

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }

    // --- LÓGICA DE AUDIO ---
    public void SetMusicVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("MusicVol", dB);
        PlayerPrefs.SetFloat("MusicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("SFXVol", dB);
        PlayerPrefs.SetFloat("SFXVol", value);
    }

    // --- LÓGICA DE PANTALLA ---
    void SetupResolution()
    {
        Resolution[] allResolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < allResolutions.Length; i++)
        {
            // Solo añadimos la resolución si no existe ya una con el mismo ancho y alto
            // Esto elimina los duplicados por Hz
            if (!filteredResolutions.Any(res => res.width == allResolutions[i].width && res.height == allResolutions[i].height))
            {
                filteredResolutions.Add(allResolutions[i]);
            }
        }

        // Ahora creamos los textos para el Dropdown con la lista limpia
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string option = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
            options.Add(option);

            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resDropdown.AddOptions(options);

        // Cargamos el índice guardado, asegurándonos de que no se salga del rango de la nueva lista
        int savedResIndex = PlayerPrefs.GetInt("ResIndex", currentResIndex);
        resDropdown.value = Mathf.Clamp(savedResIndex, 0, filteredResolutions.Count - 1);

        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Resolution res = filteredResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResIndex", index);
    }

    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
        PlayerPrefs.SetInt("Fullscreen", isFull ? 1 : 0);
    }

    public void ToggleSettingsPanel()
    {
        bool isActive = settingsPanel.activeSelf;
        settingsPanel.SetActive(!isActive);
        if (!isActive)
            UIAudioController.AssignSoundsToButtons();
    }

    // --- CARGA DE DATOS ---
    void LoadSettings()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        musicSlider.value = musicVol;
        SetMusicVolume(musicVol);

        float sfxVol = PlayerPrefs.GetFloat("SFXVol", 0.75f);
        sfxSlider.value = sfxVol;
        SetSFXVolume(sfxVol);

        bool isFull = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullToggle.isOn = isFull;
        Screen.fullScreen = isFull;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
