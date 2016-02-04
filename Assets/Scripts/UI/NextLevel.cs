using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {
	public int levelToLoad;

	public void LoadNextLevel() {
		SceneManager.LoadScene(levelToLoad);
	}
}
