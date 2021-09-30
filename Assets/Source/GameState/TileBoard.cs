using System;
using System.Collections.Generic;
using System.Linq;

public struct TileBoard
{
    // Hashset is not the best data structure for this, maybe a Tile[3,3] would be ideal
    public readonly HashSet<Tile> Tiles;

    public TileBoard(Tile[] tiles)
    {
        this.Tiles = new HashSet<Tile>(tiles);
    }

    public TileBoard Empty
    {
        get
        {
            return new TileBoard(new Coordinates[]{
                new Coordinates(0,0),
                new Coordinates(0,1),
                new Coordinates(0,2),
                new Coordinates(1,0),
                new Coordinates(1,1),
                new Coordinates(1,2),
                new Coordinates(2,0),
                new Coordinates(2,1),
                new Coordinates(2,2)}
            .Select(x => new Tile(x))
            .ToArray());
        }
    }
    /*public TileBoard WithTile(Tile tile)
	{

	}*/
    // 3x3 Tiles with X,O,Empty
    // Check for Empty in other methods
    // WithTile method including playerId
}