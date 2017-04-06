using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateCollection
{
	public List<GameState> GameStateList = new List<GameState> ();
	public List<GameState> AllGameStateList = new List<GameState> ();

	public bool IsInitialized{ get; set; }

	public void Init (GameField gameField, int currentPlayerId)
	{
		var emptyPositionsList = gameField.GetEmptySpacePositions ();
		for (int i = 0; i < emptyPositionsList.Count; i++) {
			int index = emptyPositionsList [i];
			int column = gameField.FromIndexToColumn (index);
			int row = gameField.FromIndexToRow (index);

			var state = new GameState (gameField, column, row, currentPlayerId);
			GameStateList.Add (state);
		}

		IsInitialized = false;
	}

	public void Update ()
	{
		if (IsInitialized) {
			return;
		}

		bool hasActiveState = false;
		for (int i = 0; i < GameStateList.Count; i++) {
			if (!GameStateList [i].IsInitialized) {
				GameStateList [i].Update ();
				hasActiveState = true;
			}
		}

		if (!hasActiveState) {
			
			for (int i = 0; i < GameStateList.Count; i++) {
				AddGameState (GameStateList [i]);
			}

			IsInitialized = true;
		}
	}

	private void AddGameState (GameState gameState)
	{
		AllGameStateList.Add (gameState);
		for (int i = 0; i < gameState.ChildStateList.Count; i++) {
			AddGameState (gameState.ChildStateList [i]);
		}

	}
	
	
}
