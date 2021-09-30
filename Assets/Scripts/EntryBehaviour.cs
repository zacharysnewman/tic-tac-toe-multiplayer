using UnityEngine;
using Events.InputEvents;

// UI Input > EntryBehaviour > Game > RenderBehaviour > UI Display

public class EntryBehaviour : MonoBehaviour
{
    private readonly Game game = new Game();

    private void Start()
    {
        game.Init();
    }

    public void OnActivateTile(Coordinates coords)
    {
        EventAggregator.Get<ActivateTileEvent>().Publish(coords);
    }

    public void OnNewGameStarted()
    {
        EventAggregator.Get<NewGameEvent>().Publish();
    }
}
