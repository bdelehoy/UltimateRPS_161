using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    public Transform canvas;
    public GameObject gameManager; 
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
            if (canvas.gameObject.activeInHierarchy == false) {
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0;
            } else {
                canvas.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
	}

    public void resume() {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ChangeScene(string name) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void restart() {
        ChangeScene("test_scene");
    }

    public void main() {
        ChangeScene("MainMenu");
    }

    public void exit() {
        Application.Quit();
    }

    public void controls() {
        if(gameManager.GetComponent<GameManager>().showMessage == true)
            gameManager.GetComponent<GameManager>().showMessage = false;
        else
            gameManager.GetComponent<GameManager>().showMessage = true;
    }
}
