using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public static class Extentions
    {
        public static Vector3 ToUnityVector3(this Assimp.Vector3D vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        public static Mesh ToUnityMesh(this Assimp.Mesh mesh)
        {
            Mesh unityMesh = new Mesh();
            unityMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // 大きなメッシュに対応

            // AssimpのメッシュデータからUnityのメッシュデータへの変換
            Vector3[] vertices = mesh.Vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToArray();
            int[] triangles = mesh.GetIndices().Select(i => (int)i).ToArray();

            // 頂点データの検証と修正
            ValidateAndCorrectVertices(vertices);

            unityMesh.vertices = vertices;
            unityMesh.triangles = triangles;

            if (mesh.HasNormals)
            {
                unityMesh.normals = mesh.Normals.Select(n => new Vector3(n.X, n.Y, n.Z)).ToArray();
            }

            // テクスチャ座標の変換
            if (mesh.HasTextureCoords(0)) // TextureCoordinateChannelCount の代わりに HasTextureCoords を使用
            {
                var uvList = mesh.GetTextureCoords(0);
                Vector2[] uvs = new Vector2[uvList.Count()];
                for (int i = 0; i < uvList.Count(); i++) 
                {
                    uvs[i] = new Vector2(uvList[i].X, uvList[i].Y);
                }
                unityMesh.uv = uvs;
            }

            unityMesh.RecalculateBounds();
            return unityMesh;
        }

            // 頂点データを検証し、必要に応じて修正するメソッド
        private static void ValidateAndCorrectVertices(Vector3[] vertices)
        {
            bool isInvalid = false;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (float.IsInfinity(vertices[i].x) || float.IsNaN(vertices[i].x) ||
                    float.IsInfinity(vertices[i].y) || float.IsNaN(vertices[i].y) ||
                    float.IsInfinity(vertices[i].z) || float.IsNaN(vertices[i].z))
                {
                    vertices[i] = Vector3.zero; // 無効なデータをゼロに置き換え
                    isInvalid = true;
                }
            }

            if (isInvalid)
            {
                Debug.LogWarning("無効な頂点データが修正されました。");
            }
        }


        public static Matrix4x4 ToUnityMatrix(this Assimp.Matrix4x4 matrix)
        {
            Matrix4x4 unityMatrix = new Matrix4x4();

            unityMatrix.SetRow(0, new Vector4(matrix.A1, matrix.A2, matrix.A3, matrix.A4));
            unityMatrix.SetRow(1, new Vector4(matrix.B1, matrix.B2, matrix.B3, matrix.B4));
            unityMatrix.SetRow(2, new Vector4(matrix.C1, matrix.C2, matrix.C3, matrix.C4));
            unityMatrix.SetRow(3, new Vector4(matrix.D1, matrix.D2, matrix.D3, matrix.D4));

            return unityMatrix;
        }

        public static Color ToUnityColor(this Assimp.Color4D color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
