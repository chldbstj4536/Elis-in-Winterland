using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StageResult
{
    public enum ResultStar { ZERO, ONE, TWO, THREE }

    public bool isClear;
    public int score;
    public int starCount;


    public StageResult(bool _isClear, int _Score = 0, ResultStar _starCount = ResultStar.ZERO)
    {
        isClear = _isClear;
        score = _Score;
        starCount = (int)_starCount;
    }

}
