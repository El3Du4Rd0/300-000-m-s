using System;
using UnityEngine;

public class ProyectilGameMaster : MonoBehaviour
{
    public GameObject apendices;
    public ConfigFuerza config;

    private Vector2 ultimaVelocidad;
    public Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Static;
	}

    public void Lanzar()
    {
        transform.rotation = Quaternion.Euler(0, 0, 52.394f);

        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 direccion = transform.right;
        rb.AddForce(direccion * config.velocidadInicial, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        ultimaVelocidad = rb.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D colision)
    {
        var normal = colision.contacts[0].normal;
        var direccionRebote = Vector2.Reflect(ultimaVelocidad.normalized, normal);

        rb.linearVelocity = direccionRebote * ultimaVelocidad.magnitude;

        if (colision.gameObject.CompareTag("Suelo"))
        {
            Debug.Log("Impacto en suelo");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstaculo"))
        {
            config.vidasActual--;
            config.velocidadActual = config.velocidadActual / 2;
        }
    }
}