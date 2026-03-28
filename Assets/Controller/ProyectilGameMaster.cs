using UnityEngine;

public class ProyectilGameMaster : MonoBehaviour
{
	public float velocidadConstante = 15f;
	private Rigidbody2D rb;
	private Vector2 ultimaVelocidad;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		// Lanzamiento inicial (el cañón)
		rb.linearVelocity = transform.right * velocidadConstante;
	}

	void Update()
	{
		// Guardamos la velocidad del frame anterior para calcular el rebote
		ultimaVelocidad = rb.linearVelocity;

		// El "Game Master" asegura que si no hay colisión, la velocidad no decaiga
		if (rb.linearVelocity.magnitude > 0 && rb.linearVelocity.magnitude != velocidadConstante)
		{
			rb.linearVelocity = rb.linearVelocity.normalized * velocidadConstante;
		}
	}

	private void OnCollisionEnter2D(Collision2D colision)
	{
		// El "Game Master" calcula la dirección del rebote basado en la normal del impacto
		var normal = colision.contacts[0].normal;
		var direccionRebote = Vector2.Reflect(ultimaVelocidad.normalized, normal);

		// Aplicamos la nueva dirección manteniendo la velocidad constante
		rb.linearVelocity = direccionRebote * velocidadConstante;

		// Lógica de "Historia": Si choca con el suelo, podrías decidir si muere o rebota
		if (colision.gameObject.CompareTag("Suelo"))
		{
			Debug.Log("El Game Master detectó impacto en suelo: Fin del vuelo.");
			// Aquí podrías frenarlo o disparar un evento de Game Over
		}
	}
}