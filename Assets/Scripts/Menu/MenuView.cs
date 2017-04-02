using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MenuView : MonoBehaviour
{
	public Action<GameDifficulty> OnDifficultyChanged;

	public Dropdown DifficultyDropdown;
	public Button PlayButton;

	public void Init (GameDifficulty gameDifficulty)
	{
		DifficultyDropdown.ClearOptions ();
		DifficultyDropdown.onValueChanged.RemoveListener (OnDifficultyChangedEvent);

		var difficulties = Enum.GetValues (typeof(GameDifficulty));
		var options = new List<Dropdown.OptionData> ();
		var selectedIndex = 0;

		for (int i = 0; i < difficulties.Length; i++) {
			var label = difficulties.GetValue (i).ToString ();
			var option = new Dropdown.OptionData (label);
			options.Add (option);

			if (((GameDifficulty)difficulties.GetValue (i)) == gameDifficulty) {
				selectedIndex = i;
			}
		}


		DifficultyDropdown.AddOptions (options);
		DifficultyDropdown.value = selectedIndex;
		DifficultyDropdown.onValueChanged.AddListener (OnDifficultyChangedEvent);

	}

	private void OnDifficultyChangedEvent (int selectedIndex)
	{
		if (OnDifficultyChanged != null) {
			var option = DifficultyDropdown.options [selectedIndex];
			GameDifficulty difficulty = (GameDifficulty)Enum.Parse (typeof(GameDifficulty), option.text);

			OnDifficultyChanged (difficulty);
		}
	}
	
}
