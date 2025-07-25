using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject lastSpawnedEnemyGlobal = null;
    float[] posHorizontalSpawn = new float[3] { 13, 15, 17 };
    float[] posVerticalSpawn = new float[4] { 1.7f, 0.2f, -1.3f, -2.8f };

    int[] dir = new int[4];
    public bool restartBecauseObj = false;
    public bool restartBecauseHasNothing = false;
    bool[] willSpawn = new bool[4]; // quais linhas vão spawnar


    int[] quantityForSpawn = new int[4]; // para cada cedula numeros de 0 a 2 
    bool[] wasPassTheCam = new bool[4]; // qual das linhas já foi sorteado 

    int[] indexFromTheLastObject = new int[4];

    public List<GameObject> totalEnemiesSpawned0 = new();
    public List<GameObject> totalEnemiesSpawned1 = new();
    public List<GameObject> totalEnemiesSpawned2 = new();
    public List<GameObject> totalEnemiesSpawned3 = new();

    public GameObject[] enemyPrefab;
    GameObject[] enemiesSpawn = new GameObject[4];
    List<GameObject>[] enemiesInLine = new List<GameObject>[4];
    GameObject[] theLastObjectFromLine = new GameObject[4];
    int[] indexPosObj = new int[4]; // 1 por linha

    int level = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("DrawTheFirstTime", 3);
        for (int i = 0; i < enemiesInLine.Length; i++)
        {
            enemiesInLine[i] = new List<GameObject>();
        }

        Debug.Log("Start chamado");
        //DrawTheFirstTime();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (willSpawn[i] && theLastObjectFromLine[i] != null && !wasPassTheCam[i])
            {
                CheckPosLastEnemy(i);
            }
        }

        if (totalEnemiesSpawned0 == null && totalEnemiesSpawned1 == null && totalEnemiesSpawned2 == null && totalEnemiesSpawned3 == null && restartBecauseObj)
        {
            restartBecauseObj = false;
            RestartLinesThatPassed();
        }
        // Se todas as linhas completaram (ou pelo menos alguma) e passaram da câmera, podemos sortear de novo
        else if ((wasPassTheCam[0] || wasPassTheCam[1] || wasPassTheCam[2] || wasPassTheCam[3]) || restartBecauseHasNothing)
        {
            restartBecauseHasNothing = false;
            RestartLinesThatPassed();
            Debug.Log("Alguma linha passou da câmera, reiniciando as que passaram...");
        }

        CheckAllStop();
    }

    void CheckAllStop()
    {
        bool noLinewillSpawn = true;

        for (int i = 0; i < 4; i++)
        {
            if (willSpawn[i]) // se alguma linha está marcada pra spawnar
            {
                Debug.Log($"linha {i} é true");
                noLinewillSpawn = false;
                break;
            }
        }

        if (noLinewillSpawn)
        {
            Debug.LogWarning("[SPAWN] Nenhuma linha foi sorteada. Re-desenhando...");
            DrawTheFirstTime();
        }
    }

    #region Nivel 1

    public void RestartLinesThatPassed()
    {
        bool peloMenosUma = false;

        for (int i = 0; i < 4; i++)
        {
            //if (wasPassTheCam[i])
            //{
            // Limpa objetos anteriores da linha
            foreach (GameObject obj in enemiesInLine[i])
            {
                if (obj != null) Destroy(obj);
            }

            switch (i)
            {
                case 0:
                    totalEnemiesSpawned0.Clear();
                    break;
                case 1:
                    totalEnemiesSpawned1.Clear();
                    break;
                case 2:
                    totalEnemiesSpawned2.Clear();
                    break;
                case 3:
                    totalEnemiesSpawned3.Clear();
                    break;
            }

            enemiesInLine[i].Clear();
            theLastObjectFromLine[i] = null;

            Debug.Log($"Reiniciando linha {i}");

            wasPassTheCam[i] = false;
            willSpawn[i] = Random.Range(0, 2) == 1;

            Debug.Log($"willSpawn " + willSpawn[i]);
            if (willSpawn[i])
            {
                indexPosObj[i] = 0;
                DrawGameController(i);
                peloMenosUma = true;
            }
            else
            {
                CheckAllStop();
            }
            //}
        }

        if (!peloMenosUma)
        {
            int forcaLinha = Random.Range(0, 4);
            willSpawn[forcaLinha] = true;
            indexPosObj[forcaLinha] = 0;
            DrawGameController(forcaLinha);
            Debug.LogWarning($"[FORÇADO] Nenhuma linha foi sorteada no restart! Forçando spawn na linha {forcaLinha}.");
        }
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

        quantityForSpawn[i] = Random.Range(1, 4); // quantidade de GameObjects que Serão spawnados

        indexFromTheLastObject[i] = quantityForSpawn[i];

        StartCoroutine(RepeatSpawn(i, 0.5f));
    }


    private IEnumerator RepeatSpawn(int index, float delay)
    {
        byte whichSprite = (byte)Random.Range(0, 10) < 6 ? (byte)0 : (byte)1;
        Debug.Log($"Coroutine RepeatSpawn iniciada para linha {index}");

        while (indexPosObj[index] < quantityForSpawn[index])
        {
            SpawnController(index, whichSprite);
            yield return new WaitForSeconds(delay);

            indexPosObj[index]++;
        }
        Debug.Log($"Coroutine RepeatSpawn finalizada para linha {index}");
    }

    void SpawnController(int index, byte whichSprite) //Invokar repetidademente a cada x  tempo depois cancerlar 
    {
        int posX = dir[index] == 0 ? 1 : -1;

        Debug.Log($"SpawnGameObject chamado na linha {index} - sprite {whichSprite} - direção {(dir[index] == 0 ? "esquerda" : "direita")}");

        SpawnGameObject(index, whichSprite, posX);
    }

    void SpawnGameObject(int index, byte whichSprite, int indexPos)
    {
        Quaternion rotation = Quaternion.Euler(0, dir[index], 0);
        Vector3 spawnPosition = new Vector3(indexPos * posHorizontalSpawn[indexPosObj[index]], posVerticalSpawn[index], 0);

        GameObject obj = Instantiate(enemyPrefab[whichSprite], spawnPosition, rotation);

        ObjectController enemyScript = obj.GetComponent<ObjectController>();
        enemyScript.indexInGameManager = 0;
        enemyScript.manager = this;

        switch (index)
        {
            case 0:
                totalEnemiesSpawned0.Add(obj);
                break;
            case 1:
                totalEnemiesSpawned1.Add(obj);
                break;
            case 2:
                totalEnemiesSpawned2.Add(obj);
                break;
            case 3:
                totalEnemiesSpawned3.Add(obj);
                break;
        }


        if (index == 0 && obj.GetComponent<ObjectController>().typeOfObject == TypeOfObject.Shark)
        {
            obj.GetComponent<ObjectController>().canShoting = true;
            obj.GetComponent<ObjectController>().InvokeSpawnShot();
        }
        else
        {
            obj.GetComponent<ObjectController>().canShoting = false;
        }

        enemiesInLine[index].Add(obj);
        theLastObjectFromLine[index] = obj;
        lastSpawnedEnemyGlobal = obj;
        
        Debug.Log($"Instanciado objeto na linha {index} na posição {spawnPosition}");
    }

    void CheckPosLastEnemy(int index)
    {
        if (theLastObjectFromLine[index] == null) return;

        Vector3 vpPos = Camera.main.WorldToViewportPoint(lastSpawnedEnemyGlobal.transform.position);

        if (dir[index] == 0)
        {
            if (vpPos.x < 0f)
            {
                Debug.Log($"Linha {index} - inimigo saiu pela esquerda");

                Debug.Log("Saiu pela esquerda da câmera");
                wasPassTheCam[index] = true;
                restartBecauseObj = true;

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
                restartBecauseObj = true;
            }
        }
    }
    void CheckIfLastGlobalEnemyPassedCamera()
    {
        if (lastSpawnedEnemyGlobal == null) return;

        Vector3 vpPos = Camera.main.WorldToViewportPoint(lastSpawnedEnemyGlobal.transform.position);

        bool saiuDaTela =
            vpPos.x < 0f || vpPos.x > 1f;

        if (saiuDaTela)
        {
            Debug.Log("[RESET] Último inimigo global saiu da câmera.");
            lastSpawnedEnemyGlobal = null;
            restartBecauseObj = true;
        }
    }

    bool AllEnemiesDestroyed()
    {
        return totalEnemiesSpawned0.Count == 0 &&
            totalEnemiesSpawned1.Count == 0 &&
            totalEnemiesSpawned2.Count == 0 &&
            totalEnemiesSpawned3.Count == 0;
    }
    #endregion Nivel 1

    public void RemoveEnemy(GameObject obj, int listIndex)
    {
        switch (listIndex)
        {
            case 0: totalEnemiesSpawned0.Remove(obj); break;
            case 1: totalEnemiesSpawned1.Remove(obj); break;
            case 2: totalEnemiesSpawned2.Remove(obj); break;
            case 3: totalEnemiesSpawned3.Remove(obj); break;
        }
    }
}

