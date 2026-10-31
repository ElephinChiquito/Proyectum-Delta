using System;

/// <summary>
/// Esta clase se usa para cargar las preguntas en la actividad tipo test.
/// <para/>
/// Estructura del array de textos:
/// <list type="bullet">
/// <item><description><c>PREGUNTA</c> = 0</description></item>
/// <item><description><c>OPCIONA</c> = 1</description></item>
/// <item><description><c>OPCIONB</c> = 2</description></item>
/// <item><description><c>OPCIONC</c> = 3</description></item>
/// <item><description><c>OPCIOND</c> = 4</description></item>
/// </list>
/// <para/>
/// Variables principales:
/// <list type="bullet">
/// <item><description><c>public int correctAnswer;</c></description></item>
/// <item><description><c>private string[] textos;</c></description></item>
/// </list>
/// </summary>
[Serializable]
public class Pregunta
{
    public const int QUESTION = 0;
    // Índices de cada texto dentro del array "options"
    /// <summary>Índice de la opción A en el array de textos.</summary>
    public const int OPCIONA = 1;
    /// <summary>Índice de la opción B en el array de textos.</summary>
    public const int OPCIONB = 2;
    /// <summary>Índice de la opción C en el array de textos.</summary>
    public const int OPCIONC = 3;
    /// <summary>Índice de la opción D en el array de textos.</summary>
    public const int OPCIOND = 4;

    /// <summary>Índice de la respuesta correcta: 0 = A, 1 = B, 2 = C, 3 = D.</summary>
    public int correctAnswer;
    // Textos de la pregunta y sus 4 opciones, según los índices de arriba
    public string question;
    public string[] options;

    /// <summary>
    /// Crea una pregunta con sus 4 opciones y la respuesta correcta.
    /// </summary>
    /// <param name="question">Texto del enunciado.</param>
    /// <param name="opcionA">Texto de la opción A.</param>
    /// <param name="opcionB">Texto de la opción B.</param>
    /// <param name="opcionC">Texto de la opción C (puede ir vacío).</param>
    /// <param name="opcionD">Texto de la opción D (puede ir vacío).</param>
    /// <param name="correctAnswer">Índice de la respuesta correcta (0 = A, 1 = B, 2 = C, 3 = D).</param>
    public Pregunta(string question, string opcionA, string opcionB,
                    string opcionC, string opcionD, int correctAnswer)
    {
        this.question = question;
        this.options = new string[] {opcionA, opcionB, opcionC, opcionD};
        this.correctAnswer = correctAnswer;
    }

    public Pregunta(string question, string[] options, int correctAnswer)
    {
        this.question = question;
        this.options = options;
        this.correctAnswer = correctAnswer;
    }

    /// <summary>Devuelve el índice de la respuesta correcta (0-3).</summary>
    public int getcorrectAnswer(){return correctAnswer;}
    /// <summary>Cambia el índice de la respuesta correcta (0-3).</summary>
    /// <param name="correctAnswer">Nuevo índice correcto: 0 = A, 1 = B, 2 = C, 3 = D.</param>
    public void setcorrectAnswer(int correctAnswer){this.correctAnswer = correctAnswer;}

    /// <summary>Devuelve los 4 opciones: [A, B, C, D].</summary>
    public string[] getOptions(){return options;}
    /// <summary>Sustituye los textos de la pregunta.</summary>
    /// <param name="textos">Array de 4 textos en el orden [A, B, C, D].</param>
    public void setOptions(string[] options){this.options = options;}

    public string getQuestion() {return question;}
    public void setQuestion(string question){this.question = question;}

/*  "activity": {
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
	} */

}