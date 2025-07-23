using System;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public TypeOfObject typeOfObject;
    public bool cometoLeft; //Universal praticamente

    // MOVIMENTO LINEAR UNIFORME
    public Vector3 deslocamento;
    public float tempo;
    public Vector3 posicaoInicial;
    float dir;

    // shark
    public bool isTheFirstSpawn;

    // SHAR E FISH
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
        switch (typeOfObject)
        {
            case TypeOfObject.Surface:
                break;
            case TypeOfObject.People:
            case TypeOfObject.Shot:
            case TypeOfObject.Shark:
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
        else if (typeOfObject == TypeOfObject.Shot )
        {
            
        }
        else if (typeOfObject == TypeOfObject.People || typeOfObject == TypeOfObject.Shark || typeOfObject == TypeOfObject.Fish)
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

    void OnBecameInvisible()
    {
        if (typeOfObject == TypeOfObject.Shot || typeOfObject == TypeOfObject.People)
        {
            Destroy(gameObject);
        }
    }
}
