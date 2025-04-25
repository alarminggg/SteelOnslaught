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

    [Header("FOV")]
    public Camera playerCamera;
    public Slider fovSlider;
    public TMP_Text fovValueText;

    [Header("Mouse Sens")]
    public Slider mouseSensSliderH;
    public Slider mouseSensSliderV;
    public TMP_Text mouseSensHText;
    public TMP_Text mouseSensVText;

    [Header("Controller Sens")]
    public Slider controllerSensSliderH;
    public Slider controllerSensSliderV;
    public TMP_Text controllerSensHText;
    public TMP_Text controllerSensVText;
    private PlayerController playerController;

    [Header("Motion Blur")]
    [SerializeField] private Toggle motionBlurToggle;
    [SerializeField] private VolumeProfile postProcessingVolume;
    private MotionBlur motionBlur;

    [Header("Colourblind")]
    public TMP_Dropdown colourblindDropdown;
    private ColorAdjustments colourAdjustments;
    public VolumeProfile volumeProfile;

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
        playerController = FindAnyObjectByType<PlayerController>();
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
        if (postProcessingVolume != null)
        {
            postProcessingVolume = Instantiate(postProcessingVolume);

            if(postProcessingVolume.TryGet(out motionBlur))
            {
                bool savedState = PlayerPrefs.GetInt("MotionBlur", 1) == 1;
                motionBlur.active = savedState;
                motionBlurToggle.isOn = savedState;

                motionBlurToggle.onValueChanged.AddListener(SetMotionBlur);
            }
            
        }

        ///Player Sens
        SetMouseSensH(PlayerPrefs.GetFloat("MouseSensH", 0.1f));
        SetMouseSensV(PlayerPrefs.GetFloat("MouseSensV", 0.1f));
        SetControllerSensH(PlayerPrefs.GetFloat("ControllerSensH", 0.1f));
        SetControllerSensV(PlayerPrefs.GetFloat("ControllerSensV", 0.1f));

        mouseSensSliderH.value = PlayerPrefs.GetFloat("MouseSensH", 0.1f);
        mouseSensSliderV.value = PlayerPrefs.GetFloat("MouseSensV", 0.1f);
        controllerSensSliderH.value = PlayerPrefs.GetFloat("ControllerSensH", 0.1f);
        controllerSensSliderV.value = PlayerPrefs.GetFloat("ControllerSensV", 0.1f);

        ///Colourblind
        if (volumeProfile != null && volumeProfile.TryGet<ColorAdjustments>(out colourAdjustments))
        {
            colourblindDropdown.onValueChanged.AddListener(SetColourblindFilter);
        }
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

    public void SetMouseSensH(float value)
    {
        playerController.lookSensH = value;
        PlayerPrefs.SetFloat("MouseSensH", value);
        PlayerPrefs.Save();
        if (mouseSensHText != null)
        {
            mouseSensHText.text = value.ToString("F2");
        }
    }
    public void SetMouseSensV(float value)
    {
        playerController.lookSensV = value;
        PlayerPrefs.SetFloat("MouseSensV", value);
        PlayerPrefs.Save();
        if (mouseSensVText != null)
        {
            mouseSensVText.text = value.ToString("F2");
        }
    }
    public void SetControllerSensH(float value)
    {
        playerController.controllerLookSensH = value;
        PlayerPrefs.SetFloat("ControllerSensH", value);
        PlayerPrefs.Save();
        if (controllerSensHText != null)
        {
            controllerSensHText.text = value.ToString("F2");
        }
    }
    public void SetControllerSensV(float value)
    {
        playerController.controllerLookSensV = value;
        PlayerPrefs.SetFloat("ControllerSensV", value);
        PlayerPrefs.Save();
        if (controllerSensVText != null)
        {
            controllerSensVText.text = value.ToString("F2");
        }
    }

    public void SetMotionBlur(bool isEnabled)
    {
        if(postProcessingVolume != null && postProcessingVolume.TryGet(out MotionBlur motionBlur))
        {
            motionBlur.active = isEnabled;

            PlayerPrefs.SetInt("MotionBlur", isEnabled  ? 1 : 0);
            PlayerPrefs.Save();

            UpdatePostProcessing();
        }
    }

    private void UpdatePostProcessing()
    {
        if (postProcessingVolume != null && postProcessingVolume != null)
        {
            postProcessingVolume = postProcessingVolume;
        }
    }

    public void SetColourblindFilter(int dropdownIndex)
    {
        if(colourAdjustments != null)
        {
            switch (dropdownIndex)
            {
                case 0:
                    SetTrueVision();
                    break;
                case 1:
                    SetDeuteranopia();
                    break;
                case 2:
                    SetProtanopia();
                    break;
                case 3:
                    SetTritanopia();
                    break;
            }
        }
    }

    void SetTrueVision()
    {
        if(colourAdjustments != null)
        {
            colourAdjustments.saturation.overrideState = false;
            colourAdjustments.hueShift.overrideState = false;

            colourAdjustments.saturation.value = 0f;
            colourAdjustments.hueShift.value = 0f;
        }
        
    }

    void SetDeuteranopia()
    {
        colourAdjustments.saturation.overrideState = true;
        colourAdjustments.hueShift.overrideState = true;

        colourAdjustments.saturation.value = -40f;
        colourAdjustments.hueShift.value = 25f;
    }

    void SetProtanopia()
    {
        colourAdjustments.saturation.overrideState = true;
        colourAdjustments.hueShift.overrideState = true;

        colourAdjustments.saturation.value = -40f;
        colourAdjustments.hueShift.value = -25f;
    }

    void SetTritanopia()
    {
        colourAdjustments.saturation.overrideState = true;
        colourAdjustments.hueShift.overrideState = true;

        colourAdjustments.saturation.value = -40f;
        colourAdjustments.hueShift.value = 75f;
    }
}
