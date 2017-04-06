using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CPUPlayer : Player
{
	private float pauseEndTime;


	public override void OnEnter ()
	{
		base.OnEnter ();

		pauseEndTime = Time.time + 1;
	}


	public override void Update ()
	{
		if (pauseEndTime > Time.time) {
			return;
		}

		if (!GameModel.GameStateCollection.IsInitialized) {
			return;
		}

		IsMoveComplete = true;



		var gameState = GetStateThatLeadsToVictory ();
		if (gameState != null) {
			MoveToGameState (gameState);
			return;
		}
		gameState = GetStateThatPreventsLose ();
		if (gameState != null) {
			MoveToGameState (gameState);
			return;
		}
		gameState = GetMostOptimalState ();
		if (gameState != null) {
			MoveToGameState (gameState);
			return;
		}

		MakeRandomMove ();



	}


	private void MakeRandomMove ()
	{
		var emptySpaceList = GameModel.GameField.GetEmptySpacePositions ();
		if (emptySpaceList.Count < 1) {
			return;
		}
		int listIndex = Random.Range (0, emptySpaceList.Count);
		int index = emptySpaceList [listIndex];
		int column = GameModel.GameField.FromIndexToColumn (index);
		int row = GameModel.GameField.FromIndexToRow (index);
		GameModel.GameField.SetPlayerIdAt (column, row, Id);

	}

	private GameState GetStateThatLeadsToVictory ()
	{
		if (GameModel.GameState == null) {
			return null;
		}

		for (int i = 0; i < GameModel.GameState.ChildStateList.Count; i++) {
			var gameState = GameModel.GameState.ChildStateList [i];
			if (gameState.IsFinal && gameState.WinnerPlayerId == Id) {
				return gameState;
			}
		}

		return null;
	}

	private GameState GetStateThatPreventsLose ()
	{
		if (GameModel.GameState == null) {
			return null;
		}

		for (int i = 0; i < GameModel.GameState.ChildStateList.Count; i++) {
			var gameState = GameModel.GameState.ChildStateList [i];
			var loseState = GetLoseChildState (gameState);
			if (loseState != null) {
				return loseState;
			}
		}

		return null;
	}

	private GameState GetLoseChildState (GameState gameState)
	{
		for (int i = 0; i < gameState.ChildStateList.Count; i++) {
			var childGameState = gameState.ChildStateList [i];
			if (childGameState.IsFinal) {
				if (childGameState.WinnerPlayerId != 0 && childGameState.WinnerPlayerId != Id) {
					return childGameState;
				}
			}
		}

		return null;
	}

	private void MoveToGameState (GameState gameState)
	{
		this.GameModel.GameField.SetPlayerIdAt (gameState.Column, gameState.Row, Id);
	}

	private GameState GetMostOptimalState ()
	{
		var gameStateList = GameModel.GameState != null ? GameModel.GameState.ChildStateList : GameModel.GameStateCollection.GameStateList;
		int resultPoints = int.MinValue;
		List<GameState> resultList = new List<GameState> ();

		for (int i = 0; i < gameStateList.Count; i++) {
			var gameState = gameStateList [i];
			int gameStatePoints = gameState.GetEvaluationPoints (Id);

			if (resultList.Count == 0 || gameStatePoints > resultPoints) {
				resultList.Clear ();
				resultList.Add (gameState);
				resultPoints = gameStatePoints;
			} else if (gameStatePoints == resultPoints) {
				resultList.Add (gameState);
			}
		}

		if (resultList.Count == 0) {
			return null;
		}

		int index = Random.Range (0, resultList.Count - 1);
		return resultList [index];

	}

}
