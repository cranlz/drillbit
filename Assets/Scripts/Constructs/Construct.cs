using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This will be the basis of all constructable objects
//Only put code in here that applies to all constructs
public class Construct : MonoBehaviour
{

    //integer value of a construct's health
    public int hp;
    //Mark true if Destroy() has been called
    public bool markedForDeletion = false;

    //Deal some damage to the construct
    //Can heal by using negative ints
    public void Damage(int dmg) {
        hp -= dmg;
        //If damage causes the construct's health to be
        // negative, destroy it
        if (hp <= 0 && !markedForDeletion)
        {
            markedForDeletion = true;
            Destroy(gameObject);
        }
    }

    //Sets the current HP to a specific value
    public void SetHP(int newHP) {
        hp = newHP;
    }

    //It might be worth handing object pooling here for performance later
}
