using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using UnityEngine;
using System.Xml.Schema;

/// Clase para serializar y deserializar
/// obtenida de https://gist.github.com/tiagosr/f82f8f9f5a5444e9ed41

public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    public System.Xml.Schema.XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(System.Xml.XmlReader reader)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        bool wasEmpty = reader.IsEmptyElement;
        reader.Read();

        if (wasEmpty)
            return;

        while (reader.NodeType != System.Xml.XmlNodeType.EndElement && reader.NodeType != System.Xml.XmlNodeType.None)
        {
            reader.ReadStartElement("item");

            reader.ReadStartElement("key");
            TKey key = (TKey)keySerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("value");
            TValue value = (TValue)valueSerializer.Deserialize(reader);

            reader.ReadEndElement();

            this.Add(key, value);

            reader.ReadEndElement();
            reader.MoveToContent();
        }
        if (reader.NodeType != System.Xml.XmlNodeType.None)
            reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        foreach (TKey key in this.Keys)
        {
            writer.WriteStartElement("item");

            writer.WriteStartElement("key");
            keySerializer.Serialize(writer, key);
            writer.WriteEndElement();

            TValue value = this[key];
            writer.WriteStartElement("value");
            valueSerializer.Serialize(writer, value);

            writer.WriteEndElement();
            writer.WriteEndElement();

        }
    }
}

