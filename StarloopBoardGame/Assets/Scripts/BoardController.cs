using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private int player1CurrentTile = 0;
    private int player2CurrentTile = 0;

    [HideInInspector]
    public bool isMoving;

    private void Awake()
    {
        instance = this;

        isMoving = false;
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

        player2CurrentTile = _tiles.Count - 1;
    }

    public void MovePlayer(int player)
    {
        StartCoroutine("Move", player);
    }

    IEnumerator Move(int currentPlayer)
    {
        isMoving = true;
        for (int i = 0; i < diceValue; i++)
        {
            if (currentPlayer == 1)
            {
                if (player1CurrentTile != _tiles.Count / 2)
                {
                    players[0].transform.position = _tiles[player1CurrentTile].transform.position;
                    _tiles[player1CurrentTile].GetComponent<MeshRenderer>().material = players[0].GetComponent<MeshRenderer>().material;
                    player1CurrentTile++;
                    GameObject.Find("Player1Point").transform.GetComponent<Text>().text = "Player: " + player1CurrentTile.ToString();
                }
                else
                {

                }
            }

            else
            {
                if (player2CurrentTile != _tiles.Count / 2)
                {
                    players[1].transform.position = _tiles[player2CurrentTile].transform.position;
                    _tiles[player2CurrentTile].GetComponent<MeshRenderer>().material = players[1].GetComponent<MeshRenderer>().material;
                    player2CurrentTile--;
                    GameObject.Find("Player2Point").transform.GetComponent<Text>().text = "AI: " + (_tiles.Count - 1 - player2CurrentTile).ToString();
                }
                else
                {

                }
            }

            yield return new WaitForSeconds(1f);
        }

        isMoving = false;
    }
}
