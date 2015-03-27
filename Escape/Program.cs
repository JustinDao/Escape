#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#if WINDOWS || LINUX
#else
using MonoMac.AppKit;
using MonoMac.Foundation;
#endif
#endregion

namespace Escape
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new MainGame())
                game.Run();
        }
    }
#else
	class Program
	{
		static void Main (string[] args)
		{
			NSApplication.Init ();

			using (var p = new NSAutoreleasePool ()) {
				NSApplication.SharedApplication.Delegate = new AppDelegate ();

				NSApplication.Main (args);
			}
		}
	}

	class AppDelegate : NSApplicationDelegate
	{
		private MainGame game;

		public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
		{
			game = new MainGame ();
			game.Run ();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}
#endif
}
