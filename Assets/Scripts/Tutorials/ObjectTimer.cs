using System.Collections;
using UnityEngine;

public class ObjectTimer : MonoBehaviour
{
    public GameObject objetoAMostrar; // Arrastra aquí el objeto desde la jerarquía
    public float segundos = 3.0f;    // Tiempo que durará visible

    // Este método se asignará al evento OnClick del botón
    public void IniciarMostrado()
    {
        StartCoroutine(MostrarYEsconder());
    }

    IEnumerator MostrarYEsconder()
    {
        // 1. Activamos el objeto
        objetoAMostrar.SetActive(true);

        // 2. Esperamos la cantidad de segundos configurada
        yield return new WaitForSeconds(segundos);

        // 3. Lo volvemos a desactivar
        objetoAMostrar.SetActive(false);
    }
}