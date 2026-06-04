using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float ClearTime = float.MaxValue;
    public int ClearStage;
}

[System.Serializable]
public class GameDataList
{
    public List<GameData> Records = new List<GameData>();
}

[System.Serializable]
public class AudioData
{
    public float BGMVolume = 1f;
    public float SEVolume = 1f;
}

[System.Serializable]
public class AudioDataList
{
    public AudioData data = new AudioData();
}

public class SkillSaveData
{
    public int ID;
    public bool isUnlocked;
}

public class SkillSaveDataList
{
    public List<SkillSaveData> m_skillDataList = new List<SkillSaveData>();
}

public class SaveManager : MonoBehaviour
{
    //public static SaveManager Instance;

    private string fileName = "gamedata.json";
    private string fullPath;

    private string audioFileName = "audioSettings.json";
    private string audioFullPath;

    private string skillFileName = "skillSettings.json";
    private string skillFullPath;

    void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject); // 重複を消す
        //}
        fullPath = Path.Combine(Application.persistentDataPath, fileName);
        audioFullPath = Path.Combine(Application.persistentDataPath, audioFileName);
        skillFullPath = Path.Combine(Application.persistentDataPath, skillFileName);
    }

    public void Save(GameData newdata)
    {
        GameDataList data = Load();

        data.Records.Add(newdata);

        data.Records.Sort((a,b) => a.ClearTime.CompareTo(b.ClearTime));

        if(data.Records.Count > 10 )
        {
            data.Records.RemoveRange(10,data.Records.Count - 10);
        }


        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json);
        Debug.Log("Saved to: " + fullPath + " | ClearTime: " + newdata.ClearTime);
    }

    public void AudioSave(float newBGMVolume,float newSEVolume)
    {
        AudioDataList audioDataList = new AudioDataList();
        audioDataList.data.BGMVolume = newBGMVolume;
        audioDataList.data.SEVolume = newSEVolume;

        string json = JsonUtility.ToJson(audioDataList, true);
        File.WriteAllText(audioFullPath, json);
    }

    public void SkillSave(SkillSaveDataList data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(skillFullPath, json);
    }

    public GameDataList Load()
    {
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<GameDataList>(json);
        }
        else
        {
            return new GameDataList(); // デフォルト値
        }
    }

    public AudioDataList AudioLoad()
    {
        if (File.Exists(audioFullPath))
        {
            string json = File.ReadAllText(audioFullPath);
            return JsonUtility.FromJson<AudioDataList>(json);
        }
        else
        {
            return new AudioDataList(); // デフォルト値
        }
    }

    public SkillSaveDataList SkillLoad()
    {
        if(File.Exists(skillFullPath))
        {
            string json = File.ReadAllText(skillFullPath);
            return JsonUtility.FromJson<SkillSaveDataList>(json);
        }
        else
        {
            return new SkillSaveDataList();
        }
    }

    public void ResetData()
    {
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
