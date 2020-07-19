using RogueSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {
	public int MapWidth;
	public int MapHeight;

	public Tilemap BaseTilemap;
	//public Tilemap WallsTilemap;

	public TileBase WallTile;
	public TileBase FloorTile;

	void Start() {
		var map = new MapFactory().CreateBasicMap(MapWidth, MapHeight);
		foreach (var cell in map.GetAllCells()) {
			UpdateMapCell(cell.X, cell.Y, Color.white, Color.white, cell.IsWalkable, cell.IsExplored);
		}
	}

	public void UpdateMapCell(int x, int y, Color foreColor, Color backColor, bool isWalkable, bool isExplored) {
		//if (!isExplored) // consider ca toate sunt explorate
		//	return;
		// consider ca toate sunt in fov din moment ce nu am calculat nimic 
		if (isWalkable)
			BaseTilemap.SetTile(new Vector3Int(x, y, 1), FloorTile);
		else
			BaseTilemap.SetTile(new Vector3Int(x, y, 1), WallTile);
	}
}
