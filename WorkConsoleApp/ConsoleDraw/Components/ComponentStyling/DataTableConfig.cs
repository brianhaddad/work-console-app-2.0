using System;
using System.Collections.Generic;

namespace ConsoleDraw.Components.ComponentStyling
{
    public class DataTableConfig<T>
    {
        public IEnumerable<IEnumerable<T>> Data { get; set; }
        public bool LastRowIsSum { get; set; }
        public ConsoleColor OddLineBackgroundColor { get; set; }
        public ConsoleColor OddLineForegroundColor { get; set; }
    }
}
