using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;

    public GameObject InGameUI;

    public GameObject SettingsPanel;

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
        if(isDead) return;
        {
            if (Input.GetKeyUp(KeyCode.P))
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
    }

    public void CloseSettings()
    {
        if (SettingsPanel != null)
        {
            SettingsPanel.SetActive(false);
            
        }
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
