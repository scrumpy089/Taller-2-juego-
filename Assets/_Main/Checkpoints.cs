using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoints : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Respawn.Instance.SetCheckpoint(transform.position, SceneManager.GetActiveScene().buildIndex);
        }
    }
}
