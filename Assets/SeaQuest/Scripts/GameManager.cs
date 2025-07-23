using System.Collections;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    float[] posHorizontalSpawn = new float[3] { 13, 15, 17 };
    float[] posVerticalSpawn = new float[4] { 1.7f, 0.2f, -1.3f, -2.8f };

    int[] dir = new int[4];
    bool restart = false;
    bool[] willSpawn = new bool[4]; // quais linhas vão spawnar


    int[] quantityForSpawn = new int[4]; // para cada cedula numeros de 0 a 2 
    bool[] wasPassTheCam = new bool[4]; // qual das linhas já foi sorteado 

    int[] indexFromTheLastObject = new int[4];

    public GameObject[] enemyPrefab;
    GameObject[] enemiesSpawn = new GameObject[4];
    GameObject[] theLastObjectFromLine = new GameObject[4];
    int indexPosObj = 0;

    int level = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("DrawTheFirstTime", 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeLevel()
    {
        level = (level + 1) % 4;
    }

    void ControlLevel()
    {
        switch (level)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    #region Nivel 1
    void Reset()
    {
        indexPosObj = 0;
    }
    void DrawTheFirstTime() // chamar ao iniciar e ao perder vida
    {
        for (int i = 0; i < willSpawn.Length; i++)
        {
            willSpawn[i] = Random.Range(0, 2) == 1; // se for 1 é true se for 0 é false

            if (willSpawn[i])
            {
                DrawGameController(i);
            }
        }
    }

    void DrawGameController(int i)
    {
        dir[i] = Random.Range(0, 2) == 0 ? 0 : 180; //saber a direção na hora de spawnar

        quantityForSpawn[i] = Random.Range(0, 3); // quantidade de GameObjects que Serão spawnados

        indexFromTheLastObject[i] = quantityForSpawn[i];

        StartCoroutine(RepeatSpawn(i, 0.5f));
    }


    private IEnumerator RepeatSpawn(int index, float delay)
    {
        while (indexPosObj < quantityForSpawn[index])
        {
            SpawnController(index);
            yield return new WaitForSeconds(delay);

            indexPosObj++;
        }
    }

    void SpawnController(int index) //Invokar repetidademente a cada x  tempo depois cancerlar 
    {
        for (int i = 0; i < enemiesSpawn.Length; i++)
        {
            if (i == index)
            {
                byte whichSprite = (byte)Random.Range(0, 10) < 6 ? (byte)0 : (byte)1;
                int posX = dir[i] == 0 ? 1 : -1;

                SpawnGameObject(index, whichSprite, posX);
            }
        }
    }

    void SpawnGameObject(int index, byte whichSprite, int indexPos)
    {
        Quaternion rotation = Quaternion.Euler(0, dir[index], 0);
        enemiesSpawn[index] = Instantiate(enemyPrefab[whichSprite], new Vector3(indexPos * posHorizontalSpawn[indexPosObj], posVerticalSpawn[index]), rotation);

        //indexPosObj++;
    }

    void CheckPosLastEnemy(int index)
    {
        Vector3 vpPos = Camera.main.WorldToViewportPoint(theLastObjectFromLine[index].transform.position);

        if (dir[index] == 0)
        {
            if (vpPos.x < 0f)
            {
                Debug.Log("Saiu pela esquerda da câmera");
                wasPassTheCam[index] = true;

                // Controller de spawn
            }
        }
        else
        {
            if (vpPos.x > 1f)
            {
                Debug.Log("Saiu pela direita da câmera");
                wasPassTheCam[index] = true;
            }
        }
    }

    #endregion Nivel 1

    #region Nivel 2
    #endregion Nivel 2

    #region Nivel 3
    #endregion Nivel 3

    #region Nivel 4
    #endregion Nivel 4   
}
