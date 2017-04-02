using UnityEngine;
using System.Collections;

public class Player
{
	public int Id;

	public GameModel GameModel{get;set;}

	public bool IsActive{ get; private set; }

	public bool IsMoveComplete{ get; protected set; }

	public virtual void OnEnter ()
	{
		IsActive = true;
	}

	public virtual void OnExit ()
	{
		IsActive = false;
		IsMoveComplete = false;
	}

	public virtual void Update ()
	{
	}
}
