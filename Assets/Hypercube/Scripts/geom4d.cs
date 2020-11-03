using System;
using UnityEngine;

public class geom4d
{

    public static Vector4 projPoint = new Vector4(0f, 0f, 0f, 4f);

    public static Matrix4x4 rotationXW(float angle)
    {
        // Rotation matrix in XW plane
        Matrix4x4 mat = new Matrix4x4();
        mat[0, 0] = Mathf.Cos(angle);
        mat[0, 1] = 0;
        mat[0, 2] = 0;
        mat[0, 3] = Mathf.Sin(angle);

        mat[1, 0] = 0;
        mat[1, 1] = 1;
        mat[1, 2] = 0;
        mat[1, 3] = 0;

        mat[2, 0] = 0;
        mat[2, 1] = 0;
        mat[2, 2] = 1;
        mat[2, 3] = 0;

        mat[3, 0] = -Mathf.Sin(angle);
        mat[3, 1] = 0;
        mat[3, 2] = 0;
        mat[3, 3] = Mathf.Cos(angle);

        return mat;
    }

    public static Matrix4x4 rotationYW(float angle)
    {
        // Rotation matrix in YW plane
        Matrix4x4 mat = new Matrix4x4();
        mat[0, 0] = 1;
        mat[0, 1] = 0;
        mat[0, 2] = 0;
        mat[0, 3] = 0;

        mat[1, 0] = 0;
        mat[1, 1] = Mathf.Cos(angle);
        mat[1, 2] = 0;
        mat[1, 3] = Mathf.Sin(angle);

        mat[2, 0] = 0;
        mat[2, 1] = 0;
        mat[2, 2] = 1;
        mat[2, 3] = 0;

        mat[3, 0] = 0;
        mat[3, 1] = -Mathf.Sin(angle);
        mat[3, 2] = 0;
        mat[3, 3] = Mathf.Cos(angle);

        return mat;
    }

    public static Matrix4x4 rotationZW(float angle)
    {
        // Rotation matrix in ZW plane
        Matrix4x4 mat = new Matrix4x4();
        mat[0, 0] = 1;
        mat[0, 1] = 0;
        mat[0, 2] = 0;
        mat[0, 3] = 0;

        mat[1, 0] = 0;
        mat[1, 1] = 1;
        mat[1, 2] = 0;
        mat[1, 3] = 0;

        mat[2, 0] = 0;
        mat[2, 1] = 0;
        mat[2, 2] = Mathf.Cos(angle);
        mat[2, 3] = Mathf.Sin(angle);

        mat[3, 0] = 0;
        mat[3, 1] = 0;
        mat[3, 2] = -Mathf.Sin(angle);
        mat[3, 3] = Mathf.Cos(angle);

        return mat;
    }

    public static Vector3[] ProjectParallel(Vector4[] vertices)
    {
        //Parallel projection of hypercube vertices to 3d along W axis
        int sz = vertices.GetUpperBound(0);

        Vector3[] vertices3d = new Vector3[sz+1];

        for (int i=0; i<=sz; i++)
        {
            vertices3d[i] = new Vector3(vertices[i][0], vertices[i][1], vertices[i][2]);
        }
        
        return vertices3d;
    }

    public static Vector3[] ProjectPerspective(Vector4[] vertices, float localScaleAx4)
    {
        //Perspective projection of hypercube vertices to 3d from point projPoint onto w=0
        int sz = vertices.GetUpperBound(0);

        Vector3[] vertices3d = new Vector3[sz + 1];

        for (int i = 0; i <= sz; i++)
        {
            float pScale = projPoint[3]*localScaleAx4 / (projPoint[3]*localScaleAx4 - vertices[i][3]);
            vertices3d[i] = new Vector3(projPoint[0] + pScale * vertices[i][0], projPoint[1] + pScale * vertices[i][1], projPoint[2] + pScale * vertices[i][2]);
        }

        return vertices3d;
    }

    public static Vector4 RotateVertex4d(Vector4 vertex, float angleXW, float angleYW, float angleZW)
    {
        //Transform vertex location through 4d principal rotations

        Matrix4x4 matXW = rotationXW(angleXW);
        Matrix4x4 matYW = rotationYW(angleYW);
        Matrix4x4 matZW = rotationZW(angleZW);

        Vector4 vertexNew = matZW*matYW*matXW*vertex;

        return vertexNew;
    }
}
