using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTank : Enemy
{
    private void Start()
    {
        _speed += _speed;
        _scoreForKilled = 100;
    }
}
