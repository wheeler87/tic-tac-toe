using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameState
{
	public GameState Parent;

	public GameField gameField;

	public int Column;
	public int Row;
	public int PlayerId;

	public bool IsFinal;
	public int WinnerPlayerId;

	public bool IsInitialized;
	private bool initializationJustStarted;



	public List<GameState> ChildStateList = new List<GameState> ();

	public GameState (GameField gameField, int column, int row, int currentPlayerId)
	{
		this.gameField = gameField.GetCopy ();
		this.gameField.SetPlayerIdAt (column, row, currentPlayerId);

		this.Column = column;
		this.Row = row;
		this.PlayerId = currentPlayerId;

		IsInitialized = false;
		initializationJustStarted = true;

	}

	public void Update ()
	{
		if (IsInitialized) {
			return;
		}

		if (initializationJustStarted) {
			initializationJustStarted = false;

			bool player1Won = gameField.IsWonByPlayer (1);
			bool player2Won = gameField.IsWonByPlayer (2);
			if (player1Won || player2Won || !gameField.HasEmptySpace ()) {
				IsInitialized = true;
				IsFinal = true;

				if (player1Won) {
					WinnerPlayerId = 1;
				} else if (player2Won) {
					WinnerPlayerId = 2;
				} else {
					WinnerPlayerId = 0;
				}

				return;
			}

			var freeSpacePositions = gameField.GetEmptySpacePositions ();
			int currentPlayerId = PlayerId == 1 ? 2 : 1;

			for (int i = 0; i < freeSpacePositions.Count; i++) {
				int index = freeSpacePositions [i];
				int column = gameField.FromIndexToColumn (index);
				int row = gameField.FromIndexToRow (index);

				var gameState = new GameState (gameField, column, row, currentPlayerId);
				gameState.Parent = this;
				ChildStateList.Add (gameState);
			}

			return;
		}

		bool hasActiveChild = false;
		for (int i = 0; i < ChildStateList.Count; i++) {
			if (!ChildStateList [i].IsInitialized) {
				ChildStateList [i].Update ();
				hasActiveChild = true;
			}
		}

		IsInitialized = !hasActiveChild;
	}



	public List<GameState> GetSubChildren (int depth)
	{
		return ExtractSubChildren (this, 1, depth);		
	}

	private List<GameState> ExtractSubChildren (GameState gameState, int currentDepth, int targetDepth)
	{
		var result = new List<GameState> ();
		if (currentDepth == targetDepth) {
			result.AddRange (gameState.ChildStateList);
		} else if (currentDepth < targetDepth) {
			for (int i = 0; i < gameState.ChildStateList.Count; i++) {
				result.AddRange (ExtractSubChildren (gameState.ChildStateList [i], currentDepth + 1, targetDepth));
			}
		}

		return result;
	}

	public static GameState FindGameState (List<GameState> gameStateList, GameField gameField)
	{
		for (int i = 0; i < gameStateList.Count; i++) {
			if (Enumerable.SequenceEqual (gameStateList [i].gameField.Field, gameField.Field)) {
				return gameStateList [i];
			}
		}

		return null;
	}

}
