using UnityEngine;
using System.Collections;
using System;

public class Storage
{

	private static Storage instance = new Storage ();

	public static Storage Instance{ get { return instance; } }

	public GameModel GameModel{get;private set;}



	public Storage ()
	{
		if (instance != null) {
			throw new Exception ("Instance of Storage already exista");
		}

		this.GameModel = new GameModel ();
	}

}
