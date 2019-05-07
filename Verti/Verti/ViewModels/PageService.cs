using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Verti.ViewModels
{
    class PageService : IPageService
    {
        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await App.Current.MainPage.DisplayAlert(title, message, ok, cancel);
        }

        public async Task PushModalAsync(Page page)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(page);
        }
    }
}
