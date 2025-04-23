using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    private PlayerUINavigation playerUINavigation;

    [Header("Buttons")]
    [SerializeField] private Button cameraButton;
    [SerializeField] private Button videoButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button controlsButton;

    [Header("Panels")]
    [SerializeField] private GameObject cameraPanel;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject controlsPanel;

    private void Awake()
    {
        playerUINavigation = FindAnyObjectByType<PlayerUINavigation>();
    }

    void Start()
    {
        cameraButton.onClick.AddListener(() => ShowPanel(cameraPanel));
        videoButton.onClick.AddListener(() => ShowPanel(videoPanel));
        audioButton.onClick.AddListener(() => ShowPanel(audioPanel));
        controlsButton.onClick.AddListener(() => ShowPanel(controlsPanel));
        
        ShowPanel(cameraPanel);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
    }

    

    private void ShowPanel(GameObject panelToShow)
    {
        cameraPanel.SetActive(false);
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);

        panelToShow.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
