using Reversi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Rules
{
  public class GameRules
  {
    #region member variables
    #endregion

    #region properties

    /// <summary>
    /// gets/sets the data within each board location
    /// </summary>
    private BoardLocationData BoardLocationFromData
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets the board locations indicating player moves
    /// </summary>
    public Players[,] BoardLocations
    {
      get { return BoardLocationFromData.BoardLocations; }
      set { BoardLocationFromData.BoardLocations = value; }
    }

    /// <summary>
    /// gets/sets the directory the game save is located
    /// </summary>
    public string FileSavePath
    {
      get { return BoardLocationFromData.FileSaveDirectory; }
      set { BoardLocationFromData.FileSaveDirectory = value; }
    }

    /// <summary>
    /// gets/sets the active player
    /// </summary>
    public Players StoredActivePlayer
    {
      get { return BoardLocationFromData.StoredActivePlayer; }
      set { BoardLocationFromData.StoredActivePlayer = value; }
    }

    #endregion

    #region construction / destruction

    /// <summary>
    /// create a new Rules object
    /// </summary>
    public GameRules()
    {
      BoardLocationFromData = new BoardLocationData();
    }

    #endregion

    #region methods

    /// <summary>
    /// determine if a player can make a move within a game board location
    /// </summary>
    /// <param name="activePlayer">the active player in the game</param>
    /// <param name="row">row to determine if a move can be made</param>
    /// <param name="column">column to determine if a move can be made</param>
    /// <returns>true if a move can be made, false if a move cannot be made</returns>
    public bool[,] CanMakeMove(Players activePlayer, int row, int column)
    {
      bool[,] makeMoveLocations = new bool[8, 8];

      // determine which board locations to highlight, mark those locations to be highlighted as true
      for (int boardLocationRow = 0; boardLocationRow < 8; boardLocationRow++)
      {
        for (int boardLocationColumn = 0; boardLocationColumn < 8; boardLocationColumn++)
        {
          makeMoveLocations[boardLocationRow, boardLocationColumn] = false;
        }
      }

      Players opposingPlayer = activePlayer == Players.FirstPlayer ? Players.SecondPlayer : Players.FirstPlayer;
      bool opposingPlayerFound = false;
      bool activePlayerFound = false;

      int rowCount = row - 1;
      int columnCount = column;

      // check for an empty location to make a move on
      if (BoardLocationFromData.BoardLocations[row, column] == Players.NoPlayer)
      {
        // search the column
        // check locations to row zero
        while (rowCount >= 0)
        {
          // stop the find if a location with no player move is encountered
          if (BoardLocationFromData.BoardLocations[rowCount, column] == Players.NoPlayer)
          {
            break;
          }
          else
          {
            // first search for the opposing player
            if (!opposingPlayerFound)
            {
              opposingPlayerFound = BoardLocationFromData.BoardLocations[rowCount, column] == opposingPlayer;

              // if the opposing player has not yet been found, but the activePlayer has been found
              if (!opposingPlayerFound && BoardLocationFromData.BoardLocations[rowCount, column] == activePlayer)
              {
                break;
              }
            }

            if (opposingPlayerFound && !activePlayerFound)
            {
              activePlayerFound = BoardLocationFromData.BoardLocations[rowCount, column] == activePlayer;

              if (activePlayerFound)
              {
                makeMoveLocations[row, column] = true;

                // set all disks on the current column as highlighted
                int diskHighlightCounter = rowCount;
                while (diskHighlightCounter <= row)
                {
                  makeMoveLocations[diskHighlightCounter, column] = true;
                  diskHighlightCounter++;
                }
              }
            }
          }

          rowCount--;
        }

        // check locations to row seven
        rowCount = row + 1;
        opposingPlayerFound = false;
        activePlayerFound = false;

        while (rowCount <= 7)
        {
          // stop the find if a location with no player move is encountered
          if (BoardLocationFromData.BoardLocations[rowCount, column] == Players.NoPlayer)
          {
            break;
          }
          else
          {
            // first search for the opposing player
            if (!opposingPlayerFound)
            {
              opposingPlayerFound = BoardLocationFromData.BoardLocations[rowCount, column] == opposingPlayer;

              // if the opposing player has not yet been found, but the activePlayer has been found
              if (!opposingPlayerFound && BoardLocationFromData.BoardLocations[rowCount, column] == activePlayer)
              {
                break;
              }
            }

            // if opposing player is found, search for the active player
            if (opposingPlayerFound && !activePlayerFound)
            {
              activePlayerFound = BoardLocationFromData.BoardLocations[rowCount, column] == activePlayer;

              if (activePlayerFound)
              {
                makeMoveLocations[row, column] = true;

                // set all disks on the current column as highlighted
                int diskHighlightCounter = rowCount;
                while (diskHighlightCounter >= row)
                {
                  makeMoveLocations[diskHighlightCounter, column] = true;
                  diskHighlightCounter--;
                }
              }
            }
          }

          rowCount++;
        }


        // search the row
        // check locations to column zero from one column to the left of the current column
        columnCount = column - 1;
        opposingPlayerFound = false;
        activePlayerFound = false;

        while (columnCount >= 0)
        {
          // stop the find if a location with no player move is encountered
          if (BoardLocationFromData.BoardLocations[row, columnCount] == Players.NoPlayer)
          {
            break;
          }
          else
          {
            // first search for the opposing player
            if (!opposingPlayerFound)
            {
              opposingPlayerFound = BoardLocationFromData.BoardLocations[row, columnCount] == opposingPlayer;

              // if the opposing player has not yet been found, but the activePlayer has been found
              if (!opposingPlayerFound && BoardLocationFromData.BoardLocations[row, columnCount] == activePlayer)
              {
                break;
              }
            }

            if (opposingPlayerFound && !activePlayerFound)
            {
              activePlayerFound = BoardLocationFromData.BoardLocations[row, columnCount] == activePlayer;

              if (activePlayerFound)
              {
                makeMoveLocations[row, column] = true;

                // set all disks on the current row as highlighted
                int diskHighlightCounter = columnCount;
                while (diskHighlightCounter <= column)
                {
                  makeMoveLocations[row, diskHighlightCounter] = true;
                  diskHighlightCounter++;
                }
              }
            }
          }

          columnCount--;
        }


        // check locations to column seven
        columnCount = column + 1;
        opposingPlayerFound = false;
        activePlayerFound = false;

        while (columnCount <= 7)
        {
          // stop the find if a location with no player move is encountered
          if (BoardLocationFromData.BoardLocations[row, columnCount] == Players.NoPlayer)
          {
            break;
          }
          else
          {
            // first search for the opposing player
            if (!opposingPlayerFound)
            {
              opposingPlayerFound = BoardLocationFromData.BoardLocations[row, columnCount] == opposingPlayer;

              // if the opposing player has not yet been found, but the activePlayer has been found
              if (!opposingPlayerFound && BoardLocationFromData.BoardLocations[row, columnCount] == activePlayer)
              {
                break;
              }
            }

            // if opposing player is found, search for the active player
            if (opposingPlayerFound && !activePlayerFound)
            {
              activePlayerFound = BoardLocationFromData.BoardLocations[row, columnCount] == activePlayer;

              if (activePlayerFound)
              {
                makeMoveLocations[row, column] = true;

                // set all disks on the current row as highlighted
                int diskHighlightCounter = columnCount;
                while (diskHighlightCounter >= column)
                {
                  makeMoveLocations[row, diskHighlightCounter] = true;
                  diskHighlightCounter--;
                }
              }
            }
          }

          columnCount++;
        }


        // search the diagonal to row zero and column zero
        rowCount = row - 1;
        columnCount = column - 1;
        opposingPlayerFound = false;
        activePlayerFound = false;

        if (rowCount <= 7 && columnCount <= 7)
        {
          while (rowCount >= 0 && columnCount >= 0)
          {
            // if there is no player at the next loction, no need to continue searching this diagonal
            if (BoardLocationFromData.BoardLocations[rowCount, columnCount] == Players.NoPlayer)
            {
              break;
            }
            else
            {
              // search for the opposing player
              if (!opposingPlayerFound)
              {
                opposingPlayerFound = BoardLocationFromData.BoardLocations[rowCount, columnCount] == opposingPlayer;

                // if the opposing player has not yet been found, but the activePlayer has been found, no need to continue searching this diagonal
                if (!opposingPlayerFound && BoardLocationFromData.BoardLocations[rowCount, columnCount] == activePlayer)
                {
                  break;
                }
              }

              // if opposing player is found, search for the active player
              if (opposingPlayerFound && !activePlayerFound)
              {
                activePlayerFound = BoardLocationFromData.BoardLocations[rowCount, columnCount] == activePlayer;

                if (activePlayerFound)
                {
                  makeMoveLocations[row, column] = true;

                  // set all disks on the current row as highlighted
                  int diskHighlightRowCounter = rowCount;
                  int diskHighlightColumnCounter = columnCount;
                  while (diskHighlightRowCounter <= row && diskHighlightColumnCounter <= column)
                  {
                    makeMoveLocations[diskHighlightRowCounter, diskHighlightColumnCounter] = true;
                    diskHighlightRowCounter++;
                    diskHighlightColumnCounter++;
                  }
                }
              }
            }

            rowCount--;
            columnCount--;
          }
        }


        // search the diagonal to row seven and column seven
        rowCount = row + 1;
        columnCount = column + 1;
        opposingPlayerFound = false;
        activePlayerFound = false;

        if (rowCount >= 0 && columnCount >= 0)
        {
          while (rowCount <= 7 && columnCount <= 7)
          {
            // if there is no player at the next loction, no need to continue searching this diagonal
            if (BoardLocationFromData.BoardLocations[rowCount, columnCount] == Players.NoPlayer)
            {
              break;
            }
            else
            {
              // search for the opposing player
              if (!opposingPlayerFound)
              {
                opposingPlayerFound = BoardLocationFromData.BoardLocations[rowCount, columnCount] == opposingPlayer;

                // if the opposing player has not yet been found, but the activePlayer has been found, no need to continue searching this diagonal
                if (!opposingPlayerFound && BoardLocationFromData.BoardLocations[rowCount, columnCount] == activePlayer)
                {
                  break;
                }
              }

              // if opposing player is found, search for the active player
              if (opposingPlayerFound && !activePlayerFound)
              {
                activePlayerFound = BoardLocationFromData.BoardLocations[rowCount, columnCount] == activePlayer;

                if (activePlayerFound)
                {
                  makeMoveLocations[row, column] = true;

                  // set all disks on the current row as highlighted
                  int diskHighlightRowCounter = rowCount;
                  int diskHighlightColumnCounter = columnCount;
                  while (diskHighlightRowCounter >= row && diskHighlightColumnCounter >= column)
                  {
                    makeMoveLocations[diskHighlightRowCounter, diskHighlightColumnCounter] = true;
                    diskHighlightRowCounter--;
                    diskHighlightColumnCounter--;
                  }
                }
              }
            }

            rowCount++;
            columnCount++;
          }
        }


        // search the diagonal to row seven and column zero
        rowCount = row + 1;
        columnCount = column - 1;
        opposingPlayerFound = false;
        activePlayerFound = false;

        if (columnCount <= 7 && rowCount >= 0)
        {
          while (rowCount <= 7 && columnCount >= 0)
          {
            // if there is no player at the next loction, no need to continue searching this diagonal
            if (BoardLocationFromData.BoardLocations[rowCount, columnCount] == Players.NoPlayer)
            {
              break;
            }
            else
            {
              // search for the opposing player
              if (!opposingPlayerFound)
              {
                opposingPlayerFound = BoardLocationFromData.BoardLocations[rowCount, columnCount] == opposingPlayer;

                // if the opposing player has not yet been found, but the activePlayer has been found, no need to continue searching this diagonal
                if (!opposingPlayerFound && BoardLocationFromData.BoardLocations[rowCount, columnCount] == activePlayer)
                {
                  break;
                }
              }

              // if opposing player is found, search for the active player
              if (opposingPlayerFound && !activePlayerFound)
              {
                activePlayerFound = BoardLocationFromData.BoardLocations[rowCount, columnCount] == activePlayer;

                if (activePlayerFound)
                {
                  makeMoveLocations[row, column] = true;

                  // set all disks on the current row as highlighted
                  int diskHighlightRowCounter = rowCount;
                  int diskHighlightColumnCounter = columnCount;
                  while (diskHighlightRowCounter >= row && diskHighlightColumnCounter <= column)
                  {
                    makeMoveLocations[diskHighlightRowCounter, diskHighlightColumnCounter] = true;
                    diskHighlightRowCounter--;
                    diskHighlightColumnCounter++;
                  }
                }
              }
            }

            rowCount++;
            columnCount--;
          }
        }


        // search the diagonal to row zero and column seven
        rowCount = row - 1;
        columnCount = column + 1;
        opposingPlayerFound = false;
        activePlayerFound = false;

        if (rowCount <= 7 && columnCount >= 0)
        {
          while (rowCount >= 0 && columnCount <= 7)
          {
            // if there is no player at the next loction, no need to continue searching this diagonal
            if (BoardLocationFromData.BoardLocations[rowCount, columnCount] == Players.NoPlayer)
            {
              break;
            }
            else
            {
              // search for the opposing player
              if (!opposingPlayerFound)
              {
                opposingPlayerFound = BoardLocationFromData.BoardLocations[rowCount, columnCount] == opposingPlayer;

                // if the opposing player has not yet been found, but the activePlayer has been found, no need to continue searching this diagonal
                if (!opposingPlayerFound && BoardLocationFromData.BoardLocations[rowCount, columnCount] == activePlayer)
                {
                  break;
                }
              }

              // if opposing player is found, search for the active player
              if (opposingPlayerFound && !activePlayerFound)
              {
                activePlayerFound = BoardLocationFromData.BoardLocations[rowCount, columnCount] == activePlayer;

                if (activePlayerFound)
                {
                  makeMoveLocations[row, column] = true;

                  // set all disks on the current row as highlighted
                  int diskHighlightRowCounter = rowCount;
                  int diskHighlightColumnCounter = columnCount;
                  while (diskHighlightRowCounter <= row && diskHighlightColumnCounter >= column)
                  {
                    makeMoveLocations[diskHighlightRowCounter, diskHighlightColumnCounter] = true;
                    diskHighlightRowCounter++;
                    diskHighlightColumnCounter--;
                  }
                }
              }
            }

            rowCount--;
            columnCount++;
          }
        }
      }

      return makeMoveLocations;
    }

    /// <summary>
    /// store the player associated with each given location
    /// </summary>
    /// <param name="activePlayer">this player will be marked in the board locations as having disks according to locations in the given locations array</param>
    /// <param name="makeMoveLocations">tracks which locations the player needs to be associated with</param>
    public void StoreBoardLocations(Players activePlayer, bool[,] makeMoveLocations)
    {
      for (int row = 0; row < 8; row++)
      {
        for (int column = 0; column < 8; column++)
        {
          if (makeMoveLocations[row, column])
          {
            BoardLocationFromData.BoardLocations[row, column] = activePlayer;
          }
        }
      }
    }

    /// <summary>
    /// perform a calculation from the move made to determine if the move is a winning move
    /// </summary>
    /// <param name="activePlayer">the currently active player within the game</param>
    /// <param name="makeMoveLocations">locations to be associated with the currently active player</param>
    /// <param name="firstPlayerDisksCount">a total of disks the first player has on the game board</param>
    /// <param name="secondPlayerDisksCount">a total of disks the second player has on the game board</param>
    /// <returns>MoveResult identifying the state of the game</returns>
    public MoveResults TrackMoves(Players activePlayer, bool[,] makeMoveLocations, out int firstPlayerDisksCount, out int secondPlayerDisksCount)
    {
      MoveResults gameStatus = MoveResults.NoChange;

      StoreBoardLocations(activePlayer, makeMoveLocations);

      bool firstPlayerCanMakeMove = false;
      bool secondPlayerCanMakeMove = false;
      firstPlayerDisksCount = 0;
      secondPlayerDisksCount = 0;

      for (int row = 0; row < 8; row++)
      {
        for (int column = 0; column < 8; column++)
        {
          firstPlayerCanMakeMove = firstPlayerCanMakeMove ? firstPlayerCanMakeMove : DetermineIfPlayerCanMakeMove(CanMakeMove(Players.FirstPlayer, row, column));
          secondPlayerCanMakeMove = secondPlayerCanMakeMove ? secondPlayerCanMakeMove : DetermineIfPlayerCanMakeMove(CanMakeMove(Players.SecondPlayer, row, column));
        }
      }

      if (!firstPlayerCanMakeMove && activePlayer != Players.FirstPlayer || !secondPlayerCanMakeMove && activePlayer != Players.SecondPlayer)
      {
        gameStatus = MoveResults.PlayerCannotMove;
      }

      if (!firstPlayerCanMakeMove && !secondPlayerCanMakeMove)
      {
        // neither player can make a move, game over. determine the winner
        foreach (Players selectedPlayer in BoardLocationFromData.BoardLocations)
        {
          if (selectedPlayer == Players.FirstPlayer)
          {
            firstPlayerDisksCount++;
          }
          else if (selectedPlayer == Players.SecondPlayer)
          {
            secondPlayerDisksCount++;
          }
        }

        if (firstPlayerDisksCount > secondPlayerDisksCount)
        {
          gameStatus = MoveResults.FirstPlayerWin;
        }
        else if (firstPlayerDisksCount < secondPlayerDisksCount)
        {
          gameStatus = MoveResults.SecondPlayerWin;
        }
        else if (firstPlayerDisksCount == secondPlayerDisksCount)
        {
          gameStatus = MoveResults.TieGame;
        }
      }

      return gameStatus;
    }

    /// <summary>
    /// determine if the given moves contain at least one possible move a player can make
    /// </summary>
    /// <param name="moves">moves made by the players</param>
    /// <returns>true if one or more moves can be made, false otherwise</returns>
    public bool DetermineIfPlayerCanMakeMove(bool[,] moves)
    {
      bool canMakeMove = false;

      for (int row = 0; row < 8; row++)
      {
        for (int column = 0; column < 8; column++)
        {
          if (moves[row, column])
          {
            canMakeMove = true;
          }
        }
      }

      return canMakeMove;
    }

    /// <summary>
    /// reset the status of the stored board locations
    /// </summary>
    public void ResetGame()
    {
      BoardLocationFromData.BoardLocations = new Players[8, 8];
      for (int row = 0; row < 8; row++)
      {
        for (int column = 0; column < 8; column++)
        {
          BoardLocationFromData.BoardLocations[row, column] = Players.NoPlayer;
        }
      }
    }

    /// <summary>
    /// get the board location data
    /// </summary>
    public BoardLocationData RetrieveBoardLocationData()
    {
      return BoardLocationFromData;
    }

    /// <summary>
    /// load the state of the game
    /// </summary>
    /// <param name="state"></param>
    public void LoadGameState(object state)
    {
      BoardLocationFromData = state as BoardLocationData;
    }

    #endregion

    #region event handlers
    #endregion
  }
}
