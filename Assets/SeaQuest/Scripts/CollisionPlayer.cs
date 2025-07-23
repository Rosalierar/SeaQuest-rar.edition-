using UnityEngine;
using UnityEngine.UI;

public class CollisionPlayer : MonoBehaviour
{
    CollisionPlayer collision1;
    LifeController lifeController;
    PlayerController player;
    public TypeOfObject whatObject;

    public bool startGame; //SURFACE

    [Header("PEOPLE")]
    GameObject[] peopleObj;
    public bool getCatch;
    private byte maxPeople = 6;
    [SerializeField] int currentPeopleGet = 0;
    public GameObject parentPainelForImagePeople;
    public Image peopleImageCanva;
    public Sprite sprite;
    int[] posObjX = new int[6] { -200, -120, -40, 40, -120, -200 };
    int posY = 0;



    void Awake()
    {
        collision1 = GameObject.Find("Fundo").GetComponent<CollisionPlayer>();
        player = FindFirstObjectByType<PlayerController>();
        lifeController = FindFirstObjectByType<LifeController>();
        print("COLLISON CONTROLELR: Payer - " + player);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        switch (whatObject)
        {
            case TypeOfObject.Surface:
                player.canShot = false;
                lifeController.isDownSea = false;
                lifeController.tempo = 0;

                lifeController.barWarning = false;
                lifeController.handle.color = new Color(lifeController.R(1), lifeController.G(1), lifeController.B(1));

                if (!player.getPeople && startGame) //não pegou ninguem tira vida
                {
                    print("COLLISISON PLAYER: nao pegou nada");
                    lifeController.CallCoroutineSpawn();
                }

                else if (player.getPeople && startGame) // pegou alguem faz pontuação
                {
                    print("COLLISISON PLAYER: Colocando pontuação");
                }

                startGame = true;
                break;

            case TypeOfObject.People:
                if (!player.getPeople)
                    player.getPeople = true;

                lifeController.CallCoroutineSpawn();
                collision1.startGame = false;
                Destroy(gameObject);
                break;

            case TypeOfObject.Shot:
                lifeController.CallCoroutineSpawn();
                collision1.startGame = false;
                Destroy(gameObject);
                break;

            //dar dano no no jogador
            case TypeOfObject.Fish:
            case TypeOfObject.Shark:
                lifeController.CallCoroutineSpawn();
                collision1.startGame = false;
                Destroy(gameObject);
                break;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (whatObject == TypeOfObject.Surface && collision.CompareTag("Player"))
        {
            player.canShot = true;
            player.getPeople = false;

            lifeController.isDownSea = true;
            lifeController.tempo = 0;

            lifeController.barWarning = false;
            lifeController.handle.color = new Color(lifeController.R(1), lifeController.G(1), lifeController.B(1));
        }
    }

    void AddPeople()
    {
        int SetPeopleIndex = Mathf.Clamp(currentPeopleGet + 1, 0, maxPeople);
        currentPeopleGet = (byte)SetPeopleIndex; 

        int currentgetPeople = Mathf.Clamp(currentPeopleGet - 1, 0, maxPeople);
        currentgetPeople = currentPeopleGet;

        if (currentPeopleGet == 6 && peopleObj[currentgetPeople] != null)
        {
            return;
        }

        peopleObj[currentgetPeople] = new GameObject("SubPeople");

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
        player.ReSpawnController();

        peopleObj[currentPeopleGet] = null;
        print("RETIRANDO PESSOA DO CANVA");
    }
}
