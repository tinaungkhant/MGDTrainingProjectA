using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlantCube : MonoBehaviour
{
	public TglInteract[] InteractToggles = new TglInteract[4];

	[SerializeField] private GameObject loadingObj;
	[SerializeField] private TextMeshProUGUI txtLoading;

	//Tracking which toggle is active and notify PlantCubeManager what modes player did choose
	public void OnToggleSelectionChange()
	{
		if (PlantCubeManager.instance != null)
		{
			TglInteract activeToggle = null; 
			foreach (TglInteract tgl in InteractToggles)
			{
				if (tgl.m_Toggle.isOn)
				{
					activeToggle = tgl;
				}
			}

			if (activeToggle != null)
			{
				PlantCubeManager.instance.InteractType = activeToggle.InteractType;
				PlantCubeManager.instance.SelectedCubeType = activeToggle.CubeType;
			}
			else
			{
				PlantCubeManager.instance.InteractType = E_InteractType.Unknown;
				PlantCubeManager.instance.SelectedCubeType = E_CubeType.Unknown;
			}
		}
	}

	public void OnClickSave()
	{
		//ToDo need to notify save all cubes
		txtLoading.text = "Saving...";
		loadingObj.SetActive(true);

		PlantCubeManager.instance.StartSaveCubes(OnFinishThread);
	}

	public void OnClickLoad()
	{
		//ToDo need to notify loading all cubes from save file
		txtLoading.text = "Loading...";
		loadingObj.SetActive(true);

		PlantCubeManager.instance.LoadCubes(OnFinishThread);
	}

	public void OnFinishThread()
	{
		loadingObj.SetActive(false);
	}
}
