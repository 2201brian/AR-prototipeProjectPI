using UnityEngine;

[CreateAssetMenu(fileName = "GroupInfo", menuName = "ScriptableObjects/GroupInfo", order = 0)]
public class GroupInfo : ScriptableObject
{
    [Header("Información Grupo")]
    public string nameGI;
    public string webpageGI;
    public Sprite logoGI;
}
