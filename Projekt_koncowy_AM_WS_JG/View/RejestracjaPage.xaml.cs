﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Projekt_koncowy_AM_WS_JG.Presenter;


namespace Projekt_koncowy_AM_WS_JG.View
{
    /// <summary>
    /// Logika interakcji dla klasy RejestracjaPage.xaml
    /// </summary>
    public partial class RejestracjaPage : UserControl
    {
        public event EventHandler RejestracjaGotowa;
        public event EventHandler WrocNacisniete;

        public RejestracjaPage()
        {
            InitializeComponent();
        }
        public bool CzyEmailPrawidlowy(string email)
        {
            string wzorzec = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, wzorzec);
        }
        private void WrocDoLogowaniaKliknieta(object sender, RoutedEventArgs e)
        {
            WrocNacisniete?.Invoke(this, EventArgs.Empty);
        }


        private void RejestrujGotowaKliknieta(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(nazwaużytkownika.Text))
            {
                if (!string.IsNullOrEmpty(email.Text) && CzyEmailPrawidlowy(email.Text))
                {
                    if (haslo.Password == powtorzhaslo.Password && !string.IsNullOrEmpty(haslo.Password))
                    {
                        RejestracjaGotowa?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Hasła się nie zgadzają", "Niepraiwdłowe hasło");
                    }
                }
                else
                {
                    MessageBox.Show("Nieprawidłowy adres email", "Nieprawidłowy email");
                }
            }
            else
            {
                MessageBox.Show("Podaj nazwę użytkownika", "Nieprawidłowa nazwa użytkownika");
            }
        }
        

        public string Haslo
        {
            get => haslo.Password;
            set => haslo.Password = value;
        }

        public string PowtorzHaslo
        {
            get => powtorzhaslo.Password;
            set => powtorzhaslo.Password = value;
        }

        public string Email
        {
            get => email.Text; 
            set => email.Text = value;
        }

        public string NazwaUżytkownika
        {
            get => nazwaużytkownika.Text;
            set => nazwaużytkownika.Text = value;
        }

    }
}
