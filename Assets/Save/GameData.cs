using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public float playerHp;
    public Vector3 playerPos;
    
    public GameData() {
        this.playerHp = 100f;
        this.playerPos = Vector3.zero;
    }
    
    
}
