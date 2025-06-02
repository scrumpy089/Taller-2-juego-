using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivatePause();
        }
    }

    void ActivatePause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DeactivatePause()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f; 
    }
}
