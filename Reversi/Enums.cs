using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
  /// <summary>
  /// track the players of the game
  /// </summary>
  public enum Players
  {
    FirstPlayer = 0,
    SecondPlayer,
    NoPlayer
  }

  /// <summary>
  /// track possible results of the game
  /// </summary>
  public enum MoveResults
  {
    NoChange = 0,
    FirstPlayerWin,
    SecondPlayerWin,
    TieGame,
    PlayerCannotMove
  }
}
