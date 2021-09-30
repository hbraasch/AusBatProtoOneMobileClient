using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using Xamarin.Forms;

namespace DocGenOneMobileClient.Views
{
    public class SearchResultsTabPage : ContentPageBase
    {

        public SearchResultsTabPage(SearchPageViewModel viewModel) : base(viewModel)
        {

            BindingContext = viewModel;

            var resultsListView = new ListView { SelectionMode = ListViewSelectionMode.Single };
            resultsListView.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(SearchPageViewModel.ResultsDisplayItems), BindingMode.TwoWay));
            resultsListView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SearchPageViewModel.ResultsSelectedItem), BindingMode.TwoWay));
            resultsListView.ItemTapped += (s, e) => { viewModel.OnResultsListTapped.Execute(null); };
            resultsListView.Refreshing += (s, e) => { };
            resultsListView.ItemTemplate = new DataTemplate(() => {
                var descriptionLabel = new Label { VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                descriptionLabel.SetBinding(Label.TextProperty, new Binding(nameof(SearchPageViewModel.ResultDisplayItem.Description), BindingMode.TwoWay));
                var descriptionLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { descriptionLabel }, Margin = 5 };
                return new ViewCell { View = descriptionLayout };
            });

            var scrollView = new ScrollView
            {
                Content = resultsListView,
                Orientation = ScrollOrientation.Vertical
            };

            var finalLayout = new AbsoluteLayout
            {
                Children = { scrollView, activityIndicator },
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = 5
            };
            AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(scrollView, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator, new Rectangle(0.5, .5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            Title = "Results";

            BackgroundImageSource = Constants.BACKGROUND_IMAGE;
            Content = finalLayout;


        }

    }
}
