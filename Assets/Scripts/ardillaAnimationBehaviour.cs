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
        anim.SetBool("talking",isTalking);
    }

    public void setBehaviour(){
        anim = GetComponent<Animator>();
        audioSourceCmp = GetComponent<AudioSource>();
    }

    public void doGreetAnim()
    {   
        
        isGreeting = true;
        string audioName = "bienvenida-escuela";
        audioClip = Resources.Load(string.Format("audioSources/{0}",audioName)) as AudioClip;
        audioSourceCmp.clip = audioClip;
        audioSourceCmp.PlayDelayed(2);
        Invoke("stopGreetAnim",audioSourceCmp.clip.length);

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
        isTalking = true;
        audioClip = Resources.Load(string.Format("audioSources/{0}-{1}",funcionAudio,nombreGrupo)) as AudioClip;
        audioSourceCmp.clip = audioClip;
        audioSourceCmp.PlayDelayed(0);
        //Invoke("stopTalkingAnim",audioSourceCmp.clip.length);
    }

    private void stopTalkingAnim()
    {
        isTalking = false;
    }
}
