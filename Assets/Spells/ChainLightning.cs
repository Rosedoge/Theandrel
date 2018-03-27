using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChainLightning : MonoBehaviour, Spell {


    int level;
    int damage;
    public Material lightningmat;
    public Sprite mSprite;

    List<GameObject> enemies;
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
        get { return 10; }
        set { manaCost = value; }
    }

    public float castTime
    {
        get { return 0; }
        set { castTime = value; }
    }

    public ChainLightning(int level)
    {
        enemies = new List<GameObject>();
        this.level = level;

        damage = 3 * level;
        this.mSprite = Resources.Load<Sprite>("ChainLightning") as Sprite;
    }


    /// <summary>
    /// Needs to link to (level) targets and deal damage to them.
    /// </summary>
    public void Cast()
    {
        Debug.Log("ChainLightning");
        for(int i = 0; i < level; i++)
        {
            if (enemies.Count == 0 && i == 0)
            {
                //start with the player because it's always the first link.
                enemies.Add(FindClosestEnemyChain(PlayerScript.instance.gameObject));

            }
            else
            {
                enemies.Add(FindClosestEnemyChain(enemies[enemies.Count-1]));
            }
        }

        int count = 0;

        //maybe hard set the linerenderer from the player to the first enemy

        if (!PlayerScript.instance.GetComponent<LineRenderer>())
        {
            PlayerScript.instance.gameObject.AddComponent<LineRenderer>();
        }
        PlayerScript.instance.GetComponent<LineRenderer>().startWidth = 0.25f;
        PlayerScript.instance.GetComponent<LineRenderer>().endWidth = 0.25f;
        PlayerScript.instance.GetComponent<LineRenderer>().SetPosition(0, PlayerScript.instance.lightningSpawn.position);
        PlayerScript.instance.GetComponent<LineRenderer>().material = lightningmat;
        PlayerScript.instance.GetComponent<LineRenderer>().SetPosition(1, enemies[0].transform.GetChild(0).position);
        Destroy(PlayerScript.instance.GetComponent<LineRenderer>(), 0.25f);

        //CreateLineRenderers that connect each object.
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.GetComponent<LineRenderer>())
            {
                enemy.AddComponent<LineRenderer>();
            }
            enemy.GetComponent<LineRenderer>().startWidth = 0.25f;
            enemy.GetComponent<LineRenderer>().endWidth = 0.25f;
            enemy.GetComponent<LineRenderer>().SetPosition(0, enemy.transform.GetChild(0).position);
            enemy.GetComponent<LineRenderer>().material = lightningmat;
            if(count+1 < enemies.Count)
            {
                enemy.GetComponent<LineRenderer>().SetPosition(1, enemies[count + 1].transform.GetChild(0).position);
            }
            
            Destroy(enemy.GetComponent<LineRenderer>(), 0.25f);
            enemy.GetComponent<IEnemy>().Damage(damage - 3 * count);
            count += 1;
        }
        enemies.Clear();
    }

    GameObject FindClosestEnemyChain(GameObject start)
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        //obligatory high value
        float dist = 10;
        foreach (GameObject enemy in enemies)
        {
           
            if (Vector3.Distance(enemy.transform.position, start.transform.position) < dist)
            {
                //not the same enemy we started with
                if (enemy != start)
                {
                    dist = Vector3.Distance(enemy.transform.position, start.transform.position);
                    closest = enemy;
                }
            }
        }
        if (enemies.Length < 1)
        {
            return start.gameObject;
        }
        return closest;
    }
}
