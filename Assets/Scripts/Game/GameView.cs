using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameView : MonoBehaviour
{
	public Action<int,int> OnCellSelected;
	public Button BackButton;
	public Transform CellContainer;
	public GameObject CellPrefab;
	public Text WinsText;
	public Text LosesText;
	public Text DrawsText;
	public Text DifficultyText;

	private List<Cell> cellInstanceList = new List<Cell> ();


	public void SetSize (int size)
	{
		int length = size * size;

		while (cellInstanceList.Count > length) {
			var cell = cellInstanceList [cellInstanceList.Count - 1];
			cell.OnMouseDown -= this.OnMouseDown;
			cellInstanceList.RemoveAt (cellInstanceList.Count - 1);
			Destroy (cell);
		}

		while (cellInstanceList.Count < length) {
			var cellObject = Instantiate (CellPrefab, CellContainer) as GameObject;
			var cell = cellObject.GetComponent<Cell> ();
			cell.OnMouseDown += OnMouseDown;
			cellInstanceList.Add (cell);
		}

		for (int i = 0; i < cellInstanceList.Count; i++) {
			Cell cell = cellInstanceList [i];
			int column = i % size;
			int row = i / size;
			cell.SetPosition (column, row, size);
		}

	}

	public void SetParameters (int wins, int loses, int draws, GameDifficulty difficulty)
	{
		WinsText.text = "Wins: " + wins;
		LosesText.text = "Loses: " + loses;
		DrawsText.text = "Draws: " + draws;
		DifficultyText.text = difficulty.ToString ();
	}

	public void SetGameField (GameField gameField, int firstMovePlayerId)
	{
		for (int i = 0; i < cellInstanceList.Count; i++) {
			int playerId = gameField.GetPlayerIdAt (cellInstanceList [i].Column, cellInstanceList [i].Row);
			cellInstanceList [i].SetState (playerId, firstMovePlayerId);
		}
	}

	public void SetHighlightedCells (List<Vector2> positions)
	{
		for (int i = 0; i < positions.Count; i++) {
			var position = positions [i];
			var cell = GetCellAt ((int)position.x, (int)position.y);
			cell.SetHighlightedState (true);
		}
	}

	private Cell GetCellAt (int column, int row)
	{
		for (int i = 0; i < cellInstanceList.Count; i++) {
			var cell = cellInstanceList [i];
			if (cell.Column == column && cell.Row == row) {
				return cell;
			}
		}

		return null;
	}

	private void OnMouseDown (int column, int row)
	{
		if (OnCellSelected != null) {
			OnCellSelected (column, row);
		}
	}

}
