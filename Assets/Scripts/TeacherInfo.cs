using UnityEngine;

[CreateAssetMenu(fileName = "TeacherInfo", menuName = "ScriptableObjects/TeacherInfo", order = 0)]
public class TeacherInfo : ScriptableObject
{
    [Header("Información Profesor")]
    public string nombre;
    public string apellidos;
    public string cargo;
    public string email;
    public string URLprofile;
    public Sprite profilePic;
}
