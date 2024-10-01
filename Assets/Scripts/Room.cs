using UnityEngine;
/* ### Dalton_Christopher: Room class used to keep room data for Procedual level generator Maze/Rooms - 2D Platformer ### 
  
  ##################################################################
  ### Dalton Christopher - ID: A00122255                         ###
  ### TUA - AIP201: Artificial Intelligence & Physics for Games  ###
  ### - Assesment - 3                                            ###
  ### - 04/2024                                                  ###
  ##################################################################

*/
/// <summary>
/// Represents a room in the leve, holding the width/height and start postion needed for background item spawning / management
/// </summary>
public class Room
{
    public Vector2Int StartPos { get; set; } // The start position of the room
    public int Width { get; set; } // The width of the room
    public int Height { get; set; } //  The height of the room
    public Vector2Int EntryPos { get; set; } // The entry position of the room (where a ladder would be)

    /// <summary>
    /// Conjstructor to create a new room
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Room(Vector2Int startPosition, int width, int height)
    {
        StartPos = startPosition;
        Width = width;
        Height = height;
    }
}
