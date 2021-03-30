using Microsoft.Ajax.Utilities;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelixWebsite.Core.Helpers
{
    public class PdfTableGenerator
    {
        private int amountOfColumns;
        private int amountOfRows;
        private XGraphics gfx;
        private int xOffset;
        private int yOffset;
        private XPen pen;
        private const int cellPadding = 15;
        private const int cellHeight = 30;
        private const int cellWidth = 120;
        private int remarksWidth = 200;

        private XFont boldFont;
        private XFont normalFont;
        private XBrush brush;

        public PdfTableGenerator(XGraphics gfx, int xOffset, int yOffset)
        {
            this.gfx = gfx;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            pen = new XPen(new XColor { R = 0, G = 0, A = 0 });
            boldFont = new XFont("Verdana", 11, XFontStyle.Bold);
            normalFont = new XFont("Verdana", 11, XFontStyle.Regular);
            brush = XBrushes.Black;
        }

        public void CreateTable(IEnumerable<string> headerValues, IEnumerable<IEnumerable<string>> rowValues)
        {
            amountOfColumns = headerValues.Count();
            amountOfRows = rowValues.Count();

            CreateHeaderRow(headerValues.ToList());
            for (var i = 0; i < amountOfRows; i++)
            {
                CreateTableRow(rowValues.Skip(i).FirstOrDefault().ToList(), i);

            }
        }

        public void CreateHeaderRow(List<string> headerValues)
        {
            var xLocation = xOffset;
            var yLocation = yOffset;
            for (var i = 0; i < headerValues.Count(); i++)
            {
                if (i == headerValues.Count - 1)
                {
                    DrawRemarksCell(xLocation, yLocation);
                    gfx.DrawString(headerValues[i], boldFont, brush, xLocation + cellPadding, yLocation + cellPadding);
                }
                else
                {
                    DrawCell(xLocation, yLocation);
                    gfx.DrawString(headerValues[i], boldFont, brush, xLocation + cellPadding, yLocation + cellPadding);
                }
                xLocation += cellWidth;
            }
        }

        public void CreateTableRow(List<string> rowValues, int currentRowIndex)
        {
            var xLocation = xOffset;
            var yLocation = yOffset + cellHeight * ++currentRowIndex;
            for (var i = 0; i < rowValues.Count(); i++)
            {
                if (i == rowValues.Count - 1)
                {
                    DrawRemarksCell(xLocation, yLocation);
                    gfx.DrawString(rowValues[i] ?? string.Empty, normalFont, brush, xLocation + cellPadding, yLocation + cellPadding);
                    return;
                }
                if(i == 0)
                {
                    DrawCell(xLocation, yLocation);
                    gfx.DrawString(rowValues[i] ?? string.Empty, normalFont, brush, xLocation + cellPadding, yLocation + cellPadding);
                }
                else if (rowValues[i] == "1")
                {
                    DrawCellWithCross(xLocation, yLocation);
                }
                else
                {
                    DrawCell(xLocation, yLocation);
                }
                xLocation += cellWidth;
            }
        }

        private void DrawRemarksCell(int xLocation, int yLocation)
        {
            var leftUpPoint = new XPoint(xLocation, yLocation);
            var rightUpPoint = new XPoint(xLocation + remarksWidth, yLocation);
            var leftDownPoint = new XPoint(xLocation, yLocation + cellHeight);
            var rightDownPoint = new XPoint(xLocation + remarksWidth, yLocation + cellHeight);

            gfx.DrawLine(pen, leftUpPoint, rightUpPoint);
            gfx.DrawLine(pen, leftUpPoint, leftDownPoint);
            gfx.DrawLine(pen, leftDownPoint, rightDownPoint);
            gfx.DrawLine(pen, rightUpPoint, rightDownPoint);
        }

        private void DrawCell(int xLocation, int yLocation)
        {
            var leftUpPoint = new XPoint(xLocation, yLocation);
            var rightUpPoint = new XPoint(xLocation + cellWidth, yLocation);
            var leftDownPoint = new XPoint(xLocation, yLocation + cellHeight);
            var rightDownPoint = new XPoint(xLocation + cellWidth, yLocation + cellHeight);

            gfx.DrawLine(pen, leftUpPoint, rightUpPoint);
            gfx.DrawLine(pen, leftUpPoint, leftDownPoint);
            gfx.DrawLine(pen, leftDownPoint, rightDownPoint);
            gfx.DrawLine(pen, rightUpPoint, rightDownPoint);
        }

        private void DrawCellWithCross(int xLocation, int yLocation)
        {
            var leftUpPoint = new XPoint(xLocation, yLocation);
            var rightUpPoint = new XPoint(xLocation + cellWidth, yLocation);
            var leftDownPoint = new XPoint(xLocation, yLocation + cellHeight);
            var rightDownPoint = new XPoint(xLocation + cellWidth, yLocation + cellHeight);

            gfx.DrawLine(pen, leftUpPoint, rightUpPoint);
            gfx.DrawLine(pen, leftUpPoint, leftDownPoint);
            gfx.DrawLine(pen, leftDownPoint, rightDownPoint);
            gfx.DrawLine(pen, rightUpPoint, rightDownPoint);

            gfx.DrawLine(pen, leftUpPoint, rightDownPoint);
            gfx.DrawLine(pen, rightUpPoint, leftDownPoint);
        }

        public static void DrawCellWithCross(XGraphics gfx, XPen pen, int xLocation, int yLocation, int width, int height)
        {
            var leftUpPoint = new XPoint(xLocation, yLocation);
            var rightUpPoint = new XPoint(xLocation + width, yLocation);
            var leftDownPoint = new XPoint(xLocation, yLocation + height);
            var rightDownPoint = new XPoint(xLocation + width, yLocation + height);

            gfx.DrawLine(pen, leftUpPoint, rightUpPoint);
            gfx.DrawLine(pen, leftUpPoint, leftDownPoint);
            gfx.DrawLine(pen, leftDownPoint, rightDownPoint);
            gfx.DrawLine(pen, rightUpPoint, rightDownPoint);

            gfx.DrawLine(pen, leftUpPoint, rightDownPoint);
            gfx.DrawLine(pen, rightUpPoint, leftDownPoint);
        }
    }
}
