using System;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Personagem : MonoBehaviour
{
    public Text UImoedas;
    public Text UIchave;
    public Text UIestrela;

    string TagObjetoTocado;
    string TriggerEnter;

    bool EstaTocandoAlgumColisor;
    bool ApertouBotaoPular;
    int ContadorPulos;
    int TotalPulos;

    float VelocidadeX;
    float VelocidadeY;

    float VelocidadeHorizontalMaxima;
    float DirecaoHorizontal;
    float VelocidadePuloSimples;
    float TempoBonusApple;


    int TotMoeda;
    int TotChave;
    int TotEstrela;

    bool Coletou;
    bool ApertouBotaoAtacar;
    bool AppleBonus;

    bool AtivarChave;
    bool JaCaiu;


    Vector2 VetorVelocidadePersonagem;

    Rigidbody2D CorpoRigidoPersonagem;
    Rigidbody2D CorpoRigidoChave;

    Animator Anima;
    Collider2D Colisor2dPersonagem;
    SpriteRenderer Renderer;
    PhysicsMaterial2D MaterialSemAtrito;

    Transform TransformObjetoCarregado;
    Transform TransformPersonagem;


    void Start()
    {
        TagObjetoTocado = "";
        TriggerEnter = "";

        ContadorPulos = 0;
        TotalPulos = 1;

        TempoBonusApple = 15.0f;

        AtivarChave = false;
        JaCaiu = false;

        Coletou = false;
        AppleBonus = false;
        ApertouBotaoPular = false;
        VelocidadePuloSimples = 18.0f;

        EstaTocandoAlgumColisor = false;

        Renderer = GetComponent<SpriteRenderer>();
        CorpoRigidoPersonagem = GetComponent<Rigidbody2D>();
        Anima = GetComponent<Animator>();
        Colisor2dPersonagem = GetComponent<Collider2D>();
        TransformPersonagem = GetComponent<Transform>();

        CorpoRigidoPersonagem.gravityScale = 5.0f;
        CorpoRigidoPersonagem.freezeRotation = true;

        VelocidadeX = 0f;
        VelocidadeY = 0f;
        VelocidadeHorizontalMaxima = 9.0f;
        DirecaoHorizontal = 0f;

        MaterialSemAtrito = new PhysicsMaterial2D();
        MaterialSemAtrito.friction = 0f;

        VetorVelocidadePersonagem = new Vector2(VelocidadeX, VelocidadeY);

        CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;
    }


    void Update()
    {
        MovimentoHorizontal();
        MovimentoPuloUnico();
        MovimentoPuloDuplo();
        Ganhou();
        AndarRapido();
        SoltarObjeto();
        VerificarBonus();
        VerificarChave();
    }


    void MovimentoHorizontal()
    {

        DirecaoHorizontal = Input.GetAxis("Horizontal");

        VelocidadeX = VelocidadeHorizontalMaxima * DirecaoHorizontal;

        VelocidadeY = CorpoRigidoPersonagem.velocity.y;

        VetorVelocidadePersonagem = new Vector2(VelocidadeX, VelocidadeY);

        CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;

        if (DirecaoHorizontal < 0 & CorpoRigidoPersonagem.gravityScale == 5)
        {
            Anima.SetBool("Run", true);
            Renderer.flipX = true;
        }
        else if (DirecaoHorizontal < 0 & CorpoRigidoPersonagem.gravityScale == -5)
        {
            Renderer.flipX = false;
        }
        if (DirecaoHorizontal > 0 & CorpoRigidoPersonagem.gravityScale == 5)
        {
            Anima.SetBool("Run", true);
            Renderer.flipX = false;
        }
        else if (DirecaoHorizontal > 0 & CorpoRigidoPersonagem.gravityScale == -5)
        {
            Renderer.flipX = true;
        }
        if (DirecaoHorizontal == 0)
        {
            Anima.SetBool("Run", false);
        }
    }

    void MovimentoPuloUnico()
    {

        ApertouBotaoPular = Input.GetButtonDown("Jump");

        EstaTocandoAlgumColisor = Colisor2dPersonagem.IsTouchingLayers();

        if (ApertouBotaoPular == true && EstaTocandoAlgumColisor == true)
        {

            VelocidadeX = CorpoRigidoPersonagem.velocity.x;

            VelocidadeY = VelocidadePuloSimples;

            VetorVelocidadePersonagem = new Vector2(VelocidadeX, VelocidadeY);

            CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;
        }

        if (EstaTocandoAlgumColisor == false)
        {
            Anima.SetBool("Jump", true);
        }
        else
        {
            Anima.SetBool("Jump", false);
        }

    }

    void MovimentoPuloDuplo()
    {
        ApertouBotaoPular = Input.GetButtonDown("Jump");
        if (ApertouBotaoPular == true && ContadorPulos < TotalPulos)
        {
            ContadorPulos = ContadorPulos + 1;

            VelocidadeX = CorpoRigidoPersonagem.velocity.x;

            VelocidadeY = VelocidadePuloSimples;

            VetorVelocidadePersonagem = new Vector2(VelocidadeX, VelocidadeY);

            CorpoRigidoPersonagem.velocity = VetorVelocidadePersonagem;
        }
    }



    void OnCollisionEnter2D(Collision2D objetoTocado)
    {
        TagObjetoTocado = objetoTocado.gameObject.tag;

        string tagTocada = objetoTocado.gameObject.tag;
        ContadorPulos = 0;

        if (TagObjetoTocado == "Espinho")
        {
            SceneManager.LoadScene("FaseGameOver");
            print("Voce morreu!!");

        }
        if (TagObjetoTocado.Contains("SemAtrito"))
        {
            print(" OnCollisionEnter2D():" + TagObjetoTocado);
            objetoTocado.collider.sharedMaterial = MaterialSemAtrito;
        }
        if (TagObjetoTocado == "Ganhar")
        {
            SceneManager.LoadScene("FaseGanhar");
        }
        if (TagObjetoTocado == "TiraPulo")
        {
            TotalPulos = 1;
        }
        if (TagObjetoTocado == "Jaula"  )
        {
            if (TransformObjetoCarregado != null && TransformObjetoCarregado.gameObject.tag == "ChaveVerde") 
            {
                GameObject RedStar = objetoTocado.transform.GetChild(0).gameObject;
                RedStar.transform.parent = null;

                Destroy(objetoTocado.gameObject);
            }
        }
    }

    void OnCollisionStay2D(Collision2D objetoPermaceTocando)
    {
        string tag = objetoPermaceTocando.gameObject.tag;
        if (tag == "ChaveVerde")
        {
            bool apertou = Input.GetKey(KeyCode.V);
            if (apertou == true)
            {
                objetoPermaceTocando.transform.parent = TransformPersonagem;
                TransformObjetoCarregado = objetoPermaceTocando.transform;
            }
        }

        if (tag.Contains("DestruirInimigos") == true)
        {
            bool apertou = Input.GetKey(KeyCode.LeftControl);
            if (apertou == true)
            {
                Destroy(objetoPermaceTocando.transform.parent.gameObject);
                AtivarChave = true;

                GameObject Tampa = GameObject.FindGameObjectWithTag("Tampa");
                if (Tampa != null)
                {
                    Destroy(Tampa);
                }
            }
        }
        if (tag.Contains("DestruirFlyingDemon") == true)
        {
            bool apertou = Input.GetKey(KeyCode.LeftControl);
            if (apertou == true)
            {
                Destroy(objetoPermaceTocando.transform.parent.gameObject);
            }
        }

        if (tag.Contains("TopoCaixa") == true)
        {
            bool apertouG = Input.GetKey(KeyCode.G);
            if (apertouG == true)
            {
                Transform Caixa = objetoPermaceTocando.transform.parent;

                GameObject GoldApple = null;
                for (int i = 0; i < Caixa.childCount; i++)
                {
                    if (Caixa.GetChild(i).name == "GoldApple")
                    {
                        GoldApple = Caixa.GetChild(i).gameObject;
                    }
                }

                if (GoldApple != null)
                {
                    GoldApple.transform.parent = null;
                }

                Destroy(Caixa.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D objetoTriggerEnter)
    {
        TriggerEnter = objetoTriggerEnter.gameObject.tag;

        if (TriggerEnter == "Coin")
        {
            TotMoeda = TotMoeda + 1;
            if (TotMoeda == 1)
                print("Voce tem o total de " + TotMoeda + " moeda");
            else
                print("Voce tem o total de " + TotMoeda + " moedas");
            Destroy(objetoTriggerEnter.gameObject);
            UImoedas.text = TotMoeda.ToString();
        }
        if (TriggerEnter == "+Velocidade")
        {
            VelocidadeHorizontalMaxima = VelocidadeHorizontalMaxima + 2f;
            print("Voce tem o total de " + VelocidadeHorizontalMaxima + " de velocidade");
            Destroy(objetoTriggerEnter.gameObject);
        }
        if (TriggerEnter == "-Velocidade")
        {
            VelocidadeHorizontalMaxima = VelocidadeHorizontalMaxima - 1f;
            print("Voce tem o total de " + VelocidadeHorizontalMaxima + " de velocidade");
            Destroy(objetoTriggerEnter.gameObject);
        }
        if (TriggerEnter == "-Gravidade")
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            CorpoRigidoPersonagem.gravityScale = -5f;
            Destroy(objetoTriggerEnter.gameObject);
        }
        if (TriggerEnter == "+Gravidade")
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            CorpoRigidoPersonagem.gravityScale = 5f;
            Destroy(objetoTriggerEnter.gameObject);
        }
        if (TriggerEnter == "+Pulo")
        {
            VelocidadePuloSimples = VelocidadePuloSimples + 1.5f;
            print("Voce tem o total de " + VelocidadePuloSimples + " de pulo");
            Destroy(objetoTriggerEnter.gameObject);
        }
        if (TriggerEnter == "-Pulo")
        {
            VelocidadePuloSimples = VelocidadePuloSimples - 0.5f;
            print("Voce tem o total de " + VelocidadePuloSimples + " de pulo");
            Destroy(objetoTriggerEnter.gameObject);
        }
        if (TriggerEnter == "Chave")
        {
            TotChave = TotChave + 1;
            if (TotChave == 1)
                print("Voce tem o total de " + TotChave + " chave");
            else
                print("Voce tem o total de " + TotChave + " chaves");
            Destroy(objetoTriggerEnter.gameObject);
            UIchave.text = TotChave.ToString();
        }
        if (TriggerEnter == "Estrela")
        {
            TotEstrela = TotEstrela + 1;
            if (TotEstrela == 1)
                print("Voce tem o total de " + TotEstrela + " estrela");
            else
                print("Voce tem o total de " + TotEstrela + " estrelas");
            Destroy(objetoTriggerEnter.gameObject);
            UIestrela.text = TotEstrela.ToString();
        }
        if (TriggerEnter == "PuloDuplo")
        {
            AppleBonus = true;
            TotalPulos = 2;
            Destroy(objetoTriggerEnter.gameObject);
        }
        if (TriggerEnter == "PuloTriplo")
        {
            AppleBonus = true;
            TotalPulos = 3;
            Destroy(objetoTriggerEnter.gameObject);
        }
        if (TriggerEnter == "AndarRapido")
        {
            Coletou = true;
            Destroy(objetoTriggerEnter.gameObject);
        }
        if (TriggerEnter == "Portal01")
        {
            Vector2 posicaoDestino = GameObject.FindGameObjectWithTag("Portal02").GetComponent<Transform>().position;
            float xDestino = posicaoDestino.x;
            float yDestino = posicaoDestino.y;
            xDestino = xDestino + 2.5f;
            transform.position = new Vector2(xDestino, yDestino);
        }
        if (TriggerEnter == "Portal02")
        {
            Vector2 posicaoDestino = GameObject.FindGameObjectWithTag("Portal01").GetComponent<Transform>().position;
            float xDestino = posicaoDestino.x;
            float yDestino = posicaoDestino.y;
            xDestino = xDestino - 2.5f;
            transform.position = new Vector2(xDestino, yDestino);
        }
    }
    void AndarRapido()
    {
        bool apertou = Input.GetKeyDown(KeyCode.E);
        if (apertou == true & Coletou == true)
        {
            VelocidadeHorizontalMaxima = VelocidadeHorizontalMaxima + 3;
        }

    }

    void VerificarBonus()
    {
        if (AppleBonus == true)
        {
            if (TempoBonusApple >= 0)
            {
                TempoBonusApple = TempoBonusApple - Time.deltaTime;
                print("Tempo Bonus Andar: " + TempoBonusApple);
            }
            else
            {
                AppleBonus = false;
                TotalPulos = 1;
            }

        }
    }

    void VerificarChave()
    {
        if (AtivarChave == true && JaCaiu == false)
        {
            JaCaiu = true;

            CorpoRigidoChave = GameObject.FindGameObjectWithTag("ChaveVerde").GetComponent<Rigidbody2D>();
            CorpoRigidoChave.gravityScale = 1.5f;

            Invoke("PararQueda", 1.6f);
        }
    }

    void PararQueda()
    {
        Destroy(CorpoRigidoChave);
    }

    void SoltarObjeto()
    {
        bool parouApertar = Input.GetKeyUp(KeyCode.V);
        if (parouApertar == true)
        {
            if (TransformObjetoCarregado != null)
            {
                TransformObjetoCarregado.parent = null;
                
                TransformObjetoCarregado = null;
                
            }
        }
    }
    void Ganhou()
    {
        if (TotMoeda == 50 && TotChave == 2 && TotEstrela == 3)
        {
            print("Você avançou de fase!!");
            SceneManager.LoadScene("Fase02");
        }
    }
}
