using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameViewMediator : MonoBehaviour
{

	public GameView View;

	public void Start ()
	{
		View.BackButton.onClick.AddListener (OnBackButton);
		View.OnCellSelected += OnCellSeleted;

		Storage.Instance.GameModel.OnGameStarted += OnGameStarted;
		Storage.Instance.GameModel.OnGameComplete += OnGameComplete;
		Storage.Instance.GameModel.OnMoveMade += OnMoveMade;

		Storage.Instance.GameModel.StartGame ();
	}

	public void OnDestroy ()
	{
		View.BackButton.onClick.RemoveListener (OnBackButton);
		View.OnCellSelected -= OnCellSeleted;

		Storage.Instance.GameModel.OnGameStarted -= OnGameStarted;
		Storage.Instance.GameModel.OnGameComplete -= OnGameComplete;
		Storage.Instance.GameModel.OnMoveMade -= OnMoveMade;
	}

	public void Update ()
	{
		Storage.Instance.GameModel.Update ();
	}


	public void OnBackButton ()
	{
		SceneManager.LoadSceneAsync (SceneNames.Menu);
	}

	private void OnGameStarted ()
	{
		View.SetSize (Storage.Instance.GameModel.GameField.Size);
		View.SetParameters (Storage.Instance.GameModel.TotalWins,
			Storage.Instance.GameModel.TotalLoses,
			Storage.Instance.GameModel.TotalDraws,
			Storage.Instance.GameModel.Difficulty);
		View.SetGameField (Storage.Instance.GameModel.GameField, 
			Storage.Instance.GameModel.FirstPlayerId);
	}

	private void OnMoveMade ()
	{
		View.SetGameField (Storage.Instance.GameModel.GameField, 
			Storage.Instance.GameModel.FirstPlayerId);
	}

	private void OnGameComplete ()
	{
		if (Storage.Instance.GameModel.LastWinnerPlayerId != 0) {
			var wonPositions = Storage.Instance.GameModel.GameField.GetWonPositions (Storage.Instance.GameModel.LastWinnerPlayerId);
			View.SetHighlightedCells (wonPositions);
		}

		SceneManager.LoadSceneAsync (SceneNames.GameResult, LoadSceneMode.Additive);
	}

	public void OnCellSeleted (int column, int row)
	{
		Storage.Instance.GameModel.HumanPlayer.OnCellSelected (column, row);
	}

}
