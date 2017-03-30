using System.Windows.Input;

namespace LogReader
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Search = new RoutedUICommand
        (
            "Search",
            "Search",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand SearchNext = new RoutedUICommand
        (
            "Next",
            "SearchNext",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Down, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand SearchPrev = new RoutedUICommand
        (
            "Prev",
            "SearchPrev",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Up, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand ToggleFilter = new RoutedUICommand
        (
            "Toggle Filter",
            "ToggleFilter",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.I, ModifierKeys.Control|ModifierKeys.Shift)
            }
        );

        public static readonly RoutedUICommand SetFilter = new RoutedUICommand
        (
            "Filter...",
            "SetFilter",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.I, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand SetFilterForSearch = new RoutedUICommand
        (
            "Set filter for search",
            "SetFilterForSearch",
            typeof(CustomCommands),
            new InputGestureCollection()
            {

            }
        );

        public static readonly RoutedUICommand SetFormat = new RoutedUICommand
        (
            "Set appearance for log",
            "SetFormat",
            typeof(CustomCommands),
            new InputGestureCollection()
            {

            }
        );

        public static readonly RoutedUICommand SetParser = new RoutedUICommand
        (
            "Columns",
            "SetParser",
            typeof(CustomCommands),
            new InputGestureCollection()
            {

            }
        );

        public static readonly RoutedUICommand CloseAll = new RoutedUICommand
        (
            "Close All",
            "CloseAll",
            typeof(CustomCommands),
            new InputGestureCollection()
            {

            }
        );

        public static readonly RoutedUICommand Exit = new RoutedUICommand
        (
            "Exit",
            "Exit",
            typeof(CustomCommands),
            new InputGestureCollection()
            {

            }
        );

    }

}
