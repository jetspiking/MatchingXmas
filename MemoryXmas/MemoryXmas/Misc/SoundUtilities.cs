using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace MatchingXmas.Misc
{
    internal class SoundUtilities
    {
        private static MediaPlayer? MusicPlayerInstance { get; set; }
        private static MediaPlayer? EffectPlayerInstance { get; set; }
        public static MediaPlayer GetMusicInstance()
        {
            if (SoundUtilities.MusicPlayerInstance == null) SoundUtilities.MusicPlayerInstance = new MediaPlayer();
            return SoundUtilities.MusicPlayerInstance;
        }
        public static MediaPlayer GetEffectsInstance()
        {
            if (SoundUtilities.EffectPlayerInstance == null) SoundUtilities.EffectPlayerInstance = new MediaPlayer();
            return SoundUtilities.EffectPlayerInstance;
        }

        public static void PlaySoundLooping(Sounds audio, Extensions extension)
        {
            MediaPlayer player = SoundUtilities.GetMusicInstance();
            player.Stop();
            player.Open(BuildSoundUri(audio, extension));
            player.Play();
            player.MediaEnded += (object? sender, EventArgs e) =>
            {
                player.Position = TimeSpan.Zero;
                player.Play();
            };
        }

        public static void PlaySoundEffect(Sounds audio, Extensions extension)
        {
            MediaPlayer player = SoundUtilities.GetEffectsInstance();
            player.Stop();
            player.Open(BuildSoundUri(audio, extension));
            player.Play();
        }

        private static Uri BuildSoundUri(Sounds audio, Extensions extension)
        {
            String filePath = Path.Combine("Audio",$"{audio}.{extension.ToString().ToLowerInvariant()}");

            return new Uri(filePath, UriKind.RelativeOrAbsolute);
        }



        private static Uri BuildPackUri(string postFix)
        {
            return new Uri(Path.Join("pack://application:,,,", postFix));
        }
    }
}
