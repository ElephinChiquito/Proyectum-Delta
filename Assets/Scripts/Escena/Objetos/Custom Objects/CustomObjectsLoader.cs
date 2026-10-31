using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dummiesman;
using System.Linq;
using GLTFast;
using TMPro;


public class CustomObjectsLoader : MonoBehaviour
{
    [SerializeField] public static string DirectoryName;
    public static List<GameObject> prefabCache = new List<GameObject>();

    public static bool isLoadingComplete = false;
    public static Slider loadingBar; // UnityEngine.UI
    public static TextMeshProUGUI loadingText;

    public static System.Collections.IEnumerator StartCustomObjectsLoader(MonoBehaviour caller)
    {
        yield return caller.StartCoroutine(LoadAllCustomObjectsCoroutine());
    }

    public static System.Collections.IEnumerator LoadAllCustomObjectsCoroutine()
    {
        DirectoryName = VariablesLoader.Data.folderPath;

        string ruta = DirectoryName;

        if (!Directory.Exists(ruta))
        {
            Debug.LogError("La carpeta no existe: " + ruta);
            yield break;
        }

        int totalObjs = Directory.GetFiles(ruta, "*.obj", SearchOption.AllDirectories).Length;
        int totalGLTFs = Directory.GetFiles(ruta, "*.glb", SearchOption.AllDirectories).Length +
                        Directory.GetFiles(ruta, "*.gltf", SearchOption.AllDirectories).Length;
        int total = totalObjs + totalGLTFs;
        int current = 0;

        string[] todosLosGLB = Directory.GetFiles(ruta, "*.glb", SearchOption.AllDirectories);
        string[] todosLosGLTF = Directory.GetFiles(ruta, "*.gltf", SearchOption.AllDirectories);
        string[] todosLosArchivos = todosLosGLB.Concat(todosLosGLTF).ToArray();
        int counter = 0;

        // OBJ
        foreach (string folder in Directory.GetDirectories(ruta))
        {
            string[] archivoArray = Directory.GetFiles(folder, "*.obj", SearchOption.AllDirectories);

            if (archivoArray.Length == 0)
                continue; // saltar al siguiente folder

            string archivo = archivoArray[0];
            string[] mtlPathArray = Directory.GetFiles(folder, "*.mtl", SearchOption.AllDirectories);

            if (mtlPathArray.Length == 0)
                continue; // saltar al siguiente folder

            string mtlPath = mtlPathArray[0];

            GameObject tempObject = null;

            if (File.Exists(mtlPath))
            {
                string objFolderPath = Path.GetDirectoryName(archivo);

                var objLoader = new OBJLoader();
                var mtlLoader = new MTLLoader();
                mtlLoader.SearchPaths.Clear();
                mtlLoader.SearchPaths.Add(objFolderPath);
                mtlLoader.SearchPaths.Add(Path.Combine(objFolderPath, "Textures"));
                mtlLoader.SearchPaths.Add(Path.Combine(objFolderPath, "images"));
                mtlLoader.SearchPaths.Add(string.Empty);
                var materials = mtlLoader.Load(mtlPath);
                objLoader.Materials = materials;
                tempObject = objLoader.Load(archivo);
            }
            else
            {
                tempObject = CreateObjectWithOutMTL(archivo, tempObject);
            }

            if (tempObject != null)
            {
                ResizeObjects(tempObject);
                tempObject = CenterObjects(tempObject);
                AddCollider(tempObject);
                tempObject.SetActive(false);
                prefabCache.Add(tempObject);

                current++;
                if (loadingBar != null)
                {
                    loadingBar.value = (float)current / total;
                    if (loadingText != null)
                        loadingText.text = $"Loading Objects: {current}/{total}";
                }
            }

            //Debug
            if (!Directory.GetFiles(folder, "*.obj").Any() &&
                !Directory.GetFiles(folder, "*.glb").Any() &&
                !Directory.GetFiles(folder, "*.gltf").Any())
            {
                Debug.Log($"Carpeta sin modelos detectada: {folder}");
            }

        }

        Debug.Log("Carga completa de modelos OBJ");
        
        foreach (string folder in Directory.GetDirectories(ruta))
        {

            // GLTF/GLB


            string glbGltf = null;
            string[] glbs = Directory.GetFiles(folder, "*.glb", SearchOption.AllDirectories);
            if (glbs.Length > 0)
            {
                glbGltf = glbs[0];
            }
            else
            {
                string[] gltfs = Directory.GetFiles(folder, "*.gltf", SearchOption.AllDirectories);
                if (gltfs.Length > 0)
                {
                    glbGltf = gltfs[0];
                }
            }

            if (!string.IsNullOrEmpty(glbGltf) && Path.GetExtension(glbGltf).ToLower() == ".gltf")
            {
                string binExpected = Path.Combine(Path.GetDirectoryName(glbGltf), Path.GetFileNameWithoutExtension(glbGltf) + ".bin");
                if (!File.Exists(binExpected))
                {
                    Debug.LogWarning($"Falta el archivo .bin para: {glbGltf}, se omite.");
                    continue;
                }
            }

            if (glbGltf != null)
            {
                Debug.Log($"Archivo 3D encontrado ({counter + 1}/{todosLosArchivos.Length}): {glbGltf}");

                counter++;

                var gltf = new GltfImport();
                var loadTask = gltf.Load(glbGltf);
                while (!loadTask.IsCompleted)
                    yield return null;

                if (loadTask.Result)
                {
                    GameObject tempObject = new GameObject(Path.GetFileNameWithoutExtension(glbGltf));
                    var instTask = gltf.InstantiateMainSceneAsync(tempObject.transform);
                    while (!instTask.IsCompleted)
                        yield return null;


                    ResizeObjects(tempObject);
                    tempObject = CenterObjects(tempObject);
                    AddCollider(tempObject);
                    tempObject.SetActive(false);
                    prefabCache.Add(tempObject);

                    Debug.Log("Modelo cargado y almacenado en caché: " + glbGltf);

                    current++;
                    if (loadingBar != null)
                    {
                        loadingBar.value = (float)current / total;
                        if (loadingText != null)
                            loadingText.text = $"Loading Objects: {current}/{total}";
                    }
                }
                else
                {
                    Debug.LogWarning("Error al cargar modelo glTF: " + glbGltf);
                }

                yield return null; // opcional, permite que Unity actualice la UI/frame
            }
        }

        //GLB de un único archivo...
		string[] glbExteriores = Directory.GetFiles(ruta, "*.glb", SearchOption.TopDirectoryOnly);
		for (int i = 0; i < glbExteriores.Length; i++)
		{

			string glbExt = glbExteriores[i];
			Debug.Log($"Archivo 3D encontrado ({counter + 1}/{todosLosArchivos.Length}): {glbExt}");

			counter++;

			var gltf = new GltfImport();
			var loadTask = gltf.Load(glbExt);
			while (!loadTask.IsCompleted)
				yield return null;

			if (loadTask.Result)
			{
				GameObject tempObject = new GameObject(Path.GetFileNameWithoutExtension(glbExt));
				var instTask = gltf.InstantiateMainSceneAsync(tempObject.transform);
				while (!instTask.IsCompleted)
					yield return null;


				ResizeObjects(tempObject);
				tempObject = CenterObjects(tempObject);
				AddCollider(tempObject);
				tempObject.SetActive(false);
				prefabCache.Add(tempObject);

				Debug.Log("Modelo cargado y almacenado en caché: " + glbExt);

				current++;
				if (loadingBar != null)
				{
					loadingBar.value = (float)current / total;
					if (loadingText != null)
						loadingText.text = $"Loading Objects: {current}/{total}";
				}
			}
			else
			{
				Debug.LogWarning("Error al cargar modelo glb: " + glbExt);
			}

			yield return null; // opcional, permite que Unity actualice la UI/frame
		}

		//GLTF de un único archivo...
		string[] gltfExteriores = Directory.GetFiles(ruta, "*.gltf", SearchOption.TopDirectoryOnly);
		for (int i = 0; i < gltfExteriores.Length; i++)
		{

			string gltfExt = gltfExteriores[i];
			Debug.Log($"Archivo 3D encontrado ({counter + 1}/{todosLosArchivos.Length}): {gltfExt}");

			counter++;

			var gltf = new GltfImport();
			var loadTask = gltf.Load(gltfExt);
			while (!loadTask.IsCompleted)
				yield return null;

			if (loadTask.Result)
			{
				GameObject tempObject = new GameObject(Path.GetFileNameWithoutExtension(gltfExt));
				var instTask = gltf.InstantiateMainSceneAsync(tempObject.transform);
				while (!instTask.IsCompleted)
					yield return null;


				ResizeObjects(tempObject);
				tempObject = CenterObjects(tempObject);
				AddCollider(tempObject);
				tempObject.SetActive(false);
				prefabCache.Add(tempObject);

				Debug.Log("Modelo cargado y almacenado en caché: " + gltfExt);

				current++;
				if (loadingBar != null)
				{
					loadingBar.value = (float)current / total;
					if (loadingText != null)
						loadingText.text = $"Loading Objects: {current}/{total} current: {tempObject.name}";
				}
			}
			else
			{
				Debug.LogWarning("Error al cargar modelo glTF: " + gltfExt);
			}

			yield return null; // opcional, permite que Unity actualice la UI/frame
		}

        Debug.Log("Carga completa de modelos glTF/GLB. Voy a poner isLoadingComplete = true");

        if (loadingBar != null)
        {
            loadingBar.value = 1f;
            if (loadingText != null)
                loadingText.text = "Carga completada";
        }
        loadingBar.transform.root.gameObject.SetActive(false); // para ocultar al terminar

        isLoadingComplete = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direccion"></param>
    /// <param name="go"></param> Utilizo esto para intentar que no se creen dos GameObjects, sino solo uno.
    /// <returns></returns>
    public static GameObject CreateObjectWithOutMTL(string direccion, GameObject go)
    {
        go = new OBJLoader().Load(direccion);

        Material defaultMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        defaultMaterial.color = Color.gray; // Puedes usar otros tonos también

        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.material = defaultMaterial;
        }

        return go;
    }

