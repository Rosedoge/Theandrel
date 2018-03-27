using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lightning : MonoBehaviour, Spell{
    int level;
    int damage;
    public Material lightningmat;
    public Sprite mSprite;

    Sprite Spell.mySprite
    {
        get
        {
            return mSprite;
        }

        set
        {
            mSprite = value;
        }
    }

    public int manaCost
    {
        get { return 5; }
        set { manaCost = value; }
    }


    public float castTime
    {
        get { return 3; }
        set { castTime = value; }
    }

    public Lightning(int level)
    {
        this.level = level;

        damage = 5 * level;
        this.mSprite = Resources.Load<Sprite>("Lightning") as Sprite;
    }

	public void Cast()
    {
        
        if (!PlayerScript.instance.GetComponent<LineRenderer>())
        {
            PlayerScript.instance.gameObject.AddComponent<LineRenderer>();
        }
        PlayerScript.instance.GetComponent<LineRenderer>().startWidth = 0.25f;
        PlayerScript.instance.GetComponent<LineRenderer>().endWidth = 0.25f;
        PlayerScript.instance.GetComponent<LineRenderer>().SetPosition(0, PlayerScript.instance.lightningSpawn.position);
        PlayerScript.instance.GetComponent<LineRenderer>().material = lightningmat;
        PlayerScript.instance.GetComponent<LineRenderer>().SetPosition(1, FindClosestEnemy());
        Destroy(PlayerScript.instance.GetComponent<LineRenderer>(), 0.25f);
        
    }


/// <summary>
/// finds the closest enemy
/// </summary>
/// <returns></returns>
    Vector3 FindClosestEnemy()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        //obligatory high value
        float dist = 100f;
        foreach (GameObject enemy in enemies)
        {
           // Debug.Log("Distance to " + enemy.name + " is " + Vector3.Distance(enemy.transform.position, PlayerScript.instance.transform.position));
            if (Vector3.Distance(enemy.transform.position, PlayerScript.instance.transform.position)<dist)
            {
                dist = Vector3.Distance(enemy.transform.position, PlayerScript.instance.transform.position);
                closest = enemy;
            }
        }
        if(enemies.Length < 1)
        {
            return PlayerScript.instance.transform.position;
        }
        closest.GetComponent<IEnemy>().Damage(damage);
        return closest.transform.GetChild(0).transform.position;
    }
}
