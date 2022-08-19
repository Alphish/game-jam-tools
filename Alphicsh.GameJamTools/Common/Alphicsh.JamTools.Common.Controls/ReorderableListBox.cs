using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Alphicsh.JamTools.Common.Controls
{
    public abstract class ReorderableListBox<TItem> : ListBox
        where TItem : class
    {
        public ReorderableListBox() : base()
        {
            AllowDrop = true;
            PreviewMouseLeftButtonDown += OnElementClicked_TryTake;
            PreviewMouseMove += OnDrag_MoveElement;
            PreviewMouseLeftButtonUp += OnElementRelease_LeaveElement;
            Drop += OnDrop_TryPlaceElement;
        }

        // ----------
        // After drop
        // ----------

        public virtual void AfterDrop(TItem item) { }

        // -----------------------
        // Handling the reordering
        // -----------------------

        private UIElement? SelectedElementContainer = null;

        private void OnElementClicked_TryTake(object sender, MouseButtonEventArgs e)
        {
            if (!AllowDrop)
                return;

            var mouseInListBoxPosition = e.GetPosition(this);
            SelectedElementContainer = GetElementAtPosition(mouseInListBoxPosition);
        }

        private void OnDrag_MoveElement(object sender, MouseEventArgs e)
        {
            if (!AllowDrop)
                return;

            if (SelectedElementContainer == null)
                return;

            var mouseInListBoxPosition = e.GetPosition(this);
            UIElement? hoverElementContainer = GetElementAtPosition(mouseInListBoxPosition);
            if (hoverElementContainer == SelectedElementContainer)
                return;

            var draggedItem = (TItem)ItemContainerGenerator.ItemFromContainer(SelectedElementContainer);
            SelectedItem = draggedItem;

            SelectedElementContainer = null;
            var dragData = new DragData(this, draggedItem);
            DragDrop.DoDragDrop(this, new DataObject(typeof(DragData), dragData), DragDropEffects.Move);
        }

        private void OnElementRelease_LeaveElement(object sender, MouseButtonEventArgs e)
        {
            if (!AllowDrop)
                return;

            SelectedElementContainer = null;
        }

        private void OnDrop_TryPlaceElement(object sender, DragEventArgs e)
        {
            if (!AllowDrop)
                return;

            var dragData = e.Data.GetData(typeof(DragData)) as DragData;
            if (dragData == null)
                return;

            var mouseInListBoxPosition = e.GetPosition(this);
            UIElement? hoverElementContainer = GetElementAtPosition(mouseInListBoxPosition);
            object? hoverElement = hoverElementContainer != null
                ? ItemContainerGenerator.ItemFromContainer(hoverElementContainer)
                : null;

            var droppedItem = dragData.Item;
            if (object.Equals(hoverElement, droppedItem))
                return;

            var sourceListBox = dragData.SourceList;
            var source = (IList<TItem>)sourceListBox.ItemsSource;
            var originalIndex = source.IndexOf(droppedItem);
            source.Remove(droppedItem);

            var destination = (IList<TItem>)ItemsSource;
            if (hoverElement == null)
            {
                destination.Add(droppedItem);
            }
            else
            {
                var hoverElementIndex = Items.IndexOf(hoverElement);
                var isMouseAtBottomHalf = e.GetPosition(hoverElementContainer).Y >= ((FrameworkElement)hoverElementContainer!).ActualHeight / 2;
                var insertIndex = hoverElementIndex + (isMouseAtBottomHalf ? 1 : 0);
                insertIndex -= (source == destination && insertIndex >= originalIndex) ? 1 : 0;
                destination.Insert(insertIndex, droppedItem);
            }

            AfterDrop(droppedItem);
        }

        private UIElement? GetElementAtPosition(Point point)
        {
            UIElement? container = VisualTreeHelper.HitTest(this, point)?.VisualHit as UIElement;
            if (container == null)
                return null;

            object item = ItemContainerGenerator.ItemFromContainer(container);

            // search for UI element that directly corresponds to the ListBox item
            while (item == DependencyProperty.UnsetValue)
            {
                container = VisualTreeHelper.GetParent(container) as UIElement;
                if (container == this)
                    return null;

                item = ItemContainerGenerator.ItemFromContainer(container);
            }

            return container;
        }

        // ---------
        // Drag data
        // ---------

        private class DragData
        {
            public ReorderableListBox<TItem> SourceList { get; }
            public TItem Item { get; }

            public DragData(ReorderableListBox<TItem> sourceList, TItem item)
            {
                SourceList = sourceList;
                Item = item;
            }
        }
    }
}
