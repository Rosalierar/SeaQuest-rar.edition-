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
    int lastPontuation = 0;
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

        MoreDamage();
    }

    void MoreDamage()
    {
        pontuationInText = int.Parse(textPontuation.text);

        if (pontuationInText > lastPontuation + 1000)
        {
            pontuationForShot = Mathf.Clamp(pontuationForShot + 10, 20, 90);

            lastPontuation = pontuationInText;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("ShotS") &&  !collision.CompareTag("People"))
        {
            print("SHOT CONTROLLER: Colisao: " + collision);
            Destroy(collision.gameObject);

            pontuationInText = int.Parse(textPontuation.text);
            int result = pontuationInText + pontuationForShot;
            textPontuation.text = result.ToString();
        }

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
