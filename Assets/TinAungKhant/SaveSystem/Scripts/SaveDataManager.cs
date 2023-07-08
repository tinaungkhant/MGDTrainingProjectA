using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace TinAungKhant.SaveSystem
{
	public class SaveDataManager : MonoBehaviour
	{
		public static SaveDataManager Instance;
		public string SaveFileName;

		[Space()]
		public SaveInformation m_SaveData;

		public void Awake()
		{
			if (Instance != null)
			{
				Destroy(this.gameObject);
				return;
			}
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
			SetApplicationSetting();
		}

		private void SetApplicationSetting()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Application.runInBackground = true;
		}

		public void SaveData()
		{
			string SaveString = JsonUtility.ToJson(m_SaveData);
			string SavePath = Application.persistentDataPath + "/" + GetSaveFilePath();
			JsonHandler.SaveJson(SavePath, SaveString);
		}

		public void LoadData()
		{
		
			string SavePath = Application.persistentDataPath + "/" + GetSaveFilePath();

			if (JsonHandler.CheckSaveFileExist(SavePath))
			{
				string SaveString = JsonHandler.LoadJson(SavePath);
				m_SaveData = JsonUtility.FromJson<SaveInformation>(SaveString);
			}

		}

		public string GetSaveFilePath()
		{
			return $"{SaveFileName}.txt";
		}

		[ContextMenu("ForceSave")]
		public void ForceSave()
		{
			SaveData();
		}
	}
}
[System.Serializable]
public class SaveInformation
{
	public List<CubeData> CubesList = new List<CubeData>();
}
