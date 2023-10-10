using UnityEngine;

public static class MathUtils
{
	public static Vector3 GetDirection(Vector3 origin, Vector3 destination)
	{
		return (destination - origin).normalized;
    }

	public static float GetDistance(Vector2 vector1, Vector2 vector2)
	{
		return GetDistance(vector1.x, vector1.y, vector2.x, vector2.y);
	}

	public static float GetDistance(Transform transform1, Transform transform2)
	{
		return GetDistance(
			transform1.position.x,
			transform1.position.y,
			transform2.position.x,
			transform2.position.y
		);
	}

	public static float GetDistance(GameObject object1, GameObject object2)
	{
		return GetDistance(
			object1.transform.position.x,
			object1.transform.position.y,
			object2.transform.position.x,
			object2.transform.position.y
		);
	}

	public static float GetDistance(float x1, float y1, float x2, float y2)
	{
		return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
	}

	/// <summary>
	/// When we want to compare to distances, we compare the squared distances,
	/// so that we don't execute Mathf.Sqrt at all
	/// </summary>
	public static float GetSquaredDistance(Vector2 vector1, Vector2 vector2)
	{
		return GetSquaredDistance(vector1.x, vector1.y, vector2.x, vector2.y);
	}

	/// <summary>
	/// When we want to compare to distances, we compare the squared distances,
	/// so that we don't execute Mathf.Sqrt at all
	/// </summary>
	public static float GetSquaredDistance(
		float x1, float y1, float x2, float y2)
	{
		return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
	}

	/// <summary>
	/// Calculates the traveled distance given the final velocitiy,
	/// initial velocity and acceleration.
	/// Based on (vf^2 - vi^2) = 2ad
	/// </summary>
	public static float GetDistance(
		float finalVelocity,
		float initialVelocity,
		float acceleration)
	{
		return ((finalVelocity * finalVelocity)
			- (initialVelocity * initialVelocity))
			/ (2 * acceleration);
	}

	/// <summary>
	/// Calculates the size of a transform by calculating its rendering limits
	/// and subtracting them on both X and Y axis.
	/// </summary>
	public static Vector2 GetSizeOfTransform(
		Transform transform,
		bool includeMasked = true,
		bool includeInactive = false,
		float frustumDistance = 0)
	{
		Vector4 limits = GetLimitsOfTransform(
			transform,
			includeMasked,
			includeInactive,
			frustumDistance
		);
		return new Vector2(limits.w - limits.z, limits.x - limits.y);
	}

