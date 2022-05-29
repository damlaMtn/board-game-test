using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : MonoBehaviour
{
    private Sprite[] _diceSides;
    private Image _diceSprite;
    private int _rollLength = 10;
    private int _playerTurn = 1;

    private bool isRolling = false;

    private void Start()
    {
        _diceSides = Resources.LoadAll<Sprite>("Images/DiceSides/");
        _diceSprite = GetComponent<Image>();
        _diceSprite.sprite = _diceSides[0];
    }

    public void RollTheDice()
    {
        if (!isRolling)
        {
            StartCoroutine("Roll");
        }
    }

    private IEnumerator Roll()
    {
        isRolling = true;
        int randomDiceSide = 0;

        for (int i = 0; i < _rollLength; i++)
        {
            randomDiceSide = Random.Range(0, _diceSides.Length);

            _diceSprite.sprite = _diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        BoardController.instance.diceValue = randomDiceSide + 1;

        if (_playerTurn == 1)
        {
            BoardController.instance.MovePlayer(1);
        }
        else
        {
            BoardController.instance.MovePlayer(2);
        }

        _playerTurn *= -1;
        isRolling = false;
    }
}
