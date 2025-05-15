using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench : MonoBehaviour
{
    [SerializeField] public Animator wrenchAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //wrenchAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void StartAttack()
    {
        wrenchAnimator.SetTrigger("Attack");
        Invoke("HideWrench", 0.2f); // Ajusta al tiempo real de tu animación
    }

    void HideWrench()
    {
        this.gameObject.SetActive(false);
    }
}
