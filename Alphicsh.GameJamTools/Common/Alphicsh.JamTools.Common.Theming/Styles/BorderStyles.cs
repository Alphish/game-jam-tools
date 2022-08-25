using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Theming.Styles
{
    public partial class BorderStyles : ResourceDictionary
    {
        private static T? FindClosestAncestor<T>(DependencyObject source) where T : DependencyObject
        {
            DependencyObject ancestor = source;

            while (ancestor != null && ancestor is not T)
            {
                ancestor = VisualTreeHelper.GetParent(ancestor);
            }

            return (T?)ancestor;
        }

        private void ModalHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var senderWindow = FindClosestAncestor<Window>((DependencyObject)sender)!;
            senderWindow.DragMove();
        }
    }
}
