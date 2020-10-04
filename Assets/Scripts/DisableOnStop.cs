using UnityEngine;

public class DisableOnStop : MonoBehaviour
{
    /// <summary>
    /// Get animation to play once enable and calls toDisable()
    /// when animation is done playing
    /// </summary>

    private void OnEnable()
    {
        GetComponent<Animation>().Play();
    }

    public void toDisable()
    {
        gameObject.SetActive(false);
    }
}
