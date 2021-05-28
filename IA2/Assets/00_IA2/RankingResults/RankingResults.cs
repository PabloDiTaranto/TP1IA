using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RankingResults : MonoBehaviour
{
    private static CharacterModel[] _players;
    private string[] _rankings;

    void Awake()
    {
        _players = new CharacterModel[6];
        for (int i = 0; i < _players.Length; i++)
        {
            _players[i] = new CharacterModel();
        }

        _players[0]._playerName = "KennethGod";
        _players[1]._playerName = "Paco";
        _players[2]._playerName = "Pepe";
        _players[3]._playerName = "Juan";
        _players[4]._playerName = "Carla";

        _players[0]._newScore = 10000;
        _players[1]._newScore = 26;
        _players[2]._newScore = 15;
        _players[3]._newScore = 38;
        _players[4]._newScore = 33;
    }


    //IA2-P3
    public string[] CheckValues(CharacterModel currentPlayer, int rankingLenght)
    {
        if (currentPlayer._newScore < _players[rankingLenght - 1]._newScore)
        {
            return _rankings;
        }

        _players[_players.Count() - 1] = currentPlayer;

        var players = _players.OrderByDescending(n => n._newScore)
                              .Take(5);

        var scoreList = players.Aggregate(new List<int>(), (newScoreList, player) =>
        {
            newScoreList.Add(player._newScore);
            return newScoreList;
        });

        var namesList = players.Select(n => n._playerName);

        _players = players.ToArray();

        var rankingResults = namesList.Zip(scoreList, (names, scores) => names + ": " + scores);

        _rankings = rankingResults.ToArray();

        return _rankings;
    }

}
