using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using System;


public partial class IA : MonoBehaviour
{
    /// <summary>
    /// Tamaño de la ventana en la que se va a aprender
    /// </summary>
    public byte windowSize = 2;

    /// <summary>
    /// Acciones totales realizadas por el jugador
    /// </summary>
    public string totalActions = "";

    /// <summary>
    /// Acciones que puede realizar el jugador
    /// </summary>
    public string possibleActions = "";

    /// <summary>
    /// Referencia al GamePredictor para guardar los valores necesarios
    /// </summary>
    public GamePredictor predictor;

    /// <summary>
    /// Singleton
    /// </summary>
    public static IA instance;

    /// <summary>
    /// Posibles defensas de la IA
    /// </summary>
    public List<string> Defenses = new List<string>();

    /// <summary>
    /// Defensa que va a realizar la IA
    /// </summary>
    public string chosenDefense;

    /// <summary>
    /// Score
    /// </summary>
    public int counter = 0;

    /// <summary>
    /// Referencia al serializador
    /// </summary>
    XmlWriter writer;

    /// <summary>
    /// Referencia al deserializador
    /// </summary>
    XmlReader reader;

    /// <summary>
    /// Enumeración de los movimientos o defensas que puede hacer
    /// </summary>
    enum Movement { Q = 1, W, E, QWE, EWQ, EQW }
    Movement currentMovement;

    /// <summary>
    /// Booleano para controlar si se ha defendido correctamente
    /// </summary>
    public bool defended = false;

    /// <summary>
    /// Textos de interfaz
    /// </summary>
    public Text action;
    public Text score;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        predictor = new GamePredictor();
        enemyAnim = GetComponent<Animator>();

        //creamos el reader
        XmlReaderSettings readersettings = new XmlReaderSettings();
        readersettings.ConformanceLevel = ConformanceLevel.Auto;
        try
        {
            reader = XmlReader.Create(Application.persistentDataPath + "/data.xml", readersettings);
            predictor.data.ReadXml(reader);
        }
        catch (Exception e)
        {

            Debug.LogError("Error al leer" + e.Message + " \n" + e.StackTrace);
        }
        finally { if (reader != null) reader.Close(); }

        //Añadimos las acciones posibles
        AddAction('Q');
        AddAction('E');
        AddAction('W');

    }


    /// <summary>
    /// Al cerrar el juego se guardan los datos
    /// </summary>
    void OnApplicationQuit()
    {
        XmlWriterSettings writersettings = new XmlWriterSettings();
        writersettings.ConformanceLevel = ConformanceLevel.Auto;

        writer = XmlWriter.Create(Application.persistentDataPath + "/data.xml", writersettings);

        predictor.data.WriteXml(writer);
        writer.Close();
    }


    private void Update()
    {
        action.text = "IA defendiendo: " + chosenDefense;
        score.text = counter.ToString();
    }

    /// <summary>
    /// Método para añadir una acción a possible actions
    /// </summary>
    /// <param name="action"></param>
    public void AddAction(char action)
    {
        possibleActions += action;

    }

    /// <summary>
    /// Método para elegir una acción aleatoria
    /// </summary>
    /// <returns></returns>
    private char RandomGuess()
    {
        return possibleActions[UnityEngine.Random.Range(0, possibleActions.Length)];
    }

    /// <summary>
    ///Lógica del algoritmo, esta función elegirá una acción para contraatacar
    /// </summary>
    public void Guess()
    {
        defended = false;

        string lastActions = "";

        char guess;

        if (totalActions.Length >= windowSize)
        {
            //tomamos las últimas acctiones para predecir la siguiente
            lastActions = totalActions.Substring(totalActions.Length - windowSize, windowSize);
            guess = predictor.GetMostLikely(lastActions);
            if (guess == ' ')
            {
                guess = RandomGuess();
            }

            //Elección de la defensa 
            if (Defenses.Contains(lastActions + guess))
                chosenDefense = lastActions + guess;
            else
            {
                chosenDefense = guess.ToString();
                //guardamos la acción elegida
                predictor.RegisterActions(chosenDefense);
            }

        }
        else
        {
            guess = RandomGuess();

            //Eleccion de la defensa 
            chosenDefense = guess.ToString();

            //guardamos la acción elegida
            predictor.RegisterActions(chosenDefense);
        }

        currentMovement = (Movement)Enum.Parse(typeof(Movement), chosenDefense);

        //Animaciones
        if ((int)currentMovement <= 3)
        {
            AnimateEnemy(1);
        }
        else
        {
            AnimateEnemy(2);
        }

        RegisterAction(guess);
    }

    /// <summary>
    /// Método para registrar las acciones realizadas en Data
    /// </summary>
    /// <param name="guess"></param>
    public void RegisterAction(char guess)
    {
        //Obtenemos lastActions
        string lastActions = "";
        if (totalActions.Length >= windowSize)
        {
            lastActions = totalActions.Substring(totalActions.Length - windowSize, windowSize);
        }
        else if (totalActions.Length != 0)
        {
            lastActions = totalActions;
        }
        else
        {
            lastActions = "";
        }

        //Registramos las acciones junto con la elección
        predictor.RegisterActions(lastActions + guess);

        if (lastActions == "")
        {
            predictor.data[guess.ToString()].counts[guess]++;
            predictor.data[guess.ToString()].total++;
        }
        else
        {
            predictor.data[lastActions].counts[guess]++;
            predictor.data[lastActions].total++;
        }

    }
}
