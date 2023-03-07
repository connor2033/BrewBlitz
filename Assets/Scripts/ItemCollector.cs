using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int beanCount = 0;
    private int waterCount = 0;
    private int milkCount = 0;

    [SerializeField] private Text textDisplay;

    [SerializeField] private AudioSource collectionSoundEffect;
    [SerializeField] private AudioSource stealSoundEffect;
    [SerializeField] private AudioSource dropSoundEffect;

    [SerializeField] private PlayerMovement movementScript;
    [SerializeField] private float itemWeightFactor = 0.1f;

    [SerializeField] private GameObject beanPrefab;
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private GameObject milkPrefab;

    [SerializeField] private GameObject finish;

    private int beansRequired;
    private int waterRequired;
    private int milkRequired;

    private void Start()
    {
        beansRequired = finish.GetComponent<Finish>().getRequiredBeanCount();
        waterRequired = finish.GetComponent<Finish>().getRequiredWaterCount();
        milkRequired = finish.GetComponent<Finish>().getRequiredMilkCount();
        Debug.Log(beansRequired);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dropItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Thief") && beanCount+waterCount+milkCount>0)
        {
            
            bool stolen = false;
            while (!stolen) {
                System.Random rnd = new System.Random();
                int randItem = rnd.Next(3);

                if (randItem == 0 && beanCount > 0)
                {
                    beanCount--;
                    stolen = true;
                }else if (randItem == 1 && waterCount > 0)
                {
                    waterCount--;
                    stolen = true;
                } else if (randItem == 2 && milkCount > 0)
                {
                    milkCount--;
                    stolen = true;
                }
            }
            stealSoundEffect.Play();
        }

        if (collision.gameObject.CompareTag("Bean"))
        {
            beanCount++;
            Destroy(collision.gameObject);
            collectionSoundEffect.Play();
        } 
        else if (collision.gameObject.CompareTag("Water"))
        {
            waterCount++;
            Destroy(collision.gameObject);
            collectionSoundEffect.Play();
        }
        else if (collision.gameObject.CompareTag("Milk"))
        {
            milkCount++;
            Destroy(collision.gameObject);
            collectionSoundEffect.Play();
        }

        updateItemCounts();
    }

    private void updateItemCounts()
    {
        movementScript.updateItemWeight((float)((beanCount + waterCount + milkCount) * itemWeightFactor));
        textDisplay.text = "Beans: " + beanCount + "/"+beansRequired +"\nWater: " + waterCount + "/"+waterRequired+"\nMilk: " + milkCount + "/"+milkRequired+"\nSpeed: " + movementScript.getMoveSpeed();
    }

    private void dropItem()
    {
        if (beanCount + waterCount + milkCount > 0)
        {
            bool dropped = false;
            while (!dropped)
            {
                System.Random rnd = new System.Random();
                int randItem = rnd.Next(3);

                if (randItem == 0 && beanCount > 0)
                {
                    beanCount--;
                    Instantiate(beanPrefab, new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y), Quaternion.identity);
                    updateItemCounts();
                    dropSoundEffect.Play();
                    dropped = true;
                }
                else if (randItem == 1 && waterCount > 0)
                {
                    waterCount--;
                    Instantiate(waterPrefab, new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y), Quaternion.identity);
                    updateItemCounts();
                    dropSoundEffect.Play();
                    dropped = true;
                }
                else if (randItem == 2 && milkCount > 0)
                {
                    milkCount--;
                    Instantiate(milkPrefab, new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y), Quaternion.identity);
                    updateItemCounts();
                    dropSoundEffect.Play();
                    dropped = true;
                }
            }
        }
        
    }

    public int getBeanCount()
    {
        return beanCount;
    }

    public int getWaterCount()
    {
        return waterCount;
    }

    public int getMilkCount()
    {
        return milkCount;
    }

}