	/// <summary>
	/// Caluclates the rendering limits of a transform on X and Y axis.
	/// By limits we understand the points that the object gets rendered
	/// between. The function takes all <see cref="SpriteRenderer"/>,
	/// all <see cref="MeshRenderer"/> and all <see cref="Camera"/> components
	/// from the <paramref name="transform"/> and its children and finds the
	/// starting and ending rendering points on both X and Y axis.
	/// </summary>
	public static Vector4 GetLimitsOfTransform(
		Transform transform,
		bool includeMasked = true,
		bool includeInactive = false,
		float frustumDistance = 0)
	{
		Vector4 limits = Vector4.zero; // x - Up, y - down, z - left, w - right
		float possibleLimit;
		bool firstRenderer = true;
		foreach (Renderer renderer
			in transform.GetComponentsInChildren<Renderer>())
		{

			if (renderer is SpriteRenderer spriteRenderer)
			{
				if (!includeMasked
					&& spriteRenderer.maskInteraction
					!= SpriteMaskInteraction.None)
				{
					continue;
				}
			}
			if (!includeInactive
				&& !renderer.gameObject.activeInHierarchy)
			{
				continue;
			}
			if (firstRenderer)
			{
				firstRenderer = false;
				limits.x = renderer.transform.position.y
					+ renderer.bounds.size.y / 2f;
				limits.y = renderer.transform.position.y
					- renderer.bounds.size.y / 2f;
				limits.z = renderer.transform.position.x
					- renderer.bounds.size.x / 2f;
				limits.w = renderer.transform.position.x
					+ renderer.bounds.size.x / 2f;
			}
			else
			{
				possibleLimit = renderer.transform.position.y
					+ renderer.bounds.size.y / 2f;
				if (possibleLimit > limits.x)
				{
					limits.x = possibleLimit;
				}
				possibleLimit = renderer.transform.position.y
					- renderer.bounds.size.y / 2f;
				if (possibleLimit < limits.y)
				{
					limits.y = possibleLimit;
				}
				possibleLimit = renderer.transform.position.x
					- renderer.bounds.size.x / 2f;
				if (possibleLimit < limits.z)
				{
					limits.z = possibleLimit;
				}
				possibleLimit = renderer.transform.position.x
					+ renderer.bounds.size.x / 2f;
				if (possibleLimit > limits.w)
				{
					limits.w = possibleLimit;
				}
			}
		}

		Vector2 cameraSize;
		foreach (Camera camera
			in transform.GetComponentsInChildren<Camera>())
		{
			if (!includeInactive
				&& !camera.gameObject.activeInHierarchy)
			{
				continue;
			}

			if (camera.orthographic)
			{
				cameraSize = CameraUtils.GetOrtographicSize(camera);
			}
			else
			{
				cameraSize = CameraUtils.GetPerspectiveFrustumSize(
					camera,
					frustumDistance
				);
			}
			if (firstRenderer)
			{
				firstRenderer = false;
				limits.x = camera.transform.position.y
					+ cameraSize.y / 2f;
				limits.y = camera.transform.position.y
					- cameraSize.y / 2f;
				limits.z = camera.transform.position.x
					- cameraSize.x / 2f;
				limits.w = camera.transform.position.x
					+ cameraSize.x / 2f;
			}
			else
			{
				possibleLimit = camera.transform.position.y
					+ cameraSize.y / 2f;
				if (possibleLimit > limits.x)
				{
					limits.x = possibleLimit;
				}
				possibleLimit = camera.transform.position.y
					- cameraSize.y / 2f;
				if (possibleLimit < limits.y)
				{
					limits.y = possibleLimit;
				}
				possibleLimit = camera.transform.position.x
					- cameraSize.x / 2f;
				if (possibleLimit < limits.z)
				{
					limits.z = possibleLimit;
				}
				possibleLimit = camera.transform.position.x
					+ cameraSize.x / 2f;
				if (possibleLimit > limits.w)
				{
					limits.w = possibleLimit;
				}
			}
		}
		return limits;
	}

	/// <summary>
	/// Loops through all  <see cref="SpriteRenderer"/>,
	/// all <see cref="MeshRenderer"/> and all <see cref="Camera"/> components
	/// from the <paramref name="transform"/> and its children and finds
	/// the biggest rendered half dimension.
	/// </summary>
	public static Vector2 GetBiggestHalfDimensions(Transform transform)
	{
		Vector2 biggestHalfDimensions = Vector2.zero;
		float possibleBiggestHalfDimension;
		foreach (SpriteRenderer spriteRenderer
			in transform.GetComponentsInChildren<SpriteRenderer>())
		{
			possibleBiggestHalfDimension = spriteRenderer.bounds.size.x / 2f;
			if (possibleBiggestHalfDimension > biggestHalfDimensions.x)
			{
				biggestHalfDimensions.x = possibleBiggestHalfDimension;
			}
			possibleBiggestHalfDimension = spriteRenderer.bounds.size.y / 2f;
			if (possibleBiggestHalfDimension > biggestHalfDimensions.y)
			{
				biggestHalfDimensions.y = possibleBiggestHalfDimension;
			}
		}
		foreach (MeshRenderer meshRenderer
			in transform.GetComponentsInChildren<MeshRenderer>())
		{
			possibleBiggestHalfDimension = meshRenderer.bounds.size.x / 2f;
			if (possibleBiggestHalfDimension > biggestHalfDimensions.x)
			{
				biggestHalfDimensions.x = possibleBiggestHalfDimension;
			}
			possibleBiggestHalfDimension = meshRenderer.bounds.size.y / 2f;
			if (possibleBiggestHalfDimension > biggestHalfDimensions.y)
			{
				biggestHalfDimensions.y = possibleBiggestHalfDimension;
			}
		}
		Vector2 cameraSize;
		foreach (Camera camera
			in transform.GetComponentsInChildren<Camera>())
		{
			cameraSize = CameraUtils.GetOrtographicSize(camera);
			possibleBiggestHalfDimension = cameraSize.x / 2f;
			if (possibleBiggestHalfDimension > biggestHalfDimensions.x)
			{
				biggestHalfDimensions.x = possibleBiggestHalfDimension;
			}
			possibleBiggestHalfDimension = cameraSize.y / 2f;
			if (possibleBiggestHalfDimension > biggestHalfDimensions.y)
			{
				biggestHalfDimensions.y = possibleBiggestHalfDimension;
			}
		}
		return biggestHalfDimensions;
	}

