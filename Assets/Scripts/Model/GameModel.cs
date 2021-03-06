﻿using UnityEngine;
using System.Collections;
using System;

public class GameModel
{
	public Action OnGameStarted;
	public Action OnMoveMade;
	public Action OnGameComplete;

	public int TotalWins = 0;
	public int TotalLoses = 0;
	public int TotalDraws = 0;

	private GameDifficulty difficulty = GameDifficulty.Easy;

	public GameDifficulty Difficulty { get { return difficulty; } set { difficulty = value; } }


	public int FirstPlayerId = 0;
	public int CurrentPlayerId = 0;
	public bool GameIsComplete;
	public int LastWinnerPlayerId;

	private State state;


	public GameField GameField;
	public GameState GameState;
	public GameStateCollection GameStateCollection;

	public HumanPlayer HumanPlayer;
	public CPUPlayer CPUPlayer;
	public Player CurrentPlayer;

	public void StartGame ()
	{
		if ((TotalWins + TotalDraws + TotalLoses) == 0) {
			FirstPlayerId = 1;
		} else if (LastWinnerPlayerId != 0) {
			FirstPlayerId = LastWinnerPlayerId;
		} else {
			FirstPlayerId = FirstPlayerId == 1 ? 2 : 1;
		}

		CurrentPlayerId = FirstPlayerId;
		GameIsComplete = false;
		LastWinnerPlayerId = 1;

		GameField = new GameField (3);
		GameState = null;

		GameStateCollection = new GameStateCollection ();
		GameStateCollection.Init (GameField, CurrentPlayerId);

		switch (difficulty) {
		case GameDifficulty.Easy:
			GameState.evaluationPoints = EvaluationPoints.Easy;
			break;
		case GameDifficulty.Medium:
			GameState.evaluationPoints = EvaluationPoints.Medium;
			break;
		case GameDifficulty.Invincible:
			GameState.evaluationPoints = EvaluationPoints.Invincible;
			break;
		}

		HumanPlayer = new HumanPlayer ();
		HumanPlayer.Id = 1;
		HumanPlayer.GameModel = this;

		CPUPlayer = new CPUPlayer ();
		CPUPlayer.Id = 2;
		CPUPlayer.GameModel = this;


		CurrentPlayer = FirstPlayerId == 1 ? HumanPlayer as Player : CPUPlayer as Player;
		CurrentPlayer.OnEnter ();

		state = State.PlayersMove;


		if (OnGameStarted != null) {
			OnGameStarted ();
		}

	}

	public void Update ()
	{
		switch (state) {
		case State.PlayersMove:
			UpdatePlayerMoveState ();
			break;
		case State.OutcomeEvaluation:
			UpdateOutcomeEvaluationState ();
			break;
		case State.GameComplete:
			UpdateGameCompleteState ();
			break;
		}
	}

	private void UpdatePlayerMoveState ()
	{
		GameStateCollection.Update ();
		CurrentPlayer.Update ();
		if (CurrentPlayer.IsMoveComplete) {
			CurrentPlayer.OnExit ();
			GameState = GameState == null ? GameState.FindGameState (GameStateCollection.GameStateList, GameField) : GameState.FindGameState (GameState.ChildStateList, GameField);
			if (GameState == null) {
				throw new Exception ("Can not find corresponding state");
			}

			if (OnMoveMade != null) {
				OnMoveMade ();
			}

			state = State.OutcomeEvaluation;
		}
	}

	private void UpdateOutcomeEvaluationState ()
	{
		GameStateCollection.Update ();
		if (GameState.IsFinal) {
			if (GameState.WinnerPlayerId == 1) {
				TotalWins++;
			} else if (GameState.WinnerPlayerId == 2) {
				TotalLoses++;
			} else {
				TotalDraws++;
			}

			GameIsComplete = true;
			LastWinnerPlayerId = GameState.WinnerPlayerId;
			state = State.GameComplete;

			if (OnGameComplete != null) {
				OnGameComplete ();
			}
		} else {
			CurrentPlayerId = CurrentPlayerId == 1 ? 2 : 1;
			CurrentPlayer = CurrentPlayerId == 1 ? HumanPlayer as Player : CPUPlayer as Player;
			CurrentPlayer.OnEnter ();

			state = State.PlayersMove;
		}
	}

	private void UpdateGameCompleteState ()
	{
		
	}



}
