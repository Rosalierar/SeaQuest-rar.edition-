using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeopleController : MonoBehaviour
{
    ObjectController surface;
    float[] posHorizontalSpawn = new float[3] { 13, 15, 17 };
    float[] posVerticalSpawn = new float[4] { 1.7f, 0.2f, -1.3f, -2.8f };

    int[] dir = new int[4];
    
    public bool restart = false;
    bool[] willSpawn = new bool[4]; // quais linhas vão spawnar

    int[] quantityForSpawn = new int[4]; // para cada cedula numeros de 0 a 1
    bool[] wasPassTheCam = new bool[4]; // qual das linhas já foi sorteado 

    public GameObject peoplePrefab;
    GameObject peopleSpawn0, peopleSpawn1, peopleSpawn2, peopleSpawn3;
    List<GameObject>[] peopleInLine = new List<GameObject>[4];
    int[] indexPosObj = new int[4]; // 1 por linha

    int level = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("DrawTheFirstTime", 4);
        for (int i = 0; i < peopleInLine.Length; i++)
        {
            peopleInLine[i] = new List<GameObject>();
        }
        
        Debug.Log("Start chamado");
    }

    // Update is called once per frame
    void Update()
    {
         for (int i = 0; i < 4; i++)
        {
            if (willSpawn[i] && !wasPassTheCam[i])
            {
                CheckPosLastPeople(i);
            }
        }
        // Se todas as linhas completaram (ou pelo menos alguma) e passaram da câmera, podemos sortear de novo
        if ((wasPassTheCam[0] || wasPassTheCam[1] || wasPassTheCam[2] || wasPassTheCam[3]) || restart)
        {
            restart = false;
            Invoke("Reset", 5f);
            Debug.Log("Alguma linha passou da câmera, reiniciando as que passaram...");
        }
    }

    void Reset()
    {
        Debug.Log("ResetLevel1 chamado");

        for (int i = 0; i < 4; i++)
        {
            wasPassTheCam[i] = false;
            willSpawn[i] = false;
            indexPosObj[i] = 0;
            quantityForSpawn[i] = 0;
            peopleInLine[i].Clear();

            if (i == 0)
            {
                if (peopleSpawn0 != null)
                    Destroy(peopleSpawn0);
            }
            else if (i == 1)
            {
                if (peopleSpawn1 != null)
                    Destroy(peopleSpawn1);
            }
            else if (i == 2)
            {
                if (peopleSpawn2 != null)
                    Destroy(peopleSpawn2);
            }
            else if (i == 3)
            {
                if (peopleSpawn3 != null)
                    Destroy(peopleSpawn3);
            }

            

        }
        DrawTheFirstTime();
        //Invoke("DrawTheFirstTime", 3.5f);
    }
    void DrawTheFirstTime() // chamar ao iniciar e ao perder vida
    {
        bool peloMenosUma = false;

        Debug.Log("DrawTheFirstTime chamado");

        for (int i = 0; i < willSpawn.Length; i++)
        {
            willSpawn[i] = Random.Range(0, 2) == 1; // se for 1 é true se for 0 é false

            if (willSpawn[i])
            {
                peloMenosUma = true;
                DrawGameController(i);
            }
        }
        
        if (!peloMenosUma)
        {
            int forcaLinha = Random.Range(0, 4);
            willSpawn[forcaLinha] = true;
            DrawGameController(forcaLinha);
            Debug.LogWarning($"[FORÇADO] Nenhuma linha sorteada! Forçando spawn na linha {forcaLinha}.");
        }
    }

    void DrawGameController(int i)
    {
        Debug.Log($"Linha {i} vai spawnar {quantityForSpawn[i]} objetos da direção {dir[i]}");

        dir[i] = Random.Range(0, 2) == 0 ? 0 : 180; //saber a direção na hora de spawnar

        quantityForSpawn[i] = Random.Range(0, 2); 

        StartCoroutine(RepeatSpawn(i, 0.5f));
    }


    private IEnumerator RepeatSpawn(int index, float delay)
    {
        Debug.Log($"Coroutine RepeatSpawn iniciada para linha {index}");

        while (indexPosObj[index] < quantityForSpawn[index])
        {
            SpawnController(index);
            yield return new WaitForSeconds(delay);

            indexPosObj[index]++;
        }
        Debug.Log($"Coroutine RepeatSpawn finalizada para linha {index}");
    }

    void SpawnController(int index) //Invokar repetidademente a cada x  tempo depois cancerlar 
    {
                
        int posX = dir[index] == 0 ? 1 : -1;

        Debug.Log($"SpawnGameObject chamado na linha {index}- direção {(dir[index] == 0 ? "esquerda" : "direita")}");

        SpawnGameObject(index, posX);
    }

    void SpawnGameObject(int index, int indexPos)
    {
        Quaternion rotation = Quaternion.Euler(0, dir[index], 0);
        Vector3 spawnPosition = new Vector3(indexPos * posHorizontalSpawn[indexPosObj[index]], posVerticalSpawn[index], 0);
        GameObject obj = Instantiate(peoplePrefab, spawnPosition, rotation);

        if (index == 0)
        {
            peopleSpawn0 = obj;
        }
        else if (index == 1)
        {
            peopleSpawn1 = obj;
        }
        else if (index == 2)
        {
            peopleSpawn2 = obj;
        }
        else if (index == 3)
        {
            peopleSpawn3 = obj;
        }

        peopleInLine[index].Add(obj);

        Debug.Log($"Instanciado objeto na linha {index} na posição {spawnPosition}");
    }

    void CheckPosLastPeople(int index)
    {
        GameObject target = null;

        if (index == 0)
            target = peopleSpawn0;
        else if (index == 1)
            target = peopleSpawn1;
        else if (index == 2)
            target = peopleSpawn2;
        else if (index == 3)
            target = peopleSpawn3;

        if (target == null)
        {
            Debug.LogWarning($"Linha {index} - Nenhum objeto para verificar (null).");

            if (peopleSpawn0 && peopleSpawn1 && peopleSpawn2 && peopleSpawn3)
            {
                Reset();
            }

            return;
        }

        Vector3 vpPos = Vector3.zero;

        if (index == 0)
        {
            vpPos = Camera.main.WorldToViewportPoint(peopleSpawn0.transform.position);
        }
        else if (index == 1)
        {
            vpPos = Camera.main.WorldToViewportPoint(peopleSpawn1.transform.position);
        }
        else if (index == 2)
        {
            vpPos = Camera.main.WorldToViewportPoint(peopleSpawn2.transform.position);
        }
        else if (index == 3)
        {
            vpPos = Camera.main.WorldToViewportPoint(peopleSpawn3.transform.position);
        }

       

        if (dir[index] == 0)
        {
            if (vpPos.x < 0f)
            {
                Debug.Log($"Linha {index} - inimigo saiu pela esquerda");

                Debug.Log("Saiu pela esquerda da câmera");
                wasPassTheCam[index] = true;
                Reset();
                // Controller de spawn
            }
        }
        else
        {
            if (vpPos.x > 1f)
            {
                Debug.Log($"Linha {index} - inimigo saiu pela direita");

                Debug.Log("Saiu pela direita da câmera");
                wasPassTheCam[index] = true;
                Reset();
            }
        }
    }
}
