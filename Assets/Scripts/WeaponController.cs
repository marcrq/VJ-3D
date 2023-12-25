using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject pistolPBR; // Referencia al GameObject del arma de pistola
    public GameObject assaultRiflePBR; // Referencia al GameObject del arma de rifle de asalto
    public Animator animador; // Referencia al Animator del personaje

    void Start()
    {
        // Desactivar arma de rifle de asalto al inicio
        assaultRiflePBR.SetActive(false);
        pistolPBR.SetActive(true);
    }

    void Update()
    {
        // Verificar si se presiona la tecla M para cambiar el arma
        if (Input.GetKeyDown(KeyCode.M))
        {
            CambiarArma();
        }
    }

    public void CambiarArma()
    {
        // Cambiar la visibilidad de las armas
        pistolPBR.SetActive(!pistolPBR.activeSelf);
        assaultRiflePBR.SetActive(!assaultRiflePBR.activeSelf);

        // Cambiar la animación del personaje según el arma equipada
        if (pistolPBR.activeSelf)
        {
            // Configurar el parámetro del Animator para la animación del arma de pistola
            animador.SetTrigger("PistolTrigger");
        }
        else
        {
            // Configurar el parámetro del Animator para la animación del arma de rifle de asalto
            animador.SetTrigger("RifleTrigger");
        }
    }
}