using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    private PlayerUINavigation playerUINavigation;
    public Camera playerCamera;
    public Slider fovSlider;
    public TMP_Text fovValueText;

    

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

        ///resolution
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        ///FOV
        fovSlider.minValue = 60f;
        fovSlider.maxValue = 110f;
        fovSlider.value = playerCamera.fieldOfView;
        SetFOV(fovSlider.value);
        
        ///Motion Blur
 
    }

    

    private void ShowPanel(GameObject panelToShow)
    {
        cameraPanel.SetActive(false);
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);

        panelToShow.SetActive(true);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetFOV(float newFov)
    {
        if(playerCamera != null)
        {
            playerCamera.fieldOfView = newFov;
        }
        if(fovValueText != null)
        {
            fovValueText.text = Mathf.RoundToInt(newFov).ToString();
        }
    }

    
}
