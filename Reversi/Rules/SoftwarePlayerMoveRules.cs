using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Rules
{
  public class SoftwarePlayerMoveRules
  {
    #region member variables
    #endregion

    #region properties

    private GameRules SoftwarePlayerRules
    {
      get;
      set;
    }

    #endregion

    #region construction / destruction

    /// <summary>
    /// construct a new SoftwarePlayerMoveRules object
    /// </summary>
    public SoftwarePlayerMoveRules()
    {
    }

    #endregion

    #region methods

    /// <summary>
    /// deterimine the row and column location of where the software player will make a move
    /// </summary>
    /// <param name="rules"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void FindDiskLocations(GameRules rules, out int row, out int column)
    {
      SoftwarePlayerRules = rules;
      row = -1;
      column = -1;


    }

    #endregion

    #region event handlers
    #endregion
  }
}
