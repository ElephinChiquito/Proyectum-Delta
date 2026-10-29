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
/// <item><description><c>public int answerNumber;</c></description></item>
/// <item><description><c>private string[] textos;</c></description></item>
/// </list>
/// </summary>
public class Pregunta
{
    // Índices de cada texto dentro del array "textos"
    /// <summary>Índice del enunciado en el array de textos.</summary>
    public const int PREGUNTA = 0;
    /// <summary>Índice de la opción A en el array de textos.</summary>
    public const int OPCIONA = 1;
    /// <summary>Índice de la opción B en el array de textos.</summary>
    public const int OPCIONB = 2;
    /// <summary>Índice de la opción C en el array de textos.</summary>
    public const int OPCIONC = 3;
    /// <summary>Índice de la opción D en el array de textos.</summary>
    public const int OPCIOND = 4;

    /// <summary>Índice de la respuesta correcta: 0 = A, 1 = B, 2 = C, 3 = D.</summary>
    public int answerNumber;
    // Textos de la pregunta y sus 4 opciones, según los índices de arriba
    private string[] textos;

    /// <summary>
    /// Crea una pregunta con sus 4 opciones y la respuesta correcta.
    /// </summary>
    /// <param name="pregunta">Texto del enunciado.</param>
    /// <param name="opcionA">Texto de la opción A.</param>
    /// <param name="opcionB">Texto de la opción B.</param>
    /// <param name="opcionC">Texto de la opción C (puede ir vacío).</param>
    /// <param name="opcionD">Texto de la opción D (puede ir vacío).</param>
    /// <param name="answerNumber">Índice de la respuesta correcta (0 = A, 1 = B, 2 = C, 3 = D).</param>
    public Pregunta(string pregunta, string opcionA, string opcionB,
                    string opcionC, string opcionD, int answerNumber)
    {
        textos = new string[] {pregunta, opcionA, opcionB, opcionC, opcionD};
        this.answerNumber = answerNumber;
    }

    /// <summary>Devuelve el índice de la respuesta correcta (0-3).</summary>
    public int getAnswerNumber(){return answerNumber;}
    /// <summary>Cambia el índice de la respuesta correcta (0-3).</summary>
    /// <param name="answerNumber">Nuevo índice correcto: 0 = A, 1 = B, 2 = C, 3 = D.</param>
    public void setAnswerNumber(int answerNumber){this.answerNumber = answerNumber;}

    /// <summary>Devuelve los 5 textos: [enunciado, A, B, C, D].</summary>
    public string[] getTextos(){return textos;}
    /// <summary>Sustituye los textos de la pregunta.</summary>
    /// <param name="textos">Array de 5 textos en el orden [enunciado, A, B, C, D].</param>
    public void setTextos(string[] textos){this.textos = textos;}


}