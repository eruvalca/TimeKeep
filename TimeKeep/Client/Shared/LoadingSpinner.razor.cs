using Microsoft.AspNetCore.Components;

namespace TimeKeep.Client.Shared
{
    public partial class LoadingSpinner
    {
        [Parameter]
        public int? Margin { get; set; }
        [Parameter]
        public int? MarginTop { get; set; }
        [Parameter]
        public int? MarginBottom { get; set; }
        [Parameter]
        public int? MarginRight { get; set; }
        [Parameter]
        public int? MarginLeft { get; set; }
        [Parameter]
        public int? FontSize { get; set; }

        private string StyleText { get; set; }

        protected override void OnParametersSet()
        {
            if (Margin is not null || MarginTop is not null || MarginBottom is not null || MarginRight is not null || MarginLeft is not null || FontSize is not null)
            {
                StyleText = string.Empty;

                if (Margin is not null)
                {
                    StyleText += $"margin: {Margin}px; ";
                }
                else
                {
                    if (MarginTop is not null)
                    {
                        StyleText += $"margin-top: {MarginTop}px; ";
                    }

                    if (MarginBottom is not null)
                    {
                        StyleText += $"margin-bottom: {MarginBottom}px; ";
                    }

                    if (MarginRight is not null)
                    {
                        StyleText += $"margin-right: {MarginRight}px; ";
                    }

                    if (MarginLeft is not null)
                    {

                        StyleText += $"margin-left: {MarginLeft}px; ";
                    }
                }

                if (FontSize is not null)
                {
                    StyleText += $"font-size: {FontSize}px;";
                }
            }
        }
    }
}
