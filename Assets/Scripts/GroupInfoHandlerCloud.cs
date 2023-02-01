using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class GroupInfoHandlerCloud : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text nombreTxtField;
    [SerializeField] private SpriteRenderer logoImage;

    private void Start() {

    }

    public void SetInfoGroup(string groupName,string imageLogoGroup){
        nombreTxtField.text = groupName;
        Texture2D imageSearch = SearchImageInAssets("logosGIs", imageLogoGroup);
        Debug.Log(imageSearch);
        if(imageSearch is not null){
            Sprite spriteLogo = Sprite.Create(imageSearch, new Rect(0, 0, imageSearch.width, imageSearch.height), new Vector2(0.5f,0.5f));
            logoImage.sprite = spriteLogo;
        }
        //BuscarLogo dentro de los assets  con el nombre en str imageLogoGroup para cambiar el sprite
        //logoImage.sprite = imageLogoGroup;
    }

    public static Texture2D SearchImageInAssets(string directoryPath, string imageRefName){
       Texture2D imgRslt = Resources.Load(string.Format("{0}/{1}",directoryPath,imageRefName)) as Texture2D;
       return imgRslt;
    }

    /*
    public static Object[] SearchImageInAssets(string imageRefName){
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
    public static Object[] SearchImageInAssets(string imageRefName, string path){
        string[] results = AssetDatabase.FindAssets(imageRefName, new[] {path});
        Object[] imagenObj = new Object[1];

        foreach (string result in results)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(result));
            string imagePath = AssetDatabase.GUIDToAssetPath(result);
            imagenObj[0] = AssetDatabase.LoadAssetAtPath(imagePath, typeof(Object));
        }
        return imagenObj;
    }
    */

    /*
    public void ExecuteUrl(){
        Application.OpenURL(groupInfo.webpageGI);
    }
    */
}
