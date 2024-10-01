using UnityEngine;

/* ### Dalton_Christopher: Tile Class for Procedual level generator Maze/Rooms - 2D Platformer ### 
  
  ##################################################################
  ### Dalton Christopher - ID: A00122255                         ###
  ### TUA - AIP201: Artificial Intelligence & Physics for Games  ###
  ### - Assesment - 3                                            ###
  ### - 04/2024                                                  ###
  ##################################################################

  #################################################################################################################################################
  # ------ References ------
  #
  # -- The mentioned Maze/Room Dungeon Generation by Bob Nystrom:
  # - Procedual generation based on Bob Nystrom's, Implementation/Explanation of this method used in his, web-based roguelike written - [2014] 
  #   - https://journal.stuffwithstuff.com/2014/12/21/rooms-and-mazes/
  #
  # --The Game his Prodcedual generation was developed for and that the article refers to [Very cool project]
  #   - Hauberk: [2014 - 2024] https://github.com/munificent/hauberk
  #
  # - W3Schools. [n.d.]. C# Multidimensional Arrays. W3Schools. https://www.w3schools.com/cs/cs_arrays_multi.php
  #
  # - C# Helper. (n.d.). How to solve a maze. C# Helper. http://www.csharphelper.com/howtos/howto_solve_maze.html
  #
  # - Unity Technologies. (n.d.). Random.Range Method. Unity Scripting API. https://docs.unity3d.com/ScriptReference/Random.Range.html
  # 
  # -- Player sprites -- https://craftpix.net
  #   - Free Game Assets. (2019). Free Tiny Hero Sprites - Pixel Art. itch.io. https://free-game-assets.itch.io/free-tiny-hero-sprites-pixel-art 
  #  
  # -- Level tile / sprites / sheets -- https://craftpix.net
  #   - Free Game Assets. (2020). Free Swamp 2D Tileset - Pixel Art. itch.io. https://free-game-assets.itch.io/free-swamp-2d-tileset-pixel-art 
  #
  ###################################################################################################################################################
*/

// Enum for the Tile Type 
public enum TileType
{
    Wall,
    Path,
    Grass
}

public class Tile : MonoBehaviour
{
    [SerializeField] Sprite WallSprite;
    [SerializeField] Sprite PathSprite;
    [SerializeField] Sprite GrassSprite;
    SpriteRenderer SpriteRenderer;
    BoxCollider2D BoxCollider2d;

    public TileType Type { get; internal set; }

    private void Awake() // On awake get the SpriteRenderer
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2d = GetComponent<BoxCollider2D>();
    }

    // Method to change the type of the tile and update sprite
    public void SetType(TileType newType)
    {
        Type = newType; // Set the Type to the passed sprite
        UpdateSprite(); // call Update Sprite
    }

    // Method to update the sprite based on type
    private void UpdateSprite()
    {
        switch (Type) // Switch statement for the Tile Type
        {
            case TileType.Wall: // Case wall Type, Set Wall Sprite
                SpriteRenderer.sprite = WallSprite;
                SpriteRenderer.sortingOrder = 0;

                SpriteRenderer.sortingLayerName = "Walls"; // Stefan ..alters sorting layer?
                break;
            case TileType.Path: // Case Path Type, Set Wall Sprite
                SpriteRenderer.sprite = PathSprite;
                SpriteRenderer.sortingOrder = -10; // have path seth further back than walls/tiles
                gameObject.layer = LayerMask.NameToLayer("Default"); // Dalton attempt to bugfix 
                BoxCollider2d.enabled = false;
                break;
            case TileType.Grass: // Case Grass Type, Set Grass Sprite
                SpriteRenderer.sprite = GrassSprite;
                SpriteRenderer.sortingOrder = 0;
                break;
            default:
                break;
        }
    }
}
