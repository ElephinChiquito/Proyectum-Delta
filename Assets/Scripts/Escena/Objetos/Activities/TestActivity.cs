using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla una actividad tipo test dentro de Unity.
/// Muestra una pregunta con 4 opciones (A-D), colorea la respuesta
/// al pulsar un botón y avanza a la siguiente pregunta si se acierta.
/// </summary>
public class TestActivity : MonoBehaviour
{
	// ---------------------------------------------------------------
	// Referencias a objetos de la UI (se asignan desde el Inspector)
	// ---------------------------------------------------------------

	/// <summary>Botón de la opción A (índice 0).</summary>
	public GameObject buttonA;
	/// <summary>Botón de la opción B (índice 1).</summary>
	public GameObject buttonB;
	/// <summary>Botón de la opción C (índice 2).</summary>
	public GameObject buttonC;
	/// <summary>Botón de la opción D (índice 3).</summary>
	public GameObject buttonD;
	/// <summary>Objeto de texto donde se muestra el enunciado de la pregunta.</summary>
	public GameObject enunciado;

	// Array con todos los componentes de texto, en el mismo orden que
	// los índices de Pregunta (enunciado, A, B, C, D). Se rellena en Start().
	private GameObject[] componentes;

	// Lista con todas las preguntas del test.
	// NonSerialized: evita que Unity intente serializarla en el Inspector.
	[System.NonSerialized] public List<Pregunta> allQuestions;

	/// <summary>Índice (0-3) de la respuesta correcta de la pregunta que se está mostrando.</summary>
	public int currentAnswer;
	/// <summary>Índice de la pregunta actual dentro de la lista de preguntas.</summary>
	public int progress;

	// Constantes para identificar cada botón al responder
	const int resA = 0;
	const int resB = 1;
	const int resC = 2;
	const int resD = 3;

	public const string PREGUNTA_INICIAL = "¿Comenzar el test?";

	void Awake()
	{
		allQuestions = new List<Pregunta>();
		allQuestions.Add(new Pregunta(PREGUNTA_INICIAL, "Sí", "No", "", "", 0));
	}

	/// <summary>Preguntas a guardar en el JSON, sin la pregunta inicial (la añade Awake).</summary>
	public Pregunta[] PreguntasParaGuardar()
	{
		List<Pregunta> resultado = new List<Pregunta>();
		foreach (Pregunta p in allQuestions)
		{
			if (p.question != PREGUNTA_INICIAL)
				resultado.Add(p);
		}
		return resultado.ToArray();
	}

    // Start se llama una vez, antes del primer Update, al crearse el MonoBehaviour
    void Start()
	{
		// Orden importante: debe coincidir con los índices de Pregunta
		// (0 = enunciado, 1 = A, 2 = B, 3 = C, 4 = D)
		componentes = new GameObject[] {enunciado, buttonA, buttonB, buttonC, buttonD};
		

		// Empezamos en la primera pregunta y con los botones en gris
		progress = 0;
		SetPregunta(0);
		Colorear(-1);
	}

	/// <summary>
	/// Añade una pregunta nueva al final de la lista del test.
	/// </summary>
	/// <param name="pregunta">Texto del enunciado.</param>
	/// <param name="opcionA">Texto de la opción A.</param>
	/// <param name="opcionB">Texto de la opción B.</param>
	/// <param name="opcionC">Texto de la opción C (puede ir vacío).</param>
	/// <param name="opcionD">Texto de la opción D (puede ir vacío).</param>
	/// <param name="answerNumber">Índice de la respuesta correcta (0 = A, 1 = B, 2 = C, 3 = D).</param>
	public void AddNewQuestion
	(string pregunta, string opcionA, string opcionB, 
	 string opcionC, string opcionD, int answerNumber)
	{
		allQuestions.Add(new Pregunta(
			pregunta, opcionA, opcionB, opcionC, opcionD, answerNumber
		));
	}

	public void AddNewQuestion
	(string pregunta, string[] options, int answerNumber)
	{
		allQuestions.Add(new Pregunta(
			pregunta, options, answerNumber
		));
	}

	/// <summary>
	/// Elimina la pregunta que está en la posición indicada de la lista.
	/// </summary>
	/// <param name="i">Posición de la pregunta a eliminar (empieza en 0).</param>
	public void EraseQuestion(int i)
	{
		allQuestions.RemoveAt(i);
	}


	// true mientras se muestra el resultado; bloquea pulsaciones extra
	private bool esperando = false;

