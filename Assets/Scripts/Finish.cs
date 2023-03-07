using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private Text textDisplay;

    [SerializeField] private AudioSource finishSoundEffect;
    [SerializeField] private AudioSource incompleteSoundEffect;

    [SerializeField] private int beansRequired = 1;
    [SerializeField] private int waterRequired = 1;
    [SerializeField] private int milkRequired = 1;

    [SerializeField] private GameObject coffeePrefab;

    private bool finished = false;


    // Start is called before the first frame update
    private void Start()
    {
        finishSoundEffect = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name == "Player" && !finished && itemsComplete())
        {
            finished = true;
            finishSoundEffect.Play();
            textDisplay.color = Color.white;
            Instantiate(coffeePrefab, new Vector2(gameObject.transform.position.x - 2, gameObject.transform.position.y - 1), Quaternion.identity);
            Invoke("CompleteLevel", 2f); //call after 2 seconds
        }
        else
        {
            textDisplay.color = Color.red;
            incompleteSoundEffect.Play();
            Invoke("revertTextColor", 1f);
        }

    }

    private void revertTextColor()
    {
        textDisplay.color = Color.white;
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private bool itemsComplete()
    {
        int beans = player.GetComponent<ItemCollector>().getBeanCount();
        int water = player.GetComponent<ItemCollector>().getWaterCount();
        int milk = player.GetComponent<ItemCollector>().getMilkCount();

        if (beans < beansRequired)
        {
            return false;
        } 
        else if (water < waterRequired)
        {
            return false;
        } 
        else if (milk < milkRequired)
        {
            return false;
        }

        Debug.Log("Items complete");

        return true;
    }

    public int getRequiredBeanCount()
    {
        return beansRequired;
    }

    public int getRequiredWaterCount()
    {
        return waterRequired;
    }

    public int getRequiredMilkCount()
    {
        return milkRequired;
    }
}
