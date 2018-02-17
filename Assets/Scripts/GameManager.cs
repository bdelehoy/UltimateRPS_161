using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int width = 4; //will be doubled for symmetry
    public GameObject[] gameBoard = new GameObject[width * 2];

    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject exclamationPoint;
    public GameObject timer;
    public GameObject inputManager;
    private AudioSource AudioManager;

    private PlayerScript playerOneScript;
    private AIScript playerTwoScript;
    private TimerScript timerScript;
    private InputScript iScript;
    public AudioClip RoundBeginSound;

    public float movementMultiplier = 2;
    public int playerOneIndex = width - 1;
    public int playerTwoIndex = width;

    public float minTime = 2.0f;
    public float maxTime = 7.0f;
    public float reactionTime = 1.0f;
    private float currentRoundTime;

    public bool gameOver = false;
    private bool acceptingInputs = false;
    private bool endOfTurn = false;
    private bool showMessage = true;   // used for debug control display UI

    private enum Moves {NOMOVE, ROCK, PAPER, SCISSORS};
    private int p1_move = 0;   // dummy: "player 1 hasn't made a move yet"
    private int p2_move = 0;   // dummy: "player 2 hasn't made a move yet"

    // player 1 input = row, player 2 input = col
    private int[,] gameLogic = {{ 0,-1,-1,-1},
                                { 1, 0,-1, 1},
                                { 1, 1, 0,-1},
                                { 1,-1, 1, 0}};

    ///////////////////////////////////

    private void Awake() {
        gameBoard[playerOneIndex] = playerOne;
        gameBoard[playerTwoIndex] = playerTwo;
        AudioManager = GetComponent<AudioSource>();
        playerOneScript = playerOne.GetComponent<PlayerScript>();
        playerTwoScript = playerTwo.GetComponent<AIScript>();   // CHANGE ME LATER!!!  MAYBE!! (idk multiplayer looks scary)
        timerScript = timer.GetComponent<TimerScript>();
        iScript = inputManager.GetComponent<InputScript>();
        Debug.Log("Press V to start accepting input....");
    }

	void Update () {
        if(Input.GetKeyDown("v") && !gameOver && timerScript.time == 0f) { // DEBUG (game should proceed automatically, move after move)
            // start the timer
            // while the timer is ticking down, accept player input
            // detect player 1 input
            //      was it on time?  what move did they choose?
            // generate input for player 2
            // after player 1 gives an input, or after the timer runs out:
            //      calculate and display the result
            //      end the turn
            showMessage = false;
            Debug.Log("--------ROUND STARTED!--------");
            HideGraphics();
            p1_move = p2_move = 0;
 
            AudioManager.PlayOneShot(RoundBeginSound);
 
            currentRoundTime = Random.Range(minTime, maxTime) + RoundBeginSound.length;
            timerScript.setTime(currentRoundTime);
            timer.SetActive(true);
            acceptingInputs = true;
        }
        while(acceptingInputs) {
            StartCoroutine(Anticipation( currentRoundTime ));            
            acceptingInputs = false;
        }
        if(endOfTurn) {
            StopAllCoroutines();    // dangerous.  but it works.
            inputManager.SetActive(false);
            playerOneScript.ShowMoveGraphic(p1_move); playerTwoScript.ShowMoveGraphic(p2_move); exclamationPoint.SetActive(false);
            EndTurn();
            p1_move = p2_move = 0;
            Debug.Log("--------ROUND ENDED!--------");
            endOfTurn = false;
            CheckForWin();
            showMessage = true;
        }
	}

    private void HideGraphics() {
        exclamationPoint.SetActive(false);
        playerOneScript.HideMoveGraphic();
        playerTwoScript.HideMoveGraphic();
    }

    private IEnumerator Anticipation(float seconds) {
        while(true) {
            //Debug.Log("Waiting....");
            yield return new WaitForSecondsRealtime(seconds);
            exclamationPoint.SetActive(true);
            inputManager.SetActive(true);
            StartCoroutine(PromptForMoves(reactionTime));
        }
    }

    private IEnumerator PromptForMoves(float seconds) {
        while(true) {
            //Debug.Log("NOW!");
            p1_move = iScript.move;
            p2_move = playerTwoScript.GetMove();
            yield return new WaitForSecondsRealtime(seconds);
            endOfTurn = true;
        }
    }

    private void EndTurn() {
        int result = ResolveMove(p1_move, p2_move);
        switch (result) {
            case -1:
                // if player 2 wins, move both players left
                Debug.Log("----Player 2 wins this round!");
                MovePlayersLeft();
                break;
            case 0:
                // if tie, nothing (unless powerups but ehhhh for now)
                // play tie animation :^)
                Debug.Log("----TIE this round!");
                break;
            case 1:
                // if player 1 wins, move both players right
                Debug.Log("----Player 1 wins this round!");
                MovePlayersRight();
                break;
        }
    }

    private int ResolveMove(int p1, int p2) {
        // returns -1 if player 1 wins, 0 if tie, and 1 if player 1 wins
        string s = string.Format("---- END RESULTS: Player 1: {0}, Player 2: {1}", p1, p2);
        Debug.Log(s);
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
            Debug.Log("Player two wins the game!");
            gameOver = true;
        }
        if(playerTwoIndex == (width*2)-1) {
            Debug.Log("Player one wins the game!");
            gameOver = true;
        }
    }

    void OnGUI() {
        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 40;

        if(showMessage)
            GUI.Box(new Rect(20,20,800,60), "Press V to start a round!");
	}
}
