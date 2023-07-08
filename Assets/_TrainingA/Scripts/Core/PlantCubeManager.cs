using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TinAungKhant.SaveSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantCubeManager : MonoBehaviour
{
    public static PlantCubeManager instance;

	[Header("Properties")]
	public E_InteractType InteractType = E_InteractType.Unknown;
	public E_CubeType SelectedCubeType = E_CubeType.Unknown;

	public LayerMask InteractLayer;

	[Header("Cubes References")]
	public CubeInstance[] Cubes = new CubeInstance[3];

	public Dictionary<Vector2,CubeInstance> CubesDictionary = new Dictionary<Vector2,CubeInstance>();

	private void Start()
	{
		Application.targetFrameRate = 60;
	}

	public void OnEnable()
	{
		instance = this;
	}

	public void OnDisable()
	{
		instance = null;
	}

	private void Update()
	{
		if (EventSystem.current.currentSelectedGameObject != null)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,1000f,InteractLayer))
			{
				float posX = Mathf.Round(hit.point.x);
				float posZ = Mathf.Round(hit.point.z);

				switch (InteractType)
				{
					case E_InteractType.Plant:

					//only plant when space is free
					HandlePlantingProcess(posX,posZ);

					break;

					case E_InteractType.Delete:

					HandleDeletingProcess(posX,posZ);

					break;
				}
			}
		}
		else if (Input.GetMouseButton(0))
		{
			if (InteractType == E_InteractType.Delete)
			{
				RaycastHit hit;
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,1000f,InteractLayer))
				{
					float posX = Mathf.Round(hit.point.x);
					float posZ = Mathf.Round(hit.point.z);

					//only delete when cube is availble
					HandleDeletingProcess(posX,posZ);
				}		
			}
		}
	}

	private void HandlePlantingProcess(float posX,float posZ)
	{
		if (SelectedCubeType == E_CubeType.Unknown)
		{
			return;
		}

		Vector2 vectorValue = new Vector2(posX, posZ);

		if(CubesDictionary.ContainsKey(vectorValue))
		{
			return;
		}

		int Index = (int)SelectedCubeType;

		CubeData data = new CubeData() { CubeType = SelectedCubeType, CubePosition = vectorValue};

		CubeInstance ins= Instantiate(Cubes[Index],new Vector3(posX,0.4f,posZ),Quaternion.identity,this.transform);

		StoreIntoDictionary(ins,vectorValue);

		//fill data ref here
		ins.Data = data;
	}

	private void HandleDeletingProcess(float posX,float posZ)
	{
		Vector2 vectorValue = new Vector2(posX,posZ);

		if (CubesDictionary.ContainsKey(vectorValue))
		{
			CubeInstance ins = CubesDictionary[vectorValue];
			Destroy(ins.gameObject);
			RemoveFromDictionary(vectorValue);
		}
	}

	public void StoreIntoDictionary(CubeInstance ins,Vector2 vectorValue)
	{
		if (!CubesDictionary.ContainsKey(vectorValue))
		{
			CubesDictionary.Add(vectorValue,ins);
		}
	}

	public void RemoveFromDictionary(Vector2 vectorValue)
	{
		if (CubesDictionary.ContainsKey(vectorValue))
		{
			CubesDictionary.Remove(vectorValue);
		}
	}

	public void StartSaveCubes(Action callback)
	{
		SaveDataManager.Instance.m_SaveData.CubesList.Clear();

		SavingCubes(callback);
	}

	private async void SavingCubes(Action callback)
	{
		foreach (CubeInstance ins in CubesDictionary.Values)
		{
			SaveDataManager.Instance.m_SaveData.CubesList.Add(ins.Data);
		}

		SaveDataManager.Instance.SaveData();

		await Task.Delay(500);

		callback.Invoke();
	}

	public void LoadCubes(Action callback)
	{
		SaveDataManager.Instance.LoadData();

		LoadingCubes(callback);
	}

	private async void LoadingCubes(Action callback)
	{
		foreach (CubeInstance ins in CubesDictionary.Values)
		{
			Destroy(ins.gameObject);

			await Task.Delay(0);
		}
		CubesDictionary.Clear();

		await Task.Delay(20);

		foreach (CubeData data in SaveDataManager.Instance.m_SaveData.CubesList)
		{
			int Index = (int)data.CubeType;

			CubeInstance ins = Instantiate(Cubes[Index],new Vector3(data.CubePosition.x,0.4f,data.CubePosition.y),Quaternion.identity,this.transform);

			ins.Data = data;

			StoreIntoDictionary(ins,data.CubePosition);

			await Task.Delay(20);
		}

		callback.Invoke();
	}
}
