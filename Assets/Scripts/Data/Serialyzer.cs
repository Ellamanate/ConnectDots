using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


public static class Serialyzer
{
    private const string _path = "BestScore";
    private static int _bestScore; 

    public static int BestScore => _bestScore;

    public static bool TrySaveScore(int score)
    {
        float bestScore = LoadBestScore();

        if (score > bestScore)
        {
            Save(new ScoreInfo(score), _path);
            return true;
        }

        return false;
    }

    public static int LoadBestScore() 
    {
        _bestScore = Load<ScoreInfo>(_path).Score;
        return _bestScore;
    }

    private static void Save(object data, string path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + path + ".gd");
        binaryFormatter.Serialize(file, JsonUtility.ToJson(data));
        file.Close();
    }

    private static T Load<T>(string path)
    {
        if (File.Exists(Application.persistentDataPath + "/" + path + ".gd"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + path + ".gd", FileMode.Open);
            string json = (string)binaryFormatter.Deserialize(file);
            file.Close();
            T saves = (T)JsonUtility.FromJson(json, typeof(T));

            return saves;
        }

        return default;
    }

    [Serializable]
    private struct ScoreInfo
    {
        public int Score;

        public ScoreInfo(int score)
        {
            Score = score;
        }
    }
}