	public static float GetLookAtAngle(
		Transform lookAtTransform,
		Vector3 referencePosition)
	{
		Vector3 referencePositionRelativeToLookAtTransform
			= lookAtTransform.InverseTransformPoint(referencePosition);
		return GetLookAtAngle(referencePositionRelativeToLookAtTransform);
	}

	public static float GetLookAtAngle(
		Vector3 referencePositionRelativeToLookAtTransform)
	{
		float angle
			= Mathf.Atan2(
				referencePositionRelativeToLookAtTransform.y,
				referencePositionRelativeToLookAtTransform.x
			)
			* Mathf.Rad2Deg;
		angle = ClampAngle(angle);
		return angle;
	}

	public static float GetLookAtAngleXY(
		Vector3 initVector,
		Vector3 lookAtVector)
	{
		var dir = lookAtVector - initVector;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		angle = ClampAngle(angle);
		return angle;
	}

	public static float GetLookAtAngleXZ(
		Vector3 initVector,
		Vector3 lookAtVector)
	{
		var dir = lookAtVector - initVector;
		var angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
		angle = ClampAngle(angle);
		return angle;
	}

	public static float GetLookAtAngleYZ(
	   Vector3 initVector,
	   Vector3 lookAtVector)
	{
		var dir = lookAtVector - initVector;
		var angle = Mathf.Atan2(dir.z, dir.y) * Mathf.Rad2Deg;
		angle = ClampAngle(angle);
		return angle;
	}

	public static Vector3 GetVectorFromAngleXY(float angle)
	{
		var radians = Mathf.Deg2Rad * angle;
        return new Vector3(
			Mathf.Sin(radians),
            Mathf.Cos(radians),
            0
		);
	}

    public static Vector3 GetVectorFromAngleXZ(float angle)
    {
        var radians = Mathf.Deg2Rad * angle;
        return new Vector3(
            Mathf.Sin(radians),
            0,
            Mathf.Cos(radians)
        );
    }

    public static Vector3 GetVectorFromAngleYZ(float angle)
    {
        var radians = Mathf.Deg2Rad * angle;
        return new Vector3(
            0,
            Mathf.Sin(radians),
            Mathf.Cos(radians)
        );
    }

    public static Vector2 NormalizeVector(this Vector2 vector)
	{
		float vectorLength = VectorLength(vector);
		return new Vector2(vector.x / vectorLength, vector.y / vectorLength);
	}

	public static Vector3 NormalizeVector(this Vector3 vector)
	{
		float vectorLength = Mathf.Sqrt((vector.x * vector.x)
			+ (vector.y * vector.y) + (vector.z * vector.z));
		return new Vector3(
			vector.x / vectorLength,
			vector.y / vectorLength,
			vector.z / vectorLength
		);
	}

	public static float NormalizeValue(
		float value,
		float newStart,
		float newEnd,
		float originalStart,
		float originalEnd)
	{
		float scale = (newEnd - newStart) / (originalEnd - originalStart);
		return newStart + ((value - originalStart) * scale);
	}

	/// <summary>
	/// Vector length is also called vector mangnitude.
	/// </summary>
	public static float VectorLength(Vector2 vector)
	{
		return Mathf.Sqrt((vector.x * vector.x) + (vector.y * vector.y));
	}

	/// <summary>
	/// Vector length is also called vector mangnitude.
	/// </summary>
	public static float VectorLength(float x, float y)
	{
		return Mathf.Sqrt((x * x) + (y * y));
	}

	/// <summary>
	/// Calculates the direction in which a vector is pointing.
	/// Ex: useful for when we have velocity on x and velocity on y and we want
	/// to know the direction between them
	/// </summary>
	public static float VectorDirection(
		this Vector2 vector)
	{
		return Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
	}

