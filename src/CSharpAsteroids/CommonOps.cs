using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using Microsoft.DirectX.DirectSound;

namespace Asteroids
{
    /// <summary>
    /// Summary description for CommonDraw.
    /// </summary>
    public abstract class CommonOps
    {
        #region Constructors
        static CommonOps()
        {
            SoundDirectory = Path.Combine("Resources", "Sounds");
        }
        #endregion

        private static readonly string SoundDirectory;
        public const int iMaxX = 10000;
        public const int iMaxY = 7500;
        public const double FPS = 60;
        public static Random rndGen = new Random();
        private static Device devSound;
        private static Control ctrlSound;
        private static ArrayList alSounds;

        public static void InitSound()
        {
            devSound = new Device();
            ctrlSound = new Control(null);
            devSound.SetCooperativeLevel(ctrlSound, CooperativeLevel.Priority);
            alSounds = new ArrayList();
        }

        public static void PlayQueuedSounds()
        {
            ArrayList alCopy;

            // Copy the sound list - so we can release the lock
            lock (alSounds)
            {
                alCopy = new ArrayList(alSounds);
                alSounds.Clear();
            }
            // Play all the sounds
            foreach (string sSoundFile in alCopy)
            {
                string filePath = Path.Combine(SoundDirectory, sSoundFile);
                SecondaryBuffer bufSound = new SecondaryBuffer(filePath, devSound);
                bufSound.Play(0, BufferPlayFlags.Default);
            }
        }

        public static void PlaySound(string sSoundFile)
        {
            // add sounds under lock
            lock (alSounds)
            {
                if (!alSounds.Contains(sSoundFile))
                {
                    alSounds.Add(sSoundFile);
                }
            }
        }
    }
}
