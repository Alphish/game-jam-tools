﻿using Alphicsh.JamTools.Common.Controls.Modals;
using Alphicsh.JamTools.Common.Mvvm.Modals;

namespace Alphicsh.JamPackager.Controls
{
    public static class ModalsRegistration
    {
        public static void Register()
        {
            ModalWindowMapping.Add<SimpleMessageViewModel, SimpleMessageModal>();
            ModalWindowMapping.Add<SaveOnCloseViewModel, SaveOnCloseModal>();
        }
    }
}
