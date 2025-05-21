using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Private Variables")]
    [Space]

    [Tooltip("La vida máxima que el jugador puede tener (se puede modificar desde Unity).")]
    [SerializeField] private float maxHealth = 4;
    [Tooltip("Referencia al Rigidbody2D del jugador, usado para aplicar física y movimiento.")]
    [SerializeField] private Rigidbody2D rb;
    [Tooltip("Un transform que indica la posición donde se verifica si el jugador está tocando el suelo.")]
    [SerializeField] private Transform groundCheck;
    [Tooltip("El radio de la zona de verificación del suelo (es un círculo).")]
    [SerializeField] private float groundRadius;
    [Tooltip("La capa en la que se consideran los objetos \"suelo\".")]
    [SerializeField] private LayerMask GroundLayer;
    [Tooltip("La capa en la que se consideran los objetos \"barro\", que afectan al movimiento del jugador.")]
    [SerializeField] private LayerMask mudLayer;
    [Tooltip("El sistema de partículas que se reproducirá cuando el jugador reciba daño.")]
    [SerializeField] private ParticleSystem hit_ps;
    [Tooltip("El componente Animator para controlar las animaciones del jugador.")]
    [SerializeField] public Animator playerAnimator;

    private float movement;

    [Header("Public Variables")]
    [Space]

    [Tooltip("La vida actual del jugador.")]
    public float health;
    [Tooltip("La velocidad de movimiento del jugador.")]
    public float speed;
    [Tooltip("La fuerza de salto del jugador.")]
    public float jumpForce;
    [Tooltip("Determina si el jugador puede saltar (está en el suelo o no en barro).")]
    public bool canJump;
    [Tooltip("El tiempo que el jugador estará siendo empujado después de recibir un golpe.")]
    public float hitTime;
    [Tooltip("La fuerza de empuje en el eje horizontal cuando el jugador recibe un golpe.")]
    public float hitForceX;
    [Tooltip("La fuerza de empuje en el eje vertical cuando el jugador recibe un golpe.")]
    public float hitForceY;
    [Tooltip("Determina si el golpe fue desde la derecha o no (usado para direccionar el empuje).")]
    public bool hitFromRight;
    [Tooltip("Texto en la UI que muestra la vida del jugador.")]
    public TextMeshProUGUI healthText;
    [Tooltip("La cantidad de partículas que se generarán cuando el jugador reciba daño.")]
    public float particleCount;


    [SerializeField] private Sprite[] particleSprites;
    public int spriteIndex;

    public PlatformEffector2D effector;
    public float waitTime = 0.5f;

    private float lastTapTimeRight = 0f;
    private float lastTapTimeLeft = 0f;
    public float doubleTapThreshold = 0.3f;

    private bool isSprinting = false;
    public float sprintMultiplier = 2f;

    private int jumpCount = 0;
    public int maxJumps = 1;

    public GameObject wrenchObject;

    [SerializeField] private Animator lifeBarAnimator;

    // Start is called before the first frame update
    void Start()
    {
        health = 4;
        rb = GetComponent<Rigidbody2D>(); 
        playerAnimator = GetComponent<Animator>();  
        hit_ps = GetComponentInChildren<ParticleSystem>();

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        // En nivel 1 solo puede hacer un salto, en nivel 2 o más: doble salto
        if (currentScene == 2) // o el índice del nivel donde desbloqueas doble salto
        {
            maxJumps = 1;
        }
        else
        {
            maxJumps = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Physics2D.OverlapCircle(groundCheck.position, groundRadius, GroundLayer))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
        */

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, GroundLayer);
        canJump = grounded;

        if (grounded)
        {
            jumpCount = 0; 
        }

        /*
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && canJump == true)
        {          
            playerAnimator.SetTrigger("IsJumping");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
        }
        */

        if ((//Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.W)) && jumpCount < maxJumps)
        {
            playerAnimator.SetTrigger("IsJumping");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }

        playerAnimator.SetFloat("IsMoving", movement);

        movement = Input.GetAxisRaw("Horizontal");
        //movement = 0f;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement = 0f;
        }

        // Detecta Sprint a la derecha
        if (Input.GetKeyDown(KeyCode.D))
        {
            
            if (Time.time - lastTapTimeRight < doubleTapThreshold)
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
                lastTapTimeRight = Time.time;
            }
        }

        // Detecta Sprint a la derecha
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            if (Time.time - lastTapTimeLeft < doubleTapThreshold)
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
                lastTapTimeLeft = Time.time;
            }
        }
        // Detiene sprint si no hay movimiento
        if (movement == 0)
        {
            isSprinting = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimator.SetTrigger("Attack");
            wrenchObject.SetActive(true);
            wrenchObject.GetComponentInChildren<Wrench>().StartAttack();
        }
        

        playerAnimator.SetBool("IsSprinting", isSprinting);

        if (Input.GetKeyDown(KeyCode.S))
        {
            effector.rotationalOffset = 180f;
            StartCoroutine(ResetPlatform());
        }

        if (movement < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        else if (movement > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (hitTime <= 0)  
        {

            //transform.Translate(Time.deltaTime * (Vector2.right * movement) * speed);
            transform.Translate(Time.deltaTime * Vector2.right * movement * speed * (isSprinting ? sprintMultiplier : 1f));
            //rb.velocity = new Vector2(movement * speed * (isSprinting ? sprintMultiplier : 1f), rb.velocity.y);

        }
        else  
        {
            
            if (hitFromRight)
            {
                rb.velocity = new Vector2(-hitForceX, hitForceY); 
            }
            else if (!hitFromRight)
            {
                rb.velocity = new Vector2(hitForceX, hitForceY);  
            }
            hitTime -= Time.deltaTime;  

            //playerAnimator.SetTrigger("IsAttacked");  
        
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //playerAnimator.SetTrigger("IsAttacked");
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage; 
        //healthText.text = $"Health: {health}/{maxHealth}"; 

        if (health < 0)
        {  
            health = 0; 
        }

        lifeBarAnimator.SetInteger("HealthLevel", (int)health);

        //ParticleSystem.Burst burst = hit_ps.emission.GetBurst(0);  
        //ParticleSystem.MinMaxCurve count = burst.count; 

        //count.constant = particleCount;  
        //burst.count = count;  

        //hit_ps.emission.SetBurst(0, burst);  
        //hit_ps.textureSheetAnimation.SetSprite(0, particleSprites[spriteIndex]);
        
        //hit_ps.Play(); 
       

    }

    // Función para agregar salud al jugador.
    public void AddHealth(float _health)
    {
        if (health + _health > maxHealth)  
        {
            health = maxHealth;

        }
        else
        {
            health += _health; 
        }

        lifeBarAnimator.SetInteger("HealthLevel", (int)health);

        //healthText.text = $"Health: {health}/{maxHealth}"; 

        ParticleSystem.Burst burst = hit_ps.emission.GetBurst(0);   
        ParticleSystem.MinMaxCurve count = burst.count; 

        count.constant = particleCount; 
        burst.count = count;  

        hit_ps.emission.SetBurst(0, burst);  

        hit_ps.textureSheetAnimation.SetSprite(0, particleSprites[spriteIndex]);
        ;
        hit_ps.Play();
    }

    IEnumerator ResetPlatform()
    {
        yield return new WaitForSeconds(waitTime);
        effector.rotationalOffset = 0f;
    }

}

