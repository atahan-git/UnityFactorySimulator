using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// A helper class to hold positional information
/// Basically a sort of Vector2 specifically made for this game
/// </summary>
[System.Serializable]
public struct Position {
	public int x;
	public int y;

	public static float defaultPositionVector3Z = 0;

	public enum Type { world, belt, item, building, drone };

	public Position (int _x, int _y) {
		x = _x;
		y = _y;
	}

	public static Position operator + (Position a, Position b) {
		return new Position(a.x + b.x, a.y + b.y);
	}

	public static Position operator - (Position a, Position b) {
		return new Position(a.x - b.x, a.y - b.y);
	}

	public static Position operator - (Position a, Vector2 b) {
		return new Position(a.x - (int)b.x, a.y - (int)b.y);
	}

	public static bool operator ==(Position a, Position b) {
		return (a.x == b.x) && (a.y == b.y);
	}
	
	public static bool operator !=(Position a, Position b) {
		return !((a.x == b.x) && (a.y == b.y));
	}

	public Vector3 Vector3 (Type type, float z = 0){
		return new Vector3(x, y, z);
	}

	public override string ToString () {
		return string.Format("pos({0}, {1})", x, y);
	}


	public static int Distance(Position a, Position b) {
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
	}

	public static Position MoveTowards(Position start, Position end, int amount) {
		return new Position(start.x + Mathf.Clamp(end.x - start.x, -amount, amount), start.y + Mathf.Clamp(end.y - start.y, -amount, amount));
	}

	public static int CardinalDirection(Position start, Position end) {
		if (end.y  > start.y) {
			return 1;
		} else if(end.y < start.y) {
			return 3;
		}else if (end.x > start.x) {
			return 2;
		}else if (end.x < start.x) {
			return 4;
		} else {
			return 0;
		}
	}
}
