using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    [SerializeField] GameObject _circlePrefab;
    [SerializeField] GameObject _squarePrefab;
    [SerializeField] TextMeshProUGUI _moveLabel;
    [SerializeField] GameObject _mainTransform;
    private GameObject _maintwo;
    private float[] _squareArray;
    private float[] _circleArray;
    private int _moves = 0;
    private Circle _currentCircle;
    void Awake()
    {
        _squareArray = DataStorage.levelSquareStorage[DataStorage.currentLevel];
        _circleArray = DataStorage.levelCircleStorage[DataStorage.currentLevel];
        InitLevel();
        Debug.Log("Init");
        _mainTransform.name = "game" + DataStorage.currentLevel;
        _maintwo = _mainTransform;     
    }

    private void Update()
    {

        if (Math.Max(_squareArray.Length,_circleArray.Length) - Math.Min(_squareArray.Length, _circleArray.Length) == _mainTransform.transform.childCount)
        {
            DataStorage.currentScene = 2;
            DataStorage.movesCount = _moves;
            SceneManager.LoadScene(DataStorage.currentScene);
        }
    }

    public void CheckCircleClick(Circle circle){
        if(searchIndex(circle.transform) != -1)
            _currentCircle = circle;
    }

    public void CheckSquareClick(Square square)
    {
        if (_mainTransform != null)
        {
            if (_currentCircle != null && searchIndex(square.transform) != -1)
            {
                if (Math.Round(square.transform.localScale.x * Mathf.Sqrt(2), 3) == Math.Round(_currentCircle.transform.localScale.x, 3))
                {
                    if (searchIndex(_currentCircle.transform) != -1)
                    {
                        _mainTransform.transform.GetChild(searchIndex(_currentCircle.transform)).position = square.transform.position;
                        _mainTransform.transform.GetChild(searchIndex(square.transform)).parent = null;
                        _mainTransform.transform.GetChild(searchIndex(_currentCircle.transform)).parent = null;
                    }
                }
                _currentCircle = null;
                _moves++;

            }

            _moveLabel.text = _moves.ToString();
        }
    }

    private void InitLevel()
    {
        float x, y;
        for (int i = 0; i < _squareArray.Length; i++)
        {
            GameObject square = Instantiate(_squarePrefab);
            square.transform.localScale = new Vector3(_squareArray[i], _squareArray[i], 1);
            square.name = "Square" + i;
            x = UnityEngine.Random.Range(-9f, 9f);
            y = UnityEngine.Random.Range(-3.5f, 3.5f);
            square.transform.position = new Vector3(x, y, 0);

            while (!checkPosition(square))
            {
                x = UnityEngine.Random.Range(-9f, 9f);
                y = UnityEngine.Random.Range(-3.5f, 3.5f);
                square.transform.position = new Vector3(x, y, 0);
            }
            square.transform.parent = _mainTransform.transform;
            square.tag = "Square";
            square.AddComponent<BoxCollider2D>();
            square.AddComponent<Square>();
        }

        
        for (int i = 0; i < _circleArray.Length; i++)
        {
            GameObject circle = Instantiate(_circlePrefab);
            circle.transform.localScale = new Vector3(_circleArray[i] * Mathf.Sqrt(2), _circleArray[i] * Mathf.Sqrt(2), 1);
            circle.name = "Circle" + i;
            x = UnityEngine.Random.Range(-9f, 9f);
            y = UnityEngine.Random.Range(-3.5f, 3.5f);
            circle.transform.position = new Vector3(x, y, 0);
            while (!checkPosition(circle))
            {
                x = UnityEngine.Random.Range(-9f, 9f);
                y = UnityEngine.Random.Range(-3.5f, 3.5f);
                circle.transform.position = new Vector3(x, y, 0);
            }
            circle.transform.parent = _mainTransform.transform;
            circle.tag = "Circle";
            circle.AddComponent<BoxCollider2D>();
            circle.AddComponent<Circle>();
        }
    }
    private bool checkPosition(GameObject gameobject)
    {
        for(int i = 0;i < _mainTransform.transform.childCount; i++)
        {
            Transform currentChild = _mainTransform.transform.GetChild(i).transform;
            float distance = Vector3.Distance(currentChild.transform.position, gameobject.transform.position);

            if (distance < (currentChild.localScale.x + gameobject.transform.localScale.x)/1.75)
            {
                return false;
            }
        }
        return true; 
    }

     int searchIndex(Transform searchObject)
    {
        if (_mainTransform != null)
        {
            for (int i = 0; i < _mainTransform.transform.childCount; i++)
            {
                if (searchObject.name.Equals(_mainTransform.transform.GetChild(i).transform.name))
                {
                    return i;
                }
            }
        }
        return -1;
    }
}
