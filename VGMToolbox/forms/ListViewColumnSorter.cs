using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using VGMToolbox.format.iso;

namespace VGMToolbox.forms
{
    // courtesy of http://www.fryan0911.com/2009/07/c-how-to-sort-listview-by-clicked.html
    public class ListViewColumnSorter : IComparer
    {
        private int ColumnToSort;
        private SortOrder OrderOfSort;
        CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }


        public int Compare(object x, object y)
        {
            int compareResult;
            string columnName;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // get the name of the column
            columnName = listviewX.ListView.Columns[ColumnToSort].Text;

            // Compare the two items
            if ((columnName.Equals("Size") || columnName.Equals("LBA")) && 
                (listviewX.SubItems.Count > ColumnToSort))
            {
                compareResult = ObjectCompare.Compare(UInt64.Parse(listviewX.SubItems[ColumnToSort].Text), UInt64.Parse(listviewY.SubItems[ColumnToSort].Text));
            }
            else if (columnName.Equals("Date") &&
                (listviewX.SubItems.Count > ColumnToSort))
            {
                compareResult = ObjectCompare.Compare(DateTime.Parse(listviewX.SubItems[ColumnToSort].Text), DateTime.Parse(listviewY.SubItems[ColumnToSort].Text));
            }
            else if (listviewX.SubItems.Count > ColumnToSort)
            {
                
                if ((typeof(IDirectoryStructure).IsAssignableFrom(listviewX.Tag.GetType())) && 
                    (typeof(IFileStructure).IsAssignableFrom(listviewY.Tag.GetType())))
                {
                    // Force Directories to the top
                    if (OrderOfSort == SortOrder.Ascending)
                    {
                        compareResult = -1;
                    }
                    else
                    {
                        compareResult = 1;
                    }
                }
                else if ((typeof(IFileStructure).IsAssignableFrom(listviewX.Tag.GetType())) &&
                         (typeof(IDirectoryStructure).IsAssignableFrom(listviewY.Tag.GetType())))
                {
                    // Force Directories to the top
                    if (OrderOfSort == SortOrder.Ascending)
                    {
                        compareResult = 1;
                    }
                    else
                    {
                        compareResult = -1;
                    }
                }
                else
                {
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                }
            }
            else
            {
                compareResult = 0;
            }
            
            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }


        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }


        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

    }
}
