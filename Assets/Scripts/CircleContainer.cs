using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class CircleContainer : MonoBehaviour
{
    /// <summary>
    /// Contains all circle group and their
    /// values needed to transform circles
    /// </summary>

    [Header("Circle values")]
    public int maxRingSize = 60;
    public float transitionSpeed = 1f;
    public float[] circleSizes;
    public CircleGroup[] circles;

    [Header("Animation")]
    public GameObject expandCircle;
    private SpriteRenderer expandRend;

    // On transition sound
    private AudioSource nextLevel;

    // Index of smallest circle
    private int _first = 0;
    private int first
    {
        set
        {
            _first = value;
            if (_first >= circles.Length)
                _first = 0;
            else if (_first < 0)
                _first = circles.Length - 1;
        }

        get
        {
            return _first;
        }
    }

    private void OnEnable()
    {
        // Set circle initial sizes
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].SetSize(circleSizes[i]);
            circles[i].SetSize(circleSizes[i]);
        }
    }

    private void Start()
    {
        nextLevel = GetComponent<AudioSource>();
        expandRend = expandCircle.GetComponent<SpriteRenderer>();
    }

    private void OnValidate()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].radius = circleSizes[i];
            circles[i].SetSize(circleSizes[i]);
        }
    }

    // Try to send smallest empty circle to outside
    public IEnumerator TrySwap()
    {
        // Check if smallest circle is empty
        if (circles[first].Check())
        {
            // Play transition sound
            nextLevel.Play();

            // Set interior empty circles to outside camera
            expandCircle.SetActive(true);
            Color circleColor = circles[first].transform.GetChild(0).GetComponent<SpriteShapeRenderer>().color;
            expandRend.color = new Color(circleColor.r, circleColor.g, circleColor.b, 0.125f);

            while (circles[first].Check())
            {
                circles[first].SetSize(maxRingSize);
                circles[first].radius = maxRingSize;
                circles[first].ActiveAll();

                first++;
            }

            // Get last circle value
            int lastRing = first--;
            first++;
            yield return new WaitForEndOfFrame();

            // Play circle closing in animation
            int i = 0;
            for (; i < circles.Length - 1; i++)
            {
                StartCoroutine(circles[first].SetSizeSlow(circleSizes[i], transitionSpeed));
                first++;
                yield return new WaitForEndOfFrame();
            }

            // Play last circle coming in animation (slower)
            StartCoroutine(circles[first].SetSizeSlow(circleSizes[i], transitionSpeed * 1.5f));
            first++;
            yield return new WaitForEndOfFrame();
        }
    }

}
