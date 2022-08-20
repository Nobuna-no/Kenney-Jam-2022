using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePool : PoolManager<GamePool>
{
    protected override GamePool GetInstance()
    {
        return this;
    }

    private void Start()
    {
        ResetManager();
    }
}
