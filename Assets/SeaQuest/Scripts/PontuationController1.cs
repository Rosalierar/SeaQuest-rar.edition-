using UnityEngine;
using TMPro;

public class PontuationController1 : MonoBehaviour
{
    int pontuationShot = 20;
    int lastPontuation = 0;
    ShotController shot;

    [Header("PONTUATION")]
    [SerializeField] TextMeshProUGUI textPontuation;
    [SerializeField] int pontuationInText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoreDamage();

        //textPontuation.text = result.ToString();
    }

    void MoreDamage()
    {
        pontuationInText = int.Parse(textPontuation.text);

        if (lastPontuation + 1000 >= pontuationInText)
        {
            pontuationShot = shot.pontuationForShot;
            shot.pontuationForShot = Mathf.Clamp(pontuationShot + 10, 20, 90);

            lastPontuation = pontuationInText;
        }
    }
}
