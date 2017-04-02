using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameResultView : MonoBehaviour
{

	public Text ResultText;
	public Button OkButton;


	public void SetWinner (int playerId)
	{
		if (playerId == 1) {
			ResultText.text = "You Won";
		} else if (playerId == 2) {
			ResultText.text = "Enemy Won";
		} else {
			ResultText.text = "Draw";
		}
	}
}
