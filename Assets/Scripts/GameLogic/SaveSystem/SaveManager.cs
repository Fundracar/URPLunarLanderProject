using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] GameManager gameManagerRef;

    public static SaveManager saveManager;

    private int numberOfRegisteredProfiles;

    public List<PlayerProfile> listOfPlayerProfiles;



    // Start is called before the first frame update
    void Start()
    {
        saveManager = this;
        gameManagerRef = GetComponent<GameManager>();
        listOfPlayerProfiles = new List<PlayerProfile>();
    }

    // Update is called once per frame

    public void CreateNewPlayerProfile(string _profileName)
    {
        PlayerProfile createdProfile = new PlayerProfile(_profileName, new Dictionary<int, int>(), 1);

        listOfPlayerProfiles.Add(createdProfile);

        foreach (var profile in listOfPlayerProfiles)
        {
            PlayerPrefs.HasKey("Profile" + createdProfile);


        }
    }
    public void FetchNumberOfProfiles()
    {

    }
    public void FetchPlayerPrefsData()
    {

    }

}
