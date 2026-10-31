using System.Windows.Forms;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageButton : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI languageButton;
	[SerializeField] TutorialPopUp tutorialPopUp;
	[SerializeField] TextMeshProUGUI RelativeText;
	[SerializeField] TextMeshProUGUI HoldText;

	public void StartLanguage()
	{
		string language = VariablesLoader.Data.language;
		SetLanguage(language);



	}

	public void CycleLanguage()
	{
		if (VariablesLoader.Data.language.Equals("español"))
		{
			SetLanguage("english");
		}
		else
		{
			SetLanguage("español");
		}
	}



	public void SetLanguage(string language)
	{

		if (language.Equals("español"))
		{
			languageButton.text = "español";
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
			VariablesLoader.Data.language = "español";
			VariablesLoader.Save();

			//Tutorial...
			TutorialPopUp.dialogs = new string[] {
				"Bienvenido al tutorial, si quieres continuar, pulsa 'C', si quieres saltártelo, pulsa 'M'.",//0
                "Muévete con WASD o las flechas, mueve la cámara con el ratón.",//1
                "También tienes las teclas 'Q' y 'E' para bajar y subir.",//2
                "Abre el menú principal con el 'ESPACIO'.",//3
                "Pulse el botón de abajo a la izquierda para salir del menu, tiene un icono de una pantalla y una flecha hacia abajo. Fíjese que también podría pulsar la tecla escape.",//4
                "Vuelve a abrir el menú y carga la escena predeterminada, abre el menu principal con 'ESPACIO' y pulsa el botón de abajo a la derecha \"Crear Escena\" con el lápiz y +.",//5
                "Ahora, cierra el menú para observar la escena y localiza al NPC. Una vez lo encuentres, apunta con el cursor al objeto que creaste anteriormente y pulsa la tecla Z para borrarlo.",//6
                "Deshaz tus eliminaciones con 'CTRL+Z'.",//7
                "Abre el menú con 'Espacio'. Crea puertas a habitaciones nuevas con los botones de arriba. Para borrarla púlsalo de nuevo, ROOM x:0 y:0 representa la habitación en la que estás.",//8
                "Viaja a la nueva habitación haciendo click izquierdo en cualquier puerta creada en la escena. Ten en cuenta que tu posición sigue la logica de un mapa. Norte y sur el eje Y, este y oeste el eje X",//9
                "Ve al menú principal y pulsa la pestaña OBJETOS, allí se ven los objetos que tienes cargados / disponibles.",//10
                "Puedes también buscar objetos en específico en el buscador. Regresa al menú pulsando la flecha de arriba a la izquierda, o con el ESC y espacio.",//11
                "Haz click en CONFIGURACIÓN, aquí hay configuraciones extras donde puedes cambiar la carpeta de objetos o las formas de movimiento.",//12
                "Cambia las configuraciones que gustes, estas se guardarán aun cerrando el proyecto. ¡Hagamos lo más importante, creemos nuestra propia escena!, Primero, vuelve al menú principal y copia el Prompt de la IA.",//13
                "Ve a tu navegador y utiliza la IA de tu preferencia, pégalo, y añade el tema que quieres enseñar al final. Copia la respuesta entera y pégala en el campo de la izquierda.",//14
                "Genial, ahora que está cargado, ya puedes pulsar el botón crear la escena.",//15
                "Observa tu nueva escena, haz click izquierdo sobre cualquier objeto para acceder a un menu donde puedes ajustar dicho objeto.",//16
                "Aquí podrás ajustar la posición, el tamaño, y la rotación, pulsa los botones o utiliza las teclas asociadas.",//17
                "¡Bien Hecho! Pulsa la tecla 'C' para terminar el tutorial.", //18
				"Esto es el final, si estás leyendo esto es un error." }; //19
			tutorialPopUp.Reload();

			if (VariablesLoader.Data.movementType.Equals("relative"))
				RelativeText.text = "Relativo";
			else
				RelativeText.text = "Absoluto";

			if (VariablesLoader.Data.movementHold.Equals("hold"))
				HoldText.text = "Mantener";
			else
				HoldText.text = "No mantener";

		}
		else if (language.Equals("english"))
		{
			languageButton.text = "english";
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
			VariablesLoader.Data.language = "english";
			VariablesLoader.Save();

			//Tutorial...
			TutorialPopUp.dialogs = new string[] {
				"This is a tutorial, press the 'C' key to continue the tutorial, if you wish to skip it, please press the 'M' key.",//0
                "You can move with WASD or the arrow keys, and look with the mouse",//1
                "You can also move with Q and E to go down and up.",//2
                "Open the menu with SPACE",//3
                "Then press the down-left icon with the screen and an arrow down to hide this menu or press 'Esc'",//4
                "Open the menu again and create the default scene, press space and click on the bottom right icon \"Create Scene\" with a plus sign.",//5
                "Now, close the menu to observe the scene and locate the NPC. Once you find them, aim your cursor at the object you previously created and press the Z key to delete it.",//6
                "Undo your deletion with CTRL+Z",//7
                "Create a door to a new room with any of the top icons on the menu. To erase them, click on the button again, The ROOM x:0 y:0 represents which room you are on",//8
                "Travel to the next room by left clicking on any door created. Please note that doors follow cardinal directions, and the room counter follows a map count. North & South for Y axis, East & West for X axis",//9
                "Go to the menu and click on the Objects tab, this menu displays all of the objects available / loaded.",//10
                "You can search anything by typing on the input box, now return to the menu by clicking on the top-left arrow or by pressing ESC.",//11
                "Go to the menu and click on the Configuration tab, this menu displays extra settings.",//12
                "You can change anything you wish, they'll save even if you close the app. Now let's do the most important thing, create our own scene! First, go back to the main menu and copy the AI prompt",//13
                "Go to your preffered AI, paste it, and ask for anything. Copy the entire response and paste it on the ForceJson field, and click it.",//14
                "Great, now on the main menu, create the new scene.",//15
                "Observe your new scene, left click any objects to open the Click Menu.",//16
                "Here you can click on the buttons to adjust any object, or use the referenced keys. Well Done, you completed the tutorial!",//17
                "Well Done! Press 'C' to end the tutorial.", //18
				"You shouldn't be reading this, this is an error" //19
				}; //18
			tutorialPopUp.Reload();

			if (VariablesLoader.Data.movementType.Equals("relative"))
				RelativeText.text = "Relative";
			else
				RelativeText.text = "Absolute";

			if (VariablesLoader.Data.movementHold.Equals("hold"))
				HoldText.text = "Hold";
			else
				HoldText.text = "Not Hold";

		}

	}

}