using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingSystem 
{
    public static void SavePlanet (string planet, bool isCompleted)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + planet + ".data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlanetData data = new PlanetData(isCompleted);

        formatter.Serialize(stream, data);

        stream.Close();

    }

    public static PlanetData LoadPlanet(string planet)
    {
        string path = Application.persistentDataPath + "/" + planet + ".data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlanetData data = (PlanetData)formatter.Deserialize(stream);
            stream.Close();

            return data;

        }
        else
        {
            return null;
        }
    }

   
}
