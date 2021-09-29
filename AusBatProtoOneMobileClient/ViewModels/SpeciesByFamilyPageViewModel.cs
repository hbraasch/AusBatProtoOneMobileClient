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
    public class SpeciesByFamilyPageViewModel : ViewModelBase
    {
        public class PageModel
        {
            public string SpeciesName { get; set; }
            public string FriendlyName { get; set; }
        }
        public class PageTypeGroup : List<PageModel>
        {
            public string FamilyName { get; set; }
            public string ShortFamilyName { get; set; } // Will be used for jump lists
            public string FriendlyName { get; set; }

            public PageTypeGroup() { }
            public PageTypeGroup(string familyName, string shortFamilyName)
            {
                FamilyName = familyName;
                ShortFamilyName = shortFamilyName;
            }

            public static IList<PageTypeGroup> All { set; get; }
        }

        public PageTypeGroup pageTypeGroup {get;set;}

        public PageModel SelectedItem { get; set; }

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
            pageTypeGroup = new PageTypeGroup();
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

            var Groups = new List<PageTypeGroup>();
            foreach (var family in App.dbase.Classifications.Where(o=>o.Type == Classification.ClassificationType.Family))
            {
                var genusInFamily = App.dbase.Classifications.Where(o => o.Parent == family.Id).ToList();
                var speciesInFamily = App.dbase.Classifications.Where(o => genusInFamily.Select(g=>g.Id).Contains(o.Parent)).ToList();
                var pageTypeGroup = new PageTypeGroup(family.Id, family.Id.Substring(4));
                foreach (var species in speciesInFamily)
                {
                    pageTypeGroup.Add(new PageModel { SpeciesName = species.Id, FriendlyName = App.dbase.Bats.FirstOrDefault(o => o.SpeciesId.ToLower() == species.Id.ToLower())?.Name});
                }
                Groups.Add(pageTypeGroup);
            }
            PageTypeGroup.All = Groups;
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

        
    }
}
    

