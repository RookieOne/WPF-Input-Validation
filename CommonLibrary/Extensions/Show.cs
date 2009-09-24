using System;
using System.Windows;
using CommonLibrary.Domain;
using CommonLibrary.Wpf;

namespace CommonLibrary.Extensions
{
    public class Show
    {
        public static Action InWindow<T>() where T : ViewModel
        {
            return () =>
                       {
                           object viewModel = Activator.CreateInstance(typeof(T), new InMemoryPartyInviteeRepository());

                           var w = new Window();
                           w.Content = viewModel;
                           w.Show();
                       };
        }
    }
}