using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    private void Start()
    {
        if (cam == null && Camera.main != null)
        {
            cam = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}