	/// <summary>
	/// Método que deben llamar los botones (OnClick) al responder.
	/// Si ya hay una respuesta en curso, se ignora la pulsación.
	/// </summary>
	/// <param name="res">Opción pulsada: 0 = A, 1 = B, 2 = C, 3 = D.</param>
	public void AnswerWithWait(int res)
	{
		if(esperando) return;

		StartCoroutine(Answer(res));
	}

	/// <summary>
	/// Corrutina que procesa una respuesta: colorea los botones, espera
	/// 1 segundo y pasa a la siguiente pregunta si se ha acertado
	/// (si se falla, se repite la misma). Tras la última vuelve a la primera.
	/// </summary>
	/// <param name="res">Opción pulsada: 0 = A, 1 = B, 2 = C, 3 = D.</param>
	/// <returns>Enumerador de la corrutina (se usa con StartCoroutine).</returns>
	public IEnumerator Answer(int res)
	{
		esperando = true;

		Pregunta preguntaActual = allQuestions[progress];
		// Muestra verde/rojo según el acierto
		Colorear(res);
		// Dormir 1 segundo
		yield return new WaitForSeconds(1f);

		// Solo se avanza si se ha acertado; si no, se repite la misma pregunta.
		// El módulo hace que al terminar la última se vuelva a la primera.
		if(preguntaActual.getcorrectAnswer() == res)
		{
			progress = (progress + 1)%allQuestions.Count;
			
		}

		// Carga en pantalla la pregunta (nueva o la misma) y limpia colores
		SetPregunta(progress);

		Colorear(-1);
		esperando = false;
	}

	/// <summary>
	/// Colorea los 4 botones según la respuesta dada:
	/// la correcta en verde, la pulsada (si es incorrecta) en rojo y el resto en gris.
	/// </summary>
	/// <param name="res">Opción pulsada (0-3), o -1 para dejar todos los botones en gris.</param>
	private void Colorear(int res)
	{
		GameObject[] botones = { buttonA, buttonB, buttonC, buttonD };

		for (int i = 0; i < botones.Length; i++)
		{
			Image imagen = botones[i].GetComponent<Image>();

			if (res == -1)
				imagen.color = Color.grey;

			else if (i == currentAnswer)
				imagen.color = Color.green;

			else if (i == res)
				imagen.color = Color.red;

			else
				imagen.color = Color.grey;
		}
		
		//Fin
	}

	/// <summary>
	/// Muestra en la UI la pregunta indicada (enunciado y 4 opciones)
	/// y guarda cuál es su respuesta correcta en <c>currentAnswer</c>.
	/// </summary>
	/// <param name="num">Posición de la pregunta en la lista (empieza en 0).</param>
	public void SetPregunta(int num)
	{
		progress = num;
		Pregunta preguntaActual = allQuestions[num];
		string[] options = preguntaActual.getOptions();

		// Actualizar los options de enunciado y botones.
		// Recorre los 5-1 elementos (1 = opcion A ... 4 = opción D)
		for(int i = Pregunta.OPCIONA; i <= Pregunta.OPCIOND; i++) //Bucle, tamaño de pregunta es d) = 4
		{
			componentes[i].GetComponentInChildren<TextMeshProUGUI>().SetText(options[i-1]);
		}

		// Enunciado
		string question = preguntaActual.getQuestion();
		componentes[Pregunta.QUESTION].GetComponentInChildren<TextMeshProUGUI>().SetText(question);


		currentAnswer = preguntaActual.getcorrectAnswer();
	}





	/* 
	Ejemplo de formato JSON que se podría usar para cargar actividades
	y objetos de la escena desde un fichero externo:

{
    "objects": [
        {
            "type": "tree",
            "position": { "x": 0, "y": 0, "z": 0 },
            "scale": 1,
            "rotation": "north",
            "physics": false
        },
        {
            "type": "npc",
            "position": { "x": 4, "y": 0, "z": 2 },
            "scale": 1,
            "rotation": "east",
            "dialog": "¡Hola! Acércate al panel para hacer el test.",
            "physics": true
        }
    ],
    "activity": {
		"type": "TestActivity",
		"position": { "x": 6, "y": 0, "z": 2 },
		"scale": 1,
		"rotation": "south",
		"physics": false,
		"questions": [
			{
				"question": "¿Qué absorben los árboles?",
				"options": ["Oxígeno", "Dióxido de carbono", "Helio", ""],
				"correctAnswer": 1
			},
			{
				"question": "¿Qué producen los árboles?",
				"options": ["Oxígeno", "Metano", "Hidrógeno", "Nitrógeno"],
				"correctAnswer": 0
			}
		]
	}
}
	
	*/

}