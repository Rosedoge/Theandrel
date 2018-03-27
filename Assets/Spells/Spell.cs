using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public interface Spell {

    int manaCost
    {
        get;
        set;
    }

    float castTime
    {
        get;
        set;
    }
    Sprite mySprite
    {
        get;
        set;
    }
    void Cast();
}
