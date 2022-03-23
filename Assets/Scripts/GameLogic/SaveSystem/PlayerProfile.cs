using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile
{
    public  string playerName;
    Dictionary<int, int> dictionnaryOfPlayerHighscores;

    Dictionary<int, float> dictionnaryOfPlayerBestTimes;


    public PlayerProfile(string _playerName, Dictionary<int, int> _dictOfHighScores,  Dictionary<int, float> _dictionnaryOfPlayerBestTimes)
    {
        playerName = _playerName;
        
        dictionnaryOfPlayerHighscores = new Dictionary<int, int>();
        dictionnaryOfPlayerHighscores = _dictOfHighScores;

        dictionnaryOfPlayerBestTimes = new Dictionary<int, float>();
        dictionnaryOfPlayerBestTimes = _dictionnaryOfPlayerBestTimes;
    }
}
