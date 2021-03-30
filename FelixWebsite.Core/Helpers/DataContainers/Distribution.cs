using System.Collections.Generic;
using System.Linq;

namespace FelixWebsite.Core.Helpers.DataContainers
{
    public class Distribution<T> : List<int>
    {
        public List<T> Items { get; set; }

        public Distribution(IEnumerable<T> items)
        {
            Items = items.ToList();
        }

        /// <summary>
        /// Returns the required bootstrap classes for the given item to get an equal distribution
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetBootstrapClassesForItem(T item)
        {
            return GetBootstrapClassesForIndex(Items.IndexOf(item));
        }

        /// <summary>
        /// Returns the required bootstrap classes for the given index to get an equal distribution
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetBootstrapClassesForIndex(int index)
        {
            var maxAmountOfItemsPerRow = this.Max();
            var classes = GetBootstrapColClassForAmountOfItemsPerRow(maxAmountOfItemsPerRow);

            var firstItemOfRowIndex = GetFirstItemOfRowIndex(index);
            if (firstItemOfRowIndex >= 0)
            {
                //it's the first item in its row, add an offset
                classes = $"{classes} {GetBootstrapColOffsetClass(maxAmountOfItemsPerRow, this[firstItemOfRowIndex])}";
            }

            return classes;
        }

        /// <summary>
        /// Returns the index of the row if the items searched for is the first one in its row, otherwise returns -1
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetFirstItemOfRowIndex(int index)
        {
            var currentIndex = 0;
            for (int i = 0; i < Count; i++)
            {
                if (currentIndex == index)
                {
                    return i;
                }
                currentIndex += this[i];
            }

            return -1;
        }

        /// <summary>
        /// Returns the bootstrap class for the amount of items per row
        /// </summary>
        /// <param name="amountOfItemsPerRow"></param>
        /// <returns></returns>
        private string GetBootstrapColClassForAmountOfItemsPerRow(int amountOfItemsPerRow)
        {
            switch (amountOfItemsPerRow)
            {
                case 12:
                    return "col-md-1";
                case 6:
                    return "col-md-2";
                case 4:
                    return "col-md-3";
                case 3:
                    return "col-md-4";
                case 2:
                    return "col-md-6";
                default:
                    return "col-md-4";
            }
        }

        /// <summary>
        /// Determines the offset depending on the amount of items per row and the amount of items in this row
        /// </summary>
        /// <param name="amountOfItemsPerRow"></param>
        /// <param name="amountOfItemsInThisRow"></param>
        /// <returns></returns>
        private string GetBootstrapColOffsetClass(int amountOfItemsPerRow, int amountOfItemsInThisRow)
        {
            switch (amountOfItemsPerRow)
            {
                case 12:
                    return "col-md-offset-" + ((12 - amountOfItemsPerRow) / 2);
                case 6:
                    return "col-md-offset-" + (6 - amountOfItemsPerRow);
                case 4:
                    switch (amountOfItemsInThisRow)
                    {
                        case 3:
                            return "col-md-offset-1";
                        case 2:
                            return "col-md-offset-3";
                        case 1:
                            return "col-md-offset-4";
                        default:
                            return string.Empty;
                    }
                case 3:
                    switch (amountOfItemsInThisRow)
                    {
                        case 2:
                            return "col-md-offset-2";
                        case 1:
                            return "col-md-offset-4";
                        default:
                            return string.Empty;
                    }
                case 2:
                    switch (amountOfItemsInThisRow)
                    {
                        case 1:
                            return "col-md-offset-3";
                        default:
                            return string.Empty;
                    }
                default:
                    return string.Empty;
            }
        }

        #region For percentage distribution

        /// <summary>
        /// Returns the required width percentage for the given item to get an equal distribution
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public decimal GetWidthPercentageForItem(T item)
        {
            return GetWidthPercentageForIndex(Items.IndexOf(item));
        }

        /// <summary>
        /// Returns the required width percentage for the given index to get an equal distribution
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public decimal GetWidthPercentageForIndex(int index)
        {
            var rowForIndex = 0;
            var currentIndex = 0;
            for (int i = 0; i < Count; i++)
            {
                currentIndex += this[i];
                if (currentIndex >= index)
                {
                    rowForIndex = i;
                    break;
                }
            }

            return (decimal)100 / this[rowForIndex];
        }

        #endregion
    }
}