    static void AddCollider(GameObject iniGo)
    {
        int total = 0;
        int conMalla = 0;

        foreach (Transform t in iniGo.transform.root.GetComponentsInChildren<Transform>())
        {
            total++;
            GameObject go = t.gameObject;

            // Caso MeshFilter
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
            {
                MeshCollider mc = go.AddComponent<MeshCollider>();
                mc.sharedMesh = mf.sharedMesh;
                mc.convex = true;
                conMalla++;
                continue;
            }

            // Caso SkinnedMeshRenderer
            SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
            if (smr != null && smr.sharedMesh != null)
            {
                MeshCollider mc = go.AddComponent<MeshCollider>();
                mc.sharedMesh = smr.sharedMesh;
                mc.convex = true;
                conMalla++;
            }
        }

        Debug.Log($"Total de objetos: {total}, con Mesh (MeshFilter o Skinned): {conMalla}");
    }

    static GameObject CenterObjects(GameObject go)
    {
        GameObject root = go.transform.root.gameObject;
        var renderers = root.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("No hay renderers en: " + root.name);
            return go;
        }

        Bounds combinedBounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            combinedBounds.Encapsulate(renderers[i].bounds);

        float bottomY = combinedBounds.min.y;
        float centerX = combinedBounds.center.x;
        float centerZ = combinedBounds.center.z;

        Vector3 offset = new Vector3(-centerX, -bottomY, -centerZ);
        root.transform.position += offset;

        GameObject parent000 = new GameObject(root.name);
        root.name = root.name + "(Child)";
        parent000.transform.position = Vector3.zero;
        root.transform.SetParent(parent000.transform, true);

        Debug.Log("Objeto centrado en X/Z y colocado al suelo Y=0: " + parent000.name);
        return parent000;
    }


    static void ResizeObjects(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds b = renderers[0].bounds;
        foreach (var r in renderers)
            b.Encapsulate(r.bounds);

        float largest = Mathf.Max(b.size.x, b.size.y, b.size.z);

        float maxSize = 2.5f;
        float minSize = 1.0f;
        if (largest > maxSize)
        {
            float factor = maxSize / largest;
            go.transform.localScale *= factor;
        }
        if (largest < minSize)
        {
            float factor = minSize / largest;
            go.transform.localScale *= factor;
        }
    
    }

}