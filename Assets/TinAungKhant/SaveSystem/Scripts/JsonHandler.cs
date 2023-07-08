using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace TinAungKhant.SaveSystem
{
	//Removed encryption part since this is for beginners
	public class JsonHandler : MonoBehaviour
	{
		public static void SaveJson(string path,string json)
		{
			File.WriteAllText(path,json);
		}
		public static string LoadJson(string path)
		{
			string data = File.ReadAllText(path);

			return data;
		}

		public static bool CheckSaveFileExist(string path)
		{
			return File.Exists(path);
		}
	}
}
