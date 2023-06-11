using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FromImageToAscii
{
    public partial class MainWindow : Window
    {
        public event Action<string> Draw;

        public MainWindow()
        {
            InitializeComponent();

            Draw += MainWindow_Draw;
        }

        private void MainWindow_Draw(string frame)
        {
            Dispatcher.Invoke(() => (Result.Text = frame));
        }

        private void StartConverting(object sender, RoutedEventArgs e)
        {
            try
            {
                ConvertImg.WIDTH_SIZE = int.Parse(WidthSize.Text);
                ConvertVideo.WIDTH_SIZE = int.Parse(WidthSize.Text);


                if (Picture.IsChecked == true)
                {

                    Task<string> convertImgTask = ConvertImg.Convert();
                    string res = convertImgTask.Result;

                    Result.Text = res;
                    WriteInFile(new string[] { res });

                }
                else if (Video.IsChecked == true)
                {
                    new Task(ConvertTask).Start();
                }

                else
                {
                    MessageBox.Show("Не выбраны параметры", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConvertTask()
        {
            Task<string[]> convertVideoTask = ConvertVideo.Convert();
            var frames = convertVideoTask.Result;

            MessageBoxResult result = MessageBox.Show("Продолжить?", "Подтвердите действие", MessageBoxButton.OKCancel);

            while (result == MessageBoxResult.OK)
            {
                foreach (var frame in frames)
                {
                    Draw?.Invoke(frame);
                    Thread.Sleep(42);
                }
                result = MessageBox.Show("Повторить?", "Подтвердите действие", MessageBoxButton.OKCancel);
            }
            WriteInFile(frames);
        }
        private async void WriteInFile(string[] sumthingFrames)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Text | *.txt", Title = "Выберите файл для сохранения результата" };
            openFileDialog.ShowDialog();

            using (StreamWriter writer = new StreamWriter(openFileDialog.FileName))
            {
                for (int i = 0; i < sumthingFrames.Length; i++)
                {
                    await writer.WriteLineAsync(sumthingFrames[i]);
                }
            }
        }
    }
}
