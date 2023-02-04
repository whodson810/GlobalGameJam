using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ScoreEvent
/// Created by Hon
/// </summary>
public class ScoreEvent
{
    public int playerIndex;
    public int score;
    public bool wasSpecialNPCScore;
    public bool IsCopScore;
    public int recentPoints; //recent points just added

    public ScoreEvent(int playerIndex, int score, bool wasSpecialSpawn, bool isCopScore, int recentPoints)
    {
        this.playerIndex = playerIndex;
        this.score = score;
        this.wasSpecialNPCScore = wasSpecialSpawn;
        this.IsCopScore = isCopScore;
        this.recentPoints = recentPoints;
    }

    public override string ToString()
    {
        return $"Score: \n {score}";
    }

}
