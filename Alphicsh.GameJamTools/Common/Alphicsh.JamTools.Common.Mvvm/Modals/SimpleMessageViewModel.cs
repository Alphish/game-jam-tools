namespace Alphicsh.JamTools.Common.Mvvm.Modals
{
    public class SimpleMessageViewModel : ModalViewModel
    {
        public string Message { get; }

        public SimpleMessageViewModel(string caption, string message)
            : base(caption)
        {
            Message = message;
        }

        public static void ShowModal(string caption, string message)
        {
            var viewModel = new SimpleMessageViewModel(caption, message);
            viewModel.ShowOwnModal();
        }
    }
}
