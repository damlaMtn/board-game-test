using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;

    public GameObject tilePrefab;
    public GameObject tilePrefabParent;
    public GameObject[] players;

    public Material finishTileMaterial;

    [HideInInspector]
    public int diceValue;
    public float tileOffset = 1.2f;

    private int _width = 9;
    private int _height = 9;
    private List<GameObject> _tiles = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Vector3 initialPos = tilePrefabParent.transform.position;

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, tilePrefabParent.transform);

                tile.transform.position = new Vector3(initialPos.x + tileOffset * j, 0, initialPos.z - tileOffset * i);

                _tiles.Add(tile);
            }
        }

        players[0].transform.position = _tiles[0].transform.position - new Vector3(tileOffset, 0, 0);
        //_tiles[0].GetComponent<MeshRenderer>().material = players[0].GetComponent<MeshRenderer>().material;

        players[1].transform.position = _tiles[_tiles.Count - 1].transform.position + new Vector3(tileOffset, 0, 0); ;
        //_tiles[_tiles.Count - 1].GetComponent<MeshRenderer>().material = players[1].GetComponent<MeshRenderer>().material;

        _tiles[_tiles.Count / 2].GetComponent<MeshRenderer>().material = finishTileMaterial;
    }

    public void MovePlayer(int player)
    {
        StartCoroutine("Move", player);
    }

    IEnumerator Move(int currentPlayer)
    {
        for (int i = 0; i < diceValue; i++)
        {
            if (currentPlayer == 1)
            {
                players[0].transform.position = _tiles[diceValue].transform.position;
            }

            else
            {
                players[1].transform.position = _tiles[diceValue].transform.position;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
