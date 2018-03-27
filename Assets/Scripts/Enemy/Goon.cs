using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goon : MonoBehaviour, IEnemy {

    int health;
    public int Health
    {
        get { return health; }
        set { health = value; }


    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Damage(int dmg)
    {

    }
}
