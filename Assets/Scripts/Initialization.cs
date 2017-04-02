using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Initialization : MonoBehaviour
{

	private static bool initialized = false;

	void Start ()
	{
		if (initialized) {
			return;
		}
		initialized = true;

		SceneManager.LoadScene (SceneNames.Menu, LoadSceneMode.Additive);
	}


}
