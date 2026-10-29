using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GeneradorDeEscena3D : MonoBehaviour
{
	/* NECESITAS CAMBIAR SAVE SCENE */

	public static string[] mainObjects;
	public static string[] customObjects;
	public static List<GameObject> customList;
	public GameObject[] npc;
	[SerializeField] ObjectTimer objectTimer;

	public static bool escenaInicialCargada = false;
	// Para evitar duplicados de escenas, no permitira crear la misma escena dos veces
	private string lastSceneCreated = "";


	public System.Collections.IEnumerator EsperarCargaYUsar()
	{
		while (!CustomObjectsLoader.isLoadingComplete)
			yield return null;

		Debug.Log("Loading custom objects list on start for generador de escenas 3d");
		customList = CustomObjectsLoader.prefabCache;
		// Cargar todos los GameObject (prefabs) de la carpeta Resources/Ojetos3D/
		// Este prefab contiene las acrtividades prefabs porque son objetos3D
		GameObject[] prefabs = Resources.LoadAll<GameObject>("Objetos3D");
		GameObject[] customPrefabs = customList.ToArray();

		mainObjects = prefabs.Select(p => p.name).ToArray();
		customObjects = customPrefabs.Select(p => p.name).ToArray();



		if (File.Exists(Path.Combine(Application.persistentDataPath, "escena " + Map2D.x + "_" + Map2D.y + ".json")))
		{
			Debug.Log("VOY A GENERAR LA ESCENA 0_0");
			CrearEscena(0, 0);
		}

		escenaInicialCargada = true;

	}

	public void Comenzar()
	{

		// Buscar el fichero más reciente
		string directorio = Application.persistentDataPath;
		string[] archivos = Directory.GetFiles(directorio, "historial_ia_*.json");

		if (archivos.Length == 0)
		{
			Debug.LogWarning("No se encontraron ficheros de escena en: " + directorio);
			GestorHistorialIA.GuardarRespuestaForzada(@"{
  ""objects"": [
        {
            ""type"": ""npc"",
            ""position"": {
                ""x"": -2.0,
                ""y"": 0.0,
                ""z"": -2.5
            },
            ""scale"": 1.0,
            ""rotation"": ""north"",
            ""dialog"": ""Hola, soy un NPC, mi función principal es ser utilizado como recurso educativo en manera de profesor interactuable, puedo ser interactuado varias veces cada punto separa mis interacciones.""
        }
    ],
	""activity"": {
		""type"": ""TestActivity"",
		""position"": { ""x"": 6, ""y"": 0, ""z"": 2 },
		""scale"": 1,
		""rotation"": ""south"",
		""physics"": false,
		""questions"": [
			{
				""question"": ""¿Qué absorben los árboles?"",
				""options"": [""Oxígeno"", ""Dióxido de carbono"", ""Helio"", """"],
				""correctAnswer"": 1
			},
			{
				""question"": ""¿Qué producen los árboles?"",
				""options"": [""Oxígeno"", ""Metano"", ""Hidrógeno"", ""Nitrógeno""],
				""correctAnswer"": 0
			}
		]
	}
}");
			//Actualizar archivos para que añada el nuevo file
			archivos = Directory.GetFiles(directorio, "historial_ia_*.json");
		}

		// Ordenamos por fecha de creaci�n y tomamos el �ltimo
		string ruta = archivos.OrderByDescending(File.GetCreationTime).First();
		if(lastSceneCreated.Equals(ruta))
        {
            Debug.Log("Escena ya creada con anterioridad, se quieren evitar duplicados.");
			return;
        }
		lastSceneCreated = ruta;

		Debug.Log("Cargando fichero: " + ruta);

		string json = File.ReadAllText(ruta);
		Escena3D escena = Escena3D.Cargar(json);

		if (escena == null || (escena.objects == null && escena.activity == null))
		{
			Debug.LogError("Error: El JSON está vacío o mal formado: " + ruta);
			return;
		}

		// Cargar objetos
		foreach (Objeto3D objeto in escena.objects)
		{
			objeto.physics = true;
			Debug.Log($"Creando objeto: {objeto.type} en {objeto.position} y con physics = {objeto.physics}");
			CrearObjeto(objeto);
		}

		// Cargar actividad unica y singular
		if(escena.activity != null && !string.IsNullOrEmpty(escena.activity.type))
		{
			Activity objeto = escena.activity;
			Debug.Log($"Creando actividad: {objeto.type} en {objeto.position} y con physics = {objeto.physics}");

			CrearActivity(objeto);
		}

		SaveScene();
		Debug.Log("Escena generada con " + escena.objects.Count + " objects.");
		objectTimer.IniciarMostrado();
	}

	public static void CrearEscena(int x, int y)
	{
		string ruta = Path.Combine(Application.persistentDataPath, "escena " + x + "_" + y + ".json");

		Debug.Log("Cargando fichero: " + ruta);

		string json = File.ReadAllText(ruta);
		Escena3D escena = Escena3D.Cargar(json);

		foreach (Objeto3D objeto in escena.objects)
		{
			Debug.Log($"Creando objeto: {objeto.type} en {objeto.position}");
			CrearObjeto(objeto);
		}
		if(escena.activity != null && !string.IsNullOrEmpty(escena.activity.type))
		{
			// El codigo actual trata activities como objetos fisicos singulares, 
			// que para las actividades que se me ocurren funciona perfectamente,
			// desconfio de la escalabilidad, pero no es objetivo escalar a tanto
			// las actividades... no? 
			Activity objeto = escena.activity;
			Debug.Log($"Creando actividad: {objeto.type} en {objeto.position} y con physics = {objeto.physics}");

			CrearActivity(objeto);
			Debug.Log("Actividad: " + escena.activity.type);
		}

		Debug.Log("Escena generada con " + escena.objects.Count + " objects.");
	}


	public static void CrearObjeto(Objeto3D objeto)
	{
		GameObject go = null;

		if (mainObjects.Contains(objeto.type.ToLower()))
		{
			GameObject prefab = Resources.Load<GameObject>("Objetos3D/" + objeto.type.ToLower());

			if (prefab == null)
			{
				Debug.LogWarning("Prefab no encontrado para: " + objeto.type);
				return;
			}

			go = Instantiate(prefab);
			if (objeto.type.ToLower().Equals("npc"))
			{
				NpcDialog dialogoScript = go.GetComponent<NpcDialog>();
				if (dialogoScript != null && !string.IsNullOrEmpty(objeto.dialog))
				{
					dialogoScript.textoDialogo = objeto.dialog;
				}
			}
		}

		//Aqui creo que se podria cambiar a minus objeto.type, habria que cambiar tamb la lista customObjects a minus.
		else if (customObjects.Contains(objeto.type))
		{
			GameObject prefab = null;

			foreach (GameObject customGO in customList)
			{
				if (customGO.name.Equals(objeto.type))
				{
					prefab = customGO;
					break;
				}
			}

			if (prefab == null)
			{
				Debug.LogWarning("Prefab no encontrado para: " + objeto.type);
				return;
			}

			go = Instantiate(prefab);
			go.SetActive(true);
			go.AddComponent<PhysicsVariable>();
			PhysicsVariable pv = go.GetComponent<PhysicsVariable>();
			pv.SetValue(objeto.physics);
		}

		else
		{
			Debug.LogWarning("type de objeto no reconocido: " + objeto.type);
			return;
		}

		go.transform.position = objeto.position.ToVector3();

		go.transform.localScale = new Vector3(objeto.scale, objeto.scale, objeto.scale);

		go.transform.rotation = objeto.rotation;
	}

	public static void CrearActivity(Activity activity)
	{
		GameObject go = null;

		// mainobjects must contain activity
		if (mainObjects.Contains(activity.type.ToLower()))
		{
			GameObject prefab = Resources.Load<GameObject>("Objetos3D/" + activity.type.ToLower());

			if (prefab == null)
			{
				Debug.LogWarning("Prefab no encontrado para: " + activity.type);
				return;
			}

			go = Instantiate(prefab);
			if (activity is TestActivityData testData)
			{
				TestActivity testScript = go.GetComponent<TestActivity>();
				if (testScript != null && testData.questions != null && testData.questions.Length != 0)
				{
					// Añadir todas las preguntas
					foreach(Pregunta pregunta in testData.questions)
					{
						string question = pregunta.question;
						string[] options = pregunta.options;
						int correctAnswer = pregunta.correctAnswer;

						testScript.AddNewQuestion(question, options, correctAnswer);

						Debug.Log("Añadiendo pregunta question: "+question+"  |  correctAnswer: " + correctAnswer);
					}
				}
				else
				{
					Debug.LogWarning("error en la creacion de activity problema con el script");
				}
			}
			// RESTO DE CASOS
			else if (false)
			{
				
			}
		}

		else
		{
			Debug.LogWarning("type de objeto no reconocido: " + activity.type);
			return;
		}

		go.transform.position = activity.position.ToVector3();

		go.transform.localScale = new Vector3(activity.scale, activity.scale, activity.scale);

		go.transform.rotation = activity.rotation;

		// IMPORTANTE
		go.tag = "Activity";
	}

	public static void SaveScene()///
	{
		GameObject[] todos = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
		Escena3D copy = new Escena3D();

		foreach (GameObject obj in todos)
		{
			if (obj.name.Contains("Clone") && !obj.tag.Equals("Activity"))
			{
				Objeto3D aux = Transform2Objeto3D(obj);
				copy.objects.Add(aux);
			}
			if (obj.name.Contains("Clone") && obj.tag.Equals("Activity"))
			{
				copy.activity = Transform2Activity(obj);
				// Activity activity = new Activity(aux.type, aux.position, aux.scale, aux.rotation, false, testActivity.allQuestions)
				// copy.activity = ;
			}
		}

		string json = copy.ToJson();
		string ruta = Path.Combine(Application.persistentDataPath, "escena " + Map2D.x + "_" + Map2D.y + ".json");
		File.WriteAllText(ruta, json);
		Debug.Log("Escena guardada en: " + ruta + "En la habitacion: " + "escena " + Map2D.x + "_" + Map2D.y + ".json");
	}

	private static void RellenarObjeto3D(GameObject obj, Objeto3D destino)
	{
		destino.type = obj.name.Replace("(Clone)", "").Trim();

		destino.position = new Vector3Data
		(
			obj.transform.position.x,
			obj.transform.position.y,
			obj.transform.position.z
		);

		destino.scale = obj.transform.localScale.x;

		//aqui habia un problema al ser primero: la rotacion esta mal calculada pq los quaternion son entre -1 y 1
		// segundo: si el objeto tiene fisicas se movera y rotara por tanto no sirve con comprobar cuatro casos especificos.

		destino.rotation = obj.transform.rotation;



		if (obj.name.Contains("npc"))
		{
			destino.dialog = obj.GetComponent<NpcDialog>().textoDialogo;
		}

		PhysicsVariable pv = obj.GetComponent<PhysicsVariable>();
		if (pv != null) { destino.physics = pv.GetValue(); }
		else { destino.physics = true; }
	}

	public static Objeto3D Transform2Objeto3D(GameObject obj)
	{
		Objeto3D aux = new Objeto3D();
		
		RellenarObjeto3D(obj, aux);
		return aux;
	}

	// Espejo para activity
	public static Activity Transform2Activity(GameObject obj)
	{
		if (!obj.tag.Equals("Activity"))
		{
			Debug.LogError("Error, se esta llamando a transform2activity con un ojeto que no es actividad con tag Activity");
		}

		TestActivity test = obj.GetComponentInChildren<TestActivity>();
		if (test != null)
		{
			TestActivityData data = new TestActivityData();
			RellenarObjeto3D(obj, data);
			data.type = TestActivityData.TYPE;   // el nombre del prefab puede variar en mayúsculas
			data.questions = test.allQuestions.ToArray();
			return data;
		}

		// else if (obj.GetComponentInChildren<OtraActividad>() ...)

		Debug.LogWarning("Actividad sin script reconocido: " + obj.name);
		return null;
	}

	public static void CleanScene()
	{
		GameObject[] todos = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
		foreach (GameObject obj in todos)
		{
			if (obj.name.Contains("Clone"))
				Destroy(obj);
		}
	}
}
