using UnityEngine;
using System.Collections;

public class EvaluationPoints
{


	public static EvaluationPoints Easy = new EvaluationPoints () {
		EvaluationPointsVictory = 10,
		EvaluationPointsTrap = 0,
		EvaluationPointsChildrenTrap = 0,
		EvaluationPointsGrandchildrenTrap = 0,
		EvaluationPointsNearWin = 1,
		EvaluationPointsChildNearWin = 0,
		EvaluationPointsDraw = 0,
		EvaluationPointsGrandchildrenOpponentTrap = 0,
		EvaluationPointsChildrenOpponentTrap = 0,
		EvaluationPointsLose = 0
	};

	public static EvaluationPoints Medium = new EvaluationPoints () {
		EvaluationPointsVictory = 10,
		EvaluationPointsTrap = 1,
		EvaluationPointsChildrenTrap = 0,
		EvaluationPointsGrandchildrenTrap = 0,
		EvaluationPointsNearWin = 1,
		EvaluationPointsChildNearWin = 0,
		EvaluationPointsDraw = -1,
		EvaluationPointsGrandchildrenOpponentTrap = 0,
		EvaluationPointsChildrenOpponentTrap = 0,
		EvaluationPointsLose = -10000
	};

	public static EvaluationPoints Invincible = new EvaluationPoints () {
		EvaluationPointsVictory = 10000,
		EvaluationPointsTrap = 1000,
		EvaluationPointsChildrenTrap = 10,
		EvaluationPointsGrandchildrenTrap = 1,
		EvaluationPointsNearWin = 0,
		EvaluationPointsChildNearWin = 0,
		EvaluationPointsDraw = -1,
		EvaluationPointsGrandchildrenOpponentTrap = -1,
		EvaluationPointsChildrenOpponentTrap = -100,
		EvaluationPointsLose = -10000
	};

	public int EvaluationPointsVictory = 10000;
	public int EvaluationPointsTrap = 1000;
	public int EvaluationPointsChildrenTrap = 10;
	public int EvaluationPointsGrandchildrenTrap = 1;
	public int EvaluationPointsNearWin = 0;
	public int EvaluationPointsChildNearWin = 0;
	public int EvaluationPointsDraw = -1;
	public int EvaluationPointsGrandchildrenOpponentTrap = -1;
	public int EvaluationPointsChildrenOpponentTrap = -100;
	public int EvaluationPointsLose = -10000;

}
