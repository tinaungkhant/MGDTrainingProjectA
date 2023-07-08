using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TglInteract : MonoBehaviour
{
	public E_InteractType InteractType;
	public E_CubeType CubeType;
	[HideInInspector]public Toggle m_Toggle;

	private void Awake()
	{
		m_Toggle = GetComponent<Toggle>();
	}
}
