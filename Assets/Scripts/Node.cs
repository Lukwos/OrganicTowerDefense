using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class Node : MonoBehaviour
{
    [Header("Node")]
    public StepColor _stepColor;
    public List<Node> _parents;
    public int _maxParent;
    public int _buyPrice;
    public int _sellPrice;
    GameSystem _gameSystem;

    [Header("Visual")]
    public Renderer _coloredRenderer;
    public GameObject _linkPrefab;
    public Vector3 _linkPosition;
    List<GameObject> _links = new List<GameObject>();
    Material _material;

    protected virtual void Update()
    {
        _gameSystem = FindObjectOfType<GameSystem>();
        _material = _coloredRenderer.material;
        UpdateFromParents();
        UpdateVisual();
    }

    protected virtual void UpdateFromParents()
    {
        _parents.RemoveAll(t => t == null);
        _stepColor = StepColor.Black;
        foreach (Node parent in _parents)
        {
            _stepColor |= parent._stepColor;
        }
    }

    void UpdateVisual()
    {
        _material.color = Utils.GetColorFromStepColor(_stepColor);

        while (_links.Count != _parents.Count)
        {
            if (_links.Count > _parents.Count)
            {
                Destroy(_links[0].gameObject);
                _links.RemoveAll(l => l == null);
            }
            if (_links.Count < _parents.Count)
            {
                GameObject link = Instantiate(_linkPrefab, transform.position, transform.rotation, transform);
                _links.Add(link);
            }
        }

        for (int i = 0; i < _parents.Count; i++)
        {
            var renderer = _links[i].GetComponent<LineRenderer>();
            renderer.positionCount = 2;
            renderer.SetPosition(0, transform.position + _linkPosition);
            renderer.SetPosition(1, _parents[i].transform.position + _parents[i]._linkPosition);
            renderer.material.color = Utils.GetColorFromStepColor(_parents[i]._stepColor);
        }
    }

    public void SetParent(Node parent)
    {
        if (this == parent)
        {
            Debug.Log("Is the same");
        }
        else if (_parents.Contains(parent))
        {
            Debug.Log("Is already a parent");
        }
        else if (_parents.Count >= _maxParent)
        {
            Debug.Log("Too many parents");
        }
        else
        {
            _parents.Add(parent);
        }
    }

    public void Sell()
    {
        _gameSystem.AddMoney(_sellPrice);
        Destroy(gameObject);
    }
}

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    public void OnSceneGUI()
    {
        var node = target as Node;
        Vector3 newLinkPosition = Handles.PositionHandle(node._linkPosition + node.transform.position, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(node, "Change link position");
            node._linkPosition = newLinkPosition - node.transform.position;
        }
    }
}