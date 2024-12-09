using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooled : CustomerState
{
    public Pooled(Customer owner)
    {
        this.owner = owner;
    }
}
