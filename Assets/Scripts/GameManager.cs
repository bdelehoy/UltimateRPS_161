using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int width = 4; //will be doubled for symmetry
    public GameObject[] gameBoard = new GameObject[width * 2];
    public GameObject playerOne;
    public GameObject playerTwo;
    public int movementMultiplier = 2;
    public int playerOneIndex = width - 1;
    public int playerTwoIndex = width;
    private void Awake()
    {
        gameBoard[playerOneIndex] = playerOne;
        gameBoard[playerTwoIndex] = playerTwo;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        //move left
		if(Input.GetKeyDown("z"))
        {
            gameBoard[playerOneIndex--];
            gameBoard[playerTwoIndex--];
        }
	}
}
