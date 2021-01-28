using UnityEngine;

public class SimpleTools
{
    public static Vector3 Vec3Rect2Round(Vector3 input)
    {
        Vector3 output = Vector3.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.z * input.z) / 2.0f);
        output.z = input.z * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }

    public static Vector2 Vec2Rect2Round(Vector2 input)
    {
        Vector3 output = Vector3.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
}