using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameResultViewMediator : MonoBehaviour
{
	public GameResultView View;

	public void Start ()
	{
		View.SetWinner (Storage.Instance.GameModel.LastWinnerPlayerId);
		View.OkButton.onClick.AddListener (OnOkButtonClick);
	}

	public void OnDestroy ()
	{
		View.OkButton.onClick.RemoveListener (OnOkButtonClick);
	}

	private void OnOkButtonClick ()
	{
		SceneManager.LoadSceneAsync (SceneNames.Menu);
	}
}
