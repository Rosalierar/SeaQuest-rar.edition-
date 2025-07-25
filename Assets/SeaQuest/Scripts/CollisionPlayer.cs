using UnityEngine;
using UnityEngine.UI;

public class CollisionPlayer : MonoBehaviour
{
    PeopleController peopleController;
    CollisionPlayer collision1;
    LifeController lifeController;
    PlayerController player;
    public TypeOfObject whatObject;

    public bool startGame; //SURFACE


    void Awake()
    {
        peopleController = FindFirstObjectByType<PeopleController>();
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
                    player.canMove = false;
                    print("COLLISISON PLAYER: Colocando pontuação");
                    collision1.GetComponent<ObjectController>().ComecarResgateVisual();
                }

                startGame = true;
                break;

            case TypeOfObject.People:
                if (!collision1.GetComponent<ObjectController>().estacheio)
                {
                    if (!player.getPeople)
                            player.getPeople = true;

                    peopleController.restart = true;
                    collision1.GetComponent<ObjectController>().pessoasResgatadas.Add(gameObject);
                    collision1.GetComponent<ObjectController>().AddPeople();
                    player.getPeople = true;
                    Destroy(gameObject);
                }
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
            //player.getPeople = false;

            lifeController.isDownSea = true;
            lifeController.tempo = 0;

            lifeController.barWarning = false;
            lifeController.handle.color = new Color(lifeController.R(1), lifeController.G(1), lifeController.B(1));
        }
    }

    
}
