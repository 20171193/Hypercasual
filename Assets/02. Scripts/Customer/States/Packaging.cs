using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packaging : CustomerState
{
    public Packaging(Customer owner)
    {
        this.owner = owner;
    }
}
