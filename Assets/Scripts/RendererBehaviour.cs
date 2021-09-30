using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Events.StateEvents;


public class RendererBehaviour : MonoBehaviour
{
    public GameObject winDisplayUI;
    public Text winDisplayText;

    //public GameObject[] tileObjects;

    private void Start()
    {
        EventAggregator.Get<StateChangedEvent>().Subscribe(OnStateChanged);
    }

    public void OnStateChanged(GameState newState)
    {

    }

    public enum WinLoseStatus { none, win, lose }
    private TileTeam WinCheck(IEnumerable<Tile> tiles)
    {
        throw new NotImplementedException();
    }
}
