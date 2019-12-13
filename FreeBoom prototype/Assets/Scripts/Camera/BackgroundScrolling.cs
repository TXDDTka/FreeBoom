using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [System.Serializable]
    private class EnvironmentLayer
    {
        public GameObject obj = null;
        [Range(0, 1)] public float speed = 1;
    }

    [SerializeField] private EnvironmentLayer[] layers = null;

    private Camera cam = null;
    private Transform camTr = null;
    private Vector2 screenBounds = Vector2.zero;
    private Vector3 lastPosition = Vector3.zero;

    private void Start()
    {
        cam = Camera.main;
        camTr = cam.transform;
        lastPosition = camTr.position;

        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        foreach(var layer in layers)
        {
            LoadChildrens(layer.obj);
        }
    }

    private void LateUpdate()
    {
        foreach (var layer in layers)
        {
            RepositionChildrens(layer.obj);
            //float difference = camTr.position.x - lastPosition.x;
            Vector3 diff = camTr.position - lastPosition;
            layer.obj.transform.Translate((Vector3.right * diff.x + Vector3.up * diff.y) * (1 - layer.speed));
        }

        lastPosition = camTr.position;
    }

    private void LoadChildrens(GameObject o)
    {
        float width = o.GetComponent<SpriteRenderer>().bounds.size.x;
        int childsNeeded = Mathf.CeilToInt(screenBounds.x * 2 / width) + 1;
        GameObject clone = Instantiate(o);

        for (int i = 0; i < childsNeeded; i++)
        {
            Vector3 position = new Vector3(i * width - Mathf.CeilToInt(screenBounds.x), o.transform.position.y, o.transform.position.z);
            GameObject go = Instantiate(clone, position, Quaternion.identity, o.transform);
            go.name = $"{o.name} {i}";
        }

        Destroy(clone);
        Destroy(o.GetComponent<SpriteRenderer>());
    }

    private void RepositionChildrens(GameObject o)
    {
        Transform[] childrens = o.GetComponentsInChildren<Transform>();
        if (childrens.Length > 1)
        {
            Transform first = childrens[1];
            Transform last = childrens[childrens.Length - 1];
            float halfWidth = last.GetComponent<SpriteRenderer>().bounds.extents.x;

            if (camTr.position.x + screenBounds.x > last.position.x + halfWidth)
            {
                first.SetAsLastSibling();
                first.position = new Vector3(last.position.x + halfWidth * 2, last.position.y, last.position.z);
            }
            else if (camTr.position.x - screenBounds.x < first.position.x - halfWidth)
            {
                last.SetAsFirstSibling();
                last.position = new Vector3(first.position.x - halfWidth * 2, first.position.y, first.position.z);
            }
        }
    }
}
