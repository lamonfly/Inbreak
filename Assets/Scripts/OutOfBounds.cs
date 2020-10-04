using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    /// <summary>
    /// Check if object goes out of camera range
    /// If so, call gameController to game over
    /// </summary>
    
    private void OnBecameInvisible()
    {
        if (GameController.Instance != null)
            GameController.Instance.GameOver();
    }
}
