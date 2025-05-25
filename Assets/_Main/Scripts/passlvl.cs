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
}
