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
    public GameObject finishPanel;

    [HideInInspector]
    public int diceValue;

    public float tileOffset = 1.2f;

    [HideInInspector]
    public int turnCount = 0;

    private int _bonusTurnCount = 5;

    private int _width = 9;
    private int _height = 9;
    private List<GameObject> _tiles = new List<GameObject>();

    [HideInInspector]
    public bool isMoving = false;

    [HideInInspector]
    public bool isBonus = false;

    Player player1;
    Player player2;


    private void Awake()
    {
        instance = this;

        isMoving = false;

        finishPanel.SetActive(false);
    }

    private void Start()
    {
        CreateBoard();

        CreatePlayers();

        GameObject.Find("Turn").transform.GetComponent<Text>().text = player1.playerName + "'s turn";
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
                if (player1.currentTile != _tiles.Count / 2)
                {
                    player1.playerPrefab.transform.position = _tiles[player1.currentTile].transform.position;

                    _tiles[player1.currentTile].GetComponent<MeshRenderer>().material = player1.material;

                    player1.currentTile++;

                    GameObject.Find("Player1Point").transform.GetComponent<Text>().text = player1.playerName + ": " + player1.currentTile.ToString();
                }
                else
                {
                    finishPanel.SetActive(true);
                    finishPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = player1.playerName + " wins";
                    yield break;
                }
            }

            else
            {
                if (player2.currentTile != _tiles.Count / 2)
                {
                    player2.playerPrefab.transform.position = _tiles[player2.currentTile].transform.position;

                    _tiles[player2.currentTile].GetComponent<MeshRenderer>().material = player2.material;

                    player2.currentTile--;

                    GameObject.Find("Player2Point").transform.GetComponent<Text>().text = player2.playerName + ": " + (_tiles.Count - 1 - player2.currentTile).ToString();
                }
                else
                {
                    finishPanel.SetActive(true);
                    finishPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = player2.playerName + " wins";
                    yield break;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        isMoving = false;

        turnCount++;

        if (turnCount == _bonusTurnCount)
        {
            ActivateBonus();
        }

        else if (turnCount == _bonusTurnCount + 2)
        {
            ActivateBonus();

            GameObject.Find("BonusDice").GetComponent<Button>().onClick.Invoke();
            turnCount = -1;
        }

        else
        {
            GameObject.Find("Dice").GetComponent<Button>().interactable = true;
            GameObject.Find("BonusDice").GetComponent<Button>().interactable = false;

            if (currentPlayer == 1)
            {
                GameObject.Find("Turn").transform.GetComponent<Text>().text = player2.playerName + "'s turn";
                GameObject.Find("Dice").GetComponent<Button>().onClick.Invoke();
            }
            else
            {
                GameObject.Find("Turn").transform.GetComponent<Text>().text = player1.playerName + "'s turn";
            }
        }
    }

    private void CreateBoard()
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

        _tiles[_tiles.Count / 2].GetComponent<MeshRenderer>().material = finishTileMaterial;
    }

    private void CreatePlayers()
    {
        player1 = new Player(players[0], 0, 0, players[0].GetComponent<MeshRenderer>().material, "PLAYER");

        player1.playerPrefab.transform.position = _tiles[0].transform.position - new Vector3(tileOffset, 0, 0);


        player2 = new Player(players[1], 1, 0, players[1].GetComponent<MeshRenderer>().material, "AI");

        player2.playerPrefab.transform.position = _tiles[_tiles.Count - 1].transform.position + new Vector3(tileOffset, 0, 0);

        player2.currentTile = _tiles.Count - 1;
    }

    private void ActivateBonus()
    {
        isBonus = true;
        GameObject.Find("Dice").GetComponent<Button>().interactable = false;
        GameObject.Find("BonusDice").GetComponent<Button>().interactable = true;
    }
}
