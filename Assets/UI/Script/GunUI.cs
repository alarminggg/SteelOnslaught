using TMPro;
using UnityEngine;

public class GunUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI reloadNotifText;
    [SerializeField] private Gun currentGun;
    
    void Update()
    {
        if(PauseMenu.GameIsPaused)
        {
            reloadNotifText.gameObject.SetActive(false);
            return;
        }
        if (currentGun != null)
        {
            if (currentGun.isReloading)
            {
                ammoText.text = "Reloading...";
                reloadNotifText.gameObject.SetActive(false);
            }
            else
            {
                ammoText.text = $"Ammo: {currentGun.currentAmmo}/{currentGun.MagazineSize}";

                if (currentGun.currentAmmo <= 0)
                {
                    reloadNotifText.gameObject.SetActive(true);
                }
                else
                {
                    reloadNotifText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetGun(Gun gun)
    {
        currentGun = gun;
    }
}
