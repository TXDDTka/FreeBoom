using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairPointsParent : MonoBehaviour
{
	public static CrosshairPointsParent Instance { get; private set; }
    public GameObject pointsParent;

    [System.Serializable]
    public class CrossHair
    {
        public GameObject points;
        public SpriteRenderer pointsSprites;
    }

    public List<CrossHair> crossHair;


    public int crosshairIndex = 0;
    public float distance = 0f;
    public Color currentColor;
    private void InitializeSingleton()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(this);
	}



    public void Awake()
	{
		InitializeSingleton();
	}


}
