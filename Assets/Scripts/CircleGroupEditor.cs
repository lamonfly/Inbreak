using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGroupEditor : CircleGroup
{
    private void OnValidate()
    {
        SetSize(radius);
    }
}
