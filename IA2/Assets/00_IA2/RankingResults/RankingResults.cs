using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RankingResults : MonoBehaviour
{
    public CharacterModel[] _players;

    public string[] _rankings;
    public string[] _names;
    public int[] _scores;

    void Awake()
    {
        _players = new CharacterModel[6];
        for (int i = 0; i < _players.Length; i++)
        {
            _players[i] = new CharacterModel();
        }

        _players[0]._playerName = "KennethGod";
        _players[1]._playerName = "Pablo";
        _players[2]._playerName = "Andrea";
        _players[3]._playerName = "Cami";
        _players[4]._playerName = "IA2";

        _players[0]._newScore = 10000;
        _players[1]._newScore = 21;
        _players[2]._newScore = 25;
        _players[3]._newScore = 20;
        _players[4]._newScore = 9999;
    }

    private void Start()
    {
        //EventManager.Subscribe("OnChangedRanking", RankingValues);
    }

    void RankingValues(params object[] parameters)
    {
        var player = (CharacterModel)parameters[0];
        var playersLenght = (int)parameters[1];
        CheckValues(player, playersLenght);
    }

    public string[] CheckValues(CharacterModel currentPlayer, int rankingLenght)
    {
        if (currentPlayer._newScore < _players[rankingLenght - 1]._newScore)
        {
            return _rankings;
        }

        _players[_players.Count() - 1] = currentPlayer;

        var players = _players.OrderByDescending(n => n._newScore)
                              .Take(5);

        var scoreList = players.Select(n => n._newScore);
        var namesList = players.Select(n => n._playerName);

        _players = players.ToArray();

        var rankingResults = namesList.Zip(scoreList, (names, scores) => names + ": " + scores);

        _rankings = rankingResults.ToArray();

        return _rankings;
    }

}
