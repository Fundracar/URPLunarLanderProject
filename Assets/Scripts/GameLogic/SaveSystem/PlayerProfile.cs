using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile
{
    string playerName;
    int playerCurrentLevelBuildIndex;
    Dictionary<int, int> dictionnaryOfPlayerHighscores;


    public PlayerProfile(string _playerName, Dictionary<int, int> _dictOfHighScores, int _CurrentLevelBuildIndex)
    {
        playerName = _playerName;
        dictionnaryOfPlayerHighscores = new Dictionary<int, int>();
        dictionnaryOfPlayerHighscores = _dictOfHighScores;
        playerCurrentLevelBuildIndex = _CurrentLevelBuildIndex;
    }
}
