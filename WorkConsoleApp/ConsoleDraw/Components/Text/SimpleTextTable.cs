using ConsoleDraw.Components.ComponentStyling;
using ConsoleDraw.DoubleBuffer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDraw.Components.Text
{
    public class SimpleTextTable : BaseComponent, IDataComponent<string>
    {
        private readonly IConsoleBuffer ConsoleBuffer;
        private IEnumerable<IEnumerable<string>> Data;
        private List<int> CellWidths = new List<int>();
        private bool LastRowIsSum;
        private ConsoleColor OddLineForegroundColor;
        private ConsoleColor OddLineBackgroundColor;

        public SimpleTextTable(IConsoleBuffer consoleBuffer)
        {
            ConsoleBuffer = consoleBuffer ?? throw new ArgumentNullException(nameof(consoleBuffer));
            Lines = new List<string>();
        }

        //TODO: the table data should be compiled from some kind of real model, maybe passing in a lambda for the table data instead of just an array of arrays.
        //public void Display(IEnumerable<ConsoleViewModel> models)
        //{
        //    var table = new SimpleTable(
        //        models.Select(m => new[] { FormatDate(m.Date), m.Description, FormatAmount(m.Value) }),
        //        targetWidth: Console.WindowWidth - 1,
        //        lastRowIsSum: true,
        //        border: true);
        //    table.Draw();
        //}
        public void LoadData(DataTableConfig<string> dataTableConfig)
        {
            Data = dataTableConfig.Data;
            foreach (var _ in Data.FirstOrDefault())
            {
                CellWidths.Add(0);
            }
            LastRowIsSum = dataTableConfig.LastRowIsSum;
            OddLineForegroundColor = dataTableConfig.OddLineForegroundColor;
            OddLineBackgroundColor = dataTableConfig.OddLineBackgroundColor;
            var tempLines = Data.Select(x => GetSampleLine()).ToList();
            if (Layout.Border)
            {
                tempLines.Insert(0, GetSampleLine());
                tempLines.Add(GetSampleLine());
            }
            for (var i = 0; i < Layout.MarginTop; i++)
            {
                tempLines.Insert(0, GetSampleLine());
            }
            for (var i = 0; i < Layout.MarginBottom; i++)
            {
                tempLines.Add(GetSampleLine());
            }
            Lines = tempLines;
            ReflowComponentLayout();
        }

        public override void ReflowComponentLayout()
        {
            CellWidths = CellWidths.Select(x => 0).ToList();
            foreach (var row in Data)
            {
                var lengths = row.Select(c => c.Length).ToArray();
                for (var i = 0; i < lengths.Length; i++)
                {
                    CellWidths[i] = Math.Max(lengths[i], CellWidths[i]);
                }
            }
            var targetWidth = Layout.SpaceToFillWidth;
            if (targetWidth > 0)
            {
                var currentTableWidth = GetCurrentTableWidth();
                if (currentTableWidth < targetWidth)
                {
                    var difference = targetWidth - currentTableWidth;
                    var allColsSum = CellWidths.Sum();
                    var coveredSoFar = 0;
                    for (var i = 0; i < CellWidths.Count; i++)
                    {
                        var myShare = (int)(((float)CellWidths[i] / allColsSum) * difference);
                        coveredSoFar += myShare;
                        var remaining = difference - coveredSoFar;
                        if (i == CellWidths.Count - 1)
                        {
                            myShare += remaining;
                        }
                        CellWidths[i] += myShare;
                    }
                }
                //TODO: what to do if the current width is beyond the target width? Probably need to start word-wrapping some of the cells?
            }
            base.ReflowComponentLayout();
        }

        public override void Draw(int originX, int originY)
        {
            var linesList = Data.Select(row => EncapsulateLine(string.Join(CellSeparation(), row.Select((c, i) => GetCell(c, CellWidths[i]))))).ToList();
            if (Layout.Border)
            {
                //TODO: the dashed lines extend into the margins
                linesList.Insert(0, '-'.RepeatCharacter(GetCurrentTableWidth()));
                linesList.Add('-'.RepeatCharacter(GetCurrentTableWidth()));
            }
            for (var i = 0; i < Layout.MarginTop; i++)
            {
                linesList.Insert(0, ' '.RepeatCharacter(GetCurrentTableWidth()));
            }
            for (var i = 0; i < Layout.MarginBottom; i++)
            {
                linesList.Add(' '.RepeatCharacter(GetCurrentTableWidth()));
            }
            var data = linesList.ToArray();
            var lastLine = 0;
            for (var i = 0; i < data.Length; i++)
            {
                var isEven = (i + lastLine) % 2 == 0;
                var print = data[i];
                if (isEven)
                {
                    ComponentStyleAndLayout.WriteColors(ConsoleBuffer);
                }
                else
                {
                    ConsoleBuffer.SetColors(OddLineForegroundColor, OddLineBackgroundColor);
                }
                if (LastRowIsSum && i == data.Length - 1 - Layout.MarginBottom - Layout.BottomBorderThickness)
                {
                    //TODO: the dashed line extends into the margins
                    ConsoleBuffer.WriteLineToBuffer('-'.RepeatCharacter(print.Length), originX + Layout.X, originY + Layout.Y + i);
                    lastLine = 1;
                    if (isEven)
                    {
                        ConsoleBuffer.SetColors(OddLineForegroundColor, OddLineBackgroundColor);
                    }
                    else
                    {
                        ComponentStyleAndLayout.WriteColors(ConsoleBuffer);
                    }
                }
                ConsoleBuffer.WriteLineToBuffer(print, originX + Layout.X, originY + Layout.Y + i + lastLine);
            }
            base.Draw(originX, originY);
        }

        private string GetSampleLine() => EncapsulateLine(string.Join(CellSeparation(), CellWidths.Select(c => 'x'.RepeatCharacter(c))));

        private int GetCurrentTableWidth() => GetSampleLine().Length;

        private string CellSeparation()
        {
            return Layout.Border
                ? ' '.RepeatCharacter(Layout.PaddingRight) + "|" + ' '.RepeatCharacter(Layout.PaddingLeft)
                : ' '.RepeatCharacter(Layout.PaddingLeft + Layout.PaddingRight);
        }

        private string EncapsulateLine(string line)
        {
            var sideLeft = ' '.RepeatCharacter(Layout.MarginLeft)
                + (Layout.Border ? "|" : "")
                + (' '.RepeatCharacter(Layout.PaddingLeft));
            var sideRight = ' '.RepeatCharacter(Layout.PaddingRight)
                + (Layout.Border ? "|" : "")
                + ' '.RepeatCharacter(Layout.MarginRight);
            return  sideLeft + line + sideRight;
        }

        private string GetCell(string text, int width)
        {
            //SIMPLE VERSION, does not support cell alignment or anything
            return text + ' '.RepeatCharacter(width - text.Length);
        }
    }
}
