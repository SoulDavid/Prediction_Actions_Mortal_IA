using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public partial class Player : MonoBehaviour
{
    /// <summary>
    /// Enumeración de los movimientos que puede hacer
    /// </summary>
    enum Movement { Q = 1, W, E, QWE, EWQ, EQW }
    Movement currentMovement;

    /// <summary>
    /// Movimientos posibles
    /// </summary>
    public List<string> Movements = new List<string>();

    /// <summary>
    /// Acción actual
    /// </summary>
    char combo;

    /// <summary>
    /// Método para comprobar si se está haciendo una animación
    /// </summary>
    bool makingAnimation;

    /// <summary>
    /// Texto de interfaz
    /// </summary>
    public Text key_pressed;

    void Start()
    {

        playerAnimator = GetComponent<Animator>();
        //las defensas de la IA serán similares a nuestros movimientos
        IA.instance.Defenses = Movements;

        //Guess
        IA.instance.Guess();
    }

    void Update()
    {
        //Si no se está haciendo una animación
        if (!makingAnimation)
        {
            //Añadimos las acciones al combo y llamamos a la IA
            if (Input.GetKeyDown(KeyCode.Q))
            {
                combo = 'Q';
                currentMovement = Movement.Q;
                CheckAction();

            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                combo = 'E';
                currentMovement = Movement.E;
                CheckAction();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                combo = 'W';
                currentMovement = Movement.W;
                CheckAction();
            }


        }
    }

    /// <summary>
    /// Método para llamar a la IA y comprobar los resultados
    /// </summary>
    private void CheckAction()
    {
        makingAnimation = true;

        key_pressed.text = "Tecla pulsada: " + combo.ToString();

        Debug.Log("Jugador data");

        //Registramos la acción realizada
        IA.instance.RegisterAction(combo);

        //Añadimos a total actions
        IA.instance.totalActions += combo;
        string _totalActions = IA.instance.totalActions;


        //COMPROBAMOS SI ES COMBO
        if (_totalActions.Length > 2 && Movements.Contains(_totalActions.Substring(_totalActions.Length - 3, 3)))
        {
           

            //COMPROBAMOS ACION - DEFENSA (si ha fallado la defensa, le restamos 0,2 al probabilities)
            if (_totalActions.Substring(_totalActions.Length - 3, 3) == IA.instance.chosenDefense)
            {
                IA.instance.counter += 5;
                IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]] = Mathf.Clamp(
                IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]] += 0.2f, 0, 1);
                IA.instance.defended = true;
            }
            else
            {
                if (IA.instance.chosenDefense.Length > 2)
                {
                    IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]] = Mathf.Clamp(
                    IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]] -= 0.2f, 0, 1);

                    Debug.Log("Probabilidades: " + IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]]);

                }
                else
                {
                    IA.instance.predictor.data[IA.instance.chosenDefense].probabilities[IA.instance.chosenDefense[0]] = Mathf.Clamp(
                    IA.instance.predictor.data[IA.instance.chosenDefense].probabilities[IA.instance.chosenDefense[0]] -= 0.2f, 0, 1);
                    Debug.Log("Probabilidades: " + IA.instance.predictor.data[IA.instance.chosenDefense].probabilities[IA.instance.chosenDefense[0]]);
                }

                IA.instance.counter -= 5;
            }


            // BORRAMOS totalactions 
            IA.instance.totalActions = "";
            AnimatePlayer(4);

        }
        else
        {
            // COMPROBAMOS SI LA ULTIMA TECLA PULSADA ES ACCION DE LA DEFENSA GUESEADA (si ha fallado la defensa, le restamos 0,1 al probabilities)
            if (Movements.Contains(combo.ToString()))
            {         
                if (combo.ToString() == IA.instance.chosenDefense)
                {
                    if (IA.instance.chosenDefense.Length > 2)
                    {
                        IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]] = Mathf.Clamp(
                        IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]] += 0.1f, 0, 1);

                        Debug.Log("Probabilidades: " + IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]]);
                    }
                    else
                    {
                        IA.instance.predictor.data[IA.instance.chosenDefense].probabilities[IA.instance.chosenDefense[0]] = Mathf.Clamp(
                        IA.instance.predictor.data[IA.instance.chosenDefense].probabilities[IA.instance.chosenDefense[0]] += 0.1f, 0, 1);

                        Debug.Log("Probabilidades: " + IA.instance.predictor.data[IA.instance.chosenDefense].probabilities[IA.instance.chosenDefense[0]]);
                    }
                    IA.instance.defended = true;
                    IA.instance.counter += 1;
                   
                }
                else
                {
                    if (IA.instance.chosenDefense.Length > 2)
                    {
                        IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]] = Mathf.Clamp(
                        IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]] -= 0.1f, 0, 1);

                        Debug.Log("Probabilidades: " + IA.instance.predictor.data[IA.instance.chosenDefense.Substring(0, 2)].probabilities[IA.instance.chosenDefense[2]]);
                    }
                    else
                    {
                        IA.instance.predictor.data[IA.instance.chosenDefense].probabilities[IA.instance.chosenDefense[0]] = Mathf.Clamp(
                        IA.instance.predictor.data[IA.instance.chosenDefense].probabilities[IA.instance.chosenDefense[0]] -= 0.1f, 0, 1);

                        Debug.Log("Probabilidades: " + IA.instance.predictor.data[IA.instance.chosenDefense].probabilities[IA.instance.chosenDefense[0]]);
                    }

                    IA.instance.counter -= 1;
                }
                AnimatePlayer((int)currentMovement);
            }
        }
    }

    /// <summary>
    /// Llamado desde evento de animación para que la IA reaccione
    /// </summary>
    void DoGuess()
    {
        //Guess
        IA.instance.Guess();
    }
}
