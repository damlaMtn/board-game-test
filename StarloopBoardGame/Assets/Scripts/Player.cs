using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public GameObject playerPrefab;
    public int playerId;
    public int currentTile;
    public Material material;
    public string playerName;

    public Player(GameObject player, int id, int tile, Material mat, string name)
    {
        playerPrefab = player;
        playerId = id;
        currentTile = tile;
        material = mat;
        playerName = name;
    }
}
