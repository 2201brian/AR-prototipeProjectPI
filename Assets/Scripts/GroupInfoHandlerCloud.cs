using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;


public class GroupInfoHandlerCloud : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text nombreTxtField;
    [SerializeField] private SpriteRenderer logoImage;

    private void Start() {

    }

    public void SetInfoGroup(string groupName,string imageLogoGroup){
        nombreTxtField.text = groupName;
        Object[] imageSearchRslts = SearchImageInAssets(imageLogoGroup);
        Object imageSearchRslt = imageSearchRslts[0];
        if(imageSearchRslt is not null){
            Texture2D tex = imageSearchRslt as Texture2D;
            Sprite spriteLogo = Sprite.Create(imageSearchRslt as Texture2D, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f));
            logoImage.sprite = spriteLogo;
        }
        //BuscarLogo dentro de los assets  con el nombre en str imageLogoGroup para cambiar el sprite
        //logoImage.sprite = imageLogoGroup;
    }

    public Object[] SearchImageInAssets(string imageRefName){
        string[] results = AssetDatabase.FindAssets(imageRefName, new[] {"Assets/Arte/Sprites/logosGIs"});
        Object[] imagenObj = new Object[1];

        foreach (string result in results)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(result));
            string imagePath = AssetDatabase.GUIDToAssetPath(result);
            imagenObj[0] = AssetDatabase.LoadAssetAtPath(imagePath, typeof(Object));
        }
        return imagenObj;
    }

    /*
    public void ExecuteUrl(){
        Application.OpenURL(groupInfo.webpageGI);
    }
    */
}
