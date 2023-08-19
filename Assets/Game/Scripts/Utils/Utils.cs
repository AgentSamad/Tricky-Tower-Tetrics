using UnityEngine;

public static class Utils
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static int GetColliderHighetsY(this Transform t)
    {
        Collider[] colls = t.GetComponents<Collider>();
        float maxY = 0;
        foreach (var coll in colls)
        {
            if (coll.bounds.max.y > maxY)
                maxY = coll.bounds.max.y;
        }
        return (int)maxY;
    }
}