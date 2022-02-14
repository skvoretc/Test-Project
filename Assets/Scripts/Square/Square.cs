using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public static event EventAggregator.SquareClickEvent onSquareClick;

    private void OnMouseDown()
    {
        onSquareClick?.Invoke(this);

    }
}
