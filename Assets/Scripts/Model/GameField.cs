using System.Collections.Generic;
using UnityEngine;

public class GameField
{

	public int Size{ get; private set; }

	public int[] Field{ get; private set; }

	public GameField (int size)
	{
		this.Size = size;

		this.Field = new int[size * size];
	}

	public GameField GetCopy ()
	{
		var result = new GameField (Size);
		for (int i = 0; i < Field.Length; i++) {
			result.Field [i] = Field [i];
		}

		return result;
	}

	public int FromPositionToIndex (int column, int row)
	{
		return row * Size + column;
	}

	public int FromIndexToColumn (int index)
	{
		return index % Size;
	}

	public int FromIndexToRow (int index)
	{
		return index / Size;
	}

	public void SetPlayerIdAt (int column, int row, int playerId)
	{
		int index = FromPositionToIndex (column, row);
		Field [index] = playerId;
	}

	public int GetPlayerIdAt (int column, int row)
	{
		int index = FromPositionToIndex (column, row);
		return Field [index];
	}

	public bool HasEmptySpace ()
	{

		for (int i = 0; i < Field.Length; i++) {
			if (Field [i] == 0) {
				return true;
			}
		}

		return false;

	}

	public List<int> GetEmptySpacePositions ()
	{

		var result = new List<int> ();
		for (int i = 0; i < Field.Length; i++) {
			if (Field [i] == 0) {
				result.Add (i);
			}
		}

		return result;
	}


	public bool IsWonByPlayer (int playerId)
	{
		if (IsTLBRDiagonalWonByPlayer (playerId)) {
			return true;
		}

		if (IsTRBLDiagonalWonByPlayer (playerId)) {
			return true;
		}

		for (int i = 0; i < Size; i++) {
			if (IsRowWonByPlayer (i, playerId)) {
				return true;
			}

			if (IsColumnWonByPlayer (i, playerId)) {
				return true;
			}
		}

		return false;
	}

	public List<Vector2> GetWonPositions (int playerId)
	{
		if (IsTLBRDiagonalWonByPlayer (playerId)) {
			return GetLTBRPositions ();
		}

		if (IsTRBLDiagonalWonByPlayer (playerId)) {
			return GetTRBLPositions ();
		}

		for (int i = 0; i < Size; i++) {
			if (IsRowWonByPlayer (i, playerId)) {
				return GetRowPositions (i);
			}

			if (IsColumnWonByPlayer (i, playerId)) {
				return GetColumnPositions (i);
			}
		}

		return new List<Vector2> ();
	}



	private bool IsColumnWonByPlayer (int column, int playerId)
	{

		for (int row = 0; row < Size; row++) {
			if (GetPlayerIdAt (column, row) != playerId) {
				return false;
			}
		}

		return Size > 0;
	}

	private bool IsRowWonByPlayer (int row, int playerId)
	{

		for (int column = 0; column < Size; column++) {
			if (GetPlayerIdAt (column, row) != playerId) {
				return false;
			}
		}

		return Size > 0;
	}

	private bool IsTLBRDiagonalWonByPlayer (int playerId)
	{

		for (int i = 0; i < Size; i++) {
			if (GetPlayerIdAt (i, i) != playerId) {
				return false;
			}
		}

		return Size > 0;
	}

	private bool IsTRBLDiagonalWonByPlayer (int playerId)
	{

		for (int i = 0; i < Size; i++) {
			if (GetPlayerIdAt (Size - 1 - i, i) != playerId) {
				return false;
			}
		}

		return Size > 0;

	}




	public bool HasNearWin (int playerId, int movesLeft = 1)
	{
		if ((GetPlayerMoveCountAtTLRBRDiagonal (playerId) == (Size - movesLeft)) &&
		    (GetPlayerMoveCountAtTLRBRDiagonal (0) == movesLeft)) {
			return true;
		}

		if ((GetPlayerMoveCountAtTRBLDiagonal (playerId) == (Size - movesLeft)) &&
		    (GetPlayerMoveCountAtTRBLDiagonal (0) == movesLeft)) {
			return true;
		}

		for (int i = 0; i < Size; i++) {
			if ((GetPlayerMoveCountAtColumn (i, playerId) == (Size - movesLeft)) &&
			    (GetPlayerMoveCountAtColumn (i, 0) == movesLeft)) {
				return true;
			}

			if ((GetPlayerMoveCountAtRow (i, playerId) == (Size - movesLeft)) &&
			    (GetPlayerMoveCountAtRow (i, 0) == movesLeft)) {
				return true;
			}
		}

		return false;
	}

	private int GetPlayerMoveCountAtRow (int row, int playerId)
	{
		int result = 0;
		for (int column = 0; column < Size; column++) {
			if (GetPlayerIdAt (column, row) == playerId) {
				result++;
			}
		}

		return result;
	}


	private int GetPlayerMoveCountAtColumn (int column, int playerId)
	{
		int result = 0;
		for (int row = 0; row < Size; row++) {
			if (GetPlayerIdAt (column, row) == playerId) {
				result++;
			}
		}

		return result;
	}

	private int GetPlayerMoveCountAtTLRBRDiagonal (int playerId)
	{
		int result = 0;
		for (int i = 0; i < Size; i++) {
			if (GetPlayerIdAt (i, i) == playerId) {
				result++;
			}
		}

		return result;
	}

	private int GetPlayerMoveCountAtTRBLDiagonal (int playerId)
	{
		int result = 0;
		for (int i = 0; i < Size; i++) {
			if (GetPlayerIdAt (Size - 1 - i, i) == playerId) {
				result++;
			}
		}

		return result;
	}

	private List<Vector2> GetColumnPositions (int column)
	{
		var positions = new List<Vector2> ();
		for (int row = 0; row < Size; row++) {
			positions.Add (new Vector2 (column, row));
		}
		return positions;
	}

	private List<Vector2> GetRowPositions (int row)
	{
		var positions = new List<Vector2> ();
		for (int column = 0; column < Size; column++) {
			positions.Add (new Vector2 (column, row));
		}
		return positions;
	}

	private List<Vector2> GetLTBRPositions ()
	{
		var positions = new List<Vector2> ();

		for (int i = 0; i < Size; i++) {
			positions.Add (new Vector2 (i, i));
		}

		return positions;
	}

	private List<Vector2> GetTRBLPositions ()
	{
		var positions = new List<Vector2> ();

		for (int i = 0; i < Size; i++) {
			positions.Add (new Vector2 (Size - 1 - i, i));
		}

		return positions;
	}

}
