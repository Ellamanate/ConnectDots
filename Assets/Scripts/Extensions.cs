using UnityEngine;


namespace Extensions
{
    public static class Extensions
    {
        public static Vector3 ChangeZ(this Vector3 vector, float z) => new Vector3(vector.x, vector.y, z);
    }
}
