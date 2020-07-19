using RogueSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFactory
{
    // TODO: try to refactor to strategy for dungeonMapFactory
    // TODO: should be singleton
    public MapFactory() { }

    public DungeonMap CreateBasicMap(int width, int height) {
        var map = new DungeonMap();
        map.Initialize(width, height);
        foreach(var cell in map.GetAllCells()) 
            map.SetCellProperties(cell.X, cell.Y, true, true, true);
        foreach(var cell in map.GetCellsInRows(0, height-1)) 
            map.SetCellProperties(cell.X, cell.Y, false, false, true);
        foreach (var cell in map.GetCellsInColumns(0, width - 1))
            map.SetCellProperties(cell.X, cell.Y, false, false, true);
        return map;
    }
}
