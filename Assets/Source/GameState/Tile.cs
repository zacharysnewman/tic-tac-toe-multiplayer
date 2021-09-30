using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile
{
    public readonly Coordinates Coords;
    public readonly TileTeam Team;

    public Tile(Coordinates coords)
    {
        this.Coords = coords;
        this.Team = TileTeam.None;
    }
    private Tile(Coordinates coords, TileTeam team)
    {
        this.Coords = coords;
        this.Team = team;
    }

    public Tile WithTeam(TileTeam team) => new Tile(this.Coords, team);
}