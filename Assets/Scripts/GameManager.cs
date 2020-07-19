using DG.Tweening;
using RogueSharp;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {
	public int MapWidth;
	public int MapHeight;

	public Grid Grid;
	public Tilemap BaseTilemap;

	public TileBase WallTile;
	public TileBase FloorTile;

	public Actor PlayerPrefab;
	public Actor Player;

	private DungeonMap _map;

	void Start() {
		_map = new MapFactory().CreateBasicMap(MapWidth, MapHeight);
		SpawnPlayer(1, 1);
		foreach (var cell in _map.GetAllCells()) {
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

	// Move to factory?
	public void SpawnPlayer(int xCoord, int yCoord) {
		var playerPos = BaseTilemap.GetCellCenterWorld(new Vector3Int(xCoord, yCoord, 1));
		Player = Instantiate(PlayerPrefab, playerPos, Quaternion.identity);
		Player.X = xCoord;
		Player.Y = yCoord;
		// Calculate player FoV
		_map.ComputeFov(Player.X, Player.Y, Player.Awareness, false);
	}

	private float _turnTime = 1f;
	private int _playerLayerPos = 10;
	public void MovePlayer(Vector3Int destination) {
		var diagonalCost = 1.5d;
		var sourceCell = _map.GetCell(Player.X, Player.Y);
		var destCell = _map.GetCell(destination.x, destination.y);
		_map.SetCellProperties(sourceCell.X, sourceCell.Y, sourceCell.IsTransparent, true);
		_map.SetCellProperties(destCell.X, destCell.Y, destCell.IsTransparent, true);

		var path = new PathFinder(_map, diagonalCost).TryFindShortestPath(sourceCell, destCell);
		if(path != null) {
			// TODO: kill the sequence if something comes in the way 
			// TODO: make sure to take movements in order (initiative maybe with die roll) if it is about to move to a tile make it unwalkable, if moving to an unwalkable tile recalculate path
			var sequence = DOTween.Sequence();
			var playerTransform = Player.transform;
			foreach(var step in path.Steps) {
				var worldDest = BaseTilemap.CellToWorld(new Vector3Int(step.X, step.Y, _playerLayerPos));
				sequence.Append(playerTransform.DOMove(worldDest, _turnTime)).OnStepComplete(() => { 
					// leaving tile open
					// if destination tile is closed kill sequence and redraw path
					// destination tile closed 	
				});
			}
		}

		_map.SetCellProperties(sourceCell.X, sourceCell.Y, sourceCell.IsTransparent, false);
		_map.SetCellProperties(destCell.X, destCell.Y, destCell.IsTransparent, false);
	}
}
