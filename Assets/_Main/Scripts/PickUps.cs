using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    [SerializeField] public int MenosVida;
    [SerializeField] public int MasVida;
    [SerializeField] private Animator itemAnimator;

    [SerializeField] public int Particles;

    public int itemIndex;

    public bool isHealing;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.spriteIndex = itemIndex;
            player.particleCount = Particles;

            if (MenosVida > 0)
            {
                player.playerAnimator.SetTrigger("IsAttacked");
            }

            if (isHealing)
            {
                player.AddHealth(MasVida);

            }
            else
            {

                player.TakeDamage(MenosVida);
            }


            this.gameObject.SetActive(false);
        }
    }
}
