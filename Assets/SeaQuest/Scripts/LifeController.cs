using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour
{
    PlayerController player;
    public bool playing = true;
    public bool isSpawning;
    public bool isDownSea = false;
    public GameObject painelDeafeat;

    [Header("OCILAÇÃO DE COR")]
    private bool lostLife = false;
    private Coroutine currentCoroutine;
    public Image handle;  
    Color color1, color2;
    float baseR;
    float baseG;
    float baseB;

    [Header("PERDER/RECEBER VIDA")]

    /// <summary>
    /// PERDER/RECEBER VIDA
    /// </summary>

    public bool barWarning;
    private byte maxLife = 10;
    [SerializeField] int currentLife = 3;
    public GameObject parentPainelForImage;
    public GameObject[] obgLife;
    public Image lifeImageCanva;
    public Sprite sprite;

    int[] posObjX = new int[10] { -460, -360, -260, -160, -60, 40, 140, 240, 340, 440 };
    int posY = -33;

    [Header("MOVIMENTAÇÃO")]

    /// <summary>
    /// MOVIMENTAÇÃO Linear Scrol BAR
    /// </summary>

    [SerializeField] private float lifeValue;
    public Scrollbar lifeBar;
    [SerializeField] private float velocidadeRecarga;
    [SerializeField] private float vel;
    public float tempo;
    private float valueInicial, valueAtual;

    void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        valueInicial = lifeValue;
        valueAtual = lifeValue;

        for (int i = 0; i < obgLife.Length; i++)
        {
            if (i < currentLife)
            {
                obgLife[i] = new GameObject("SubLife");

                RectTransform rect = obgLife[i].AddComponent<RectTransform>();
                obgLife[i].AddComponent<CanvasRenderer>();

                lifeImageCanva = obgLife[i].AddComponent<Image>();
                lifeImageCanva.sprite = sprite;

                obgLife[i].transform.SetParent(parentPainelForImage.transform, false);

                rect.sizeDelta = new Vector2(80, 50);
                rect.anchoredPosition = new Vector2(posObjX[i], posY);

                print("LIFE CONTROLLER: OBJECT IMAGE" + obgLife);
            }
            print("LIFE CONTROLLER: OBJECT IMAGE" + lifeImageCanva);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            LowerLifeBar();
        }
    }

    void LowerLifeBar()
    {
        if (isDownSea && !isSpawning)
        {
            tempo += Time.deltaTime;
            lifeValue = valueInicial + -vel * tempo; //formula do sorvete diminuindo
            lifeValue = Mathf.Clamp(lifeValue, 0, 100);

            float result = lifeValue / 100;
            valueAtual = lifeValue; //pega a novo quantidade 

            lifeBar.size = result;
            lifeBar.size = Mathf.Clamp(lifeBar.size, 0, 1);

            if (lifeValue < 30)
            {
                if (!barWarning)
                {
                    handle.color = new Color(R(1), G(1), B(1));
                    barWarning = true;

                    if (currentCoroutine != null)
                        StopCoroutine(currentCoroutine);

                    currentCoroutine = StartCoroutine(EraseTheBar(true));
                }
                if (lifeValue < 1)
                {
                    handle.color = new Color(R(2), G(2), B(2));
                    CallCoroutineSpawn();
                    GameObject.Find("Fundo").GetComponent<CollisionPlayer>().startGame = false;
                }
            }
            else
            {
                if (barWarning)
                {
                    barWarning = false;

                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                        currentCoroutine = null;
                    }

                    handle.color = new Color(R(1), G(1), B(1));
                }
            }

            print("IS DOWN SEA");
        }
        else if (!isDownSea && !isSpawning)
        {
            tempo += Time.deltaTime;
            lifeValue = valueAtual + (vel + velocidadeRecarga) * tempo; //formula do sorvete aumentando
            lifeValue = Mathf.Clamp(lifeValue, 0, 100);

            float result = lifeValue / 100;
            valueInicial = lifeValue; //pega um novo inicio 

            lifeBar.size = result;
            lifeBar.size = Mathf.Clamp(lifeBar.size, 0, 1);

            if (lifeValue < 99)
            {
                if (!barWarning)
                {
                    handle.color = new Color(R(1), G(1), B(1));
                    barWarning = true;

                    if (currentCoroutine != null)
                        StopCoroutine(currentCoroutine);

                    currentCoroutine = StartCoroutine(EraseTheBar(false));
                }

                player.canMove = false;
            }
            else
            {
                if (barWarning)
                {
                    barWarning = false;

                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                        currentCoroutine = null;
                    }

                    handle.color = new Color(R(1), G(1), B(1));
                }

                if (!player.getPeople)
                    player.canMove = true;
                    //GameObject.FindAnyObjectByType<GameManager>().GetComponent<GameManager>().restartBecauseObj = true;
            }

            print("ISN'T DOWN SEA");
        }
    }

    public void CallCoroutineSpawn()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        DeleteHearth();
        isSpawning = true;

        yield return new WaitForSeconds(0.9f);

        isSpawning = false;
        StopCoroutine(Spawning());
    }

    public void AddHearth()
    {
        int currentHearth = Mathf.Clamp(currentLife - 1, 0, maxLife);
        currentLife = currentHearth;

        if (currentLife == -1)
        {
            print("Perdeu");
        }

        obgLife[currentHearth] = new GameObject("SubLife");

        RectTransform rect = obgLife[currentHearth].AddComponent<RectTransform>();

        obgLife[currentHearth].AddComponent<CanvasRenderer>();
        lifeImageCanva = obgLife[currentHearth].AddComponent<Image>();

        lifeImageCanva.sprite = sprite;

        obgLife[currentHearth].transform.SetParent(parentPainelForImage.transform, false);

        rect.sizeDelta = new Vector2(80, 50);
        rect.anchoredPosition = new Vector2(posObjX[currentHearth], posY);

        print("LIFE CONTROLLER: OBJECT IMAGE" + obgLife);
    }

    void DeleteHearth()
    {
        int currentHearth = Mathf.Clamp(currentLife - 1, -1, maxLife);

        currentLife = (byte)currentHearth;

        if (currentHearth == -1)
        {
            painelDeafeat.SetActive(true);
            return;
        }

        Destroy(obgLife[currentLife]);
        player.ReSpawnController();

        print("RETIRANDO VIDA");
    }

    public float R(byte index)
    {
        switch (index)
        {
            case 1: // AZUL
                baseR = 0.6470588f;
                break;
            case 2: //VERMELO
                baseR = 0.9686275F;
                break;
            case 3: //BRANCO
                baseR = 1f;
                break;
        }

        return baseR;
    }

    public float G(byte index)
    {
        switch (index)
        {
            case 1: // AZUL
                baseG = 0.8823529f;
                break;
            case 2: //VERMELO
                baseG = 0.6470588f;
                break;
            case 3: //BRANCO
                baseG = 1f;
                break;
        }

        return baseG;
    }

    public float B(byte index)
    {
        switch (index)
        {
            case 1: // AZUL
                baseB = 0.9686275f;
                break;
            case 2: //VERMELO
                baseB = 0.6606777f;
                break;
            case 3: //BRANCO
                baseB = 1f;
                break;
        }

        return baseB;
    }

    IEnumerator EraseTheBar(bool lostL)
    {
        lostLife = lostL;
        //RGB AZUL - R 0.6470588 G 0.8823529 B 0.9686275
        //RGB VERMELHO - R 0.9686275 G 0.6470588 B 0.6606777
        //RGB BRANCO - R 1 G 1 B 1

        print("Handle"+ handle.gameObject.name + " LOSTLIFE "+ lostL);

        while (barWarning)
        {
            float t = Mathf.PingPong(Time.time, 1f); // varia entre 0 e 1

            if (lostL && isDownSea)
            {
                print("ESTÁ ENBAIXO E PERDENDO");
                color1 = new Color(R(1), G(1), B(1));
                color2 = new Color(R(2), G(2), B(2));
            }
            
            if (!lostL && !isDownSea)
            {
                print("ESTÁ ACIMA E REGENERANDO");
                color1 = new Color(R(1), G(1), B(1));
                color2 = new Color(R(3), G(3), B(3));
            }

            handle.color = Color.Lerp(color1, color2, t);

            Debug.Log("Ação com controle...");
            yield return null;
        }
    }
}
