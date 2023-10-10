using System;
using UnityEngine;

public class BezierCurve
{
	private Vector3[] controlPoints;
    public Vector3[] ControlPoints
    {
        get
        {
            return controlPoints;
        }
        set
        {
            controlPoints = value;
            CalculateParameters();
        }
    }


    private int segmentCount = 0;
    public void Initialize(
        Vector3 p0,
        Vector3 p1,
        Vector3 p2,
        Vector3 p3,
        int segmentCount)
    {
        this.segmentCount = segmentCount;
        controlPoints[0] = p0;
        controlPoints[1] = p1;
        controlPoints[2] = p2;
        controlPoints[3] = p3;
        CalculateParameters();
    }

    private Vector3[] segmentsPoints;
	public Vector3[] SegmentsPoints
	{
		get
		{
			return segmentsPoints;
		}
	}

    private float[] arcLengths;
	private float length;

	public BezierCurve(
		Vector3 p0,
		Vector3 p1,
		Vector3 p2,
		Vector3 p3,
		int segmentCount)
	{
		controlPoints = new Vector3[4];
		Initialize(p0, p1, p2, p3, segmentCount);
	}

	

	public int SegmentCount
	{
		get
		{
			return segmentCount;
		}
		set
		{
			segmentCount = value;
			CalculateParameters();
		}
	}

	public float[] GetDistancesFromStartToPoint()
	{
		return arcLengths;
	}

	public float GetDistanceFromStartToPoint(int index)
	{
		return arcLengths[index];
	}

	public float TotalArcLength
	{
		get { return length; }
	}

	private void CalculateParameters()
	{
		CalculateBezierSegmentsPoints();
		CalculateBezierSegmentsLength();
	}

	private void CalculateBezierSegmentsPoints()
	{
		segmentsPoints = new Vector3[segmentCount];
		float t;
		for (int i = 0; i < segmentCount; i++)
		{
			t = i / (float)segmentCount;
			segmentsPoints[i] = GetPointAtTime(t);
		}
	}

	public Vector3 GetParametricPointAtTime(float t)
	{
		var a = GetParametricValueForTargetLength(t * length);
		return GetPointAtTime(a);
    }

	public Vector3 GetPointAtTime(float t)
	{
		return (1 - t) * (1 - t) * (1 - t) * controlPoints[0]
			+ 3 * (1 - t) * (1 - t) * t * controlPoints[1]
			+ 3 * (1 - t) * t * t * controlPoints[2]
			+ t * t * t * controlPoints[3];
	}

	public void DrawBezierSegments(LineRenderer lineRenderer)
	{
		lineRenderer.positionCount = segmentsPoints.Length;
		for (int i = 0; i < segmentsPoints.Length; i++)
		{
			lineRenderer.SetPosition(i, segmentsPoints[i]);
		}
	}

	public void CalculateBezierSegmentsLength()
	{
		length = 0;
		arcLengths = new float[segmentsPoints.Length];
		for (int i = 0; i < segmentsPoints.Length - 1; i++)
		{
			arcLengths[i] = length;
			length += Vector3.Distance(
				segmentsPoints[i],
				segmentsPoints[i + 1]
			);
		}
		arcLengths[segmentsPoints.Length - 1] = length;
	}

    /// <summary>
    /// Given an array of calculated arc lengths, find the parametric bezier
	/// value that approximates to a specific target length contained in the
	/// bezier curve.
    /// The accuracy of this depends on the length of the arcLengths array.
    /// </summary>
    /// <param name="targetLength">The length to find within the bezier curve
	/// </param>
    public float GetParametricValueForTargetLength(float targetLength)
    {
        int low = 0;
        int index = 0;
        int high = arcLengths.Length - 1;

        // We do a simple binary search for the largest length that's smaller
		// than our target length
        while (low < high)
        {
            index = low + (((high - low) / 2) | 0);

            if (arcLengths[index] < targetLength)
            {
                low = index + 1;
            }
            else
            {
                high = index;
            }
        }

        if (arcLengths[index] > targetLength)
        {
            index--;
        }
		index = (int)MathUtils.ClampValue(index, 0, arcLengths.Length - 1);

        float lengthBefore = arcLengths[index];

        // If we found the exact length we need in our array,
		// return its t value
        if (lengthBefore == targetLength)
        {
            return index / (arcLengths.Length - 1);
        }
        else
        {
            // Otherwise, return the interpolation represented by
			// the remainder (targetLength - lengthBefore)
            return (index
					+ (targetLength - lengthBefore)
					/ (arcLengths[index + 1] - lengthBefore))
				/ (arcLengths.Length - 1);
        }
    }
}
