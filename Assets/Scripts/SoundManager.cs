using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{
    //Interface objects
    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text soundVolumeText;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private Slider inGameVolumeSlider;
    [SerializeField] private Image soundImage;
    [SerializeField] private Sprite unmuteSprite;
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private GameObject inGameVolumePanel;

    //variables for corrects volume change and display
    private int volume;
    private bool gameIsOn;

    //Audiosources
    public AudioClip[] sounds;
    public AudioSource music;
    public AudioSource soundEffect;
    public AudioSource clickSound;
    
    public void PlayMusic(int index) //For changing main theme
    {
        music.clip = sounds[index];
        music.Play();
    }

    public void PlaySoundEffect(int index) //For playing sound effects
    {
        soundEffect.clip = sounds[index];
        soundEffect.Play();
    }

    public void ClickSound() //For playing click sound on button press
    {
        clickSound.clip = sounds[10];
        clickSound.Play();
    }

    public void StopSound()//Stopping sound effects
    {
        soundEffect.Stop();
    }

    void Start()
    {
        PlayMusic(0);
        SetGameIsOn(false);
    }

    void Update() //To change volume by sliders
    {
        SetVolume(musicVolumeSlider, music, musicVolumeText);
        SetVolume(soundVolumeSlider, soundEffect, soundVolumeText);
        clickSound.volume = soundEffect.volume;//click sound is same volume level as sound effects
    }

    private void SetVolume(Slider slider, AudioSource source, TMP_Text text)
    {
        if (gameIsOn) // during the game the volume set in main menu is 1, in game volume slider is changing it from set volume to 0
        {
            source.volume = slider.value * inGameVolumeSlider.value;

            if (inGameVolumeSlider.value == 0) //Changing mute/unmute symbol
            {
                ChangePicture(muteSprite);
            }
            else
            {
                ChangePicture(unmuteSprite);
            }
        }
        else //in main menu there's a possibility to change main theme and sound volumes separately
        {
            source.volume = slider.value;
        }
        volume = (int)(slider.value * 10); //displaying volume level on screen
        text.text = volume.ToString();
    }

    private void ChangePicture(Sprite sprite)//For changing volume status
    {
        soundImage.sprite = sprite;
    }

    public void ResetSound()//Setting music to default (main theme, no effects, game is not started)
    {
        PlayMusic(0);
        soundEffect.Stop();
        SetGameIsOn(false);
    }

    //setter for correct volume change
    public void SetGameIsOn(bool isOn)
    {
        gameIsOn = isOn;
    }

    public void OpenSlider()//to open/close in game volume slider
    {
        inGameVolumePanel.SetActive(!(inGameVolumePanel.active));
    }
}
