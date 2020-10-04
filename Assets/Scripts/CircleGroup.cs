using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class CircleGroup : MonoBehaviour
{
    /// <summary>
    /// Contains and controls circle parts
    /// to make them from a circle group of set size
    /// </summary>

    [Header("Circle values")]
    [Range(2.0f, 60.0f)]
    public float radius = 3;
    [Range (0.1f, 0.9f)]
    public float spacing = 0.1f;

    private CircleContainer parentContainer;
    // number of circle part making the group
    private int numberOfPart = 0;

    private void OnEnable()
    {
        // Set number of part from number of childs
        numberOfPart = transform.childCount;
    }

    private void Start()
    {
        parentContainer = GetComponentInParent<CircleContainer>();
    }

    // Called from child when hit
    public void RemovePart()
    {
        numberOfPart--;
        if (numberOfPart <= 0)
            StartCoroutine(parentContainer.TrySwap());
    }

    // Called from parent when checking if circle is empty
    public bool Check()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
                return false;
        }

        return true;
    }

    // Set active all childs
    public void ActiveAll()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    // Set size in aTime seconds
    public IEnumerator SetSizeSlow(float toRadius, float aTime = 0f)
    {
        for (float t = 0; t <= 1.0f; t += (1f / aTime) / 4f)
        {
            SetSize(Mathf.SmoothStep(radius, toRadius, t));
            yield return new WaitForSeconds((1f /aTime) / 4f);
        }

        radius = toRadius;
    }


    public void SetSize(float toRadius)
    {
        float angle = 360.0f / transform.childCount;
        float insideSize = ((toRadius - 1f - spacing) * 2 * Mathf.PI) / (transform.childCount * 2);
        float outsideSize = ((toRadius + 1f - spacing) * 2 * Mathf.PI) / (transform.childCount * 2);

        int i = 0;
        foreach (Transform child in transform)
        {
            i++;
            // Rotation
            float currentAngle = angle + (angle * i);

            child.localPosition = new Vector3(toRadius, 0, 0);
            child.rotation = new Quaternion();
            child.RotateAround(transform.position, Vector3.forward, currentAngle);

            SpriteShapeController childShape = child.GetComponentInChildren<SpriteShapeController>();

            // left inside
            childShape.spline.SetPosition(0, new Vector2(-1, -insideSize));
            // right inside
            childShape.spline.SetPosition(2, new Vector2(-1, insideSize));
            // right outside
            childShape.spline.SetPosition(3, new Vector2(1, outsideSize));
            // left outside
            childShape.spline.SetPosition(5, new Vector3(1, -outsideSize));

            // inside curve
            childShape.spline.SetPosition(1, new Vector2(-1 + (toRadius * 0.021f), 0));

            // outside curve
            childShape.spline.SetPosition(4, new Vector2(1 + (toRadius * 0.03f), 0));

            // set inside curve to continuous
            childShape.spline.SetTangentMode(1, ShapeTangentMode.Continuous);
            childShape.spline.SetRightTangent(1, new Vector2(0, 0.07f * toRadius));
            childShape.spline.SetLeftTangent(1, new Vector2(0, -0.07f * toRadius));

            // set outside curve to continuous
            childShape.spline.SetTangentMode(4, ShapeTangentMode.Continuous);
            childShape.spline.SetRightTangent(4, new Vector2(0, -0.1f * toRadius));
            childShape.spline.SetLeftTangent(4, new Vector2(0, 0.1f * toRadius));
        }
    }
}
