using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tragaperras : MonoBehaviour
{
    private string[] opciones = { "Null", "Bomba", "Corazon", "Moneda", "Silla", "Vel" };
    private int rolling = 0;
    private bool down = false;
    public Animator animator;
    private List<Animator> animSlots = new List<Animator>();
    private List<string> resultadoSlots = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Animator anim in GetComponentsInChildren<Animator>())
        {
            if (anim.gameObject != gameObject)
            {
                animSlots.Add(anim);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame) &&
            rolling == 0)
        {
            if (!down){
                down = true;
                animator.SetBool("Rolling", true);
                Debug.Log("Iniciar a rodar");
                StartCoroutine(Tirar(0.5f, animSlots[0], result =>
                {
                    resultadoSlots.Add(result);
                }));
                StartCoroutine(Tirar(1f, animSlots[1], result =>
                {
                    resultadoSlots.Add(result);
                }));
                StartCoroutine(Tirar(1.5f, animSlots[2], result =>
                {
                    resultadoSlots.Add(result);
                    Debug.Log("Resultados: " + string.Join(", ", resultadoSlots));
                    ActualizarEfectosPermamentes(resultadoSlots);

					resultadoSlots.Clear();
                }));
            } else
            {
                animator.SetBool("Rolling", false);
                Debug.Log("Aplicar buffs");
                down = false;
            }
        }
    }

    IEnumerator Tirar(float seg, Animator anim, Action<string> callback)
    {
        rolling++;
        anim.SetTrigger("Rolling");
        yield return new WaitForSeconds(seg);
        int index = UnityEngine.Random.Range(0, opciones.Length);
        string result = opciones[index];
        anim.SetTrigger(result);
        callback(result);
        rolling--;
    }

	public ConfigFuerza config;

	private void ActualizarEfectosPermamentes(List<string> tags)
	{

		if (tags == null || tags.Count != 3)
		{
			return;
		}

		if (tags[0] == tags[1] && tags[1] == tags[2])
		{
			string tagGanador = tags[0];

			switch (tagGanador)
			{
				case "Bomba":
					Debug.Log("¡Triple Match! Bomba");

					config.velocidadInicial += 5f;

					// Funcion de efecto cuando es triple
					break;

				case "Corazon":
					Debug.Log("¡Triple Match! Corazon");

					config.vidasInicial += 1;

					// Funcion de efecto cuando es triple
					break;

				case "Moneda":
					Debug.Log("¡Triple Match! Moneda");

					config.vidasInicial += 1;

					// Funcion de efecto cuando es triple
					break;

				case "Silla":
					Debug.Log("¡Triple Match! Silla");

					config.reboteInicial += 1;

					// Funcion de efecto cuando es triple
					break;

				case "Vel":
					Debug.Log("¡Triple Match! velocidaMaximaInicial");

					config.velocidaMaximaInicial += 1;

					// Funcion de efecto cuando es triple
					break;
			}
		}
	}
}
