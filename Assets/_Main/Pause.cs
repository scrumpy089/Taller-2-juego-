using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;

    [Header("Audio")]
    [SerializeField] private AudioSource[] musicSources;
    [SerializeField] private float pausedPitch = 0.55f;
    [SerializeField] private float normalPitch = 1f;

    private bool isPaused = false;

    void Start()
    {
        pauseUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                DeactivatePause();
            }
            else
            {
                ActivatePause();
            }
        }
    }

    public void ActivatePause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        foreach (AudioSource source in musicSources)
        {
            if (source != null)
            {
                source.pitch = pausedPitch;
            }
        }
    }

    public void DeactivatePause()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        foreach (AudioSource source in musicSources)
        {
            if (source != null)
            {
                source.pitch = normalPitch;
            }
        }
    }
}