using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Generator : Node
{
    protected override void UpdateFromParents()
    {
        // Disabled
    }
}

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : NodeEditor
{

}