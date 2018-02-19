using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MenuScript : MonoBehaviour {


    public void ChangeScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("test_scene");
    }
}
