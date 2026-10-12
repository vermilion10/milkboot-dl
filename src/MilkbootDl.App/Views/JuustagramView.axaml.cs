using Avalonia.Controls;
using MilkbootDl.App.ViewModels;

namespace MilkbootDl.App.Views;

public partial class JuustagramView : UserControl
{
    public JuustagramView()
    {
        InitializeComponent();

        // A newly picked post starts at its top.
        DataContextChanged += (_, _) =>
        {
            if (DataContext is JuustagramViewModel juustagram)
            {
                juustagram.PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName == nameof(JuustagramViewModel.SelectedPost))
                    {
                        PostScroll.ScrollToHome();
                    }
                };
            }
        };
    }
}
