/*
 * Copyright (c) 2019 Dummiesman
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
*/

using Dummiesman;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class MTLLoader {
    public List<string> SearchPaths = new List<string>() { "%FileName%_Textures", string.Empty};

    private FileInfo _objFileInfo = null;

    /// <summary>
    /// The texture loading function. Overridable for stream loading purposes.
    /// </summary>
    /// <param name="path">The path supplied by the OBJ file, converted to OS path seperation</param>
    /// <param name="isNormalMap">Whether the loader is requesting we convert this into a normal map</param>
    /// <returns>Texture2D if found, or NULL if missing</returns>
    public virtual Texture2D TextureLoadFunction(string path, bool isNormalMap)
    {
        //find it
        foreach (var searchPath in SearchPaths)
        {
            //replace varaibles and combine path
            string processedPath = (_objFileInfo != null) ? searchPath.Replace("%FileName%", Path.GetFileNameWithoutExtension(_objFileInfo.Name)) 
                                                          : searchPath;
            string filePath = Path.Combine(processedPath, path);

            //return if eists
            if (File.Exists(filePath))
            {
                Texture2D tex = null;
                try
                {
                    tex = ImageLoader.LoadTexture(filePath);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Fallo al cargar " + filePath + ": " + e.Message);

                    // Si era un .dds, intenta con .png
                    if (Path.GetExtension(filePath).ToLower() == ".dds")
                    {
                        string fallbackPng = Path.ChangeExtension(filePath, ".png");
                        if (File.Exists(fallbackPng))
                        {
                            Debug.Log("Intentando cargar textura alternativa: " + fallbackPng);
                            tex = ImageLoader.LoadTexture(fallbackPng);
                        }
                    }
                }

                if(isNormalMap)
                    tex = ImageUtils.ConvertToNormalMap(tex);

                return tex;
            }
        }

        //not found
        return null;
    }

    private Texture2D TryLoadTexture(string texturePath, bool normalMap = false)
    {
        //swap directory seperator char
        texturePath = texturePath.Replace('\\', Path.DirectorySeparatorChar);
        texturePath = texturePath.Replace('/', Path.DirectorySeparatorChar);

        return TextureLoadFunction(texturePath, normalMap);
    }
    
    private int GetArgValueCount(string arg)
    {
        switch (arg)
        {
            case "-bm":
            case "-clamp":
            case "-blendu":
            case "-blendv":
            case "-imfchan":
            case "-texres":
                return 1;
            case "-mm":
                return 2;
            case "-o":
            case "-s":
            case "-t":
                return 3;
        }
        return -1;
    }

    private int GetTexNameIndex(string[] components)
    {
        for(int i=1; i < components.Length; i++)
        {
            var cmpSkip = GetArgValueCount(components[i]);
            if(cmpSkip < 0)
            {
                return i;
            }
            i += cmpSkip;
        }
        return -1;
    }

    private float GetArgValue(string[] components, string arg, float fallback = 1f)
    {
        string argLower = arg.ToLower();
        for(int i=1; i < components.Length - 1; i++)
        {
            var cmp = components[i].ToLower();
            if(argLower == cmp)
            {
                return OBJLoaderHelper.FastFloatParse(components[i+1]);
            }
        }
        return fallback;
    }

    string debugString;
    private string GetTexPathFromMapStatement(string processedLine, string[] splitLine)
    {
        int texNameCmpIdx = GetTexNameIndex(splitLine);
        if (texNameCmpIdx < 0)
        {
            Debug.LogError($"texNameCmpIdx < 0 on line {processedLine}. Texture not loaded.");
            return null;
        }

        int texNameIdx = processedLine.IndexOf(splitLine[texNameCmpIdx]);
        string texturePath = processedLine.Substring(texNameIdx);

        // Si es una ruta absoluta, nos quedamos solo con el nombre del archivo
        if (Path.IsPathRooted(texturePath))
        {
            texturePath = Path.GetFileName(texturePath);
        }
        
        return texturePath;
    }

    /// <summary>
    /// Loads a *.mtl file
    /// </summary>
    /// <param name="input">The input stream from the MTL file</param>
    /// <returns>Dictionary containing loaded materials</returns>.
    /// 
    /// Voy a añadir debugging para ver cuando lee y cuando no lee los mapas de texturas etc...
    public Dictionary<string, Material> Load(Stream input)
    {
        var inputReader = new StreamReader(input);
        var reader = new StringReader(inputReader.ReadToEnd());
        debugString = "El objeto: " + input + " Ha utilizado lo siguiente: \n";

        Dictionary<string, Material> mtlDict = new Dictionary<string, Material>();
        Material currentMaterial = null;

        for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
        {
            debugString += "Estoy leyendo la linea: " + line + "\n";

            if (string.IsNullOrWhiteSpace(line))
            {
                debugString += "Saltando esta linea en blanco \n";
                continue;
            }

            string processedLine = line.Clean();
            string[] splitLine = processedLine.Split(' ');

            //blank or comment
            if (splitLine.Length < 2 || processedLine[0] == '#')
                continue;

            //newmtl
            if (splitLine[0] == "newmtl")
            {
                string materialName = processedLine.Substring(7);

                var newMtl = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = materialName };
                mtlDict[materialName] = newMtl;
                currentMaterial = newMtl;

                debugString += "Esta creando un nuevo material llamado: " + materialName + "\n";

                continue;
            }

            //anything past here requires a material instance
            if (currentMaterial == null)
            {
                debugString += "No tengo acceso a un material actualmente. \n"; 
                continue;
            }

            //diffuse color
            if (splitLine[0] == "Kd" || splitLine[0] == "kd")
            {
                // MODIFICADO: Se usa "_BaseColor" en lugar de "_Color"
                var currentColor = currentMaterial.GetColor("_BaseColor");
                var kdColor = OBJLoaderHelper.ColorFromStrArray(splitLine);
                
                currentMaterial.SetColor("_BaseColor", new Color(kdColor.r, kdColor.g, kdColor.b, currentColor.a));
                debugString += "Estoy cargando un Kd \n";
                continue;
            }

            //diffuse map
            if (splitLine[0] == "map_Kd" || splitLine[0] == "map_kd")
            {
                string texturePath = GetTexPathFromMapStatement(processedLine, splitLine);
                debugString += "Voy a utilizar un map_Kd!!!!!! La ruta supuesta de la textura es: " + texturePath + "\n";
                
                if (texturePath == null)
                {
                    debugString += "He fallado al cargar GetTexPathFromMapStatement(processedLine, splitLine) Y texturePath es null\n";
                    continue; //invalid args or sth
                }

                var KdTexture = TryLoadTexture(texturePath);
                // MODIFICADO: Se usa "_BaseMap" en lugar de "_MainTex"
                currentMaterial.SetTexture("_BaseMap", KdTexture);

                //set transparent mode if the texture has transparency
                if (KdTexture != null && (KdTexture.format == TextureFormat.DXT5 || KdTexture.format == TextureFormat.ARGB32))
                {
                    debugString += "set transparent mode if the texture has transparency\n";
                    OBJLoaderHelper.EnableMaterialTransparency(currentMaterial);
                }

                //flip texture if this is a dds
                if (Path.GetExtension(texturePath).ToLower() == ".dds")
                {
                    debugString += "Estoy corriendo un codigo por ser un archivo .dds Y hay una linea cambiada lo cual puede llevar a problemas. \n";
                    // MODIFICADO: Se usa SetTextureScale para apuntar a "_BaseMap"
                    currentMaterial.SetTextureScale("_BaseMap", new Vector2(1f, -1f));
                }

                //Cambiar Kd pq si es una textura con 0 0 0 no aparece nada tiene q ser 1 1 1
                debugString += "Estoy cambiando el color a 1 1 1, antes el color era: " + currentMaterial.color.r + ", " + currentMaterial.color.g + ", " + currentMaterial.color.b + ", " + currentMaterial.color.a;
                currentMaterial.SetColor("_BaseColor", new Color(1, 1, 1, 1));
                
                continue;
            }

            //bump map
            if (splitLine[0] == "map_Bump" || splitLine[0] == "map_bump" || splitLine[0].Contains("bump"))
            {
                string texturePath = GetTexPathFromMapStatement(processedLine, splitLine);
                debugString += "Voy a utilizar un map_bump!!!!!! La ruta supuesta de la textura es: " + texturePath + " Deberia investigar si tal vez el problema tenga que ver con que busca map_bump y no bump\n";
                if (texturePath == null)
                {
                    debugString += "He fallado al cargar GetTexPathFromMapStatement(processedLine, splitLine) Y texturePath es null\n";
                    continue; //invalid args or sth
                }

                var bumpTexture = TryLoadTexture(texturePath, false);
                float bumpScale = GetArgValue(splitLine, "-bm", 1.0f);

                if (bumpTexture != null)
                {
                    debugString += "bumpTexture no es null, voy a intentar cargar el bumpmap. \n";
                    currentMaterial.SetTexture("_BumpMap", bumpTexture);
                    currentMaterial.SetFloat("_BumpScale", bumpScale);
                    currentMaterial.EnableKeyword("_NORMALMAP");
                }
                else
                    debugString += "He fallado, bumpTexture es null. \n";

                continue;
            }

            //specular color
            if (splitLine[0] == "Ks" || splitLine[0] == "ks")
            {
                debugString += "Cargando Ks \n";
                currentMaterial.SetColor("_SpecColor", OBJLoaderHelper.ColorFromStrArray(splitLine));
                continue;
            }

            if (splitLine[0] == "map_Ks" || splitLine[0] == "map_ks")
            {
                string texturePath = GetTexPathFromMapStatement(processedLine, splitLine);
                var specularTexture = TryLoadTexture(texturePath);

                if (specularTexture != null)
                {
                    // Puedes mapearlo a "_SpecGlossMap" si usas shader personalizado,
                    // o simplemente como "_MetallicGlossMap" con ajuste adecuado
                    currentMaterial.SetTexture("_SpecGlossMap", specularTexture);
                    currentMaterial.EnableKeyword("_SPECGLOSSMAP");
                }
            }

            //emission color
            if (splitLine[0] == "Ka" || splitLine[0] == "ka")
            {
                debugString += "Cargando Ka \n";
                currentMaterial.SetColor("_EmissionColor", OBJLoaderHelper.ColorFromStrArray(splitLine, 0.05f));
                currentMaterial.EnableKeyword("_EMISSION");
                continue;
            }

            //emission map
            if (splitLine[0] == "map_Ka" || splitLine[0] == "map_ka")
            {
                string texturePath = GetTexPathFromMapStatement(processedLine, splitLine);
                debugString += "Cargando map_Ka \n";
                if (texturePath == null)
                {
                    debugString += "He fallado, la ruta es null para map_Ka. \n";
                    continue; //invalid args or sth
                }

                currentMaterial.SetTexture("_EmissionMap", TryLoadTexture(texturePath));
                continue;
            }

            //alpha
            if (splitLine[0] == "d" || splitLine[0] == "Tr" || splitLine[0] == "Tf")
            {
                debugString += "Cargando alpha: d / Tr \n";
                float visibility = OBJLoaderHelper.FastFloatParse(splitLine[1]);
                
                if(splitLine[0] == "Tr")
                    visibility = 1f - visibility;
                if (splitLine[0].ToLower() == "tf" && splitLine.Length >= 4)
                {
                    float r = OBJLoaderHelper.FastFloatParse(splitLine[1]);
                    float g = OBJLoaderHelper.FastFloatParse(splitLine[2]);
                    float b = OBJLoaderHelper.FastFloatParse(splitLine[3]);

                    visibility = (r + g + b) / 3f;

                    if (visibility < 1.0f)
                    {
                        var color = currentMaterial.GetColor("_BaseColor");
                        currentMaterial.SetColor("_BaseColor", new Color(color.r, color.g, color.b, visibility));
                        OBJLoaderHelper.EnableMaterialTransparency(currentMaterial);
                        debugString += $"[TF] Aplicada transparencia derivada de Tf: {visibility}\n";
                    }
                }
                if (visibility < (1f - Mathf.Epsilon))
                    {
                        // MODIFICADO: Se usa "_BaseColor" en lugar de "_Color"
                        var currentColor = currentMaterial.GetColor("_BaseColor");

                        currentColor.a = visibility;
                        currentMaterial.SetColor("_BaseColor", currentColor);

                        OBJLoaderHelper.EnableMaterialTransparency(currentMaterial);
                    }
                continue;
            }

            //glossiness
            if (splitLine[0] == "Ns" || splitLine[0] == "ns")
            {
                debugString += "Cargando glossiness: Ns / ns \n";
                float Ns = OBJLoaderHelper.FastFloatParse(splitLine[1]);
                Ns = (Ns / 1000f);
                // MODIFICADO: Se usa "_Smoothness" en lugar de "_Glossiness"
                currentMaterial.SetFloat("_Smoothness", Ns);
            }
        }

        Debug.Log(debugString);

        return mtlDict;
    }

    /// <summary>
    /// Loads a *.mtl file
    /// </summary>
    /// <param name="path">The path to the MTL file</param>
    /// <returns>Dictionary containing loaded materials</returns>
	public Dictionary<string, Material> Load(string path)
    {
        _objFileInfo = new FileInfo(path); //get file info
        SearchPaths.Add(_objFileInfo.Directory.FullName); //add root path to search dir

        using (var fs = new FileStream(path, FileMode.Open))
        {
            Debug.Log("Estoy cargando mtl con Load(String) con esto llamo al load principal");
            return Load(fs); //actually load
        }
        
    }
}
