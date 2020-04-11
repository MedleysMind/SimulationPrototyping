using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalAI : MonoBehaviour {
    // Controls animals health value
    private float health = 100f;
    // Controls animals hunger value
    private float hunger = 100f;
    // Controls animals thirst value
    private float thirst = 100f;
    // Controls animals social value
    private float social = 100f;
    // Controls animals comfort value
    private float comfort = 100f;
    // Controls animals happiness value
    private float happiness = 100f;
    // Is used to determine how quickly thirst and hunger values are depleted
    private float energyUsageMultiplier = 1f;
    // Location where food can be found
    private Vector3 feedingSpot;
    // Location where water can be found
    private Vector3 drinkingSpot;
    // This is the current AI agent
    UnityEngine.AI.NavMeshAgent agent;
    // Only accepts clicks on ground ---- FOR DEBUGGING ONLY
    public LayerMask clickMask;

    void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        // Sets the spot for hunger to be added ---- FOR DEBUGGING ONLY
        feedingSpot = new Vector3 (0, 0, 0);
        // Sets the Hunger Controller to run every 2 seconds starting 5 seconds after creation
        InvokeRepeating ("HungerController", 5.0f, 2.0f);
        // Sets the Thirst Controller to run every 2 seconds starting 5 seconds after creation
        InvokeRepeating ("ThirstController", 5.0f, 2.0f);
        // Sets the Socail Controller to run every 2 seconds starting 5 seconds after creation
        InvokeRepeating ("SocialController", 10.0f, 5.0f);
    }

    void Update () {
        HealthController();
        if (agent.hasPath == true) {
            energyUsageMultiplier = 2;
        }
        if (agent.hasPath == false) {
            energyUsageMultiplier = 1;
        }
        // Creates a path based off of mouse click location ---- FOR DEBUGGING ONLY
        if (Input.GetMouseButtonDown (0)) {
            RaycastHit hit;
            if (!EventSystem.current.IsPointerOverGameObject ()) {
                if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 1000, clickMask)) {
                    agent.destination = hit.point;
                }
            }
        }
    }

    void HungerController () {
        // every two seconds hunger goes down by a base times a movement multiplier
        hunger -= 1f * energyUsageMultiplier;
        // If hunger reaches half, start looking for food
        if (hunger <= 50f) {
            happiness--;
            comfort--;


            agent.destination = feedingSpot;
        }
        // At location of food source, add to hunger value
        if (agent.destination == feedingSpot) {
            hunger += 5f;
        }
        // If hunger is too low, health begins to go down
        if (hunger <= 25) {
            happiness--;
            comfort--;
            health--;
        }
        Debug.Log ("Hunger: "+hunger);
    }
    void ThirstController () {
        // every two seconds hunger goes down by a base times a movement multiplier
        thirst -= 1f * energyUsageMultiplier;
        // If hunger reaches half, start looking for food
        if (thirst <= 50f) {
            happiness--;
            comfort--;

            agent.destination = feedingSpot;
        }
        // At location of food source, add to hunger value
        if (agent.destination == feedingSpot) {
            thirst += 5f;
        }
        // If hunger is too low, health begins to go down
        if (thirst <= 25) {
            happiness--;
            comfort--;
            health--;
        }
        Debug.Log ("Thirst: "+thirst);
    }
    // SocialController is related to how much an animal interacts with other animals of the same type
    void SocialController () {
        social -= 1f;
        if(social <= 50){
            happiness--;
            comfort--;
        }
    }
    void ComfortController () {
        if(comfort <= 50){
            happiness--;
        }
    }
    void HappinessController () {
    }
    void HealthController(){
        if(health == 0){
            DestroyImmediate (this.gameObject, true);
        }
    }
}