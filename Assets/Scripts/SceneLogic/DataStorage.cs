using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataStorage
{
    public static int currentScene = 0;
    public static int currentLevel = 0;
    public static int movesCount;
    public static bool enableExtension = false;
    public static bool winOrLose = false;

    public static float[][] levelSquareStorage ={
    new float[] { 3, 4 },
    new float[] { 1, 2, 1},
    new float[] { 1, 2, 3, 1.5f }
};
    public static float[][] levelCircleStorage ={
    new float[] { 1, 2, 3 },
    new float[] { 1, 2},
    new float[] { 1, 2, 3}
};
    public static float[] leveltriangleStorage ={2,2,1};
}
