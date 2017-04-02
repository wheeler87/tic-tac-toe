using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuViewMediator : MonoBehaviour
{

	public MenuView View;

	public void Start ()
	{
		this.View.PlayButton.onClick.AddListener (OnPlayButtonClicked);	
		this.View.OnDifficultyChanged += OnGameDifficultyChangeRequest;
		this.View.Init (Storage.Instance.GameModel.Difficulty);
	}

	private void OnDestroy ()
	{
		this.View.PlayButton.onClick.RemoveListener (OnPlayButtonClicked);
		this.View.OnDifficultyChanged -= OnGameDifficultyChangeRequest;
	}

	private void OnPlayButtonClicked ()
	{
		SceneManager.LoadSceneAsync (SceneNames.Game, LoadSceneMode.Additive);
		SceneManager.UnloadScene (SceneNames.Menu);
	}

	private void OnGameDifficultyChangeRequest (GameDifficulty gameDifficulty)
	{
		Storage.Instance.GameModel.Difficulty = gameDifficulty;
	}
}
