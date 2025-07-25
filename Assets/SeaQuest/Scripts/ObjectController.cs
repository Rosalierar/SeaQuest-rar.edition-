using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{

    PlayerController player;
    public TypeOfObject typeOfObject;
    public bool cometoLeft; //Universal praticamente

    // MOVIMENTO LINEAR UNIFORME
    public Vector3 deslocamento;
    public float tempo;
    public Vector3 posicaoInicial;
    float dir;

    /// <summary>
    /// RESGATE DE PESSOAQS
    /// </summary>
    /// 
    public bool estacheio;
    public int pontuacaoAtual = 0;
    public int pontuacaoVisivel = 0;

    public TextMeshProUGUI textPontuation;
    public int pontosPorPessoa = 100;
    public float delayEntrePessoas = 0.2f;
    public List<GameObject> pessoasResgatadas = new List<GameObject>();


    GameObject[] peopleObj = new GameObject[6];
    public bool getCatch;
    private byte maxPeople = 6;
    [SerializeField] int currentPeopleGet = -1;
    public GameObject parentPainelForImagePeople;
    public Image peopleImageCanva;
    public Sprite sprite;
    int[] posObjX = new int[6] { -200, -120, -40, 40, 120, 200 };
    int posY = 0;



    // shark
    public bool isTheFirstSpawn;
    float countdown;
    public float timeCountDown;

    public GameObject shotSharkPrefab;
    public bool canShoting;

    // SHAR E FISH
    public int indexInGameManager;
    public GameManager manager;
    public float rotY;
    public bool objectPassTheCam;

    void Awake()
    {
        rotY = transform.rotation.y;

        if (rotY == 0)
        {
            cometoLeft = true;
            dir = -1;
            deslocamento *= -1;
        }
        else
        {
            cometoLeft = false;
            dir = +1;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = FindAnyObjectByType<GameManager>();

        switch (typeOfObject)
        {
            case TypeOfObject.Surface:
                textPontuation = GameObject.Find("PontuationText").GetComponent<TextMeshProUGUI>();
                player = FindFirstObjectByType<PlayerController>();
                parentPainelForImagePeople = GameObject.Find("PanelPeople");
                break;
            case TypeOfObject.People:
            case TypeOfObject.Shark:
                countdown = timeCountDown;
                posicaoInicial = transform.position;
                break;
            case TypeOfObject.Shot:
            case TypeOfObject.Fish:
                posicaoInicial = transform.position;
                break;
        }

        print("OBJECT CONTROLLER: Posicao Inicial -  " + posicaoInicial);
    }

    public void SetDeslocamento(float vel)
    {
        vel *= dir;
        deslocamento = new Vector3(vel, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckPosition();
    }

    void Movement()
    {
        tempo += Time.deltaTime;

        if (typeOfObject == TypeOfObject.Surface)
        {

        }
        else if (typeOfObject == TypeOfObject.People || typeOfObject == TypeOfObject.Shark || typeOfObject == TypeOfObject.Fish || typeOfObject == TypeOfObject.Shot)
        {

            transform.position = posicaoInicial + deslocamento * tempo;
        }
    }

    void CheckPosition()
    {
        switch (typeOfObject)
        {
            case TypeOfObject.Surface:
                break;

            case TypeOfObject.People:
                break;

            case TypeOfObject.Shot:
                break;

            case TypeOfObject.Shark:
            case TypeOfObject.Fish:
                Vector3 vpPos = Camera.main.WorldToViewportPoint(transform.position);
                if (cometoLeft)
                {
                    if (vpPos.x < 0f)
                    {
                        Debug.Log("Saiu pela esquerda da câmera");
                        objectPassTheCam = true;
                    }
                }
                else
                {
                    if (vpPos.x > 1f)
                    {
                        Debug.Log("Saiu pela direita da câmera");
                        objectPassTheCam = true;
                    }
                }
                break;
        }
    }

    public void DestroObject()
    {
        Destroy(gameObject);
    }

    public void InvokeSpawnShot()
    {
        InvokeRepeating("SpawnShot", 1f, countdown);
    }

    void SpawnShot()
    {
        GameObject obj = Instantiate(shotSharkPrefab, transform.position, transform.rotation);
        print("GAMEOBJECT DO TUBARAO: " + obj.name);
    }

    void OnBecameInvisible()
    {
        if (typeOfObject == TypeOfObject.Shot || typeOfObject == TypeOfObject.People)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShotS") && (typeOfObject == TypeOfObject.Shark || typeOfObject == TypeOfObject.Fish))
        {
            manager.RemoveEnemy(gameObject, indexInGameManager);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

    }

    #region resgatePessoas

    public void ComecarResgateVisual()
    {
        StartCoroutine(ResgatarComPontuacaoVisual());
    }

    private IEnumerator ResgatarComPontuacaoVisual()
    {
        foreach (GameObject pessoa in pessoasResgatadas)
        {
            Destroy(pessoa);
            DeletePeople();

            yield return new WaitForSeconds(delayEntrePessoas + 0.3f);
        }

        foreach (GameObject pessoa in pessoasResgatadas)
        {
            //pontuacaoAtual += pontosPorPessoa + int.Parse(textPontuation.text);

            Destroy(pessoa);

            StartCoroutine(AumentarPontuacaoVisual(pontosPorPessoa));

            yield return new WaitForSeconds(delayEntrePessoas);
        }

        pessoasResgatadas.Clear();
        player.getPeople = false;
        estacheio = false;
    }

    private IEnumerator AumentarPontuacaoVisual(int pontos)
    {
        int pontosAdicionados = 0;

        while (pontosAdicionados < pontos)
        {
            pontuacaoVisivel += 10; 
            pontosAdicionados += 10;

            if (pontosAdicionados > pontos)
            {
                pontuacaoVisivel = pontuacaoAtual;
            }
            
            int result = int.Parse(textPontuation.text) + pontuacaoVisivel;
            textPontuation.text = result.ToString();
            yield return new WaitForSeconds(0.05f);
        }


    }
    
    public void AddPeople()
    {
        int SetPeopleIndex = Mathf.Clamp(currentPeopleGet + 1, 0, maxPeople);
        currentPeopleGet = (byte)SetPeopleIndex;

        int currentgetPeople = Mathf.Clamp(currentPeopleGet - 1, 0, maxPeople);
        print("ADD PEOPLE CHAMADO " + currentgetPeople + "" + currentPeopleGet);
       
            peopleObj[currentgetPeople] = new GameObject("SubPeople");

        if (currentPeopleGet == 6)
        {
            estacheio = true;
        }

        RectTransform rect = peopleObj[currentgetPeople].AddComponent<RectTransform>();

        peopleObj[currentgetPeople].AddComponent<CanvasRenderer>();
        peopleImageCanva = peopleObj[currentgetPeople].AddComponent<Image>();

        peopleImageCanva.sprite = sprite;

        peopleObj[currentgetPeople].transform.SetParent(parentPainelForImagePeople.transform, false);

        rect.sizeDelta = new Vector2(60, 35);
        rect.anchoredPosition = new Vector2(posObjX[currentgetPeople], posY);

        print("COLLISION PLAYER: OBJECT IMAGE" + peopleObj);
    }

    void DeletePeople()
    {
        int currentgetPeople = Mathf.Clamp(currentPeopleGet - 1, 0, maxPeople);

        currentPeopleGet = (byte)currentgetPeople;
        Destroy(peopleObj[currentPeopleGet]);

        peopleObj[currentPeopleGet] = null;
        print("RETIRANDO PESSOA DO CANVA");

        if(currentPeopleGet == 0)
            player.canMove = true;
    }
    
    #endregion resgatePessoas
}
