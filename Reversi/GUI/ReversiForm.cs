using Reversi.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi.GUI
{
  public partial class ReversiForm : Form
  {
    #region member variables
    #endregion

    #region properties

    /// <summary>
    /// gets/sets the currently active player
    /// </summary>
    Players ActivePlayer
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets the rules for the game
    /// </summary>
    GameRules Rules
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets the locations of move sqaures users can use to make moves on the game board
    /// </summary>
    BoardLocation[,] GameBoardLocations
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets the locations of potential game board squares of which a user can place a move
    /// </summary>
    bool[,] MakeMoveLocations
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets indication if the game has been changed since starting a new game or saving an in-progress game
    /// </summary>
    bool IsDirty
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets indication if the player is playing single player mode (playing against the software), or two player mode (not playing against the software)
    /// </summary>
    bool SinglePlayerMode
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets the moves rules associated with the software player
    /// </summary>
    SoftwarePlayerMoveRules SoftwareMoveRules
    {
      get;
      set;
    }

    #endregion

    #region construction / destruction

    /// <summary>
    /// construct a new reversi game form
    /// </summary>
    public ReversiForm()
    {
      InitializeComponent();
      Rules = new GameRules();
      GameBoardLocations = new BoardLocation[8, 8];
      MakeMoveLocations = new bool[8, 8];
      StoreGameBoardLocations();
      ClearSelectedLocations();
      SoftwareMoveRules = new SoftwarePlayerMoveRules();
    }

    #endregion

    #region methods

    /// <summary>
    /// clears the game board and draws the initial disks for starting gameplay
    /// </summary>
    private void CreateInitialGameBoard()
    {
      ActivePlayer = Players.FirstPlayer;
      _playerTurnStatusLabel.Text = "Player 1 Turn (black disk)";

      // reset the structure identifying selected locations on the grid
      ClearSelectedLocations();

      // reset the stored locations status for each game board location
      Rules.ResetGame();

      // reset the status of each of the game board locations
      for (int row = 0; row < 8; row++)
      {
        for (int column = 0; column < 8; column++)
        {
          GameBoardLocations[row, column].DrawDisk(Color.Green, 1, Brushes.Green);
        }
      }

      // set up the inital four disks on the board
      GameBoardLocations[3, 4].DrawDisk(Color.Black, 1, Brushes.Black);
      GameBoardLocations[4, 3].DrawDisk(Color.Black, 1, Brushes.Black);
      MakeMoveLocations[3, 4] = true;
      MakeMoveLocations[4, 3] = true;
      Rules.StoreBoardLocations(Players.FirstPlayer, MakeMoveLocations);
      ClearSelectedLocations();

      GameBoardLocations[3, 3].DrawDisk(Color.Black, 1, Brushes.White);
      GameBoardLocations[4, 4].DrawDisk(Color.Black, 1, Brushes.White);
      MakeMoveLocations[3, 3] = true;
      MakeMoveLocations[4, 4] = true;
      Rules.StoreBoardLocations(Players.SecondPlayer, MakeMoveLocations);
      ClearSelectedLocations();
      IsDirty = false;
    }

    /// <summary>
    /// fill the game board locations tracker array with the game board locations
    /// </summary>
    private void StoreGameBoardLocations()
    {
      foreach (Control control in _gameBoardTableLayoutPanel.Controls)
      {
        if (control is BoardLocation)
        {
          BoardLocation boardLocation = control as BoardLocation;
          boardLocation.DiskFillColor = Brushes.Green;
          GameBoardLocations[boardLocation.LocationRow, boardLocation.LocationColumn] = boardLocation;
        }
      }
    }

    /// <summary>
    /// clear the selections within the locations array location by setting them to false
    /// </summary>
    private void ClearSelectedLocations()
    {
      for (int row = 0; row < 8; row++)
      {
        for (int column = 0; column < 8; column++)
        {
          MakeMoveLocations[row, column] = false;
        }
      }
    }

    /// <summary>
    /// save the state of the game
    /// </summary>
    /// <param name="saveAs">identifies if the game is to prompt the user for a file path</param>
    private void SaveGame(bool saveAs)
    {
      using (SaveFileDialog dialog = new SaveFileDialog())
      {
        dialog.FileName = string.Format("{0}{1}", Text.Replace("*", string.Empty), Text.Contains(".reversi") ? string.Empty : ".reversi");
        dialog.Filter = "reversi Files|*.reversi|All Files|*.*";
        dialog.Title = "Save Reversi Game";
        dialog.InitialDirectory = string.IsNullOrEmpty(Rules.FileSavePath) ? dialog.InitialDirectory : Rules.FileSavePath;

        if (saveAs || string.IsNullOrEmpty(Rules.FileSavePath))
        {
          if (dialog.ShowDialog() == DialogResult.OK)
          {
            Rules.FileSavePath = dialog.FileName;
            Text = Path.GetFileName(dialog.FileName);
          }
        }

        Rules.StoredActivePlayer = ActivePlayer;

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(Rules.FileSavePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        formatter.Serialize(stream, Rules.RetrieveBoardLocationData());
        Text = Text.Replace("*", string.Empty);
        IsDirty = false;
      }
    }

    #endregion

    #region event handlers

    /// <summary>
    /// handle a user selection on a board location
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BoardLocation_Click(object sender, EventArgs e)
    {
      // if the move can be made
      BoardLocation location = sender as BoardLocation;
      bool updatePlayerStatus = false;

      if (GameBoardLocations[location.LocationRow, location.LocationColumn].DiskFillColor == Brushes.Green)
      {
        if (ActivePlayer == Players.FirstPlayer)
        {
          for (int row = 0; row < 8; row++)
          {
            for (int column = 0; column < 8; column++)
            {
              if (MakeMoveLocations[row, column])
              {
                GameBoardLocations[row, column].DrawDisk(Color.Black, 1, Brushes.Black);
                updatePlayerStatus = true;
                MakeMoveLocations[row, column] = true;
                Text = Text.Contains("*") ? Text : string.Format("{0}*", Text);
                IsDirty = true;
              }
              else
              {
                MakeMoveLocations[row, column] = false;
              }
            }
          }
        }
        else
        {
          for (int row = 0; row < 8; row++)
          {
            for (int column = 0; column < 8; column++)
            {
              if (MakeMoveLocations[row, column])
              {
                GameBoardLocations[row, column].DrawDisk(Color.Black, 1, Brushes.White);
                updatePlayerStatus = true;
                MakeMoveLocations[row, column] = true;
                Text = Text.Contains("*") ? Text : string.Format("{0}*", Text);
                IsDirty = true;
              }
              else
              {
                MakeMoveLocations[row, column] = false;
              }
            }
          }
        }

        int player1DiskCount = 0;
        int player2DiskCount = 0;

        // store the move and determine if it is a winning move
        switch (Rules.TrackMoves(ActivePlayer, MakeMoveLocations, out player1DiskCount, out player2DiskCount))
        {
          case MoveResults.TieGame:
            {
              MessageBox.Show("The game is tied.");
              CreateInitialGameBoard();
              updatePlayerStatus = false;
            }
            break;
          case MoveResults.FirstPlayerWin:
            {
              MessageBox.Show(string.Format("Player 1 wins! {0} to {1}", player1DiskCount, player2DiskCount));
              CreateInitialGameBoard();
              updatePlayerStatus = false;
            }
            break;
          case MoveResults.SecondPlayerWin:
            {
              MessageBox.Show(string.Format("Player 2 wins! {0} to {1}", player2DiskCount, player1DiskCount));
              CreateInitialGameBoard();
              updatePlayerStatus = false;
            }
            break;
          case MoveResults.PlayerCannotMove:
            {
              // one of the players cannot make a move, update the status
              MessageBox.Show(string.Format("{0} cannot move, setting the active player to {1}", ActivePlayer == Players.FirstPlayer ? "Player 2" : "Player 1", ActivePlayer == Players.FirstPlayer ? "Player 1" : "Player 2"), "Player Cannot Move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              updatePlayerStatus = false;
            }
            break;
        }

        ClearSelectedLocations();
        if (updatePlayerStatus)
        {
          ActivePlayer = ActivePlayer == Players.FirstPlayer ? Players.SecondPlayer : Players.FirstPlayer;
          _playerTurnStatusLabel.Text = ActivePlayer == Players.FirstPlayer ? "Player 1 Turn (black disk)" : "Player 2 Turn (white disk)";
        }
      }

      // if one player mode is on, allow the software to make a move
      if (SinglePlayerMode)
      {
        // retrieve the location to make a move for the computer and apply the move
        int row = -1;
        int column = -1;
        SoftwareMoveRules.FindDiskLocations(Rules, out row, out column);

      }
    }

    /// <summary>
    /// determine if a move can be made, if so allow the user to make a selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void _boardLocation_MouseEnter(object sender, EventArgs e)
    {
      BoardLocation boardLocation = sender as BoardLocation;

      // identify which board locations need to be highlighted to let the user know which disks can be changed
      ClearSelectedLocations();
      MakeMoveLocations = Rules.CanMakeMove(ActivePlayer, boardLocation.LocationRow, boardLocation.LocationColumn);

      for (int rowHighlight = 0; rowHighlight < 8; rowHighlight++)
      {
        for (int columnHighlight = 0; columnHighlight < 8; columnHighlight++)
        {
          if (MakeMoveLocations[rowHighlight, columnHighlight])
          {
            GameBoardLocations[rowHighlight, columnHighlight].OutlineDrawnDisk(Color.Yellow, 3);
          }
        }
      }
    }

    /// <summary>
    /// remove all highlighting on the disk outlines where the highlighting is set as selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void _boardLocation_MouseLeave(object sender, EventArgs e)
    {
      for (int row = 0; row < 8; row++)
      {
        for (int column = 0; column < 8; column++)
        {
          if (MakeMoveLocations[row, column])
          {
            if (GameBoardLocations[row, column].DiskFillColor == Brushes.Green)
            {
              GameBoardLocations[row, column].OutlineDrawnDisk(Color.Green, 1);
            }
            else
            {
              GameBoardLocations[row, column].OutlineDrawnDisk(Color.Black, 1);
            }
          }
        }
      }
    }

    /// <summary>
    /// display the initial disks on the board and track them
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReversiForm_Shown(object sender, EventArgs e)
    {
      CreateInitialGameBoard();
    }

    /// <summary>
    /// redraw selected locations controls when the grid is resized
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReversiForm_Resize(object sender, EventArgs e)
    {
      foreach (BoardLocation location in GameBoardLocations)
      {
        location.RedrawLocation();
      }
    }

    /// <summary>
    /// create a new reversi game
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (IsDirty && MessageBox.Show("Do you want to save your game?", "Save Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        SaveGame(true);
      }

      CreateInitialGameBoard();
    }

    /// <summary>
    /// open an existing game
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog dialog = new OpenFileDialog())
      {
        dialog.Filter = "reversi files (*.reversi)|*.reversi|All files (*.*)|*.*";
        if (dialog.ShowDialog() == DialogResult.OK && dialog.CheckPathExists)
        {
          IFormatter formatter = new BinaryFormatter();
          using (Stream stream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
          {
            Rules.LoadGameState(formatter.Deserialize(stream));

            // reset the status of each of the game board locations
            for (int row = 0; row < 8; row++)
            {
              for (int column = 0; column < 8; column++)
              {
                switch (Rules.BoardLocations[row, column])
                {
                  case Players.FirstPlayer:
                    {
                      GameBoardLocations[row, column].DrawDisk(Color.Black, 1, Brushes.Black);
                    }
                    break;
                  case Players.SecondPlayer:
                    {
                      GameBoardLocations[row, column].DrawDisk(Color.Black, 1, Brushes.White);
                    }
                    break;
                  case Players.NoPlayer:
                    {
                      GameBoardLocations[row, column].DrawDisk(Color.Green, 1, Brushes.Green);
                    }
                    break;
                }
              }
            }

            ClearSelectedLocations();
            ActivePlayer = Rules.StoredActivePlayer;
            _playerTurnStatusLabel.Text = ActivePlayer == Players.FirstPlayer ? "Player 1 Turn (black disk)" : "Player 2 Turn (white disk)";
          }
        }
      }
    }

    /// <summary>
    /// save the game, if a location to save the game has not been chosen, prompt to provide a location to save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveGame(false);
    }

    /// <summary>
    /// provide a choice of a location to save the game and save it
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveGame(true);
    }

    /// <summary>
    /// exit the game, prompt if an in-progress game is to be saved
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (IsDirty && MessageBox.Show("Do you want to save your game?", "Save Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        SaveGame(true);
      }

      Close();
    }

    /// <summary>
    /// toggle the single player/two player mode status of the game
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void singlePlayerModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CreateInitialGameBoard();
      SinglePlayerMode = singlePlayerModeToolStripMenuItem.Checked;
    }
    #endregion

  }
}
