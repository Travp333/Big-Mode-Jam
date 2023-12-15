//adapted and modified from video "How to make a HEALTH BAR in Unity!" by Brackeys
//https://www.youtube.com/watch?v=BLfNP4Sc_iA
//Author: Sandeep and Travis
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script will just keep track of the player's various stats and allow other scripts to access and edit them
public class PlayerStats : MonoBehaviour, IDataPersistence
{
    public float hp = 100;
	//public HealthBar healthBar;

    public void restoreHP(float healAmount){
        if (hp + healAmount > 100){
            hp = 100;
        }
        else{
            hp += healAmount;
        }
    }

    public void takeDamage(float damage){
        if (hp - damage < 0){
            //Debug.Log("Went from "+hp+" to 0");
            hp = 0;

        }
        else {

            //Debug.Log("Went from "+hp+" to "+ Mathf.Round(hp-damage));
            if(hp != Mathf.Round(hp-damage)){
                hp = Mathf.Round(hp-damage);
            }
        }
    }
    public void LoadData(GameData data) {
        this.hp = data.playerHp;
        //this.transform.position = data.playerPos;
    }
    public void SaveData(ref GameData data) {
        data.playerHp = this.hp;
        //data.playerPos = this.transform.position;


    }
    void Start()
    {
        //Test line to see if we can set a default start point
        //when game is started, it sets the slider max value to hp value
	    //healthBar.SetMaxHealth(hp);
    }

    void Update()
    {
        //updates the slider value to match the current hp value
	    //healthBar.SetHealth(hp);
    }

}
