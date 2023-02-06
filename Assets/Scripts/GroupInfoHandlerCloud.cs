using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GroupInfoHandlerCloud : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text nombreTxtField;
    [SerializeField] private SpriteRenderer logoImage;

    private void Start() {

    }

    //Sets the logo sprite and the group name to the components of the prefab
    public void SetInfoGroup(string groupName,string imageLogoGroup){
        nombreTxtField.text = groupName;
        Texture2D imageSearch = SearchImageInAssets("logosGIs", imageLogoGroup);
        if(imageSearch is not null){
            Sprite spriteLogo = Sprite.Create(imageSearch, new Rect(0, 0, imageSearch.width, imageSearch.height), new Vector2(0.5f,0.5f));
            logoImage.sprite = spriteLogo;
        }
    }

    //Searche for a Texture2D in the Resources directory of the project
    public static Texture2D SearchImageInAssets(string directoryPath, string imageRefName){
       Texture2D imgRslt = Resources.Load(string.Format("{0}/{1}",directoryPath,imageRefName)) as Texture2D;
       return imgRslt;
    }
}
