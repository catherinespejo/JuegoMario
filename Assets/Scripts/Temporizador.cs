using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Temporizador : MonoBehaviour
{

    [SerializeField] int min = 5, seg = 0;
    [SerializeField] Text tiempo = null;

    private float rest;
    private bool enMarcha;

    private void Awake()
    {
        rest = (min * 60) - seg;
        enMarcha = true;
    }


    // Update is called once per frame
    void Update()
    {
        
        if (enMarcha)
        {
            rest -= Time.deltaTime;
            Debug.Log(rest);
            if (rest < 1)
            {
                enMarcha = false;
                // Si llegamos al ultimo nivel que desarrollamos y el jugador se le acaba
                // el tiempo, retorna al primer nivel.
                Scene escenaActiva = SceneManager.GetActiveScene();
                if (escenaActiva.buildIndex >= 2) {
                    SceneManager.LoadScene(1);
                    return;
                }
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().retryLevel();
                return;
            }

            int temMin = Mathf.FloorToInt(rest / 60);
            int temSeg = Mathf.FloorToInt(rest % 60);
            tiempo.text = string.Format("{00:00}:{01:00}", temMin, temSeg);
        }
    }
}
