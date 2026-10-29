using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class TestActivity : MonoBehaviour
{
	public GameObject buttonA;
	public GameObject buttonB;
	public GameObject buttonC;
	public GameObject buttonD;
	public GameObject enunciado;

	private GameObject[] componentes;

	/*public class Pair <L, R>
	{
		L left;
		R right;

		public Pair(L left, R right)
		{
			this.left = left;
			this.right = right;
		}

		public L getLeft(){return left;}
		public R getRight(){return right;}
		public void setLeft(L left){this.left = left;}
		public void setRight(R right){this.right = right;}
	}*/
	public class Pregunta
	{
		
		public const int PREGUNTA = 0;
		public const int OPCIONA = 1;
		public const int OPCIONB = 2;
		public const int OPCIONC = 3;
		public const int OPCIOND = 4;


		public int answerNumber;
		private string[] textos;

		public Pregunta(string pregunta, string opcionA, string opcionB,
						string opcionC, string opcionD, int answerNumber)
		{
			textos = new string[] {pregunta, opcionA, opcionB, opcionC, opcionD};
			this.answerNumber = answerNumber;
		}

		public int getAnswerNumber(){return answerNumber;}
		public void setAnswerNumber(int answerNumber){this.answerNumber = answerNumber;}

		public string[] getTextos(){return textos;}
		public void setTextos(string[] textos){this.textos = textos;}


	}

	[System.NonSerialized] List<Pregunta> allQuestions;

	public int currentAnswer;
	public int progress;

	// For answering
	const int resA = 0;
	const int resB = 1;
	const int resC = 2;
	const int resD = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
	{
		componentes = new GameObject[] {enunciado, buttonA, buttonB, buttonC, buttonD};
		
		// Lista de preguntas
		allQuestions = new List<Pregunta>();
		
		
		allQuestions.Add(new Pregunta(
			"¿Cuál es la capital de España?",
			"Barcelona",
			"Madrid",
			"Sevilla",
			"Valencia",
			1
		));

		allQuestions.Add(new Pregunta(
			"¿Cuál es la capital de Francia?",
			"París",
			"Roma",
			"Londres",
			"Berlín",
			0
		));

		progress = 0;
		SetPregunta(0);
		Colorear(-1);
	}


	private bool esperando = false;

	public void AnswerWithWait(int res)
	{
		if(esperando) return;

		StartCoroutine(Answer(res));
	}

	public IEnumerator Answer(int res)
	{
		esperando = true;

		Pregunta preguntaActual = allQuestions[progress];
		Colorear(res);
		// Dormir 1 segundo
		yield return new WaitForSeconds(1f);


		if(preguntaActual.getAnswerNumber() == res)
		{
			progress = (progress + 1)%allQuestions.Count;
			
		}

		SetPregunta(progress);

		Colorear(-1);
		esperando = false;
	}

	public void Colorear(int res)
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

	public void SetPregunta(int num)
	{
		progress = num;
		Pregunta preguntaActual = allQuestions[num];
		string[] textos = preguntaActual.getTextos();

		//Actualizar valores
		for(int i = 0; i <= Pregunta.OPCIOND; i++) //Bucle, tamaño de pregunta es d) = 4
		{
			componentes[i].GetComponentInChildren<TextMeshProUGUI>().SetText(textos[i]);
		}
		currentAnswer = preguntaActual.getAnswerNumber();
	}





	/* 
{
	"objects": [
		{
		"id": "tree_01",
		"type": "tree",
		"position": { "x": 0, "y": 0, "z": 0 },
		"scale": 1,
		"rotation": "north"
		}
	],
	"activities": [
		{
		"id": "activity_01",
		"type": "quiz",
		"title": "Los ecosistemas",
		"questions": [
			{
			"question": "¿Qué absorben los árboles?",
			"options": [
				"Oxígeno",
				"Dióxido de carbono",
				"Helio",
				""
			],
			"correctAnswer": 1
			}
		]
		},
		{
		"id": "activity_02",
		"type": "find_object",
		"title": "Encuentra el árbol",
		"targetObjectId": "tree_01"
		}
	]
}
	
	*/

}
