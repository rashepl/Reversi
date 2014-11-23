using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi.GUI
{
  public class BoardLocation : Panel
  {
    #region member variables
    #endregion

    #region properties

    /// <summary>
    /// gets/sets the row of which the board location resides
    /// </summary>
    public int LocationRow
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets the column of which the board location resides
    /// </summary>
    public int LocationColumn
    {
      get;
      set;
    }

    /// <summary>
    /// gest/sets the graphics associated with the board location
    /// </summary>
    private Graphics LocationGraphics
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets the pen associated with the board location
    /// </summary>
    private Pen LocationPen
    {
      get;
      set;
    }

    /// <summary>
    /// gets/sets the fill color for the disk on this board location
    /// </summary>
    public Brush DiskFillColor
    {
      get;
      set;
    }

    #endregion

    #region construction / destruction

    /// <summary>
    /// create a new BoardLocation object and create the graphics and pen associated with it
    /// </summary>
    public BoardLocation()
    {
      LocationGraphics = CreateGraphics();
      LocationPen = new Pen(Color.Black, 1);
    }

    #endregion

    #region methods

    /// <summary>
    /// draw the game disk on the game board location
    /// </summary>
    /// <param name="diskOutlineColor">color of the outline around the disk</param>
    /// <param name="diskOutlineWidth">width of the outline around the disk</param>
    /// <param name="diskFillColor">fill color of the disk</param>
    public void DrawDisk(Color diskOutlineColor, int diskOutlineWidth, Brush diskFillColor)
    {
      LocationGraphics.Clear(Color.Green);
      LocationPen.Color = diskOutlineColor;
      LocationPen.Width = diskOutlineWidth;
      DiskFillColor = diskFillColor;

      LocationGraphics.FillEllipse(diskFillColor, Width / 10, Height / 10, Width - (Width / 5), Height - (Height / 5));
      LocationGraphics.DrawEllipse(LocationPen, Width / 10, Height / 10, Width - (Width / 5), Height - (Height / 5));
    }

    /// <summary>
    /// draw the outline of the existing game disk on the game board location
    /// </summary>
    /// <param name="diskOutlineColor">color of the outline</param>
    /// <param name="diskOutlineWidth">width of the outline</param>
    public void OutlineDrawnDisk(Color diskOutlineColor, int diskOutlineWidth)
    {
      if (DiskFillColor != null)
      {
        LocationGraphics.Clear(Color.Green);
        LocationPen.Color = diskOutlineColor;
        LocationPen.Width = diskOutlineWidth;

        LocationGraphics.FillEllipse(DiskFillColor, Width / 10, Height / 10, Width - (Width / 5), Height - (Height / 5));
        LocationGraphics.DrawEllipse(LocationPen, Width / 10, Height / 10, Width - (Width / 5), Height - (Height / 5));
      }
    }

    /// <summary>
    /// redraw the image on the board location
    /// </summary>
    public void RedrawLocation()
    {
      if (DiskFillColor != null)
      {
        LocationGraphics.Clear(Color.Green);
        LocationGraphics.FillEllipse(DiskFillColor, Width / 10, Height / 10, Width - (Width / 5), Height - (Height / 5));
        LocationGraphics.DrawEllipse(LocationPen, Width / 10, Height / 10, Width - (Width / 5), Height - (Height / 5));
      }
    }

    #endregion

    #region event handlers
    #endregion
  }
}
