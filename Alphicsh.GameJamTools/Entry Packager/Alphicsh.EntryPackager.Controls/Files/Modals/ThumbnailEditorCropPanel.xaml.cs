using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Alphicsh.EntryPackager.ViewModel.Entry.Files.Modals;

namespace Alphicsh.EntryPackager.Controls.Files.Modals
{
    /// <summary>
    /// Interaction logic for ThumbnailEditorCropPanel.xaml
    /// </summary>
    public partial class ThumbnailEditorCropPanel : UserControl
    {
        public ThumbnailEditorCropPanel()
        {
            InitializeComponent();
        }

        // -----------------
        // Binding ViewModel
        // -----------------

        private ThumbnailEditorCropViewModel ViewModel => (ThumbnailEditorCropViewModel)DataContext;

        private void Panel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = (ThumbnailEditorCropViewModel)e.NewValue;
            viewModel.ScrollToCropRequested += OnScrollToCropRequested;

            if (viewModel.HasSource)
                OnScrollToCropRequested(viewModel, EventArgs.Empty);
        }

        // ---------
        // Scrolling
        // ---------

        private void ScrollTo(double x, double y)
        {
            x = Math.Clamp(x, 0, CropViewer.ScrollableWidth);
            y = Math.Clamp(y, 0, CropViewer.ScrollableHeight);

            CropViewer.ScrollToHorizontalOffset(x);
            CropViewer.ScrollToVerticalOffset(y);
        }

        private void OnScrollToCropRequested(object? sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                var centerX = ViewModel.CropX + ViewModel.CropWidth / 2;
                var centerY = ViewModel.CropY + ViewModel.CropHeight / 2;

                var leftX = centerX - CropViewer.ViewportWidth / 2;
                var topY = centerY - CropViewer.ViewportHeight / 2;
                ScrollTo(leftX, topY);
            }));
        }

        // ------------
        // Mouse events
        // ------------

        private void CropUI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var leftPressed = e.ChangedButton == MouseButton.Left;
            var middlePressed = e.ChangedButton == MouseButton.Middle;
            e.Handled = true;

            if (sender == CropArea && leftPressed)
            {
                var counterpart = new Point(0, 0);
                var isCentered = Keyboard.Modifiers == ModifierKeys.Shift;
                DragBegin(element, CropArea, e, counterpart, isCentered ? PlaceCropMiddleContinue : PlaceCropCornerContinue);
            }
            else if (sender == CropArea && middlePressed)
            {
                var counterpart = new Point(CropViewer.HorizontalOffset, CropViewer.VerticalOffset);
                DragBegin(element, CropViewer, e, counterpart, MoveViewContinue);
            }
            else if (sender == CropWindow && leftPressed)
            {
                var counterpart = new Point(ViewModel.CropX, ViewModel.CropY);
                DragBegin(element, CropArea, e, counterpart, MoveCropContinue);
            }
            else
            {
                e.Handled = false;
            }
        }

        private void CropUI_MouseMove(object sender, MouseEventArgs e)
        {
            if (CapturingElement == null)
                return;

            DragContinue(e);
        }

        private void CropUI_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CapturingElement == null)
                return;

            DragContinue(e);
            DragEnd(e);
        }

        // -----------------
        // Drag & Drop utils
        // -----------------

        private FrameworkElement? CapturingElement { get; set; }
        private IInputElement? CaptureRelativeTo { get; set; }
        private Point? CapturePosition { get; set; }
        private Point? CounterpartPosition { get; set; }
        private Action<Vector>? DragAction { get; set; }

        private void DragBegin(FrameworkElement sender, IInputElement relativeTo, MouseButtonEventArgs e, Point counterpartPosition, Action<Vector> dragContinue)
        {
            CapturingElement = sender;
            CaptureRelativeTo = relativeTo;
            CapturePosition = e.GetPosition(CaptureRelativeTo);
            CounterpartPosition = counterpartPosition;
            DragAction = dragContinue;
            CapturingElement.CaptureMouse();
        }

        private void DragContinue(MouseEventArgs e)
        {
            if (CapturePosition == null)
                throw new InvalidOperationException($"Drag distance can only be performed in a middle of a drag and drop operation.");

            var newPosition = e.GetPosition(CaptureRelativeTo);
            var dragDistance = newPosition - CapturePosition.Value;
            DragAction!.Invoke(dragDistance);
        }

        private void DragEnd(MouseEventArgs e)
        {
            CapturingElement!.ReleaseMouseCapture();
            CapturingElement = null;
            CaptureRelativeTo = null;
            CapturePosition = null;
            CounterpartPosition = null;
            DragAction = null;
        }

        // ---------------
        // Drag operations
        // ---------------

        private void MoveViewContinue(Vector dragDistance)
        {
            var newOffset = CounterpartPosition!.Value - dragDistance;
            ScrollTo(newOffset.X, newOffset.Y);
        }

        private void MoveCropContinue(Vector dragDistance)
        {
            var newPosition = CounterpartPosition!.Value + dragDistance;
            ViewModel.MoveTo((int)newPosition.X, (int)newPosition.Y);
            ViewModel.UpdatePreview();
        }

        private void PlaceCropCornerContinue(Vector dragDistance)
        {
            var greaterDistance = Math.Max(Math.Abs(dragDistance.X), Math.Abs(dragDistance.Y));
            var newSize = Math.Clamp((int)greaterDistance, 48, 960);
            var xShift = dragDistance.X >= 0 ? 0 : -newSize;
            var yShift = dragDistance.Y >= 0 ? 0 : -newSize;
            ViewModel.ResizeTo(newSize);

            var newPosition = CapturePosition!.Value + new Vector(xShift, yShift);
            ViewModel.MoveTo((int)newPosition.X, (int)newPosition.Y);

            ViewModel.UpdatePreview();
        }

        private void PlaceCropMiddleContinue(Vector dragDistance)
        {
            var greaterDistance = Math.Max(Math.Abs(dragDistance.X), Math.Abs(dragDistance.Y));
            var newSize = Math.Clamp(2 * (int)greaterDistance, 48, 960);
            ViewModel.ResizeTo(newSize);

            var newPosition = CapturePosition!.Value - new Vector(newSize / 2, newSize / 2);
            ViewModel.MoveTo((int)newPosition.X, (int)newPosition.Y);

            ViewModel.UpdatePreview();
        }
    }
}
