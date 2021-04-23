using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum StepColor
{
    Black = 0b000,
    Red = 0b001,
    Green = 0b010,
    Blue = 0b100,
    Yellow = 0b011,
    Cyan = 0b110,
    Magenta = 0b101,
    White = 0b111,
}

class Utils
{
    public static Color GetColorFromStepColor(StepColor stepColor)
    {
        switch (stepColor)
        {
            case StepColor.Red:
                return Color.red;
            case StepColor.Green:
                return Color.green;
            case StepColor.Blue:
                return Color.blue;
            case StepColor.Yellow:
                return Color.yellow;
            case StepColor.Cyan:
                return Color.cyan;
            case StepColor.Magenta:
                return Color.magenta;
            case StepColor.White:
                return Color.white;
            default:
                return Color.black;
        }
    }

    public static StepColor RandomStepColor(params StepColor[] wantedStepColors)
    {
        return wantedStepColors[UnityEngine.Random.Range(0, wantedStepColors.Length)];
    }
}
