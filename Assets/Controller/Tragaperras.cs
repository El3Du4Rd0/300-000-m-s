using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.STP;

public class Tragaperras : MonoBehaviour
{
    private string[] opciones = { "Null", "Bomba", "Corazon", "Moneda", "Vel" };
    private int rolling = 0;
    private bool down = false;
    public Animator animator;
    private List<Animator> animSlots = new List<Animator>();
    private List<string> resultadoSlots = new List<string>();
    public GameObject jugador;
    public ConfigFuerza config;
    public static event Action<string> mensaje;
	public ProyectilGameMaster proyectilGameMaster;

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
        jugador = GameObject.FindWithTag("Player");
        Debug.Log(jugador);

        if(jugador == null)
        {
            return;
		}

		config = jugador.GetComponent<ConfigFuerza>();
		proyectilGameMaster = jugador.GetComponent<ProyectilGameMaster>();

		if ((Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame) &&
            rolling == 0 &&
			config.vidasActual > 0)
        {
            if (!down && config.monedasActual > 0)
            {
                config.monedasActual -= 1;
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
					

					if (config != null)
                    {
                        ActualizarEfectosPermamentes(resultadoSlots);
                        
                    }
                    else
                    {
                        Debug.LogError("El objeto 'Player' existe, pero no tiene el script 'ConfigFuerza' pegado.");
                    }

                    
                }));
            }
            else if (!down)
            {
                mensaje?.Invoke("No money?");
            }
            else
            {
				ActualizarEfectosTemporales(resultadoSlots);
				animator.SetBool("Rolling", false);
                Debug.Log("Aplicar buffs");
                down = false;

				resultadoSlots.Clear();
			}
        } else if (config.vidasActual == 0)
        {
			mensaje?.Invoke("No Life No Gambling");
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
                    mensaje?.Invoke("Permanent upgrade to bombs");
                    config.velocidadInicial += 5f;
                    break;

                case "Corazon":
                    mensaje?.Invoke("Permanent upgrade to lives");
                    config.vidasInicial += 1;
                    break;

                case "Moneda":
                    mensaje?.Invoke("Permanent upgrade to coins");
                    config.vidasInicial += 1;
                    break;

                case "Silla":
                    mensaje?.Invoke("Permanent upgrade to chairs");
                    config.reboteInicial += 1;
                    break;

                case "Vel":
                    mensaje?.Invoke("Permanent upgrade to speed");
                    config.velocidaMaximaInicial += 1;
                    break;
            }
        }
    }

    private void ActualizarEfectosTemporales(List<string> tags)
    {

        if (tags == null || (tags[0] == tags[1] && tags[1] == tags[2]))
        {
            return;
        }

        var conteoTags = tags.GroupBy(x => x)
                         .ToDictionary(g => g.Key, g => g.Count());

        foreach (var conteadasTags in conteoTags)
        {
            switch (conteadasTags.Key)
            {
                case "Bomba":
                    config.velocidadActual += (int)conteadasTags.Value;
					proyectilGameMaster.DashBomba((int)conteadasTags.Value);

					break;
                case "Corazon":
                    config.vidasActual += (int)conteadasTags.Value;
                    break;
                case "Moneda":
                    config.monedasActual += (int)conteadasTags.Value;
                    break;
                case "Silla":
                    config.reboteActual += (int)conteadasTags.Value;
                    break;
                case "Vel":
                    config.velocidaMaximaActual += (int)conteadasTags.Value;
					proyectilGameMaster.DashVelocidad((int)conteadasTags.Value);
					break;
            }
        }
    }
}
