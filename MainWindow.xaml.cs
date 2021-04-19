using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace LecteurMP3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



                    //TitreMorceau.Text="Titre : ";
                    // Play the media.



        private bool pauseOnOff = false;
        private bool mediaPlayerIsPlaying = false;
        private bool timeSliderDrag = false;
        private TimeSpan TempsEcoule = new TimeSpan();

        void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
        {

            myMediaElement.Play();


            InitializePropertyValues();
        }

        void OnClickJoueMorceau(object sender, RoutedEventArgs e)
        {

            //String TitreMorceau = "Kashmir.mp3";
            //String TitreMorceau = "Backinblack.mp3"



            String myFile = @"D:\TPCDA\C#\LecteurMP3\Musique\Kashmir.mp3";
            //String myFile = @"D:\TPCDA\C#\LecteurMP3\Musique\Backinblack.mp3";
            //String Titre = "AC-DC : Back in black";
            String Titre = "LED ZEPPELIN : Kashmir";

            myMediaElement.Source = new Uri(myFile);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            mediaPlayerIsPlaying = true;
            LblTitreMorceau.Content = Titre;


            InitializePropertyValues();
            myMediaElement.Play();

        }

        void OnClickPauseMorceau(object sender, RoutedEventArgs e)
        {
            TimeSpan interval = new TimeSpan(0, 1, 5);
            if (pauseOnOff == false && mediaPlayerIsPlaying == true)
            {
                LblTempsEcoule.Content = "Pause à : " + myMediaElement.Position.ToString(@"hh\:mm\:ss");
                myMediaElement.Pause();
                pauseOnOff = true;
            }
            else
            {
                if (mediaPlayerIsPlaying == true)
                {
                    myMediaElement.Position = TempsEcoule;
                    LblTempsEcoule.Content = "Reprise à : " + myMediaElement.Position.ToString(@"hh\:mm\:ss");
                    myMediaElement.Play();
                    pauseOnOff = false;
                }
            }
        }
        void OnClickStopMorceau(object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
            mediaPlayerIsPlaying = false;
        }
        void timer_Tick(object sender, EventArgs e)
        {

            Int32 PosTrackbar;
            if (myMediaElement.Source != null)
            {
                PosTrackbar = myMediaElement.Position.Hours * 3600 + myMediaElement.Position.Minutes * 60 + myMediaElement.Position.Seconds;
                lblStatus.Content = String.Format("{0} / {1}", myMediaElement.Position.ToString(@"hh\:mm\:ss"), myMediaElement.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss"));
                sliProgress.Value = PosTrackbar;
                lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
                TempsEcoule = myMediaElement.Position;
                LblTempsEcoule.Content = myMediaElement.Position.ToString(@"hh\:mm\:ss");
            }
            else
            {
                lblStatus.Content = "No file selected...";
            }
        }

        // Pause the media.
        void OnMouseDownPauseMedia(object sender, MouseButtonEventArgs args)
        {

            // The Pause method pauses the media if it is currently running.
            // The Play method can be used to resume.

            if (myMediaElement.Clock == null)
            {
                myMediaElement.Pause();
            }
            else
            {
                myMediaElement.Play();
            }

        }

        // Stop the media.
        void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
        {

            // The Stop method stops and resets the media to be played from
            // the beginning.
            myMediaElement.Stop();
        }

        // Change the volume of the media.
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            myMediaElement.Volume = (double)volumeSlider.Value;
        }



        // When the media opens, initialize the "Seek To" slider maximum value
        // to the total number of miliseconds in the length of the media clip.
        private void Element_MediaOpened(object sender, EventArgs e)
        {

            if (myMediaElement.NaturalDuration.HasTimeSpan)
            {
                //timelineSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalSeconds;

                sliProgress.Maximum = Math.Round(myMediaElement.NaturalDuration.TimeSpan.TotalSeconds, 0);
            }

        }

        // When the media playback is finished. Stop() the media to seek to media start.
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            myMediaElement.Stop();
        }

        // Jump to different parts of the media (seek to).
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            //int SliderValue = (int)timelineSlider.Value;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            //TimeSpan ts = new TimeSpan(0, 0, 0, 0, sliProgress.Value);
            //myMediaElement.Position = ts;
        }

        void InitializePropertyValues()
        {
            // Set the media's starting Volume and SpeedRatio to the current value of the
            // their respective slider controls.
            myMediaElement.Volume = (double)volumeSlider.Value;

        }




        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            Double valeurTrackBarPosition = myMediaElement.Position.Hours * 3600 + myMediaElement.Position.Minutes * 60 + myMediaElement.Position.Seconds;
            Double valeurTrackBaActuelle = Math.Round(e.NewValue, 0);
            Double ecart = Math.Abs(valeurTrackBarPosition - valeurTrackBaActuelle);



            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
            if (ecart > 1)
            {

                myMediaElement.Pause();
                myMediaElement.Position = TimeSpan.FromSeconds(sliProgress.Value);
                LblTempsEcoule.Content = "Release à ";
                myMediaElement.Play();
            }


        }


        //LblTempsEcoule.Content = "Slider moved from " + e.OldValue + " to " + e.NewValue + "    -> " + (e.NewValue - e.OldValue) + "  " + ecart;







    }
}

