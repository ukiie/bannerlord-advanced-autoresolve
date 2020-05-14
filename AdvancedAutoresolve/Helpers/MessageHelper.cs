using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace AdvancedAutoResolve.Helpers
{
    internal static class MessageHelper
    {
        internal static void DisplayText(string text, DisplayTextStyle displayTextStyle = DisplayTextStyle.Normal)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            InformationManager.DisplayMessage(new InformationMessage(text, displayTextStyle.GetColorFromDsiplayTextStyle()));
        }

        private static Color GetColorFromDsiplayTextStyle(this DisplayTextStyle displayTextStyle)
        {
            switch (displayTextStyle)
            {
                case DisplayTextStyle.Info:
                    return new Color(0.12f, 0.73f, 1f);
                case DisplayTextStyle.Warning:
                    return new Color(1, 0, 0);
                case DisplayTextStyle.Success:
                    return new Color(0, 1, 0);
                default:
                    return Color.White;
            }
        }
    }

    internal enum DisplayTextStyle
    {
        Normal,
        Info,
        Success,
        Warning
    }
}
