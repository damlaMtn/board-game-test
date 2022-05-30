using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : MonoBehaviour
{
    private Sprite[] _diceSides;
    private Image _diceSprite;

    public GameObject bonusDice;
    private Sprite[] _bonusDiceSides;
    private Image _bonusDiceSprite;

    private int _rollLength = 10;
    private int _playerTurn = 1;

    private bool _isRolling = false;

    private void Start()
    {
        LoadDiceImages();

        bonusDice.GetComponent<Button>().interactable = false;
    }

    public void RollTheDice(int dice)
    {
        if (!_isRolling && !BoardController.instance.isMoving)
        {
            StartCoroutine("Roll", dice);
        }
    }

    private IEnumerator Roll(int dice) //0 normal dice, 1 bonus dice
    {
        if (BoardController.instance.isBonus)
        {
            _playerTurn *= -1;
            BoardController.instance.isBonus = false;
        }

        _isRolling = true;
        int randomDiceSide = 0;

        for (int i = 0; i < _rollLength; i++)
        {
            if (dice == 0)
            {
                randomDiceSide = Random.Range(0, _diceSides.Length);

                _diceSprite.sprite = _diceSides[randomDiceSide];
            }

            else if (dice == 1)
            {
                randomDiceSide = Random.Range(0, _bonusDiceSides.Length);

                _bonusDiceSprite.sprite = _bonusDiceSides[randomDiceSide];
            }

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

        _isRolling = false;
        _playerTurn *= -1;
    }

    private void LoadDiceImages()
    {
        _diceSides = Resources.LoadAll<Sprite>("Images/DiceSides/");
        _diceSprite = GetComponent<Image>();
        _diceSprite.sprite = _diceSides[0];

        _bonusDiceSides = Resources.LoadAll<Sprite>("Images/BonusDiceSides/");
        _bonusDiceSprite = bonusDice.GetComponent<Image>();
    }
}
