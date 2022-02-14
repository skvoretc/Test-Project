using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{

    public static event EventAggregator.TriangleClickEvent onTriangleClick;

    private void OnMouseDown()
    {
        onTriangleClick?.Invoke(this);
    }

}
