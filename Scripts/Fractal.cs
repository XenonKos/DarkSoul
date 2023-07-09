using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    [SerializeField, Range(1, 8)]
    int depth = 4;

    [SerializeField]
    Mesh mesh;

    [SerializeField]
    Material material;

    static Vector3[] directions = { Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
    static Quaternion[] rotations = { 
        Quaternion.identity, 
        Quaternion.Euler(0f, 0f, -90f), 
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    struct FractalPart
    {
        public Vector3 direction;
        public Quaternion rotation;
        public Transform transform;
    }
    FractalPart[][] parts;

    private void Awake()
    {
        parts = new FractalPart[depth][];
        for (int i = 0, length = 1; i < depth; i++, length *= 5)
        {
            parts[i] = new FractalPart[length];
        }

        // Creating Parts
        float scale = 1f;
        parts[0][0] = CreatePart(0, 0, scale);
        for (int level = 1; level < depth; level++)
        {
            scale *= 0.5f;
            FractalPart[] levelParts = parts[level];
            for (int child = 0; child < levelParts.Length; child += 5)
            {
                for (int i = 0; i < 5; ++i)
                {
                    levelParts[child + i] = CreatePart(level, i, scale);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion deltaRotation = Quaternion.Euler(0f, 22.5f * Time.deltaTime, 0f);
        //transform.Rotate(0f, 22.5f * Time.deltaTime, 0f);
        FractalPart rootPart = parts[0][0];
        rootPart.rotation *= deltaRotation;
        rootPart.transform.localRotation = rootPart.rotation;
        parts[0][0] = rootPart;
        //transform.localRotation = rootPart.transform.localRotation;

        for (int level = 1; level < parts.Length; level++)
        {
            FractalPart[] parentParts = parts[level - 1];
            FractalPart[] levelParts = parts[level];
            for (int child = 0; child < levelParts.Length; child++)
            {
                Transform parentTransform = parentParts[child / 5].transform;
                FractalPart part = levelParts[child];
                part.rotation *= deltaRotation;
                part.transform.localPosition = 
                    parentTransform.localPosition + 
                    parentTransform.localRotation * (1.5f * part.transform.localScale.x * part.direction);
                part.transform.localRotation =
                    parentTransform.localRotation * part.rotation;

                levelParts[child] = part;
            }
        }
    }

    FractalPart CreatePart(int levelIndex, int childIndex, float scale)
    {
        GameObject go = new GameObject("Fractal Part L" + levelIndex + " C" + childIndex);
        go.transform.SetParent(transform, false);
        go.transform.localScale = Vector3.one * scale;

        go.AddComponent<MeshFilter>().mesh = mesh;
        go.AddComponent<MeshRenderer>().material = material;

        return new FractalPart()
        {
            direction = directions[childIndex],
            rotation = rotations[childIndex],
            transform = go.transform
        };
    }
}
