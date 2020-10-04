using UnityEngine;
using UnityEngine.UI;

public class PartCirclePlay : MonoBehaviour
{
    /// <summary>
    /// Create part circle orientation
    /// </summary>

    public GameObject partCircleAnimation;

    public void playAnim(Vector3 position, Color color)
    {
        // Instantiate object animation
        GameObject tempAnim = Instantiate(partCircleAnimation, transform);

        // Rotate towards circle part
        float angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
        tempAnim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Set color
        color.a = 0.125f;
        tempAnim.GetComponentInChildren<Image>().color = color;

        Animator anim = tempAnim.GetComponentInChildren<Animator>();

        // Set frame of start
        if (position.magnitude < 7)
            anim.Play("partExpandCircle", 0, 0);
        else if (position.magnitude < 12)
            anim.Play("partExpandCircle", 0, 16f / 60);
        else if (position.magnitude < 18)
            anim.Play("partExpandCircle", 0, 24f / 60);
        else
            anim.Play("partExpandCircle", 0, 31f / 60);

        Destroy(tempAnim, 1f);
    }
}
