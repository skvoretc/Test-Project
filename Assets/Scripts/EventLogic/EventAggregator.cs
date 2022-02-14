using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAggregator : MonoBehaviour
{
    public delegate void CircleClickEvent(Circle circle);
    public delegate void SquareClickEvent(Square square);
    
    public MainController mainController;

    private void Awake()
    {
        Circle.onCircleClick += mainController.CheckCircleClick;
        Square.onSquareClick += mainController.CheckSquareClick;
    }

}
