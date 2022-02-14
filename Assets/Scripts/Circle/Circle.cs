using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{

    public static event EventAggregator.CircleClickEvent onCircleClick;

    private void OnMouseDown()
    {
            onCircleClick?.Invoke(this);
    }


}
