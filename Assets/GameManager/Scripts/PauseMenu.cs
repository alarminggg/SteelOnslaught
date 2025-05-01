using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;

    public GameObject InGameUI;

    public GameObject SettingsPanel;
    public GameObject cameraButton;
    public GameObject resumeButton;
    public GameObject retryButton;

    public GameObject startButton;

    private bool isDead = false;

    private PlayerLocomotionInput playerLocomotionInput;
    
    private PlayerController playerController;
    private AssaultRifle playerGun;

    private void Awake()
    {
        playerLocomotionInput = FindAnyObjectByType<PlayerLocomotionInput>();
        
        playerController = FindAnyObjectByType<PlayerController>();
        playerGun = FindAnyObjectByType<AssaultRifle>();

    }

    // Update is called once per frame
    void Update()
    {
        if(isDead || playerLocomotionInput == null) return;
        {
            if (playerLocomotionInput.PausePressed)
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
               
            }
        }
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EnablePlayerInputs();

        EventSystem.current.SetSelectedGameObject(null);
        

        if (InGameUI != null)
        {
            InGameUI.SetActive(true);
        }
    }

    

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DisablePlayerInputs();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        

        if (InGameUI != null)
        {
            InGameUI.SetActive(false);
        }

        
    }



    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("DevScene");

        Resume();

    }

    public void RestartHTP()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TutorialScene");

        Resume();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
    private void DisablePlayerInputs()
    {
        if (playerLocomotionInput != null)
        {
            playerLocomotionInput.enabled = false;  
        }

        if (playerController != null)
        {
            playerController.enabled = false;  
        }

        if (playerGun != null)
        {
            playerGun.enabled = false;  
        }
    }

    public void OpenSettings()
    {
        if(SettingsPanel != null)
        {
            SettingsPanel.SetActive(true);
            
        }
        if(pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(cameraButton.gameObject);
    }

    public void CloseSettings()
    {
        if (SettingsPanel != null)
        {
            SettingsPanel.SetActive(false);
            
        }
        if(pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
    }

    

    private void EnablePlayerInputs()
    {
        if (playerLocomotionInput != null)
        {
            playerLocomotionInput.enabled = true;  
        }

        if (playerController != null)
        {
            playerController.enabled = true;  
        }

        if (playerGun != null)
        {
            playerGun.enabled = true;  
        }
    }

    public void TriggerDeath()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
        isDead = true;

        deathMenuUI.SetActive(true);

        pauseMenuUI.SetActive(false);

        if (InGameUI != null)
        {   
            InGameUI.SetActive(false);
        }

        DisablePlayerInputs();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(retryButton.gameObject);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
