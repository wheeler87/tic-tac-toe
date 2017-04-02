using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CPUPlayer : Player
{
	private float pauseEndTime;


	public override void OnEnter ()
	{
		base.OnEnter ();

		var initialCase = GameModel.GameState == null;

		pauseEndTime = initialCase ? Time.time + 2 : Time.time + 1;
	}


	public override void Update ()
	{
		if (pauseEndTime > Time.time) {
			return;
		}

		IsMoveComplete = true;	
		switch (GameModel.Difficulty) {

		case GameDifficulty.Easy:
			MakeMoveEasy ();
			break;

		case GameDifficulty.Medium:
			MakeMoveMedium ();
			break;

		case GameDifficulty.Invincible:
			MakeMoveInvincible ();
			break;
		}

	}

	private void MakeMoveEasy ()
	{
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

		MakeRandomMove ();
	}

	private void MakeMoveMedium ()
	{
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
		gameState = Random.Range (1, 100) < 70 ? GetMostOptimalState (3) : null;
		if (gameState != null) {
			MoveToGameState (gameState);
			return;
		}

		MakeRandomMove ();


	}

	private void MakeMoveInvincible ()
	{
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
		gameState = GetMostOptimalState (int.MaxValue);
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

	public GameState GetMostOptimalState (int maxDepth)
	{
		var stateList = GameModel.GameState != null ? GameModel.GameState.ChildStateList : GameModel.GameStateCollection.GameStateList;
		GameState result = null;
		float resultEvaluation = int.MinValue;

		for (int i = 0; i < stateList.Count; i++) {
			var gameState = stateList [i];
			var points = EvaluateState (gameState, 1, maxDepth);
			if (points > resultEvaluation) {
				result = gameState;
				resultEvaluation = points;
			}

		}


		if (result == null && stateList.Count > 0) {
			result = stateList [0];
		}

		return result;
	}

	private float EvaluateState (GameState state, int depth, int maxDepth)
	{
		if (state.IsFinal) {
			if (state.WinnerPlayerId == Id) {
				return 1.0f / depth;
			}

			if (state.WinnerPlayerId != 0) {
				return-2.0f / depth;
			}

			return 0;
		}

		if (depth > maxDepth) {
			return 0;
		}

		float result = 0;
		for (int i = 0; i < state.ChildStateList.Count; i++) {
			result += EvaluateState (state.ChildStateList [i], depth + 1, maxDepth);
		}

		return result;
	}

}
