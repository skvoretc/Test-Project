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
    [SerializeField] GameObject _trianglePrefab;
    [SerializeField] GameObject _EnergyPanel;
    [SerializeField] TextMeshProUGUI _moveLabel;
    [SerializeField] TextMeshProUGUI _energyLabel;
    [SerializeField] GameObject _mainTransform;


    private float[] _squareArray;
    private float[] _circleArray;
    private float _triangleCount;
    private int _moves = 0;
    private int _energy = 3;
    private bool _loseCheck = false;
    private bool _winCheck = false;


    private Circle _currentCircle;
    private Triangle _currentTriangle;
    void Awake()
    {
        _squareArray = DataStorage.levelSquareStorage[DataStorage.currentLevel];
        _circleArray = DataStorage.levelCircleStorage[DataStorage.currentLevel];
        if (DataStorage.enableExtension)
        {
            _triangleCount = DataStorage.leveltriangleStorage[DataStorage.currentLevel];
            _EnergyPanel.SetActive(true);
            _energyLabel.text = _energy.ToString();
        }
        InitLevel();

        Debug.Log("Init");
        _mainTransform.name = "game" + DataStorage.currentLevel;
    }
    private void Update()
    {

        if (_winCheck)
        {
            DataStorage.currentScene = 2;
            DataStorage.winOrLose = true;
            DataStorage.movesCount = _moves;
            SceneManager.LoadScene(DataStorage.currentScene);
        }
        if (_loseCheck)
        {
            DataStorage.currentScene = 2;
            DataStorage.winOrLose = false;
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
            if (searchIndex(square.transform) != -1) {
                if (_currentCircle != null && searchIndex(square.transform) != -1)
                {
                    if (Math.Round(square.transform.localScale.x * Mathf.Sqrt(2), 3) <= Math.Round(_currentCircle.transform.localScale.x, 3))
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
                else if (_currentTriangle != null)
                {
                    if(square.transform.localScale.x != 1)
                    {
                        if (_energy > 0)
                        {
                            square.transform.localScale = new Vector3(square.transform.localScale.x - 1, square.transform.localScale.y - 1, 1);
                            Destroy(_mainTransform.transform.Find(_currentTriangle.name).gameObject);
                            _energy--;
                            _moves++;
                        }
                    }
                }
            }
            _moveLabel.text = _moves.ToString();
            _energyLabel.text = _energy.ToString();
            for (int i = 0; i < _squareArray.Length; i++)
            {

                if (_mainTransform.transform.Find("Square"+i) != null)
                {
                    _winCheck = false;
                    break;
                }
                else
                    _winCheck = true;

            }
            if (!_winCheck)
            {
                for (int i = 0; i < _circleArray.Length; i++)
                {

                    if (_mainTransform.transform.Find("Circle" + i) != null)
                    {
                        _winCheck = false;
                        break;
                    }
                    else
                        _winCheck = true;

                }
            }


            if (!checkAvailability("Triangle") && checkAvailability("Circle"))
            {
                int lose = 0;
                float squareScale = 0;
                for (int i = 0; i < _squareArray.Length; i++)
                {
                    if(_mainTransform.transform.Find("Square" + i) != null)
                        squareScale = Math.Min(_mainTransform.transform.Find("Square" + i).transform.localScale.x,squareScale);
                }
                Debug.Log(squareScale + "squareScale");

                for (int i = 0; i < _mainTransform.transform.childCount; i++)
                {
                    if(_mainTransform.transform.GetChild(i).tag == "Circle")
                    {
                        if (Math.Round(_mainTransform.transform.GetChild(i).localScale.x, 2) >= Math.Round(squareScale * Mathf.Sqrt(2), 2) && !_winCheck)
                        {
                            Debug.Log(_mainTransform.transform.GetChild(i).name + "Not lose");
                            lose++;
                        }
                    }
                }
                if (lose == 0 && squareScale != 0)
                    _loseCheck = true;
            }
            
        }
    }
    public void CheckTriangleClick(Triangle triangle)
    {
        if (searchIndex(triangle.transform) != -1)
            _currentTriangle = triangle;
    }

    private void InitLevel()
    {
        for (int i = 0; i < _squareArray.Length; i++)
        {
            GameObject square = Instantiate(_squarePrefab);
            square.transform.localScale = new Vector3(_squareArray[i], _squareArray[i], 1);
            square.name = "Square" + i;
            createElement(square, _squareArray[i], _squareArray[i]);
            square.tag = "Square";
            square.AddComponent<Square>();
        }

        
        for (int i = 0; i < _circleArray.Length; i++)
        {
            GameObject circle = Instantiate(_circlePrefab);
            circle.name = "Circle" + i;
            createElement(circle, _circleArray[i] * Mathf.Sqrt(2), _circleArray[i] * Mathf.Sqrt(2));
            circle.tag = "Circle";
            circle.AddComponent<Circle>();
        }
        if (DataStorage.enableExtension)
        {
            Debug.Log("triangle");
            for (int i = 0; i < _triangleCount; i++)
            {
                GameObject triangle = Instantiate(_trianglePrefab);
                triangle.name = "Triangle" + i;
                createElement(triangle,0.25f,0.25f);
                triangle.tag = "Triangle";
                triangle.AddComponent<Triangle>();
            }
        }
    }

    private void createElement(GameObject curObject, float scaleX, float scaleY)
    {
        float x, y;

        curObject.transform.localScale = new Vector3(scaleX, scaleY, 1);
        x = UnityEngine.Random.Range(-7f, 7f);
        y = UnityEngine.Random.Range(-3.5f, 3.5f);
        curObject.transform.position = new Vector3(x, y, 0);
        while (!checkPosition(curObject))
        {
            x = UnityEngine.Random.Range(-7f, 7f);
            y = UnityEngine.Random.Range(-3.5f, 3.5f);
            curObject.transform.position = new Vector3(x, y, 0);
        }
        curObject.transform.parent = _mainTransform.transform;
        curObject.AddComponent<BoxCollider2D>();

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

    bool checkAvailability(string type)
    {
        for(int i = 0; i < Mathf.Max(_circleArray.Length, _squareArray.Length); i++)
        {
            if(_mainTransform != null)
            {
                if(_mainTransform.transform.Find(type + i) != null)
                {
                    return true;
                }
            }
        }

        return false;

    }
   public void goHome()
    {
        DataStorage.currentScene = 0;
        SceneManager.LoadScene(DataStorage.currentScene);
    }
}
