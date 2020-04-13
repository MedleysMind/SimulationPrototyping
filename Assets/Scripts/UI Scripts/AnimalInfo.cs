using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimalInfo : MonoBehaviour
{
 public Slider HealthSlider;
 public float health;
 public Slider HungerSlider;
 public float hunger;
 public Slider ThirstSlider;
 public float thirst;
 public Slider SocialSlider;
 public float social;
 public Slider ComfortSlider;
 public float comfort;
 public Slider HappinessSlider;
 public float happiness;
 
 void Start(){
 }
 public void Update(){
     HealthSlider.value = health;
    //  HungerSlider.value = hunger;
     ThirstSlider.value = thirst;
     SocialSlider.value = social;
     ComfortSlider.value = comfort;
     HappinessSlider.value = happiness;
    //  Debug.Log(hunger);
 }
 public void SetHunger(float hunger){
     HungerSlider.value = hunger;
 }
}
