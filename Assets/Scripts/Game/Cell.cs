using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Cell : MonoBehaviour, IPointerDownHandler
{
	public Action<int,int> OnMouseDown;

	public Text text;
	public Color firstPlayerColor;
	public Color secondPlayerColor;

	public Image Background;

	[HideInInspector]
	public int Column;
	[HideInInspector]
	public int Row;

	public void SetPosition (int column, int row, int size)
	{
		this.Column = column;
		this.Row = row;

		var cellSize = 1.0f / size;

		var rectTransform = transform as RectTransform;
		rectTransform.anchorMin = new Vector2 (column * cellSize, (size - row - 1) * cellSize);
		rectTransform.anchorMax = new Vector2 ((column + 1) * cellSize, (size - row) * cellSize);
	}

	public void SetState (int playerId, int firstMovePlayerId)
	{
		if (playerId == 0) {
			text.text = string.Empty;
		} else if (playerId == firstMovePlayerId) {
			text.text = "X";
			text.color = firstPlayerColor;
		} else {
			text.text = "0";
			text.color = secondPlayerColor;
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (OnMouseDown != null) {
			OnMouseDown (Column, Row);
		}
	}

	public void SetHighlightedState (bool highlighted)
	{
		Background.color=highlighted?Color.green:Color.white;
	}

}
