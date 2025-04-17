using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
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

    void Start()
    {
        cameraButton.onClick.AddListener(() => ShowPanel(cameraPanel));
        videoButton.onClick.AddListener(() => ShowPanel(videoPanel));
        audioButton.onClick.AddListener(() => ShowPanel(audioPanel));
        controlsButton.onClick.AddListener(() => ShowPanel(controlsPanel));
        
        ShowPanel(cameraPanel);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        cameraPanel.SetActive(false);
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);

        panelToShow.SetActive(true);
    }
    
}
