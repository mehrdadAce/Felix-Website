using System;
using System.Collections.Generic;
using System.Linq;
using FelixWebsite.Core.Helpers.DataContainers;

namespace FelixWebsite.Core.Helpers
{
    public static class LayoutHelper
    {
        public static Distribution<T> DistributeInColumns<T>(this IEnumerable<T> items, int maxItemsPerRow = 12, int minItemsPerRow = 1, bool useBootstrapDistribution = true)
        {
            if (items == null)
                return null;

            #region Validate input parameters

            if (maxItemsPerRow < 1)
            {
                maxItemsPerRow = 1;
            }

            if (useBootstrapDistribution)
            {
                if (12 / maxItemsPerRow < 1 || 12 % maxItemsPerRow != 0)
                {
                    //invalid maxItemsPerRow
                    var rowsRequired = (int)Math.Ceiling(12 / (decimal)maxItemsPerRow);
                    maxItemsPerRow = 12 / rowsRequired;
                }

                if (minItemsPerRow > 12)
                {
                    minItemsPerRow = 12;
                }

                if (12 % minItemsPerRow != 0)
                {
                    var rowsRequired = 12 / minItemsPerRow;
                    minItemsPerRow = 12 / rowsRequired;
                }
            }

            #endregion

            var amountOfItems = items.Count();

            if (minItemsPerRow == 0)
                minItemsPerRow = 1;

            if (maxItemsPerRow == 0)
                maxItemsPerRow = 12;

            if (amountOfItems <= maxItemsPerRow)
            {
                return new Distribution<T>(items) { amountOfItems };
            }

            var distribution = new Distribution<T>(items);

            //Distribute based on the max amount of items per row
            var remainingItemsToDistribute = amountOfItems;
            while (remainingItemsToDistribute > 0)
            {
                if (remainingItemsToDistribute > maxItemsPerRow)
                {
                    distribution.Add(maxItemsPerRow);
                    remainingItemsToDistribute = remainingItemsToDistribute - maxItemsPerRow;
                }
                else
                {
                    distribution.Add(remainingItemsToDistribute);
                    remainingItemsToDistribute = 0;
                }
            }

            //Make sure we don't go below the minimum for each row
            while (distribution.Any(itemsPerRow => itemsPerRow < minItemsPerRow))
            {
                //if the first row has no items to distribute we stop
                if (distribution.FirstOrDefault() <= minItemsPerRow)
                    break;

                for (int i = 0; i < distribution.Count - 1; i++)
                {
                    //Keep passing items as long as this row has items and the next one doesn't have enough
                    while (distribution[i] > 0 && distribution[i + 1] < minItemsPerRow)
                    {
                        //move 1 item to the next row
                        distribution[i]--;
                        distribution[i + 1]++;
                    }
                }
            }

            return distribution;
        }
    }
}
