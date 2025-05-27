using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class passlvl : MonoBehaviour
{
    public int scene;

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene);
        }

    }

    public void play()
    {     
        SceneManager.LoadScene(1); 
    }

    public void tutorial()
    {
        SceneManager.LoadScene(7);
    }

    public void main()
    {
        SceneManager.LoadScene(0);
    }

    public void exit()
    {
      Application.Quit();
    }

    public void Retry()
    {
        if (Respawn.Instance != null && Respawn.Instance.hasCheckpoint)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Respawn.Instance.checkpointSceneIndex);
        }
        else
        {
            // Si no hay checkpoint, recarga el nivel actual (o uno por defecto)
            UnityEngine.SceneManagement.SceneManager.LoadScene(1); // reemplaza 1 con tu primer nivel
        }
    }
    public void ReturnToMenu()
    {
        if (Respawn.Instance != null)
        {
            Respawn.Instance.ResetCheckpoint();
        }

        SceneManager.LoadScene(0); // o el índice/nombre de tu menú principal
    }

}
