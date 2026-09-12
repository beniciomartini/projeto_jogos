using System;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{

    Transform TransformPlataforma;
    Vector2 PosicaoInicial;
    float TempoDecorrido;

    public float Distancia;
    public float VelocidadeMovimento;
    public Vector2 Direcao;

    void Start()
    {
        TransformPlataforma = GetComponent<Transform>();
        PosicaoInicial = TransformPlataforma.position;
        TempoDecorrido = 0f;
    }

    void Update()
    {
        mover();
    }

    void mover()
    {
        TempoDecorrido += Time.deltaTime * VelocidadeMovimento;
        float movimento = Mathf.PingPong(TempoDecorrido, Distancia);
        transform.position = PosicaoInicial + Direcao.normalized * movimento;
    }
    void OnCollisionEnter2D(Collision2D objetoTocado)
    {
        string tag = objetoTocado.gameObject.tag;
        if (tag.Contains("Player") == true)
        {
            objetoTocado.transform.parent = TransformPlataforma;
        }
    }
    void OnCollisionExit2D(Collision2D objetoParouTocar)
    {
        string tag = objetoParouTocar.gameObject.tag;
        if (tag.Contains("Player") == true)
        {
            objetoParouTocar.transform.parent = null;
        }
    }


}
