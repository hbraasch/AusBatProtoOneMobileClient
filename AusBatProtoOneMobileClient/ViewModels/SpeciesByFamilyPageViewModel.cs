﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using static DocGenOneMobileClient.Views.TemplateListPage;
using static DocGenOneMobileClient.Views.TemplateListPage.ListViewDataTemplate;
using System.Threading.Tasks;
using AusBatProtoOneMobileClient;
using AusBatProtoOneMobileClient.ViewModels;
using Mobile.ViewModels;
using Mobile.Helpers;
using TreeApp.Helpers;
using AusBatProtoOneMobileClient.Models;
using AusBatProtoOneMobileClient.Data;

namespace DocGenOneMobileClient.Views
{
    public class SpeciesByFamilyPageViewModel : ViewModelBase
    {
        public class SpeciesDisplayItem
        {
            public string SpeciesName { get; set; }
            public string FriendlyName { get; set; }
            public string ImageSource { get; set; }
            public Bat Bat { get; set; }
        }
        public class GroupedSpeciesDisplayItem : ObservableCollection<SpeciesDisplayItem>
        {
            public string FamilyName { get; set; }
            public string ImageSource { get; set; }
        }

        public ObservableCollection<GroupedSpeciesDisplayItem> FamilyGroupDisplayItems { get; set; }

        public GroupedSpeciesDisplayItem pageTypeGroup {get;set;}

        public SpeciesDisplayItem SelectedItem { get; set; }

        public void OnSelectedItemChanged()
        {
            IsSelected = SelectedItem != null;
            InvalidateMenu.Execute(null);
        }

       
        public bool IsDirty { get; set; }

        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsSelected { get; set; }

        #endregion
          

        public SpeciesByFamilyPageViewModel()
        {
            FamilyGroupDisplayItems = new ObservableCollection<GroupedSpeciesDisplayItem>();
        }

        public ICommand OnFirstAppearance => new Command(async () =>
        {
            try
            {

                cts = new CancellationTokenSource();

                ActivityIndicatorStart(() => {
                    cts.Cancel();
                    ActivityIndicatorStop();
                });

                UpdateDisplay();


            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                Debug.Write("Cancelled by user");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",ex.CompleteMessage(), "Ok");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });

        public void UpdateDisplay()
        {

            FamilyGroupDisplayItems = new ObservableCollection<GroupedSpeciesDisplayItem>();

            foreach (var family in App.dbase.Classifications.Where(o => o.Type == Classification.ClassificationType.Family))
            {
                var genusInFamily = App.dbase.Classifications.Where(o => o.Parent == family.Id).ToList();
                var speciesInFamily = App.dbase.Classifications.Where(o => genusInFamily.Select(g => g.Id).Contains(o.Parent)).ToList();
                var familyGroupDisplayItem = new GroupedSpeciesDisplayItem { FamilyName = family.Id };
                foreach (var species in speciesInFamily)
                {
                    var bat = App.dbase.Bats.FirstOrDefault(o => o.SpeciesId.ToLower() == species.Id.ToLower());
                    if (bat != null)
                    {
                        familyGroupDisplayItem.Add(new SpeciesDisplayItem
                        {
                            SpeciesName = species.Id,
                            FriendlyName = bat?.Name,
                            Bat = bat,
                            ImageSource = bat.Images.First()
                        }); 
                    }
                }
                FamilyGroupDisplayItems.Add(familyGroupDisplayItem);
            }
            Debug.WriteLine($"Data count: {FamilyGroupDisplayItems.Count}"); 

        }


        public ICommand OnSubsequentAppearance => new Command(() =>
        {
            try
            {
                ActivityIndicatorStart();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.CompleteMessage());
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });

        public ICommand OnBackMenuPressed => new Command(() =>
        {
            NavigateBack(true);
        });

        public bool isBackCancelled = false;
        public ICommand OnBackButtonPressed => new Command(() =>
        {
            isBackCancelled = true;
        });


        public ICommand OnSelectMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                if (SelectedItem == null) return;
                var viewModel = new DisplayBatTabbedPageViewModel(SelectedItem.Bat);
                var page = new DisplayBatTabbed(viewModel);
                await NavigateToPageAsync(page, viewModel);

            }
            catch (Exception ex) when (ex is BusinessException exb)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", exb.CompleteMessage(), "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Problem: ", ex.CompleteMessage(), "OK");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });
    }
}
    

