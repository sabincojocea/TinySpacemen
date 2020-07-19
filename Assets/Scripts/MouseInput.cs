using UnityEngine;

public class MouseInput : MonoBehaviour {
	public GameManager GameManager;
	
	private Grid _grid;

	private void Start() {
		_grid = GameManager.BaseTilemap.layoutGrid; 
		//_grid = GameManager.Grid;
	}

	private void FixedUpdate() {
		if(Input.GetMouseButtonUp(0)) {
			var mousePosition = Input.mousePosition;
			Debug.Log(mousePosition);
			var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
			var gridCoord = _grid.WorldToCell(mouseWorldPos);
			//Debug.Log("Clicked position: " + gridCoord);
			GameManager.MovePlayer(gridCoord);
		}
	}
}
