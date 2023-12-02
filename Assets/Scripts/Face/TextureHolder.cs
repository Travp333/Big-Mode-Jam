using UnityEngine;
[CreateAssetMenu]
public class TextureHolder : ScriptableObject {

	[SerializeField]
	Texture[] tex;
    public Texture getTex(int texID){
        return tex[texID];
    }

}