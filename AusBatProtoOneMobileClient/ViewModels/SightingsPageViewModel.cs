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
using Xamarin.Essentials;
using System.IO;
using Newtonsoft.Json;

namespace DocGenOneMobileClient.Views
{
    public class SightingsPageViewModel : ViewModelBase
    {


        public class DisplayItem 
        {
            public string SpeciesName { get; set; }
            public string SpeciesFriendlyName { get; set; }
            public string Location { get; set; }
            public string Timestamp { get; set; }
            public string ImageSource { get; set; }
            public Sighting Content { set; get; }
        }

        
        public ObservableCollection<DisplayItem> DisplayItems { get; set; }

        public DisplayItem SelectedItem { get; set; }

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

        public SightingsPageViewModel()
        { 
            DisplayItems = new ObservableCollection<DisplayItem>();
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
            DisplayItems.Clear();
            foreach (var sighting in App.dbase.Sightings.OrderByDescending(o=>o.TimeStamp))
            {
                var species = App.dbase.Species.FirstOrDefault(o => o.GenusId == sighting.GenusId && o.SpeciesId == sighting.SpeciesId);
                if (species == null) continue;
                var imageSource = (species.Images.Count > 0) ? species.Images.First() : "bat.png";
                DisplayItems.Add(new DisplayItem { 
                    SpeciesName = $"{sighting.GenusId.ToUpperFirstChar()} {sighting.SpeciesId.ToLower()}",
                    SpeciesFriendlyName = species.Name,
                    ImageSource = imageSource, 
                    Location = $"Lat: {sighting.Lat:N2} Lon: {sighting.Lon:N2}",
                    Timestamp = sighting.TimeStamp.ToString("g"),
                    Content = sighting
                });
            }
        }


        public ICommand OnSubsequentAppearance => new Command(async () =>
        {
            try
            {
                await ActivityIndicatorStart();
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
            NavigateBack(NavigateReturnType.IsCancelled);
        });

        public bool IsHomeEnabled { get; set; }
        public ICommand OnHomeMenuPressed => new Command(() =>
        {
            NavigateBack(NavigateReturnType.GotoHome);
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
                var sighting = SelectedItem.Content;
                var species = App.dbase.Species.FirstOrDefault(o => o.GenusId == sighting.GenusId && o.SpeciesId == sighting.SpeciesId);
                var viewModel = new DisplaySpeciesTabbedPageViewModel(species) { IsHomeEnabled = IsHomeEnabled };
                var page = new DisplaySpeciesTabbedPage(viewModel);
                var resultType = await NavigateToPageAsync(page, viewModel);
                if (resultType == NavigateReturnType.GotoHome) NavigateBack(NavigateReturnType.GotoHome);
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

        public ICommand OnClearAllMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {
                if (!await Application.Current.MainPage.DisplayAlert("Clear all sightings", "Are you sure?", "Ok", "Cancel")) return;
                App.dbase.Sightings.Clear();
                Dbase.Save(App.dbase);
                UpdateDisplay();
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

        public ICommand OnMailMenuPressed => commandHelper.ProduceDebouncedCommand(async () =>
        {

            try
            {

                var result = await Application.Current.MainPage.DisplayPromptAsync("Mail", "Enter email address", "Send", "Cancel", keyboard: Keyboard.Email );
                if (result == null) return;

                await ActivityIndicatorStart();

                await SendEmail($"Mail from {Constants.APP_NAME}", "Please see attachment for sightings data", new List<string> { result });

                // Exit

                async Task SendEmail(string subject, string body, List<string> recipients)
                {
                    var message = new EmailMessage
                    {
                        Subject = subject,
                        Body = body,
                        To = recipients,
                    };
                    var fn = "Sightings.json";
                    var file = Path.Combine(FileSystem.CacheDirectory, fn);
                    var sightingsJson = JsonConvert.SerializeObject(App.dbase.Sightings);
                    File.WriteAllText(file, sightingsJson);
                    message.Attachments.Add(new EmailAttachment(file));
                    await Email.ComposeAsync(message);
                }

            }
            catch (FeatureNotSupportedException)
            {
                throw new BusinessException("Email is not supported on this device"); 
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
    

