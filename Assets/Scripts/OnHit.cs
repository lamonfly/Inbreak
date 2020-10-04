using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class OnHit : MonoBehaviour
{
    /// <summary>
    /// Handle event of ball collision
    /// </summary>

    private PartCirclePlay animPlay;
    private CircleGroup parentCircle = null;
    private SpriteShapeRenderer spriteShape;
    private PolygonCollider2D coll;

    private void Start()
    {
        parentCircle = GetComponentInParent<CircleGroup>();
        animPlay = FindObjectOfType<PartCirclePlay>();
        spriteShape = GetComponent<SpriteShapeRenderer>();
        coll = GetComponent<PolygonCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Add to score
        GameController.Instance.AddPoint();
        // Play hit animation
        animPlay.playAnim(transform.position, spriteShape.color);
        // Fade to 0 alpha
        StartCoroutine(FadeTo(0, 0.1f));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        // Disbale collide
        coll.enabled = false;
        // Get previous color
        Color prevColor = spriteShape.material.color;
        // Get current alpha
        float alpha = spriteShape.material.color.a;


        // Set alpha slowly fading
        for (float t = 0.0f; t < aTime; t += Time.deltaTime)
        {
            Color newColor = new Color(prevColor.r, prevColor.g, prevColor.b, Mathf.Lerp(alpha, aValue, t));
            spriteShape.material.color = newColor;
            yield return new WaitForEndOfFrame();
        }

        // Reset object
        coll.enabled = true;
        spriteShape.material.color = prevColor;
        gameObject.SetActive(false);
        parentCircle.RemovePart();
    }
}
