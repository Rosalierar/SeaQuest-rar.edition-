using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("COMPONENTES")]
    Rigidbody2D rb;
    Animator anim;

    [Header("reviver")]
    Vector2 spawnpoint;

    [Header("TELA")]
    public bool getPeople = false;

    [Header("TROCAR LADO DO PERSONAGEM")]
    [SerializeField] bool isFacingRight = false;

    [Header("MOVIMENTAÇÃO")]
    public bool canMove;
    [SerializeField] SpriteRenderer spriteP;
    [SerializeField] float speed;
    float moveX;
    float moveY;

    [Header("ATIRAR")]
    [SerializeField] GameObject shotPrefab;
    [SerializeField] GameObject lampObj;
    [SerializeField] Transform localSpawn;
    [SerializeField] float countdown;
    public bool canShot = true;
    float countdownLeft;
    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteP = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);

        spawnpoint = transform.position;
        countdownLeft = countdown;
    }

    void Update()
    {
        Shot();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (canMove)
        {
            Vector3 posNaTela = Camera.main.WorldToScreenPoint(transform.position);

            float margem = 210f;
            float moveX = Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;
            float moveY = Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime;

            posNaTela.x += moveX;
            posNaTela.y += moveY;

            posNaTela.x = Mathf.Clamp(posNaTela.x, margem, Screen.width - margem);
            posNaTela.y = Mathf.Clamp(posNaTela.y, margem, Screen.height - margem);

            Vector3 posFinal = Camera.main.ScreenToWorldPoint(posNaTela);

            rb.MovePosition(posFinal);

            if (moveX > 0 && !isFacingRight)
            {
                anim.SetBool("isWalking", true);
                Flip(180);
            }
            else if (moveX < 0 && isFacingRight)
            {
                anim.SetBool("isWalking", true);
                Flip(0);
            }

            if (posFinal == Vector3.zero)
            {
                anim.SetBool("isWalking", false);
            }
        }
    }
    bool FacinfRight()
    {
        return isFacingRight;
    }

    void Shot()
    {
        if (canShot)
            countdownLeft -= Time.deltaTime;

        if (countdownLeft <= 0 && canShot)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F))
            {
                canShot = false;
                StartCoroutine(ActiveLamp());
            }
        }
    }

    IEnumerator ActiveLamp()
    {
        lampObj.SetActive(true);
        GameObject obj = Instantiate(shotPrefab, localSpawn.transform.position, Quaternion.identity);

        if (!FacinfRight())
        {
            obj.GetComponent<ShotController>().SetDir(-1);
        }
        else
        {
            obj.GetComponent<ShotController>().SetDir(1);
        }

        yield return new WaitForSeconds(0.1f);

        countdownLeft = countdown;
        lampObj.SetActive(false);
        canShot = true;
        StopCoroutine(ActiveLamp());
    }

    void Flip(float rot)
    {
        isFacingRight = !isFacingRight;
        //spriteP.flipX = enabled;
        transform.rotation = Quaternion.Euler(0f, rot, 0f);
    }

    public void ReSpawnController()
    {
        anim.SetTrigger("isDeath");
        Invoke("Spawn", 1);
    }

    void Spawn()
    {
        transform.position = spawnpoint;
    }
}
