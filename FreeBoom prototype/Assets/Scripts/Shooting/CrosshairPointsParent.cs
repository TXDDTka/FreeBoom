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
    public int distance = 0;
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
