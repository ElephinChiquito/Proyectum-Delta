using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ExtraccionJSON
{
    public static string ExtraerObjetos(string respuestaIA)
    {
        string extraido = "";

        LinkedList<string> marcasDeInicio = new LinkedList<string>();
        marcasDeInicio.AddLast("{\n    \"objects\": [");
        marcasDeInicio.AddLast("{\n   \"objects\": [");
        marcasDeInicio.AddLast("{\n  \"objects\": [");
        marcasDeInicio.AddLast("{\n \"objects\": [");
        marcasDeInicio.AddLast("{\n\"objects\": [");

        marcasDeInicio.AddLast("{\n    \"Objects\": [");
        marcasDeInicio.AddLast("{\n   \"Objects\": [");
        marcasDeInicio.AddLast("{\n  \"Objects\": [");
        marcasDeInicio.AddLast("{\n \"Objects\": [");
        marcasDeInicio.AddLast("{\n\"Objects\": [");

        marcasDeInicio.AddLast("{\n    \"objects\":[");
        marcasDeInicio.AddLast("{\n   \"objects\":[");
        marcasDeInicio.AddLast("{\n  \"objects\":[");
        marcasDeInicio.AddLast("{\n \"objects\":[");
        marcasDeInicio.AddLast("{\n\"objects\":[");

        marcasDeInicio.AddLast("{\n    \"Objects\":[");
        marcasDeInicio.AddLast("{\n   \"Objects\":[");
        marcasDeInicio.AddLast("{\n  \"Objects\":[");
        marcasDeInicio.AddLast("{\n \"Objects\":[");
        marcasDeInicio.AddLast("{\n\"Objects\":[");

        marcasDeInicio.AddLast("{\r\n    \"objects\": [");
        marcasDeInicio.AddLast("{\r\n   \"objects\": [");
        marcasDeInicio.AddLast("{\r\n  \"objects\": [");
        marcasDeInicio.AddLast("{\r\n \"objects\": [");
        marcasDeInicio.AddLast("{\r\n\"objects\": [");

        marcasDeInicio.AddLast("{\r\n    \"Objects\": [");
        marcasDeInicio.AddLast("{\r\n   \"Objects\": [");
        marcasDeInicio.AddLast("{\r\n  \"Objects\": [");
        marcasDeInicio.AddLast("{\r\n \"Objects\": [");
        marcasDeInicio.AddLast("{\r\n\"Objects\": [");

        marcasDeInicio.AddLast("{\r\n    \"objects\":[");
        marcasDeInicio.AddLast("{\r\n   \"objects\":[");
        marcasDeInicio.AddLast("{\r\n  \"objects\":[");
        marcasDeInicio.AddLast("{\r\n \"objects\":[");
        marcasDeInicio.AddLast("{\r\n\"objects\":[");

        marcasDeInicio.AddLast("{\r\n    \"Objects\":[");
        marcasDeInicio.AddLast("{\r\n   \"Objects\":[");
        marcasDeInicio.AddLast("{\r\n  \"Objects\":[");
        marcasDeInicio.AddLast("{\r\n \"Objects\":[");
        marcasDeInicio.AddLast("{\r\n\"Objects\":[");

        marcasDeInicio.AddLast("{\"Objects\":[");
        marcasDeInicio.AddLast("{ \"Objects\":[");
        marcasDeInicio.AddLast("{  \"Objects\":[");
        marcasDeInicio.AddLast("{   \"Objects\":[");
        marcasDeInicio.AddLast("{    \"Objects\":[");
		
        marcasDeInicio.AddLast("{\"Objects\": [");
        marcasDeInicio.AddLast("{ \"Objects\": [");
        marcasDeInicio.AddLast("{  \"Objects\": [");
        marcasDeInicio.AddLast("{   \"Objects\": [");
        marcasDeInicio.AddLast("{    \"Objects\": [");

        marcasDeInicio.AddLast("{\"objects\":[");
        marcasDeInicio.AddLast("{ \"objects\":[");
        marcasDeInicio.AddLast("{  \"objects\":[");
        marcasDeInicio.AddLast("{   \"objects\":[");
        marcasDeInicio.AddLast("{    \"objects\":[");

        marcasDeInicio.AddLast("{\"objects\": [");
        marcasDeInicio.AddLast("{ \"objects\": [");
        marcasDeInicio.AddLast("{  \"objects\": [");
        marcasDeInicio.AddLast("{   \"objects\": [");
        marcasDeInicio.AddLast("{    \"objects\": [");
        
        int inicio = -1;
        LinkedListNode<string> cursor = marcasDeInicio.First;
        while(inicio == -1 && cursor != null)
        {
            string marcaInicio = cursor.Value;
            inicio = ConseguirInicio(respuestaIA, marcaInicio);

            Debug.Log("inicio: " + inicio + " | marcaInicio: " + marcaInicio);
            cursor = cursor.Next;
        }
        
        //Last Chance
        if(inicio == -1)
        {
            marcasDeInicio = new LinkedList<string>();
            marcasDeInicio.AddLast("````");
            marcasDeInicio.AddLast("```");
            marcasDeInicio.AddLast("``");
            marcasDeInicio.AddLast("````json");
            marcasDeInicio.AddLast("```json");
            marcasDeInicio.AddLast("``json");
            marcasDeInicio.AddLast("```` json");
            marcasDeInicio.AddLast("``` json");
            marcasDeInicio.AddLast("`` json");
            marcasDeInicio.AddLast("````  json");
            marcasDeInicio.AddLast("```  json");
            marcasDeInicio.AddLast("``  json");
            marcasDeInicio.AddLast("````   json");
            marcasDeInicio.AddLast("```   json");
            marcasDeInicio.AddLast("``   json");   
            marcasDeInicio.AddLast("json");
            
            cursor = marcasDeInicio.First;
            string marcaInicio = null;
            while(inicio == -1 && cursor != null)
            {
                marcaInicio = cursor.Value;
                inicio = ConseguirInicio(respuestaIA, marcaInicio);

                Debug.Log("inicio: " + inicio + " | marcaInicio: " + marcaInicio);
                cursor = cursor.Next;
            }
            if(inicio != -1)
            {
                string respuestaIA2 = respuestaIA.Substring(inicio);
                //La posicion donde empieza ``` json, + la posicion del { a partir de ahi.
                //Tal vez haya que sumarle + 1.
                inicio = inicio + ConseguirInicio(respuestaIA2, "{");
            }
            
        }
        if(inicio == -1)
        {
            Debug.LogError("ERROR, comienzo del json no encontrado.");
            //Last desperate attempt
            inicio = ConseguirInicio(respuestaIA, "{");
        }
        
        extraido = respuestaIA.Substring(inicio);


        // Buscar la posici�n final del JSON (buscamos el primer "]}")
		// Por cambiar el json con activity singular, tuve que cambiar ] por }
		// Pero se queda como last resort ]}
        int indiceFinal = extraido.LastIndexOf("}\n}");
        if (indiceFinal == -1)
        {
            indiceFinal = extraido.LastIndexOf("}\r\n}");
        }
        if (indiceFinal == -1)
        {
            indiceFinal = extraido.LastIndexOf("}}");
            if(indiceFinal != -1)
                indiceFinal = indiceFinal-1;
        }
        if (indiceFinal == -1)
        {
            indiceFinal = extraido.LastIndexOf("} }");
        }
        if (indiceFinal == -1)
        {
            indiceFinal = extraido.LastIndexOf("}  }");
            if(indiceFinal != -1)
                indiceFinal = indiceFinal+1;
        }
        if (indiceFinal == -1)
        {
            indiceFinal = extraido.LastIndexOf("]\r\n}");
        }
        if (indiceFinal == -1)
        {
            indiceFinal = extraido.LastIndexOf("]}");
            if(indiceFinal != -1)
                indiceFinal = indiceFinal-1;
        }
        if (indiceFinal == -1)
        {
            indiceFinal = extraido.LastIndexOf("] }");
        }
        if (indiceFinal == -1)
        {
            indiceFinal = extraido.LastIndexOf("]  }");
            if(indiceFinal != -1)
                indiceFinal = indiceFinal+1;
        }
        if (indiceFinal == -1)
        {
            Debug.LogWarning("No se encontro el final del JSON ']}'.");
            return null;
        }

        // Cortamos justo hasta despu�s de "]}" (incluido)
        Debug.Log("inicio = " + inicio + " | final = " + indiceFinal+4 + " | longitud del array = " + respuestaIA.Length);

        string jsonLimpiado = "";
        if(indiceFinal + 4 > extraido.Length)
            jsonLimpiado = extraido.Substring(0);
        else
            jsonLimpiado = extraido.Substring(0, indiceFinal + 4);
        Debug.Log("Respuesta limpia guardada correctamente.");

        return jsonLimpiado;
    }

    private static int ConseguirInicio(string respuestaIA, string marcaInicio)
    {
        int inicio = respuestaIA.LastIndexOf(marcaInicio);
        Debug.Log("inicio: " + inicio);

        if (inicio == -1)
            Debug.LogWarning("No se encontro el inicio del JSON. Con marcaInicio: " + marcaInicio);

        return inicio;
    }

}