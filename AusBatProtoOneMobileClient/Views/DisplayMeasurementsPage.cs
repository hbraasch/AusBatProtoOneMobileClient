using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using static AusBatProtoOneMobileClient.ViewModels.DisplaySpeciesTabbedPageViewModel;

namespace AusBatProtoOneMobileClient
{
    public class DisplayMeasurementsPage : ContentPageBase
    {
        DisplayMeasurementsPageViewModel viewModel;
        MenuGenerator menu;
        public DisplayMeasurementsPage(DisplayMeasurementsPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;



            NavigationPage.SetTitleView(this, new Xamarin.Forms.Label { Text = "Measurements", Style = Styles.TitleLabelStyle });
            Content = GenerateLayout();

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); }, iconPath: "ic_back.png");
                
            menu.GenerateToolbarItemsForPage(this);
            menu.SetBinding(MenuGenerator.InvalidateCommandProperty, new Binding(nameof(DisplayMeasurementsPageViewModel.InvalidateMenuCommand), BindingMode.OneWayToSource, source: viewModel));
            menu.FindMenuUnit("back").isVisible = viewModel.IsPageReturnable;
            
        }

        private View GenerateLayout()
        {
            List<Grid> grids = new List<Grid>();
            var orientation = DeviceDisplay.MainDisplayInfo.Orientation;
            switch (orientation)
            {
                case DisplayOrientation.Unknown:
                    break;
                case DisplayOrientation.Landscape:
                    grids.Add(GenerateGrid(viewModel.MeasurementsTable));
                    break;
                case DisplayOrientation.Portrait:
                    grids.Add(GenerateGrid(viewModel.MeasurementsTable.FirstHalf()));
                    grids.Add(GenerateGrid(viewModel.MeasurementsTable.SecondHalf()));
                    break;
                default:
                    break;
            }

            var mainLayout = new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand };
            foreach (var grid in grids)
            {
                mainLayout.Children.Add(grid);
            }
            return mainLayout;
        }

        private Grid GenerateGrid(HtmlTable table)
        {
            var grid = new Grid() { Margin = 5, Padding = 0, RowSpacing = 0, ColumnSpacing = 0 };
            table.Rows.ForEach((row) => {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            });

            table.Rows[0].Columns.ForEach((col) => {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            });

            int left = 0, top = 0;
            foreach (var row in table.Rows)
            {
                foreach (var col in row.Columns)
                {
                    if (left == 0 && top == 0) { left++; continue; };

                    Color backgroundColor = Color.White;
                    Color borderColor = Color.Black;
                    LayoutOptions horizontalLayoutOptions = LayoutOptions.Center;
                    if (left == 0)
                    {
                        backgroundColor = Color.LightGray;
                        borderColor = Color.LightGray;
                        horizontalLayoutOptions = LayoutOptions.Start;
                    }
                    if (top == 0)
                    {
                        backgroundColor = Color.Gray;
                        borderColor = Color.Gray;
                    }

                    var label = new Label { Text = col.Value, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), HorizontalOptions = horizontalLayoutOptions, VerticalOptions = LayoutOptions.Center, Margin = 1 };
                    var frame = new Frame { BorderColor = borderColor, Margin = 0, Padding = 0, Content = label, CornerRadius = 0, BackgroundColor = backgroundColor };

                    grid.Children.Add(frame, left++, top);
                }
                left = 0;
                top++;
            }

            return grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Appearance.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            viewModel.OnBackButtonPressed.Execute(null);
            return viewModel.isBackCancelled;
        }

        private double width = 0;
        private double height = 0;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); // Must be called
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;
                // Reconfigure layout
                Content = GenerateLayout();
            }
        }
    }
}
