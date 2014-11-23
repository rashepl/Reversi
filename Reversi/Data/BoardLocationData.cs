using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Data
{
  [Serializable]
  public class BoardLocationData
  {
    #region member variables
    #endregion

    #region properties

    /// <summary>
    /// gets/sets the locations containing user selections on the game board
    /// </summary>
    public Players[,] BoardLocations
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets the directory location where the game has been saved
    /// </summary>
    public string FileSaveDirectory
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets active player
    /// </summary>
    public Players StoredActivePlayer
    {
      get;
      set;
    }

    #endregion

    #region construction / destruction

    /// <summary>
    /// create a new BoardLocationData object
    /// </summary>
    public BoardLocationData()
    {
    }

    #endregion

    #region methods
    #endregion

    #region event handlers
    #endregion
  }
}
