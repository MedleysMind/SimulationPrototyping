using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalAI : MonoBehaviour {
    // This is the current AI agent
    UnityEngine.AI.NavMeshAgent agent;
    // Controls animals health, hunger, thirst, social, comfort and happiness values
    public float health = 100f, hunger = 100f, thirst = 100f, social = 100f, comfort = 100f, happiness = 100f;
    // Is used to determine how quickly thirst and hunger values are depleted
    private float energyUsageMultiplier = 1f;
    // Location where food can be found
    private Vector3 feedingSpot;
    // Location where water can be found
    private Vector3 drinkingSpot;
    // Once health = 0, trigger bool as true
    public bool isDead = false;
    // Dictates diet of the animal
    public bool Herbivore = false;
    public bool Carnivore = false;
    // Dictates size of the animal
    public bool small = false;
    public bool medium = false;
    public bool large = false;

    // Only accepts clicks on ground ---- FOR DEBUGGING ONLY
    public LayerMask clickMask;
    void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        // Sets the spot for hunger to be added ---- FOR DEBUGGING ONLY
        feedingSpot = new Vector3 (0, 1 / 2, 0);
        // Sets the Hunger Controller to run every 2 seconds starting 5 seconds after creation
        InvokeRepeating ("HungerController", 5.0f, 2.0f);
        // Sets the Thirst Controller to run every 2 seconds starting 5 seconds after creation
        InvokeRepeating ("ThirstController", 5.0f, 2.0f);
        // Sets the Social Controller to run every 2 seconds starting 5 seconds after creation
        InvokeRepeating ("SocialController", 10.0f, 5.0f);
        // Sets the Comfort Controller to run every 2 seconds starting 5 seconds after creation
        InvokeRepeating ("ComfortController", 10.0f, 5.0f);
        // Sets the Happiness Controller to run every 2 seconds starting 5 seconds after creation
        InvokeRepeating ("HappinessController", 10.0f, 5.0f);
    }

    void Update () {
        // ExplosionDamage(agent.transform.position, 5, clickMask);
        HealthController ();

        if (agent.hasPath == true) {
            energyUsageMultiplier = 2;
        }
        if (agent.hasPath == false) {
            energyUsageMultiplier = 1;
        }
        // Wander();
        // Creates a path based off of mouse click location ---- FOR DEBUGGING ONLY
        // if (Input.GetMouseButtonDown (0)) {
        //     RaycastHit hit;
        //     if (!EventSystem.current.IsPointerOverGameObject ()) {
        //         if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 1000, clickMask)) {
        // agent.destination = hit.point;
        //         }
        //     }
        // }
    }
    // void ExplosionDamage(Vector3 center, float radius, LayerMask clickMask)
    //     {
    //         Collider[] hitColliders = Physics.OverlapSphere(center, radius);
    //         int i = 0;
    //         if (i < hitColliders.Length)
    //         {
    //             Debug.Log("Collision in sphere");
    //             // hitColliders[i].SendMessage("AddDamage");
    //             i++;
    //         }
    //     }

    // Hunger slowly goes down over time and can go down quicker the more the animal moves. Once low enough they will search for food.
    void HungerController () {
        // every two seconds hunger goes down by a base times a movement multiplier
        hunger -= 1f * energyUsageMultiplier;
        if (hunger > 80) {
            health++;
        }
        if (hunger == 100) {
            health += 5;
        }
        // If hunger reaches half, start looking for food
        if (hunger <= 50f) {
            happiness--;
            comfort--;

        }
        FindFood();
        AnimalMovement(this.transform.position);
        // If hunger is too low, health begins to go down
        if (hunger <= 25) {
            happiness--;
            comfort--;
            health--;
        }
    }
    // Thirst slowly goes down over time and can go down quicker the more the animal moves. Once low enough they will search for a water source.

    void ThirstController () {
        // every two seconds hunger goes down by a base times a movement multiplier
        thirst -= 1f * energyUsageMultiplier;
        // If hunger reaches half, start looking for food
        if (thirst > 80) {
            health++;
        }
        if (thirst == 100) {
            health += 5;
        }
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
        // Debug.Log ("Thirst: " + thirst);
        //    GameplayUI.AnimalPanel.GetComponent<AnimalInfo>().thirst = thirst;

    }
    // Social slowly goes down over time and can go down quicker the more the animal moves. Once low enough they will search for other animals of the same species.

    void SocialController () {
        social -= 1f;
        if (social <= 50) {
            happiness--;
            comfort--;
        }
        //    GameplayUI.AnimalPanel.GetComponent<AnimalInfo>().social = social;

    }
    // Comfort is determined by their environment, if there are hostile animals or if temperature is too high or too low, or if there are too many or not enough trees, comfort will go down.
    void ComfortController () {
        if (comfort <= 50) {
            happiness--;
        }
        //    GameplayUI.AnimalPanel.GetComponent<AnimalInfo>().comfort = comfort;

    }
    // Happiness is effected by all other needs excluding health. If all other needs are met the animals happiness will slowly go up.
    void HappinessController () {
        //    GameplayUI.AnimalPanel.GetComponent<AnimalInfo>().happiness = happiness;

    }
    void HealthController () {
        if (health >= 100) {
            health = 100;
        }
        if (health == 0) {
            isDead = true;
            agent.enabled = !agent.enabled;
            health = 0;
            hunger = 0;
            thirst = 0;
            social = 0;
            comfort = 0;
            happiness = 0;
            // Destroy (this.gameObject);
            // DestroyImmediate (this, true);
        }
        //    GameplayUI.AnimalPanel.GetComponent<AnimalInfo>().health = health;
    }

    // void Wander(){
    //     Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
    //      randomDirection += transform.position;
    //  UnityEngine.AI.NavMeshHit hit;
    //  UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
    //  Vector3 finalPosition = hit.position;
    // }
    // This is responsible for updating the Animal Info Panel

    public void AnimalInfoPanelUpdater () {
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().health = health;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().hunger = hunger;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().thirst = thirst;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().social = social;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().comfort = comfort;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().happiness = happiness;
    }
    public void AnimalInfoPanelResetter () {
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().health = 0;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().hunger = 0;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().thirst = 0;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().social = 0;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().comfort = 0;
        GameplayUI.AnimalPanel.GetComponent<AnimalInfo> ().happiness = 0;
    }

    public void AnimalMovement (Vector3 targetPosition) {
        if (isDead == false) {
            agent.destination = targetPosition;
        }
    }
    public void FindFood () {
        if (Herbivore == true) {
            // Search for plants of same or smaller size
            if(small == true){}
            if(medium == true){}
            if(large == true){}
        }
        if(Carnivore == true){
            //Search for animals
            if(small == true){}
            if(medium == true){}
            if(large == true){}
        }
    }
}