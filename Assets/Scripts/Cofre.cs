using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    // Algunos cofres desbloquean habilidades
    public Skills habilidad;

    public bool estaAbierto = false;

    // Especifica si este cofre se utiliza para pasar de una escena a otra
    public bool esCofreGanador = false;

    private Animator animator;
    private GameManager gm;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && esCofreGanador)
        {
            gm.showMessage();
        }

        if (collision.CompareTag("Player") && !estaAbierto && !esCofreGanador)
        {
            Debug.Log("El jugador esta en el cofre");
            animator.SetTrigger("abrirCofre");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && estaAbierto)
        {
            Debug.Log("El jugador se alejo del cofre");
        }
    }

    
}
