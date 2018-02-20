using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int width = 4; //will be doubled for symmetry
    public GameObject[] gameBoard = new GameObject[width * 2];

    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject exclamationPoint;
    public GameObject winnerSign;
    public GameObject p1Sign;
    public GameObject p2Sign;
    public GameObject timer;
    public GameObject inputManager;
    public GameObject pauseManager;
    private AudioSource AudioManager;
    private Animator playerOneAnimator;
    private Animator playerTwoAnimator;
    private bool roundActive = true;

    private bool showTimer = false;          // D E B U G, set to false before building

    private PlayerScript playerOneScript;
    private AIScript playerTwoScript;
    private TimerScript timerScript;
    private InputScript iScript;
    public AudioClip RoundBeginSound;

    public float movementMultiplier = 2;
    private int playerOneIndex = width - 1;
    private int playerTwoIndex = width;

    public float minTime = 1.0f;
    public float maxTime = 3.0f;
    public float reactionTime = 1.0f;
    private float currentRoundTime;

    private bool gameOver = false;
    private bool acceptingInputs = false;
    private bool endOfTurn = false;
    public bool showMessage = true;

    private int p1_move = 0;   // dummy: "player 1 hasn't made a move yet"
    private int p2_move = 0;   // dummy: "player 2 hasn't made a move yet"

    // player 1 input: row
    // player 2 input: col
    // Indices -  0: no move   |   1: rock   |   2: paper   |   3: scissors
    // Values  - -1: player 1 loses   |   0: tie   |   1: player 1 wins
    // example: gameLogic[2,3] = -1  -->  Player 1 played paper, player 2 played scissors, player 1 loses.
    private int[,] gameLogic = {{ 0,-1,-1,-1},
                                { 1, 0,-1, 1},
                                { 1, 1, 0,-1},
                                { 1,-1, 1, 0}};

    ///////////////////////////////////

    private void Start() {
        gameBoard[playerOneIndex] = playerOne;
        gameBoard[playerTwoIndex] = playerTwo;
        AudioManager = GetComponent<AudioSource>();
        playerOneScript = playerOne.GetComponent<PlayerScript>();
        playerTwoScript = playerTwo.GetComponent<AIScript>();   // CHANGE ME LATER!!!  MAYBE!! (idk multiplayer looks scary)
        playerOneAnimator = playerOne.GetComponent<Animator>();
        playerTwoAnimator = playerTwo.GetComponent<Animator>();
        timerScript = timer.GetComponent<TimerScript>();
        iScript = inputManager.GetComponent<InputScript>();
        Time.timeScale = 1.0f;
        //Debug.Log("Press V to start accepting input....");
    }

	void Update () {
        if(/*Input.GetKeyDown("v") &&*/ roundActive && !gameOver && timerScript.time == 0f) {
            // start the timer
            // while the timer is ticking down, accept player input
            // detect player 1 input
            //      was it on time?  what move did they choose?
            // generate input for player 2
            // after player 1 gives an input, or after the timer runs out:
            //      calculate and display the result
            //      end the turn

            // TODO: the player isn't penalized for being too quick on the draw.
            //          if input is received during Anticipation(), player 1's move is NOMOVE.
            //showMessage = false;
            Debug.Log("--------ROUND STARTED!--------");
            HideGraphics();         // hide all graphics on screen except for the players
            ResetAnimationBools();  // reset animation bools
            p1_move = p2_move = 0;  // reset the players' moves to "nothing" as default
            iScript.playerReady = false;

 
            AudioManager.PlayOneShot(RoundBeginSound);
 
            currentRoundTime = Random.Range(minTime, maxTime) + RoundBeginSound.length;
            // The length of RoundBeginSound is ~4 seconds.
            timerScript.setTime(currentRoundTime);
            timer.SetActive(true);
            acceptingInputs = true;
        }
        if(acceptingInputs) {
            StartCoroutine(Anticipation( currentRoundTime ));
            inputManager.SetActive(true);
            acceptingInputs = false;
        }
        if(endOfTurn) {
            roundActive = false;
            StopAllCoroutines();            // dangerous.  but it works.
            inputManager.SetActive(false);  // stop accepting input
            EndTurnGraphics();
            EndTurn();                      // resolve the players' moves
            Debug.Log("--------ROUND ENDED!--------");
            endOfTurn = false;
            CheckForWin();
            StartCoroutine(TimingBuffer(2.0f)); // wait 2 seconds in between rounds
            showMessage = false;            // hide the UI after every turn
        }
	}

    private IEnumerator Anticipation(float seconds) {
        while(true && !pauseManager.activeInHierarchy) {
            //Debug.Log("Waiting....");
            yield return new WaitForSeconds(seconds);
            exclamationPoint.SetActive(true);
            StartCoroutine(GetInputs(reactionTime));    // reactionTime is.... a global variable. :I
        }
    }

    private IEnumerator TimingBuffer(float seconds){
        //while (true && !pauseManager.activeInHierarchy)
        //{
            yield return new WaitForSeconds(seconds);
            roundActive = true;
            Debug.Log("buffer over");
        //}
    }

    private IEnumerator GetInputs(float seconds) {
        while(true && !pauseManager.activeInHierarchy) {
            //Debug.Log("NOW!");
            iScript.playerReady = true;
            if(p1_move == 0) {  // if player 1 didn't move before the prompt, their move should be 0 already
                p1_move = iScript.move;
            }
            else {
                p1_move = 0;
            }
            p2_move = playerTwoScript.GetMove();    // the AI should just generate a move at random
            yield return new WaitForSeconds(seconds);
            endOfTurn = true;
        }
    }

    private void HideGraphics() {
        exclamationPoint.SetActive(false);
        playerOneScript.HideMoveGraphic();
        playerTwoScript.HideMoveGraphic();
    }

    private void ResetAnimationBools() {
        playerOneAnimator.SetBool("Punch", false);
        playerOneAnimator.SetBool("Kick", false);
        playerOneAnimator.SetBool("Fireball", false);
        playerOneAnimator.SetBool("Hurt", false);
        playerTwoAnimator.SetBool("Punch", false);
        playerTwoAnimator.SetBool("Kick", false);
        playerTwoAnimator.SetBool("Fireball", false);
        playerTwoAnimator.SetBool("Hurt", false);
    }

    private IEnumerator WaitAnimation(Animator animator)
    {
        while (true && !pauseManager.activeInHierarchy)
        {
            //Debug.Log("one animation cycle");
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length); // + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            ResetAnimationBools();
            //Debug.Log("animation over");
        }
    }

    private void EndTurnGraphics() {

        // p1 win checks
        if (p1_move == 1 && p2_move == 3) {
            playerOneAnimator.SetBool("Punch", true);
            playerTwoAnimator.SetBool("Hurt", true);
        }
            
        if (p1_move == 2 && p2_move == 1) {
            playerOneAnimator.SetBool("Kick", true);
            playerTwoAnimator.SetBool("Hurt", true);
        }

        if (p1_move == 3 && p2_move == 2)
        {
            playerOneAnimator.SetBool("Fireball", true);
            playerTwoAnimator.SetBool("Hurt", true);
        }

        // p2 win checks
        if (p2_move == 1 && p1_move == 3)
        {
            playerTwoAnimator.SetBool("Punch", true);
            playerOneAnimator.SetBool("Hurt", true);
        }

        if (p2_move == 2 && p1_move == 1)
        {
            playerTwoAnimator.SetBool("Kick", true);
            playerOneAnimator.SetBool("Hurt", true);
        }

        if (p2_move == 3 && p1_move == 2)
        {
            playerTwoAnimator.SetBool("Fireball", true);
            playerOneAnimator.SetBool("Hurt", true);
        }

        // same checks
        if (p2_move == 1 && p1_move == 1)
        {
            playerTwoAnimator.SetBool("Punch", true);
            playerOneAnimator.SetBool("Punch", true);
        }

        if (p2_move == 2 && p1_move == 2)
        {
            playerTwoAnimator.SetBool("Kick", true);
            playerOneAnimator.SetBool("Kick", true);
        }

        if (p2_move == 3 && p1_move == 3)
        {
            playerTwoAnimator.SetBool("Fireball", true);
            playerOneAnimator.SetBool("Fireball", true);
        }

        // null checks
        if (p1_move == 1 && p2_move == 0)
        {
            playerOneAnimator.SetBool("Punch", true);
            playerTwoAnimator.SetBool("Hurt", true);
        }

        if (p1_move == 2 && p2_move == 0)
        {
            playerOneAnimator.SetBool("Kick", true);
            playerTwoAnimator.SetBool("Hurt", true);
        }

        if (p1_move == 3 && p2_move == 0)
        {
            playerOneAnimator.SetBool("Fireball", true);
            playerTwoAnimator.SetBool("Hurt", true);
        }

        if (p2_move == 1 && p1_move == 0)
        {
            playerTwoAnimator.SetBool("Punch", true);
            playerOneAnimator.SetBool("Hurt", true);
        }

        if (p2_move == 2 && p1_move == 0)
        {
            playerTwoAnimator.SetBool("Kick", true);
            playerOneAnimator.SetBool("Hurt", true);
        }

        if (p2_move == 3 && p1_move == 0)
        {
            playerTwoAnimator.SetBool("Fireball", true);
            playerOneAnimator.SetBool("Hurt", true);
        }

        if (p2_move == 0 && p1_move == 0)
        {
            // do nothing
        }


        StartCoroutine(WaitAnimation(playerOneAnimator));
        //StartCoroutine(WaitAnimation(playerTwoAnimator));

        playerOneScript.ShowMoveGraphic(p1_move);
        playerTwoScript.ShowMoveGraphic(p2_move);

        exclamationPoint.SetActive(false);
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
        string s = string.Format("---- ROUND RESULTS: Player 1: {0}, Player 2: {1}", p1, p2);
        Debug.Log(s);
        return gameLogic[p1,p2];
    }

    void MovePlayersLeft() {
        if(playerOneIndex > 0) {
            gameBoard[playerOneIndex-1] = gameBoard[playerOneIndex--];
            gameBoard[playerTwoIndex-1] = gameBoard[playerTwoIndex--];
            //playerOne.transform.position = Vector3.Lerp(playerOne.transform.position, new Vector3(playerOne.transform.position.x - movementMultiplier, 0, 0), Time.deltaTime * 50);
            //playerTwo.transform.position = Vector3.Lerp(playerTwo.transform.position, new Vector3(playerTwo.transform.position.x - movementMultiplier, 0, 0), Time.deltaTime * 50);

            playerOne.transform.Translate(new Vector3(-movementMultiplier, 0, 0));
            playerTwo.transform.Translate(new Vector3(-movementMultiplier, 0, 0));
        }
    }

    void MovePlayersRight() {
        if(playerTwoIndex < (width*2)-1) {
            gameBoard[playerOneIndex+1] = gameBoard[playerOneIndex++];
            gameBoard[playerTwoIndex+1] = gameBoard[playerTwoIndex++];
            //playerOne.transform.position = Vector3.Lerp(playerOne.transform.position, new Vector3(playerOne.transform.position.x + movementMultiplier, 0, 0), Time.deltaTime * 50);
            //playerTwo.transform.position = Vector3.Lerp(playerTwo.transform.position, new Vector3(playerTwo.transform.position.x + movementMultiplier, 0, 0), Time.deltaTime * 50);
            
            playerOne.transform.Translate(new Vector3(movementMultiplier, 0, 0));
            playerTwo.transform.Translate(new Vector3(movementMultiplier, 0, 0));
        }
    }

    void CheckForWin(){
        if(playerOneIndex == 0) {
            Debug.Log("Player two wins the game!");
            gameOver = true;
            playerTwoAnimator.SetBool("WinState", true);
            playerOneAnimator.SetBool("LoseState", true);
            winnerSign.SetActive(true);
            p2Sign.SetActive(true);
        }
        if(playerTwoIndex == (width*2)-1) {
            Debug.Log("Player one wins the game!");
            gameOver = true;
            playerOneAnimator.SetBool("WinState", true);
            playerTwoAnimator.SetBool("LoseState", true);
            winnerSign.SetActive(true);
            p1Sign.SetActive(true);
        }
    }

    void OnGUI() {
        // Debug UI.  Probably not gonna be in final build.
        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 40;

        if(showMessage) {
            GUI.Box(new Rect(20,20,800,60), "WAIT FOR THE SIGNAL");
            GUI.Box(new Rect(20,100,400,180), "Q - Rock\nW - Paper\nE - Scissors");
        }
        if(showTimer) {
                GUI.Box(new Rect(20,300,800,60), timerScript.time.ToString());
        }
	}
}
