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

namespace DocGenOneMobileClient.Views
{
    public class SearchPageViewModel : ViewModelBase
    {

        public class CriteriaDisplayItemBase
        {
            public int Id { get; set; }
            public string Description { get; set; } = "XXXXXXX";
            public int DisplayOrder { get; set; }
            public Action<CriteriaDisplayItemBase> OnChanged { get; set; }
            public object Content { get; set; }

        }
        public class MapRegionsDisplayItem: CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Regions";
            public List<MapRegion> MapRegions { get; set; }
        }
        public class ForeArmLengthDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Forearm length (mm)";
            public float Value { get; set; }
        }
        public class OuterCanineWidthDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Outer canine width (mm)";
            public float Value { get; set; }
        }
        public class TailLengthDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Tail length (mm)";
            public float Value { get; set; }
        }
        public class FootWithClawLengthDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Foot with claw length (mm)";
            public float Value { get; set; }
        }

        public class PenisLengthDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Penis length (mm)";
            public float Value { get; set; }
        }
        public class HeadToBodyLengthDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Head to body length (mm)";
            public float Value { get; set; }
        }
        public class WeightDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Weight (g)";
            public float Value { get; set; }
        }
        public class ThreeMetDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "3-MET (mm)";
            public float Value { get; set; }
        }

        public class IsGularPoachPresentDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Gular Poach present?";
            public bool Value { get; set; }
        }
        public class HasFleshyGenitalProjectionsDisplayItem : CriteriaDisplayItemBase
        {
            //public string Description { get; set; } = "Has fleshy genital projections?";
            public bool Value { get; set; }
        }

        public ObservableCollection<CriteriaDisplayItemBase> CriteriaDisplayItems { get; set; }

        public DisplayItem CriteriaSelectedItem { get; set; }

        public ObservableCollection<DisplayItem> ResultsDisplayItems { get; set; }

        public DisplayItem ResultsSelectedItem { get; set; }

        public void OnSelectedItemChanged()
        {
            IsSelected = CriteriaSelectedItem != null;
            InvalidateMenu.Execute(null);
        }


        public bool IsDirty { get; set; }

        public ICommand InvalidateMenu { get; set; }

        CommandHelper commandHelper = new CommandHelper();

        CancellationTokenSource cts;


        #region *// Menu related 

        public bool IsSelected { get; set; }

        #endregion


        public SearchPageViewModel()
        {
            CriteriaDisplayItems = new ObservableCollection<CriteriaDisplayItemBase>();
            ResultsDisplayItems = new ObservableCollection<DisplayItem>();
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

                CriteriaDisplayItems = GenerateCriteriaDisplay();
                ResultsDisplayItems = UpdateResultsDisplay();

            }
            catch (Exception ex) when (ex is TaskCanceledException ext)
            {
                Debug.Write("Cancelled by user");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.CompleteMessage(), "Ok");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });

        public ObservableCollection<CriteriaDisplayItemBase> GenerateCriteriaDisplay()
        {
            var displayItems = new ObservableCollection<CriteriaDisplayItemBase>();
            displayItems.Add(new MapRegionsDisplayItem());
            displayItems.Add(new ForeArmLengthDisplayItem());
#if false
            displayItems.Add(new OuterCanineWidthDisplayItem());
            displayItems.Add(new TailLengthDisplayItem());
            displayItems.Add(new FootWithClawLengthDisplayItem());
            displayItems.Add(new PenisLengthDisplayItem());
            displayItems.Add(new HeadToBodyLengthDisplayItem());
            displayItems.Add(new WeightDisplayItem());
            displayItems.Add(new ThreeMetDisplayItem());
            displayItems.Add(new IsGularPoachPresentDisplayItem());
            displayItems.Add(new HasFleshyGenitalProjectionsDisplayItem()); 
#endif
            return displayItems;
        }

        private void OnCriteriaChanged(CriteriaDisplayItemBase criteria)
        {
            switch (criteria)
            {
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        public ObservableCollection<ListViewDataTemplate.DisplayItem> UpdateResultsDisplay()
        {
            var displayItems = new ObservableCollection<ListViewDataTemplate.DisplayItem>();
            return displayItems;
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


        public ICommand OnSearchButtonPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {

                ActivityIndicatorStart();


                CriteriaDisplayItems = GenerateCriteriaDisplay();

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Problem: ", ex.Message, "OK");
            }
            finally
            {
                ActivityIndicatorStop();
            }
        });



        public ICommand OnEditMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {
                if (CriteriaSelectedItem == null) return;
                ActivityIndicatorStart();


                GenerateCriteriaDisplay();

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


        public ICommand OnDeleteMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {
            try
            {

                GenerateCriteriaDisplay();

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

        public ICommand OnRenameMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                ActivityIndicatorStart();


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