	/// <summary>
	/// dotProduct = -1 => the vectors are pointing in exactly opposite
	/// directions
	/// dotProduct = 0 => the vectors are perpendicular
	/// dotProduct = 1 => the vectors are pointing in exactly the same direction
	/// </summary>
	public static float VectorDotProduct(
		this Vector2 vector1,
		Vector2 vector2)
	{
		return vector1.x * vector2.x + vector1.y * vector2.y;
	}

	/// <summary>
	/// dotProduct = -1 => the vectors are pointing in exactly opposite
	/// directions
	/// dotProduct = 0 => the vectors are perpendicular
	/// dotProduct = 1 => the vectors are pointing in exactly the same direction
	/// </summary>
	public static float VectorDotProduct(
		this Vector3 vector1,
		Vector3 vector2)
	{
		return vector1.x * vector2.x
			+ vector1.y * vector2.y
			+ vector1.z * vector2.z;
	}

	/// <summary>
	/// We can use the result to find the sign that means the direction in which
	/// we should rotate first vector towards the second
	/// </summary>
	public static float VectorCrossProduct(
		this Vector2 vector1,
		Vector2 vector2)
	{
		// we can use the result to find the sign that means the direction in
		// which we should rotate first vector towards the second
		return vector1.x * vector2.y - vector1.y * vector2.x;
	}

	public static float AngleBetweenTwoVectors(
		this Vector2 vector1,
		Vector2 vector2)
	{
		float angle2 = ClampAngle(
			Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg
		);
		float angle1 = ClampAngle(
			Mathf.Atan2(vector1.y, vector1.x) * Mathf.Rad2Deg
		);
		float difference = angle2 - angle1;
		if (difference > 180)
		{
			difference = difference - 360;
		}
		if (difference < -180)
		{
			difference = 360 + difference;
		}
		return difference;
	}

	public static Vector2 RotateVectorByAngleAmount(
		this Vector2 vector,
		float angleAmount)
	{
		angleAmount = angleAmount * Mathf.Deg2Rad;
		return new Vector2(
			vector.x * Mathf.Cos(angleAmount)
				- vector.y * Mathf.Sin(angleAmount),
			vector.x * Mathf.Sin(angleAmount)
				+ vector.y * Mathf.Cos(angleAmount)
		);
	}

	/// <summary>
	/// Clamps the given angle between 0 and 360.
	/// </summary>
	/// <returns>Returns a value that means the same as
	/// the <param name="angle"/>, but it is clamped between 0 and 360 degrees.
	/// </returns>
	public static float ClampAngle(float angle)
	{
		if (angle < 0)
		{
			angle = 360 + angle;
		}
		if (angle > 360)
		{
			angle = angle - 360;
		}
		return angle;
	}

	public static int SumOfFirstNPositiveIntegers(int n)
	{
		return n * (n + 1) / 2;
	}

	public static int GetSign(float a)
	{
		if (a >= 0) { return 1; }
		else { return -1; }
	}

	public static bool CheckIfSameSign(float a, float b)
	{
		return (a >= 0 && b >= 0) || (a < 0 && b < 0);
	}

	public static float GetArcLength(float arcAngle, float circleRadius)
	{
		return 2 * Mathf.PI * circleRadius * (arcAngle / 360);
	}

	public static float ClampValue(
		this float value,
		float minValue,
		float maxValue)
	{
		var finalMinValue = Mathf.Min(minValue, maxValue);
		var finalMaxValue = Mathf.Max(minValue, maxValue);
		if (value < finalMinValue)
		{
			value = finalMinValue;
		}
		if (value > finalMaxValue)
		{
			value = finalMaxValue;
		}
		return value;
	}

    public static bool EqualsApproximately(
		this Vector3 v1,
		Vector3 v2,
		float allowedDifference)
    {
        if (Mathf.Abs(v1.x - v2.x) > allowedDifference)
        {
            return false;
        }
        if (Mathf.Abs(v1.y - v2.y) > allowedDifference)
        {
            return false;
        }
        if (Mathf.Abs(v1.z - v2.z) > allowedDifference)
		{
			return false;
		}
		return true;
    }

	public static int GetDigitSum(int number)
	{
        var sum = 0;
        while (number != 0)
        {
            sum += number % 10;
            number /= 10;
        }
		return sum;
    }

    public static bool AreApproximatelyEqual(this Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }
}
