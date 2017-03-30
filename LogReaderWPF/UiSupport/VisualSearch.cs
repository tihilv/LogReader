using System;
using System.Windows;
using System.Windows.Media;

namespace LogReader
{
    class VisualSearch
    {
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            return FindVisualChild<T>(obj, arg => true);
        }
        public static T FindVisualChild<T>(DependencyObject obj, Func<T, bool> predicate) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                var visualChild = child as T;
                if (visualChild != null && predicate(visualChild))
                    return visualChild;
                else
                {
                    T childOfChild = FindVisualChild<T>(child, predicate);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

    }
}
