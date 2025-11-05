// Author:  Kyle Chapman
// Created: November 3, 2025
// Updated: November 4, 2025
// Description:
// It's Notepad, but worse. I blame Muntadher.
// It's a Textbox but with save, load, clear and exit functionality (eventually).

using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace KylesWorseNotepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string fileName = "";
        const string DefaultTitle = "Kyle's Worse Notepad";

        public MainWindow()
        {
            InitializeComponent();
            textEverything.Focus();
        }

        /// <summary>
        /// Attempts to save the contents of the notepad to a local text file.
        /// </summary>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            // Create the dialog with whatever properties we intend to use.
            var saveDialog = new SaveFileDialog()
            {
                Filter = "Text files (*.txt) | *.txt",
                Title = "Save Text File"
            };
            // Show the dialog, and if the user picks a file, write to it.
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    fileName = saveDialog.FileName;
                    //File.WriteAllText(saveDialog.FileName, textEverything.Text);

                    // using clauses are informed by Copilot.
                    // https://m365.cloud.microsoft/chat/entity1-d870f6cd-4aa5-4d42-9626-ab690c041429/eyJpZCI6IlZYTmxjbFl4ZkdoMGRIQnpPaTh2YzNWaWMzUnlZWFJsTFdsdWRDNXZabVpwWTJVdVkyOXRMM3hQU1VRNllUYzROV1kwTlRndE9XWXpaaTAwTnpFMExUZzFZak10TjJVelpETmtZakF6TldOaWZHTXhNV1ExWlRBNUxXSmpZV0V0TkRreE1DMDVORFV6TFRJMU9XTmtObVUxWXpKa1kzd3lNREkxTFRFeExUQTBWREUwT2pJeU9qUXlMalUxTVRNNU9ETmEiLCJzY2VuYXJpbyI6InNoYXJlTGlua1ZpYVJpY2hDb3B5IiwicHJvcGVydGllcyI6eyJwcm9tcHRTb3VyY2UiOiJ1c2VyIiwiY2xpY2tUaW1lc3RhbXAiOiIyMDI1LTExLTA0VDE0OjIyOjQyLjMxM1oifSwiY2hhdFR5cGUiOiJ3ZWIiLCJ2ZXJzaW9uIjoxLjF9
                    using (var fileToAccess = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                    // The StreamWriter is used for the actual write operation, done with WriteLine().
                    using (var writer = new StreamWriter(fileToAccess))
                    {
                        writer.WriteLine(textEverything.Text);
                    }

                    UpdateTitle();
                }
                // Catch expected errors with the file access and writing.
                catch (IOException error)
                {
                    MessageBox.Show("Error writing your text to " + fileName + ".\n" +
                        "Message: " + error.Message, "File Save Error");
                }
                // Catch less-expected errors! Report with lots of detail.
                catch (Exception error)
                {
                    MessageBox.Show("An unexpected error has occurred." +
                        "\nType: " + error.GetType() +
                        "\nSource: " + error.Source +
                        "\nMessage: " + error.Message +
                        "\nStack Trace: " + error.StackTrace, "File Save Error");
                }
                //finally
                //{
                //  // This isn't necessary with the way this is written.
                //}
            }
            textEverything.Focus();
        }

        /// <summary>
        /// Confirm if the user wants to clear all the text. If they do, wipe it.
        /// </summary>
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            // Only confirm if the file isn't empty.
            if (textEverything.Text.Trim() != String.Empty)
            {
                if (MessageBox.Show("Are you sure you want to clear the contents?", "Confirm Clear", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    textEverything.Clear();
                    fileName = String.Empty;
                    UpdateTitle();
                }
            }
            else
            {
                textEverything.Clear();
                fileName = String.Empty;
                UpdateTitle();
            }
            textEverything.Focus();
        }

        /// <summary>
        /// Confirm if the user wants to close the program. If they do, close it.
        /// </summary>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (textEverything.Text.Trim() != String.Empty)
            {
                if (MessageBox.Show("Are you sure you want to exit the program?", "Confirm Close", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
            textEverything.Focus();
        }

        /// <summary>
        /// When the user changes the text, note whether there are unsaved changes.
        /// </summary>
        private void TextModified(object sender, TextChangedEventArgs e)
        {
            UpdateTitle("(Unsaved)");
        }

        /// <summary>
        /// Updates the title.
        /// </summary>
        /// <param name="extraText">Extra text to add to the title, or blank if not provided.</param>
        private void UpdateTitle(string extraText = "")
        {
            if (fileName == String.Empty)
            {
                this.Title = DefaultTitle + " " + extraText;
            }
            else
            {
                this.Title = DefaultTitle + " | " + fileName + " " + extraText;
            }
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            // Create the dialog with whatever properties we intend to use.
            var openDialog = new OpenFileDialog()
            {
                Filter = "Text files (*.txt) | *.txt",
                Title = "Load Text File"
            };
            // Show the dialog, and if the user picks a file, try to read it.
            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    var fileContents = File.ReadAllLines(openDialog.FileName);
                    textEverything.Clear();

                    foreach (string line in fileContents)
                    {
                        textEverything.Text += line + Environment.NewLine;
                    }

                    fileName = openDialog.FileName;
                    UpdateTitle();
                }
                // Catch expected errors with the file access and writing.
                catch (IOException error)
                {
                    MessageBox.Show("Error reading " + fileName + ".\n" +
                        "Message: " + error.Message, "File Load Error");
                }
                // Catch less-expected errors! Report with lots of detail.
                catch (Exception error)
                {
                    MessageBox.Show("An unexpected error has occurred." +
                        "\nType: " + error.GetType() +
                        "\nSource: " + error.Source +
                        "\nMessage: " + error.Message +
                        "\nStack Trace: " + error.StackTrace, "File Load Error");
                }
            }
        }
    }
}