using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;
using RotaryHeart.Lib.SerializableDictionary;

[System.Serializable]
public class DataRecord
{
    /// <summary>
    /// Numero de veces que este Data Record se ha registrado/hecho
    /// </summary>
    public int total;

    /// <summary>
    /// Veces que se ha pulsado cada tecla de la accion
    /// </summary>
    public SerializableDictionary<char, int> counts;

    /// <summary>
    /// Probabilidades de salir que tiene esta accion
    /// </summary>
    public SerializableDictionary<char, float> probabilities;
    public DataRecord()
    {
        total = 0;
        counts = new SerializableDictionary<char, int>();
        probabilities = new SerializableDictionary<char, float>();
    }
}
