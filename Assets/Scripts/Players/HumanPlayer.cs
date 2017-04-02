using UnityEngine;
using System.Collections;

public class HumanPlayer : Player
{

	public void OnCellSelected (int column, int row)
	{
		if (!IsActive) {
			return;
		}

		if (GameModel.GameField.GetPlayerIdAt (column, row) != 0) {
			return;
		}

		if (GameModel.GameIsComplete) {
			return;
		}

		GameModel.GameField.SetPlayerIdAt (column, row, Id);
		IsMoveComplete = true;
	}
}
