using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameState
{
	public static EvaluationPoints evaluationPoints = EvaluationPoints.Easy;

	public GameState Parent;

	public GameField gameField;

	public int Column;
	public int Row;
	public int PlayerId;

	public bool IsFinal;
	public int WinnerPlayerId;

	public bool IsInitialized;
	private bool initializationJustStarted;

	EvaluationPoints lastUsedEvaluationPoints;
	private int evaluationPointsPlayer = 0;
	private int evaluationPointsOpponent = 0;



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

	public int GetEvaluationPoints (int playerId)
	{
		if (lastUsedEvaluationPoints != evaluationPoints) {
			lastUsedEvaluationPoints = evaluationPoints;
			Evaluate ();
		}

		return this.PlayerId == playerId ? evaluationPointsPlayer : evaluationPointsOpponent;
	}

	private void Evaluate ()
	{
		int opponentId = PlayerId == 1 ? 2 : 1;
		evaluationPointsPlayer = EvaluateForPlayer (this.PlayerId, opponentId);

		evaluationPointsOpponent = EvaluateForPlayer (opponentId, this.PlayerId);
	}

	private int EvaluateForPlayer (int playerId, int opponentId)
	{
		int result = 0;
		if (IsFinal) {
			if (WinnerPlayerId == playerId) {
				result = evaluationPoints.EvaluationPointsVictory;
			} else if (WinnerPlayerId == 0) {
				result = evaluationPoints.EvaluationPointsDraw;
			} else {
				result = evaluationPoints.EvaluationPointsLose;
			}

			return result;
		}
		int currentStateNearWinCount = gameField.GetNearWinsCount (playerId);

		if (currentStateNearWinCount > 1) {
			result += evaluationPoints.EvaluationPointsTrap;
		} else if (currentStateNearWinCount > 0) {
			result += evaluationPoints.EvaluationPointsNearWin;
		}




		var childrenList = ExtractSubChildren (this, 0, 1);
		for (int i = 0; i < childrenList.Count; i++) {
			var currentState = childrenList [i];

			int childNearWinLast = currentState.Parent.gameField.GetNearWinsCount (playerId);

			if (childNearWinLast > 0) {
				continue;
			}

			int childNearWinCurrent = currentState.gameField.GetNearWinsCount (playerId);
			if (currentState.Parent.gameField.GetNearWinsCount (opponentId) > 1) {
				result += evaluationPoints.EvaluationPointsChildrenOpponentTrap;
				continue;
			}



			if (childNearWinCurrent > 1) {
				if (currentStateNearWinCount > 0) {
					result += evaluationPoints.EvaluationPointsTrap;
				} else {
					result += evaluationPoints.EvaluationPointsChildrenTrap;
				}
			} else if (childNearWinCurrent > 0) {
				result += evaluationPoints.EvaluationPointsChildNearWin;
			}
		}


		var enemyChildrenList = ExtractSubChildren (this, 0, 2);
		for (int i = 0; i < enemyChildrenList.Count; i++) {
			var currentState = enemyChildrenList [i];
			if (currentState.gameField.GetNearWinsCount (opponentId) > 1) {
				result += evaluationPoints.EvaluationPointsGrandchildrenOpponentTrap;
			}
		}


		var grandchildrenList = ExtractSubChildren (this, 0, 3);
		for (int i = 0; i < grandchildrenList.Count; i++) {
			var currentState = grandchildrenList [i];

			if (currentState.Parent.Parent.gameField.HasNearWin (opponentId)) {
				continue;
			}

			int grandchildNearWinLast = currentState.Parent.gameField.GetNearWinsCount (playerId);
			int grandchildNearWinCurrent = currentState.gameField.GetNearWinsCount (playerId);

			if (grandchildNearWinLast > 0) {
				continue;
			}

			if ((grandchildNearWinCurrent - grandchildNearWinLast) > 1) {
				result += evaluationPoints.EvaluationPointsGrandchildrenTrap;
			}
		}

		return result;
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
