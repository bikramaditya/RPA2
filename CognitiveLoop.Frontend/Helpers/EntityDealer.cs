using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FluentTest.Helpers
{
    public class EntityDealer
    {
        internal static void addToGrid(Grid grid, UIElement element, int colPos, bool addNewRow)
        {
            if (addNewRow)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.MinHeight = 30.0;
                grid.RowDefinitions.Add(gridRow);
            }

            Border border = new Border();
            border.Padding = new Thickness(5);
            border.Child = element;

            Grid.SetRow(border, grid.RowDefinitions.Count - 1);
            Grid.SetColumn(border, colPos);
            grid.Children.Add(border);
        }

        internal static void addToGrid(Grid grid, UIElement element, int colPos, bool addNewRow, int height)
        {
            if (addNewRow)
            {
                RowDefinition gridRow = new RowDefinition();
                if (height > 0)
                {
                    gridRow.Height = new GridLength(height);
                }
                else
                {
                    gridRow.MinHeight = 30.0;
                }
                grid.RowDefinitions.Add(gridRow);
            }

            Border border = new Border();
            border.Padding = new Thickness(5);
            border.Child = element;

            Grid.SetRow(border, grid.RowDefinitions.Count - 1);
            Grid.SetColumn(border, colPos);
            grid.Children.Add(border);
        }
    }
}
