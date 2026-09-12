using UnityEngine;
public class Inimigo : MonoBehaviour
{
    SpriteRenderer PersonagemSpriteRenderer;
    Vector2 PosicaoInicial;
    float TempoDecorrido;
    bool MovendoParaDireita = true;
    public float Distancia;
    public float VelocidadeMovimento;
    public Vector2 Direcao;

    string TagObjetoTocado;
    void Start()
    {
        TagObjetoTocado = "";
        PersonagemSpriteRenderer = GetComponent<SpriteRenderer>();
        PosicaoInicial = transform.position;
        TempoDecorrido = 0f;
    }
    void Update()
    {
        mover();
    }
    void mover()
    {
        float tempoAnterior = TempoDecorrido;
        TempoDecorrido += Time.deltaTime * VelocidadeMovimento;
        float movimento = Mathf.PingPong(TempoDecorrido, Distancia);
        if (Mathf.PingPong(tempoAnterior, Distancia) > Mathf.PingPong(TempoDecorrido, Distancia))
        {
            PersonagemSpriteRenderer.flipX = true;
        }
        else
        {
            PersonagemSpriteRenderer.flipX = false;
        }
        transform.position = PosicaoInicial + Direcao.normalized * movimento;
    }
}
