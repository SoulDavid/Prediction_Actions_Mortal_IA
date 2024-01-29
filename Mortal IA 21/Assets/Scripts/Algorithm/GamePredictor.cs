using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePredictor
{
    /// <summary>
    /// Diccionario de Data Records 
    /// </summary>
    public SerializableDictionary<string, DataRecord> data;

    public GamePredictor()
    {
        data = new SerializableDictionary<string, DataRecord>();
    }

    /// <summary>
    /// Método que guarda las acciones realizadas y el número de veces que se han hecho
    /// </summary>
    /// <param name="actions"></param>
    public void RegisterActions(string actions)
    {
        string key = "";
    
        if (actions.Length > 2)
            key = actions.Substring(0, 2);
        else
            key = actions[0].ToString();
         

        char value = actions[actions.Length - 1];

        if (!data.ContainsKey(key))
        {
            data[key] = new DataRecord();
        }
        DataRecord record = data[key];
        if (!record.counts.ContainsKey(value))
        {
            record.counts[value] = 0;
            record.probabilities[value] = 1;
        }

        Debug.Log("Data " + key);
        Debug.Log("COUNT " + value);

    }

    /// <summary>
    /// Se elige la acción que más probabilidades tiene de ser elegida
    /// </summary>
    /// <param name="actions"></param>
    /// <returns></returns>
    public char GetMostLikely(string actions)
    {
        char bestAction = ' ';
        float highestValue = 0;
        float charValue = 0;
        DataRecord record;

        //Miramos si data contiene actions
        if (data.ContainsKey(actions))
        {
            record = data[actions];
            foreach (char action in record.counts.Keys)
            {
                charValue = record.counts[action] * record.probabilities[action];
                if (charValue > highestValue)
                {
                    bestAction = action;
                    highestValue = charValue;
                }
                else if (charValue == highestValue)
                {
                    if (Random.value <= 0.5f)
                    {
                        bestAction =  action;
                        highestValue = charValue;
                    }
                }
            }
        }
        return bestAction;
    }

}
