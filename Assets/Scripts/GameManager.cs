using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int width = 4; //will be doubled for symmetry
    public GameObject[] gameBoard = new GameObject[width * 2];

    public GameObject playerOne;
    public GameObject playerTwo;

    private PlayerScript playerOneScript;
    private AIScript playerTwoScript;

    public float movementMultiplier = 2;
    public int playerOneIndex = width - 1;
    public int playerTwoIndex = width;

    private int p1_move;
    private int p2_move;

    public bool gameOver = false;
    private enum Moves {ROCK, PAPER, SCISSORS, NOMOVE};

    // player 1 input = row, player 2 input = col
    private int[,] gameLogic = {{ 0,-1, 1, 1},
                                { 1, 0,-1, 1},
                                {-1, 1, 0, 1},
                                {-1,-1,-1, 0}};

    // /////////////////////////////// //

    private void Awake() {
        gameBoard[playerOneIndex] = playerOne;
        gameBoard[playerTwoIndex] = playerTwo;
        playerOneScript = playerOne.GetComponent<PlayerScript>();
        playerTwoScript = playerTwo.GetComponent<AIScript>();   // CHANGE ME LATER!!!
    }

	void Update () {
        if(Input.GetKeyDown("v")) {
            Debug.Log("Awaiting moves....");
            PromptForMoves();
            
            /*if(Input.GetKeyDown("z"))   // DEBUG
            {
                MovePlayersLeft();
            }
            if(Input.GetKeyDown("x"))   // DEBUG
            {
                MovePlayersRight();
            }*/

            CheckForWin();
        }
	}

    IEnumerator WaitSeconds(int secs) {
        yield return new WaitForSeconds(secs);
    }

    void PromptForMoves() {
        p2_move = playerTwoScript.GetMove();
        Debug.Log("Player 2 accepted!");

        p1_move = playerOneScript.GetMove();

        StartCoroutine(WaitSeconds(3));

        int result = ResolveMove(p1_move, p2_move);

        switch (result) {
            case -1:
                // if player 2 wins, move both players left
                Debug.Log("Player 2 wins this round!");
                MovePlayersLeft();
                break;
            case 0:
                // if tie, nothing (unless powerups but ehhhh for now)
                // play tie animation :^)
                Debug.Log("TIE!");
                break;
            case 1:
                // if player 1 wins, move both players right
                Debug.Log("Player 1 wins this round!");
                MovePlayersRight();
                break;
        }
    }

    private int ResolveMove(int p1, int p2) {
        // returns -1 if player 2 wins, 0 if tie, and 1 if player 1 wins
        return gameLogic[p1,p2];
    }

    void MovePlayersLeft() {
        if(playerOneIndex > 0) {  // bounds checking weee
            gameBoard[playerOneIndex-1] = gameBoard[playerOneIndex--];
            gameBoard[playerTwoIndex-1] = gameBoard[playerTwoIndex--];
            playerOne.transform.Translate(new Vector3(-movementMultiplier, 0, 0));
            playerTwo.transform.Translate(new Vector3(-movementMultiplier, 0, 0));
        }
    }

    void MovePlayersRight() {
        if(playerTwoIndex < (width*2)-1) {  // bounds checking weee
            gameBoard[playerOneIndex+1] = gameBoard[playerOneIndex++];
            gameBoard[playerTwoIndex+1] = gameBoard[playerTwoIndex++];
            playerOne.transform.Translate(new Vector3(movementMultiplier, 0, 0));
            playerTwo.transform.Translate(new Vector3(movementMultiplier, 0, 0));
        }
    }

    void CheckForWin(){
        if(playerOneIndex == 0) {
            Debug.Log("Player two wins!");
            gameOver = true;
        }
        if(playerTwoIndex == (width*2)-1) {
            Debug.Log("Player one wins!");
            gameOver = true;
        }
    }
}
