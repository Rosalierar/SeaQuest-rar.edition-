using UnityEngine;
using TMPro;

public class ShotController : MonoBehaviour
{
    [Header("MOVIMENTAÇÃO")]
    private float rotY;
    [SerializeField] bool cometoLeft; //Universal praticamente
    // MOVIMENTO LINEAR UNIFORME
    public Vector3 deslocamento;
    private float tempo;
    private Vector3 posicaoInicial;

    [Header("PONTUATION")]
    [SerializeField] TextMeshProUGUI textPontuation;
    [SerializeField] int pontuationInText;
    public int pontuationForShot = 20;

    public void SetDir(int dir)
    {
        deslocamento *= dir;
    }

    void Start()
    {
        posicaoInicial = transform.position;
        textPontuation = GameObject.Find("PontuationText").GetComponent<TextMeshProUGUI>();   
    }

    // Update is called once per frame
    void Update()
    {
        tempo += Time.deltaTime;
        transform.position = posicaoInicial + deslocamento * tempo;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("ShotS"))
        {
            print("SHOT CONTROLLER: Colisao: " + collision);
            Destroy(collision.gameObject);

            pontuationInText = int.Parse(textPontuation.text);
            int result = pontuationInText + pontuationForShot;
            textPontuation.text = result.ToString();

            Destroy(gameObject);
        }

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
