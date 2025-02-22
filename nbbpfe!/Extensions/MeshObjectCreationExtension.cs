﻿using System.Linq;
using UnityEngine;

namespace nbppfe.Extensions
{
    public static class MeshObjectCreationExtension
    {
        public static GameObject CreateCube(Texture2D tex, bool useUVMap = true)
        {
            var cube = CreatePrimitiveObject(PrimitiveType.Cube, tex);

            /*
			 * Mesh script made thanks for Ilkinulas (http://ilkinulas.github.io/development/unity/2016/05/06/uv-mapping.html)
			 */
            if (useUVMap)
            {
                Mesh mesh = cube.GetComponent<MeshFilter>().mesh;
                mesh.Clear();
                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.uv = uvs;
                mesh.Optimize();
                mesh.RecalculateNormals();
            }

            return cube;
        }

        public static GameObject CreatePrimitiveObject(PrimitiveType type, Texture2D tex) =>
            GameObject.CreatePrimitive(type).SetObjectMat(tex);


        public static GameObject SetObjectMat(this GameObject obj, Texture2D tex)
        {
            var r = obj.GetComponent<MeshRenderer>();
            r.material = defaultMaterial;
            r.material.mainTexture = tex;
            return obj;
        }

        static readonly Vector3[] vertices = [
        new (0, 1f, 0),
        new (0, 0, 0),
        new (1.004f, 1f, 0),
        new (1f, 0, 0),

        new (0, 0, 1f),
        new (1f, 0, 1f),
        new (0, 1f, 1f),
        new (1f, 1f, 1f),

        new (0, 1f, 0),
        new (1f, 1f, 0),

        new (0, 1f, 0),
        new(0, 1f, 1f),

        new(1f, 1f, 0),
        new(1f, 1f, 1f),
    ];

        static readonly int[] triangles = [
        0, 2, 1,
        1, 2, 3,
        4, 5, 6,
        5, 7, 6,
        6, 7, 8,
        7, 9 ,8,
        1, 3, 4,
        3, 5, 4,
        1, 11,10,
        1, 4, 11,
        3, 12, 5,
        5, 12, 13];

        /*

		horizontal:
		0.2475 = 99
		0.4975 = 199
		0.5 = 200
		0.7475 = 299
		0.75 = 300
		0.25 = 100
		0.9975 = 399

		vertical:
		0.66220735 = 198
		0.6655518 = 199
		0.33110367 = 99
		0.996655518 = 298


		*/

        static readonly Vector2[] uvs = [
        new(0, 0.66f),
        new(0.25f, 0.66f),
        new(0, 0.33f),
        new(0.25f, 0.34f),

        new(0.5f, 0.66f),
        new(0.5f, 0.34f),
        new(0.75f, 0.65f),
        new(0.75f, 0.34f),

        new(1, 0.66f),
        new(1, 0.34f),

        new(0.25f, 1),
        new(0.5f, 1),

        new(0.25f, 0),
        new(0.5f, 0)];

        internal static Material defaultMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(x => x.name == "Locker_Red").First();
    }
}
