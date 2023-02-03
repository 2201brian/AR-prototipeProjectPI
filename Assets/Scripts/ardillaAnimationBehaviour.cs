using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ardillaAnimationBehaviour : MonoBehaviour
{
    private Animator anim;
    private bool isGreeting = false;
    private AudioSource audioSourceCmp;
    private AudioClip audioClip;

    //catch time of audio clips for presentation of a group
    private float timer = 0;
    //id grupo detectado
    private int idGrupo;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        //audioSourceCmp = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("greet",isGreeting);
    }

    public void setBehaviour(int idGrupoMeta){
        anim = GetComponent<Animator>();
        audioSourceCmp = GetComponent<AudioSource>();
        idGrupo = idGrupoMeta;
    }

    public void doGreetAnim()
    {   
        float lengthAudioClip;
        isGreeting = true;
        audioClip = searchAudioClip("bienvenida","escuela");
        audioSourceCmp.clip = audioClip;
        audioSourceCmp.PlayDelayed(2);
        lengthAudioClip = audioSourceCmp.clip.length;
        timer += lengthAudioClip;
        Invoke("stopGreetAnim",lengthAudioClip);
        Invoke("explanationUI",lengthAudioClip + 3f);
        //Invoke("firstGroupWelcome",timer);
    }

    private void stopGreetAnim()
    {
        isGreeting = false;
    }

    void doTalkingAnim(string funcionAudio, string nombreGrupo)
    {
        anim.SetBool("talking",true);
        audioClip = searchAudioClip(funcionAudio,nombreGrupo);
        audioSourceCmp.clip = audioClip;
        audioSourceCmp.PlayDelayed(0);
        float lengthAudioClip = audioSourceCmp.clip.length;
        if(nombreGrupo == "interfaz")
        {
            Invoke("firstGroupWelcome",lengthAudioClip + 2f);
        }
        Invoke("stopTalkingAnim",lengthAudioClip);
    }

    private void stopTalkingAnim()
    {
        anim.SetBool("talking",false);
    }

    //explicacion interfaz
    private void explanationUI()
    {
        doTalkingAnim("explicacion","interfaz");
    }

    //presentacion primer grupo en detectar (Consider Refactoring this)
    private void firstGroupWelcome()
    {
        tellSpeech("bienvenida",idGrupo);
    }

    //lineas de investigacion grupo, para boton
    public void tellSpeech(string speechType,int idGrupo){
        string grupo = "";
        switch(idGrupo)
        {
            case 1:
                grupo = "camaleon";
                break;
            case 2:
                grupo = "avispa";
                break;
        }
        doTalkingAnim(speechType,grupo);
    }

    private AudioClip searchAudioClip(string funcionAudio, string nombreGrupo)
    {
        return Resources.Load(string.Format("audioSources/{0}-{1}",funcionAudio, nombreGrupo)) as AudioClip;
    }
}
