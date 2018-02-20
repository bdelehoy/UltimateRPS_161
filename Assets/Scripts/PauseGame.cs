using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour {

    public Transform canvas;
    public GameObject gameManagerObject;
    GameManager gm;

    void Awake() {
        gm = gameManagerObject.GetComponent<GameManager>();
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
            if (!canvas.gameObject.activeInHierarchy) {
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else {
                canvas.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
	}

    public void resume() {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void main() {
        SceneManager.LoadScene("MainMenu");
    }

    public void exit() {
        Application.Quit();
    }
    
    public void controls() {
        if(gm.showMessage == true)
            gm.showMessage = false;
        else
            gm.showMessage = true;
    }

/*
    // optimized!  see above.
    public void controls() {
        if(gameManager.GetComponent<GameManager>().showMessage == true)
            gameManager.GetComponent<GameManager>().showMessage = false;
        else
            gameManager.GetComponent<GameManager>().showMessage = true;
    } */
}
