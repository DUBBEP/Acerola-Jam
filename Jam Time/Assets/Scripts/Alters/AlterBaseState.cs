using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlterBaseState
{
    public abstract void EnterState(AlterStateManager alter);
    public abstract void UpdateState(AlterStateManager alter);
    public abstract void ExitState(AlterStateManager alter);
    public abstract void OnTriggerEnter2D(AlterStateManager alter, Collider2D collision);
    public abstract void OnTriggerExit2D(AlterStateManager alter, Collider2D collision);
}
