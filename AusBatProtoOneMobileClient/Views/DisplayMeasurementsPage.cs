using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.Helpers;
using Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using static AusBatProtoOneMobileClient.ViewModels.DisplaySpeciesTabbedPageViewModel;

namespace AusBatProtoOneMobileClient
{
    public class DisplayMeasurementsPage : ContentPageBase
    {
        const float TRANSPARENCY = 0.7f;
        DisplayMeasurementsPageViewModel viewModel;
        MenuGenerator menu;

        public DisplayMeasurementsPage(DisplayMeasurementsPageViewModel viewModel) : base(viewModel)
        {
            this.viewModel = viewModel;
            BindingContext = viewModel;

            NavigationPage.SetTitleView(this, new Xamarin.Forms.Label { Text = "Measurements", Style = Styles.TitleLabelStyle });
            BackgroundColor = Color.Black;

            Content = GenerateLayout();

            menu = new MenuGenerator().Configure()
                .AddMenuItem("back", "Back", ToolbarItemOrder.Primary, (menuItem) => { viewModel.OnBackMenuPressed.Execute(null); });
                
            menu.GenerateToolbarItemsForPage(this);
            
        }

        /// <summary>
        /// If layout is portrait, split table into two and display them seperately
        /// </summary>
        /// <returns></returns>
        private View GenerateLayout()
        {
            List<Grid> grids = new List<Grid>();
            var orientation = DeviceDisplay.MainDisplayInfo.Orientation;
            switch (orientation)
            {
                case DisplayOrientation.Unknown:
                    break;
                case DisplayOrientation.Landscape:
                    grids.Add(GenerateGrid(viewModel.measurementsTable));
                    break;
                case DisplayOrientation.Portrait:
                    grids.Add(GenerateGrid(viewModel.measurementsTable.FirstHalf(), viewModel.measurementsTable.SecondHalf()));
                    break;
                default:
                    break;
            }

            var mainLayout = new StackLayout { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
            foreach (var grid in grids)
            {
                mainLayout.Children.Add(grid);
            }

            var backgroundImage = new Image { Aspect = Aspect.AspectFill, Source = viewModel.headImageSource };

            var centeredLayout = new Grid();
            centeredLayout.Children.Add(backgroundImage);
            centeredLayout.Children.Add(mainLayout);

            return centeredLayout;

            // Helper
            Grid GenerateGrid(HtmlTable topTable, HtmlTable bottomTable = null)
            {
                var grid = new Grid() { Margin = 5, Padding = 0, RowSpacing = 0, ColumnSpacing = 0 };
                topTable.Rows.ForEach((row) => {
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                });

                bottomTable?.Rows.ForEach((row) => {
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                });

                topTable.Rows[0].Columns.ForEach((col) => {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                });

                #region *// Table
                int left = 0, top = 0;
                var combinedRows = topTable.Rows.ToList();
                if (bottomTable != null) combinedRows.AddRange(bottomTable.Rows);
                foreach (var row in combinedRows)
                {
                    foreach (var col in row.Columns)
                    {
                        if (left == 0 && top == 0) { left++; continue; };
                        if (left == 0 && top == topTable.Rows.Count) { left++; continue; };

                        Color textColor = Color.White;
                        Color backgroundColor = Color.Black.MultiplyAlpha(TRANSPARENCY);
                        Color borderColor = Color.Black;
                        LayoutOptions horizontalLayoutOptions = LayoutOptions.Center;
                        if (left == 0)
                        {
                            textColor = Color.White;
                            backgroundColor = Color.Gray;
                            borderColor = Color.Gray;
                            horizontalLayoutOptions = LayoutOptions.Start;
                        }
                        if (top == 0 || top == topTable.Rows.Count)
                        {
                            textColor = Color.White;
                            backgroundColor = Color.Gray;
                            borderColor = Color.Black;
                        }

                        var label = new Label { Text = col.Value, TextColor = textColor, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), HorizontalOptions = horizontalLayoutOptions, VerticalOptions = LayoutOptions.Center, Margin = 1 };
                        var frame = new Frame { BorderColor = borderColor, HasShadow = false, Margin = 0, Padding = 0, Content = label, CornerRadius = 0, BackgroundColor = backgroundColor };

                        grid.Children.Add(frame, left++, top);
                    }
                    left = 0;
                    top++;
                } 
                #endregion

                return grid;
            }
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

        /// <summary>
        /// Used to handle screen orientation changes
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
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
