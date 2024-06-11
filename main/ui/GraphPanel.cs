// namespace ProductiveHoursTracker.ui;
//
// // using System.Net.Mime;
// using ProductiveHoursTracker.model;
// using System;
// using System.Collections.Generic;
// using System.Drawing;
// using System.Drawing.Text;
// using System.Windows.Forms;
// using System;
// using System.Collections.Generic;
// using System.Drawing;
// using System.Windows.Forms;
// using System.Drawing.Drawing2D;
//
// // creates a graph (line and scatter plot) of the daily averages of all productivity entries
// public class GraphPanel : Panel
// {
//     private static readonly Color[] DATA_COLORS = new Color[]
//     {
//         System.Drawing.Pen
//         Color.FromArgb(255, 0, 0), // ENERGY
//         Color.FromArgb(100, 255, 0), // FOCUS
//         Color.FromArgb(0, 200, 255) // MOTIVATION
//     };
//
//     private static readonly Color GRID_COLOR = Color.FromArgb(200, 200, 200);
//
//     private Graphics g2d;
//
//     private int scaleX; // means scaleX pixels in one X unit
//     private int scaleY; // means scaleY pixels in one Y unit
//     private int pointRadius;
//
//     private Dictionary<ProductivityEntry.Label, SortedDictionary<TimeSpan, double>> averageLog;
//
//     // EFFECTS: constructs a graph panel with a given averageLog
//     public GraphPanel(Dictionary<ProductivityEntry.Label, SortedDictionary<TimeSpan, double>> averageLog)
//     {
//         this.averageLog = averageLog;
//     }
//
//     // MODIFIES: this
//     // EFFECTS: calls methods to paint graph
//     protected override void OnPaint(PaintEventArgs e)
//     {
//         base.OnPaint(e);
//         g2d = e.Graphics;
//
//         scaleX = Width / 25;
//         scaleY = Height / 12;
//         pointRadius = Height / 150;
//
//         SetBackgroundColor();
//         DrawGrid();
//         PlotData();
//     }
//
//     // MODIFIES: this
//     // EFFECTS: draws grid lines for graph
//     private void DrawGrid()
//     {
//         g2d.SmoothingMode = SmoothingMode.AntiAlias;
//         using (var pen = new Pen(GRID_COLOR, 1))
//         {
//             DrawVerticalGridLines(pen);
//             DrawHorizontalGridLines(pen);
//         }
//
//         DrawAxis();
//         SetTitle();
//     }
//
//     // MODIFIES: this
//     // EFFECTS: draws 11 vertical grid lines
//     private void DrawVerticalGridLines(Pen pen)
//     {
//         for (int x = 1; x < 25; x++)
//         {
//             g2d.DrawLine(pen, x * scaleX, 0, x * scaleX, Height);
//             g2d.DrawString(TimeSpan.FromHours(x - 1).ToString(@"hh\:mm"), new MediaTypeNames.Font("Arial", 8), Brushes.Black,
//                 x * scaleX - 16, scaleY * 11 + 15);
//         }
//     }
//
//     // MODIFIES: this
//     // EFFECTS: draws 11 horizontal grid lines
//     private void DrawHorizontalGridLines(Pen pen)
//     {
//         for (int y = 1; y < 12; y++)
//         {
//             g2d.DrawLine(pen, 0, y * scaleY, Width, y * scaleY);
//             g2d.DrawString((11 - y).ToString(), new MediaTypeNames.Font("Arial", 8), Brushes.Black, scaleX - 20, y * scaleY + 5);
//         }
//     }
//
//     // MODIFIES: this
//     // EFFECTS: draws axis lines, with color = black and width = 2
//     private void DrawAxis()
//     {
//         using (var pen = new Pen(Color.Black, 2))
//         {
//             g2d.DrawLine(pen, scaleX, 0, scaleX, Height); // y-axis
//             g2d.DrawLine(pen, 0, scaleY * 11, Width, scaleY * 11); // x-axis
//         }
//
//         // label for y-axis
//         var format = new StringFormat
//         {
//             FormatFlags = StringFormatFlags.DirectionVertical
//         };
//         g2d.DrawString("Level", new MediaTypeNames.Font("Arial", 12), Brushes.Black, scaleX / 2, Height / 2, format);
//
//         // label for x-axis
//         g2d.DrawString("Time", new MediaTypeNames.Font("Arial", 12), Brushes.Black, Width / 2, scaleY * 11 + 35);
//     }
//
//     // MODIFIES: this
//     // EFFECTS: draws a title for the graph
//     private void SetTitle()
//     {
//         g2d.DrawString("Productivity Levels Over Time", new MediaTypeNames.Font("Arial", 16), Brushes.Black, Width / 2 - 150,
//             Height / 20);
//     }
//
//     // MODIFIES: this
//     // EFFECTS: makes the graph background white
//     private void SetBackgroundColor()
//     {
//         g2d.Clear(Color.White);
//     }
//
//     // MODIFIES: this
//     // EFFECTS: plots data points as points and as line
//     private void PlotData()
//     {
//         int count = 0;
//         foreach (var mapSet in averageLog)
//         {
//             g2d.SmoothingMode = SmoothingMode.AntiAlias;
//             using (var brush = new SolidBrush(DATA_COLORS[count]))
//             {
//                 int prevX = -1;
//                 double prevY = -1.0;
//                 foreach (var entry in mapSet.Value)
//                 {
//                     g2d.DrawString(mapSet.Key.ToString(), new MediaTypeNames.Font("Arial", 8), Brushes.Black,
//                         Width / 2 - (count - 1) * 2 * scaleX, scaleY * 12);
//
//                     int posX = entry.Key.Hours + 1;
//                     double posY = 11 - entry.Value;
//
//                     DrawEllipse(posX, posY, brush);
//                     DrawLineBetweenPoints(prevX, prevY, posX, posY);
//
//                     prevX = posX;
//                     prevY = posY;
//                 }
//             }
//
//             count++;
//         }
//     }
//
//     // MODIFIES: this
//     // EFFECTS: draws ellipse onto graph to create a scatter-plot
//     private void DrawEllipse(int posX, double posY, Brush brush)
//     {
//         var dot = new RectangleF(
//             posX * scaleX - pointRadius,
//             (float)(posY * scaleY - pointRadius),
//             2 * pointRadius, 2 * pointRadius
//         );
//         g2d.FillEllipse(brush, dot);
//     }
//
//     // MODIFIES: this
//     // EFFECTS: connects ellipses on graph together to create a line graph
//     private void DrawLineBetweenPoints(int prevX, double prevY, int posX, double posY)
//     {
//         if (prevX > 0)
//         {
//             using (var pen = new Pen(Color.Black, 1))
//             {
//                 g2d.DrawLine(pen,
//                     prevX * scaleX,
//                     (float)(prevY * scaleY),
//                     posX * scaleX,
//                     (float)(posY * scaleY)
//                 );
//             }
//         }
//     }
// }