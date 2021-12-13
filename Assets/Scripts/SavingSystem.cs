using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


// States of planet are managed by saving and loading data relating to a planet. Depending if a planet has been beaten or not will determine if other levels can be played or not
public static class SavingSystem 
{
    // stores planet name and whether it has been beaten in a new binary file (overwritting any old ones of the same name)
    public static void SavePlanet (string planet, bool isCompleted)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + planet + ".data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlanetData data = new PlanetData(isCompleted);

        formatter.Serialize(stream, data);

        stream.Close();

    }

    // if a file exists, the data is retrieved using the planet name from the overworld
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
