using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                Debug.Log("Game paused");
            }
            else
            {
                Time.timeScale = 1;
                Debug.Log("Game resumed");
            }
        }
    }
}
