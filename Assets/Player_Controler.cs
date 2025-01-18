using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    public float velocity = 5f;
    public int vida = 3;

    public float fuerzaSalto = 10f;
    public float fuerzaRebote = 10f;
    public float longitudRaycast = 1f;
    public LayerMask capaSuelo;

    private bool enSuelo;
    private bool recibeDanio;
    private bool ataque;
    private Rigidbody2D rb;
    public bool muerto;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!muerto)
        {
            if (!ataque)
            {
                Movimiento();

                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
                enSuelo = hit.collider != null;

                if (enSuelo && Input.GetButtonDown("Launch") && !recibeDanio)
                {
                    rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
                }
            }


            if (Input.GetKeyDown(KeyCode.Z) && !ataque && enSuelo)
            {
                Atacando();
            }
        }

        Animaciones();
    }


    public void Animaciones()
    {
        animator.SetBool("enSuelo", enSuelo);
        animator.SetBool("recibeDano", recibeDanio);
        animator.SetBool("Ataque", ataque);
        animator.SetBool("Muerto", muerto);
    }




    public void Movimiento()
    {
        float velocidadX = Input.GetAxis("Horizontal") * Time.deltaTime * velocity;

        animator.SetFloat("Movimiento", velocidadX * velocity);

        if (velocidadX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (velocidadX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        Vector3 posicion = transform.position;

        if (!recibeDanio)
            transform.position = new Vector3(velocidadX + posicion.x, posicion.y, posicion.z);
    }




    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibeDanio)
        {
            recibeDanio = true;
            vida -= cantDanio;
            if (vida <= 0)
            {
                muerto = true;
            }

            if (!muerto)
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
            }
        }
    }

    public void DesactivarDanio()
    {
        recibeDanio = false;
        rb.velocity = Vector2.zero;
    }




    public void Atacando()
    {
        ataque = true;
    }

    public void DesactivarAtaque()
    {
        ataque = false;
    }




    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}