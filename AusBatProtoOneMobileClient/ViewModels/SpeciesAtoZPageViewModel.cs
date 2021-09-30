using System;
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
    public class SpeciesAtoZPageViewModel : ViewModelBase
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
            public string Alphabet { get; set; }
        }

        public ObservableCollection<GroupedSpeciesDisplayItem> SpeciesGroupDisplayItems { get; set; }

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
          

        public SpeciesAtoZPageViewModel()
        {
            SpeciesGroupDisplayItems = new ObservableCollection<GroupedSpeciesDisplayItem>();
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

            SpeciesGroupDisplayItems = new ObservableCollection<GroupedSpeciesDisplayItem>();

            var bats = App.dbase.Bats.OrderBy(bat=>$"{bat.GenusId} {bat.SpeciesId}");
            GroupedSpeciesDisplayItem familyGroupDisplayItem = null;
            foreach (var bat in bats)
            {
                var alphabet = bat.GenusId.Substring(0, 1);
                if (!SpeciesGroupDisplayItems.ToList().Exists(o => o.Alphabet == alphabet))
                {
                    familyGroupDisplayItem = new GroupedSpeciesDisplayItem { Alphabet = alphabet.ToUpper() };
                    SpeciesGroupDisplayItems.Add(familyGroupDisplayItem);
                }
                familyGroupDisplayItem.Add(new SpeciesDisplayItem
                {
                    SpeciesName = $"{bat.GenusId} {bat.SpeciesId.ToLower()}",
                    FriendlyName = bat.Name,
                    ImageSource = bat.Images.First(),
                    Bat = bat
                });                
            }






































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
    

