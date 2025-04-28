using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigametrigger : MonoBehaviour
{
    public GameObject minigameUI;
    public GameObject interactionPrompt; // Este será el globo/mensaje
    private bool playerInRange = false;

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false); // Asegurarse de que esté oculto al principio
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ActivateMinigame();
        }
    }

    void ActivateMinigame()
    {
        minigameUI.SetActive(true);
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false); // Ocultamos el mensaje cuando entremos
        Time.timeScale = 0f; // Opcional, pausa el mundo
    }

    public void DeactivateMinigame()
    {
        minigameUI.SetActive(false);
        Time.timeScale = 1f; // Opcional, reanuda el mundo
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(true); // MOSTRAMOS el mensaje
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false); // OCULTAMOS el mensaje
        }
    }
}
