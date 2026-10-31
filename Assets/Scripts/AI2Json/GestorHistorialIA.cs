using System;
using System.IO;
using UnityEngine;

public static class GestorHistorialIA
{
    private static string directorio = Application.persistentDataPath;

    public static void GuardarNuevaRespuesta(string nuevaRespuesta)
    {
        string nombreArchivo = $"historial_ia_{DateTime.Now:yyyyMMdd_HHmmss}.json";
        string rutaCompleta = Path.Combine(directorio, nombreArchivo);
        string jsonLimpiado = ExtraccionJSON.ExtraerObjetos(nuevaRespuesta);
        File.WriteAllText(rutaCompleta, jsonLimpiado);

        Debug.Log($"Nueva respuesta guardada en: {rutaCompleta}");
    }

    public static void GuardarRespuestaForzada(string res)
    {
        string nombreArchivo = $"historial_ia_{DateTime.Now:yyyyMMdd_HHmmss}.json";
        string rutaCompleta = Path.Combine(directorio, nombreArchivo);
        res = ExtraccionJSON.ExtraerObjetos(res);
        File.WriteAllText(rutaCompleta, res);

        Debug.Log($"Nueva respuesta guardada en: {rutaCompleta}");
    }
}
