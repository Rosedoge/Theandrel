using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour {

    //Internal variables
    Animator myAnim;
    int level = 1;
    //set in case of emergency
    int mana = 25;

    private TextAsset asset; // Gets assigned through code. Reads the file.
    private StreamWriter writer; // This is the writer that writes to the file
    private StreamReader reader;
    [SerializeField] Image Lspell, Rspell, Cspell;
    public Transform lightningSpawn;
    public bool inControl = true, casting = false;
    public static PlayerScript instance = null;

    //spell cast variables
    float casttime;

    //list of spells
    List<Spell> mySpells;
    int spellCounter = 0;
    //spell booleans

    bool lightning = true;
    bool chainLightning = true;
    bool ThunderClap = false;
    /// <summary>
    /// Ideally this will be used in order to set up the spells that the player has and adds them to the inventory. 
    /// Adds the prefab to the player.
    /// </summary>
    public void SetSpells()
    {
        mySpells = new List<Spell>();
        Lightning light = new Lightning(level);
        mySpells.Add(light);
        //if (chainLightning)
        //{
        //    ChainLightning cLight = new ChainLightning(level);
        //    mySpells.Add(cLight);
        //}

        try
        {
            reader = new StreamReader("Assets/Resources/spells.txt");
            bool temp = bool.Parse(reader.ReadLine());
            //should be chain lightning
            if (temp)
            {
                ChainLightning cLight = new ChainLightning(level);
                mySpells.Add(cLight);
            }

            //ToDo add the rest of the spells

            reader.Close();

        }
        catch
        {
            Debug.Log("Error");
        }




        UpdateUI();
    }

    /// <summary>
    /// Ideally called at the start of each level. Should rewrite previous things.
    /// </summary>
    public void SetCharacter()
    {

        mana = 25 * level;
    }
    private void Awake()
    {
        SetCharacter();
        SetSpells();
        if(instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InvokeRepeating("RegenerateMana", 1f, 2f);
        myAnim = GetComponent<Animator>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (inControl)
        {
            MyInput();
            
        }
	}

    void RegenerateMana()
    {
        if (mana + 5 < 25 * level)
        {
            mana += 5;
        }
        else
        {
            mana = 25 * level;
        }
    }
    void MyInput()
    {
        //rotation
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * 70 * Time.deltaTime);
        //Forward and backward
        myAnim.SetFloat("Speed", Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Fire1"))
        {
            CastSpell();
        }

        if (Input.GetButtonDown("SwapSpellsL"))
        {
            //Debug.Log("Spellcounter = " + spellCounter);
            if (spellCounter <= 0)
            {
                spellCounter = mySpells.Count-1;
            }
            else
                spellCounter -= 1;
            UpdateUI();
        }
        if (Input.GetButtonDown("SwapSpellsR"))
        {
            if (spellCounter >= mySpells.Count-1)
            {
                spellCounter = 0;
            }
            else
                spellCounter += 1;
            UpdateUI();
        }
    }

    void UpdateUI()
    {

        Cspell.sprite = mySpells[spellCounter].mySprite;
        //ideally this should loop around.
        if (spellCounter-1 < 0)
        {
            
            Lspell.sprite = mySpells[mySpells.Count-1].mySprite;
        }
        else
        {
            Lspell.sprite = mySpells[spellCounter - 1].mySprite;
        }

        if (spellCounter + 1 >= mySpells.Count)
        {
            Rspell.sprite = mySpells[0].mySprite;
        }
        else
        {
            Rspell.sprite = mySpells[spellCounter + 1].mySprite;
        }
    }
    void CastSpell()
    {
        //if we have more mana than is needed
        if(mana >= mySpells[spellCounter].manaCost)
        {
            if (mySpells[spellCounter].castTime > 0)
            {
                myAnim.SetTrigger("SpellLightning");
            }
            Invoke("ActualCast", mySpells[spellCounter].castTime);
            casting = true;
            mana -= mySpells[spellCounter].manaCost;
        }

    }

    void ActualCast()
    {
        //myAnim.SetBool("CastTime", false);
        //myAnim.SetTrigger("Lightning");
        casting = false;
        
    }

    public void Superactualcast()
    {
        mySpells[spellCounter].Cast();
    }
}
