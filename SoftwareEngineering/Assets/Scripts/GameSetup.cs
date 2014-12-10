using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameSetup : MonoBehaviour {

    public InputField nameInputField;
    public Button submitButton;

    private void Start()
    {
        // Add listener to catch the submit (when set Submit button is pressed)
        nameInputField.onSubmit.AddListener((value) => getWarriors(value));
        // Add validation
        nameInputField.validation = InputField.Validation.Alphanumeric;

        // This is a setup for a button that grabs the field value when pressed
        submitButton.onClick.AddListener(() => getWarriors(nameInputField.value));
    }

    public void getWarriors(string numWarriors)
    {
        if (Convert.ToInt32(numWarriors) < 5)
        {
            PlayerPrefs.SetInt("Warriors1", Convert.ToInt32(numWarriors));
        }
        else
        {
            PlayerPrefs.SetInt("Warriors1", 4);
        }
        PlayerPrefs.Save();
    }

    public void getArchers(string numArchers)
    {
        numArchers.Trim();
        Debug.Log("num Archers: " + numArchers);
        if (Convert.ToInt32(numArchers) < 5)
        {
            PlayerPrefs.SetInt("Archers1", Convert.ToInt32(numArchers));
        }
        else
        {
            PlayerPrefs.SetInt("Archers1", 4);
        }
        PlayerPrefs.Save();
    }

    public void getMages(string numMages)
    {
        if (Convert.ToInt32(numMages) < 5)
        {
            PlayerPrefs.SetInt("Mages1", Convert.ToInt32(numMages));
        }
        else
        {
            PlayerPrefs.SetInt("Mages1", 4);
        }

        PlayerPrefs.Save();
    }
}
