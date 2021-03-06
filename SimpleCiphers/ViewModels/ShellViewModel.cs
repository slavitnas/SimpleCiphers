﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Gat.Controls;
using SimpleCiphers.Models;

namespace SimpleCiphers.ViewModels
{
    public sealed class ShellViewModel : Screen
    {
        public ShellViewModel()
        {
            DisplayName = "Простые шифры";
        }

        #region Выбор метода шифрования

        // ComboBox MethodCipher
        public List<string> MethodCipher => Data.CipherMethods.Keys.ToList();

        // Текущий метод шифрования в виде названия и ICipher
        // 0 - шифр Цезаря
        private string _selectedMethod = Data.CipherMethods.Keys.ElementAt(0);

        private ICipher _cipher = Data.CipherMethods[Data.CipherMethods.Keys.ElementAt(0)];

        public string SelectedType
        {
            get => _selectedMethod;

            set
            {
                if (_selectedMethod == value) return;
                _selectedMethod = value;
                // задать новый метод шифрования
                _cipher = Data.CipherMethods[value];
                ResetColors();
                ResetAlphabet();
                NotifyOfPropertyChange(() => SelectedType);
                NotifyOfPropertyChange(() => CanShowAlphabet);
            }
        }

        #endregion

        #region Ввод ключа

        private string _key;

        public string Key
        {
            get => _key;
            set
            {
                if (_key == value) return;
                _key = value.ToLower().Replace(" ", "•");
                ResetColors();
                ResetAlphabet();
                NotifyOfPropertyChange(() => Key);
                NotifyOfPropertyChange(() => CanShowAlphabet);
            }
        }

        #endregion

        #region Выбор языка и поле с исходным алфавитом

        // ComboBox LangCipher
        public List<string> LangCipher => Data.Words.Keys.ToList();

        // Текущий язык в виде названия (_selectedLang) и набора символов (Alphabet)
        private string _selectedLang = Data.Words.Keys.ElementAt(1);

        private string _alphabet = Data.Words[Data.Words.Keys.ElementAt(1)];

        public string SelectedLang
        {
            get => _selectedLang;

            set
            {
                if (_selectedLang == value) return;
                _selectedLang = value;
                // задать новый язык
                Alphabet = Data.Words[value];
                ResetColors();
                ResetAlphabet();
                NotifyOfPropertyChange(() => SelectedLang);
                NotifyOfPropertyChange(() => CanShowAlphabet);
            }
        }

        public string Alphabet
        {
            get => _alphabet;
            set
            {
                if (_alphabet == value) return;
                _alphabet = string.Join("", value.ToLower()
                    .Replace(" ", "•").Distinct());
                ResetColors();
                ResetAlphabet();
                NotifyOfPropertyChange(() => Alphabet);
                NotifyOfPropertyChange(() => CanShowAlphabet);
            }
        }

        #endregion

        #region DataGrid и кнопка с шифрованным алфавитом

        // Шифрованный алфавит
        private string[,] _encryptedAlphabet;

        public string[,] EncryptedAlphabet
        {
            get => _encryptedAlphabet;

            set
            {
                if (_encryptedAlphabet == value) return;
                _encryptedAlphabet = value;
                NotifyOfPropertyChange(() => EncryptedAlphabet);
            }
        }

        // Алфавит по строкам
        private string[] _rowAlphabet;

        public string[] RowAlphabet
        {
            get => _rowAlphabet;

            set
            {
                if (_rowAlphabet == value) return;
                _rowAlphabet = value;
                NotifyOfPropertyChange(() => RowAlphabet);
            }
        }

        // Алфавит по столбцам
        private string[] _colAlphabet;

        public string[] ColAlphabet
        {
            get => _colAlphabet;

            set
            {
                if (_colAlphabet == value) return;
                _colAlphabet = value;
                NotifyOfPropertyChange(() => ColAlphabet);
            }
        }

        // Сбросить шифрованный алфавит
        private void ResetAlphabet()
        {
            EncryptedAlphabet = null;
            RowAlphabet = ColAlphabet = null;
        }

