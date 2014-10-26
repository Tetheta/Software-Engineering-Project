using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void wasClicked()
    {
        for (int i = 0; i < GameManager.heroClicked.Count; i++)
        {
            if (GameManager.heroClicked[i])
            {
                GameManager.Instance.moveHero(transform, i);
                GameManager.heroClicked[i] = false;
                Debug.Log("Hero #" + i + " Clicked, now going to move hero!");
            }
        }
        Debug.Log("Square was clicked!");
    }
}