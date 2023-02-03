using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ardillaAnimationBehaviour : MonoBehaviour
{
    private Animator anim;
    private bool isGreeting = false;
    private bool isTalking = false;
    private AudioSource audioSourceCmp;
    private AudioClip audioClip;

    //private int idGrupo;

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

    public void setBehaviour(){
        anim = GetComponent<Animator>();
        audioSourceCmp = GetComponent<AudioSource>();
        //idGrupo = idGrupoMeta;
    }

    public void doGreetAnim()
    {   

        isGreeting = true;
        audioClip = searchAudioClip("bienvenida","escuela");
        audioSourceCmp.clip = audioClip;
        audioSourceCmp.PlayDelayed(2);
        Invoke("stopGreetAnim",audioSourceCmp.clip.length);
        Invoke("explanationUI",audioSourceCmp.clip.length + 3f);
        
        //explicacion interfaz
        //doTalkingAnim("explicacion","interfaz");
        //Debug.Log(isGreeting);
    }

    private void stopGreetAnim()
    {
        isGreeting = false;
        Debug.Log(isGreeting);
    }

    void doTalkingAnim(string funcionAudio, string nombreGrupo)
    {
        anim.SetBool("talking",true);
        audioClip = searchAudioClip(funcionAudio,nombreGrupo);
        audioSourceCmp.clip = audioClip;
        audioSourceCmp.PlayDelayed(0);
        Invoke("stopTalkingAnim",audioSourceCmp.clip.length);
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


    //lineas de investigacion grupo, para boton
    public void tellLineasInvestigacion(int idGrupo){
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
        doTalkingAnim("lineas",grupo);
    }

    private AudioClip searchAudioClip(string funcionAudio, string nombreGrupo)
    {
        return Resources.Load(string.Format("audioSources/{0}-{1}",funcionAudio, nombreGrupo)) as AudioClip;
    }
}
