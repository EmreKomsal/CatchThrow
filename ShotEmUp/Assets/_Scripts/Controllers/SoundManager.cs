using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    
    public enum EnemySoundTypes {EnemyDieSound, EnemyArrowThrowingSound };
    public enum PlayerSoundTypes {PlayerArrowThrowingSound, PlayerArrowCatchSound, PlayerDieSound };
    public enum GeneralSoundTypes {BoxBreakingSound};
    //public enum BgMusicTypes {MainBgMusic};
    
    public AudioSource enemyDieSound;
    public AudioSource enemyArrowThrowingSound;
   
    public AudioSource playerArrowThrowingSound;
    //public AudioSource playerArrowCatchSound;
    public AudioSource playerDieSound;
    public AudioSource boxBreakingSound;
    
    public AudioSource lostSound;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
   /*
    public void PlayBgMusic(BgMusicTypes currentMusic)
    {
        switch (currentMusic)
        {
            case BgMusicTypes.MainBgMusic:
                bgMusic.Play();
                bgMusic.volume = 0.1f;
                break;
        }
    }
    */

    public void PlayPlayerSound(PlayerSoundTypes currentSound)
    {
        switch (currentSound)
        {
            case PlayerSoundTypes.PlayerArrowThrowingSound:
                playerArrowThrowingSound.Play();
                break;
            /*
            case PlayerSoundTypes.PlayerArrowCatchSound:
                playerArrowCatchSound.Play();
                break;
            */
            case PlayerSoundTypes.PlayerDieSound:
                playerDieSound.Play();
                break;
        }
    }
    
    public void PlayEnemySound(EnemySoundTypes currentSound)
    {
        switch (currentSound)
        {
            case EnemySoundTypes.EnemyDieSound:
                enemyDieSound.Play();
                break;
            case EnemySoundTypes.EnemyArrowThrowingSound:
                enemyArrowThrowingSound.Play();
                break;
        }
    }
    
    public void PlayGeneralSound(GeneralSoundTypes currentSound)
    {
        switch (currentSound)
        {
            case GeneralSoundTypes.BoxBreakingSound:
                boxBreakingSound.Play();
                break;
        }
    }
    
    /*
      
      HOW TO USE;
      - SHOULD MAKE A SOUNDMANAGER EMPTY GAME OBJECT
      - MAKE CHILD SOUND OBJECTS
      - ATTACHING TO THE INSPECTOR
      
      USAGE;
    **** GENERAL SOUNDS ****
     SoundManager.Instance.PlayGeneralSound(SoundManager.SoundTypes.WinSound);
     
     **** PLAYER SOUNDS ****
     SoundManager.Instance.PlayPlayerSound(SoundManager.SoundTypes.PlayerArrowThrowingSound);
     
     **** ENEMY SOUNDS ****
     SoundManager.Instance.PlayEnemySound(SoundManager.SoundTypes.EnemyDieSound);
     
     */
}