        // Кнопка показать шифрованный алфавит 
        public void ShowAlphabet()
        {
            try
            {
                EncryptedAlphabet = _cipher.GetEncryptedAlphabet(In, Key, Alphabet);
                RowAlphabet = _cipher.GetRowAlphabet(Key, Alphabet);
                ColAlphabet = _cipher.GetColAlphabet(Key, Alphabet);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public bool CanShowAlphabet => !string.IsNullOrEmpty(Key)
                                       || _cipher is PolybiusCipher
                                       || _cipher is PortaCipher
                                       || _cipher is XorCipher;

        #endregion

        #region Исходный и шифрованный текст

        // Поле с исходным значением
        private string _in;

        public string In
        {
            get => _in;
            set
            {
                if (_in == value) return;
                _in = value.ToLower().Replace(" ", "•");
                ResetColors();
                NotifyOfPropertyChange(() => In);
                NotifyOfPropertyChange(() => CanEncrypt);
            }
        }

        // Поле с шифрованным значением
        private string _out;

        public string Out
        {
            get => _out;
            set
            {
                if (_out == value) return;
                _out = value.ToLower().Replace(" ", "•");
                ResetColors();
                NotifyOfPropertyChange(() => Out);
                NotifyOfPropertyChange(() => CanDecrypt);
            }
        }

        #endregion

        #region Кнопки

        // Кнопка Шифровать
        public void Encrypt()
        {
            try
            {
                Out = _cipher.Encrypt(In, Key, Alphabet);
                InColor = null;
                OutColor = _successBrush;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // Кнопка Дешифровать
        public void Decrypt()
        {
            try
            {
                In = _cipher.Decrypt(Out, Key, Alphabet);
                InColor = _successBrush;
                OutColor = null;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public static void ShowError(string error)
        {
            var mbox = new Xceed.Wpf.Toolkit.MessageBox
            {
                Caption = "Ошибка",
                Text = error,
                FontSize = 15,
                WindowBackground = new SolidColorBrush(Colors.DarkRed),
                CaptionForeground = new SolidColorBrush(Colors.White)
            };
            mbox.ShowDialog();
        }

        // Доступность кнопок
        public bool CanEncrypt => !string.IsNullOrEmpty(In);

        public bool CanDecrypt => !string.IsNullOrEmpty(Out);

        #endregion

        #region Цвет поля как индикатор успеха

        readonly SolidColorBrush _successBrush = new SolidColorBrush(Colors.MediumSpringGreen);

        // Цвет как индикатор успешного дешифрования
        private SolidColorBrush _inColor;

        public SolidColorBrush InColor
        {
            get => _inColor;

            set
            {
                if (Equals(_inColor, value)) return;
                _inColor = value;
                NotifyOfPropertyChange(() => InColor);
            }
        }

        // Цвет как индикатор успешного шифрования
        private SolidColorBrush _outColor;

        public SolidColorBrush OutColor
        {
            get => _outColor;

            set
            {
                if (Equals(_outColor, value)) return;
                _outColor = value;
                NotifyOfPropertyChange(() => OutColor);
            }
        }

        private void ResetColors()
        {
            InColor = OutColor = null;
        }

        #endregion

        #region Меню

        // Выход
        public void CloseApp() => Application.Current.Shutdown();

        // О программе
        public void About()
        {
            AboutControlView about = new AboutControlView();
            AboutControlViewModel vm = (AboutControlViewModel) about.FindResource("ViewModel");

            vm.ApplicationLogo = new BitmapImage(new Uri("pack://application:,,,/Images/key.png"));
            vm.PublisherLogo = new BitmapImage(new Uri("pack://application:,,,/Images/spaceman.png"));
            vm.Title = DisplayName;
            vm.Description = "Программа для демонстрации шифров";
            vm.IsSemanticVersioning = true;
            vm.HyperlinkText = "https://github.com/slavitnas/SimpleCiphers";
            vm.AdditionalNotes = "";

            vm.Window.Content = about;
            vm.Window.Show();
        }

        #endregion
    }
}