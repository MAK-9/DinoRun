using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(GameController gameController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/highScore.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataSaver data = new DataSaver(gameController);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DataSaver LoadData()
    {
        string path = Application.persistentDataPath + "/highScore.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataSaver data =  formatter.Deserialize(stream) as DataSaver;
            stream.Close();
            
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
        
    }
}
