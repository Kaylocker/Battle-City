using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatTank : Enemy
{
    private void Start()
    {
        _hitPoints += _hitPoints;
    }
}
