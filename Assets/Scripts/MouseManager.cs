using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{
    public GameObject _linkPrefab;
    public float _maxLinkLength;
    GameObject _link;
    Node _startNode;
    Node _endNode;
    Node _selectedTower;
    SpawnGhost _instancedGhost;

    [Header("UI")]
    public GameObject _contextualGeneratorPanel;
    public GameObject _contextualNodePanel;
    public GameObject _contextualTurretPanel;

    private void Update()
    {
        bool pointerOnUI = EventSystem.current.IsPointerOverGameObject();
        if (_instancedGhost)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Floor")) && !pointerOnUI)
            {
                _instancedGhost.gameObject.SetActive(true);
                _instancedGhost.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    _instancedGhost.Spawn();
                }
            }
            else
            {
                _instancedGhost.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                _instancedGhost.Destroy();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!pointerOnUI)
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Tower")))
                    {
                        _startNode = hit.transform.GetComponent<Node>();
                        _link = Instantiate(_linkPrefab, _startNode.transform.position, _startNode.transform.rotation);
                    }
                    else
                    {
                        _selectedTower = null;
                    }
                }
            }
            if (Input.GetMouseButton(0) && _startNode)
            {
                var renderer = _link.GetComponent<LineRenderer>();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Tower", "Floor")) && !pointerOnUI)
                {
                    float distance = Vector3.Distance(_startNode.transform.position + _startNode._linkPosition, hit.point);

                    renderer.positionCount = 2;
                    renderer.SetPosition(0, _startNode.transform.position + _startNode._linkPosition);
                    if (distance > _maxLinkLength)
                    {
                        Vector3 maxPosition = _startNode.transform.position + (hit.point - _startNode.transform.position + _startNode._linkPosition).normalized * _maxLinkLength;
                        renderer.SetPosition(1, maxPosition);
                    }
                    else
                    {
                        _endNode = hit.transform.GetComponent<Node>();
                        if (_endNode)
                        {
                            renderer.SetPosition(1, _endNode.transform.position + _endNode._linkPosition);
                        }
                        else
                        {
                            renderer.SetPosition(1, hit.point);
                        }
                        renderer.material.color = Utils.GetColorFromStepColor(StepColor.Black);
                    }
                }
                else
                {
                    renderer.positionCount = 0;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (_startNode && _endNode)
                {
                    if (_startNode != _endNode)
                    {
                        _endNode.SetParent(_startNode);
                    }
                    else
                    {
                        _selectedTower = _startNode;
                    }
                }

                if (_link)
                {
                    Destroy(_link.gameObject);
                }
                _link = null;
                _startNode = null;
                _endNode = null;
            }
        }
        OpenContextualMenu();

    }

    public void ChooseTower(GameObject ghostPrefab)
    {
        if (_instancedGhost)
        {
            _instancedGhost.Destroy();
        }
        _instancedGhost = Instantiate(ghostPrefab, Vector3.zero, Quaternion.identity).GetComponent<SpawnGhost>();
    }

    public void SellTower()
    {
        if(_selectedTower)
        {
            _selectedTower.Sell();
        }
    }
    public void ChooseColor(int stepColorValue)
    {
        if (_selectedTower && _selectedTower.GetType() == typeof(Generator))
        {
            _selectedTower._stepColor = (StepColor)stepColorValue;
        }
    }

    void OpenContextualMenu()
    {

        Type nodeType = _selectedTower ? _selectedTower.GetType() : null;
        _contextualGeneratorPanel.SetActive(nodeType == typeof(Generator));
        _contextualTurretPanel.SetActive(nodeType == typeof(Turret));
        _contextualNodePanel.SetActive(nodeType == typeof(Node));
    }
}
