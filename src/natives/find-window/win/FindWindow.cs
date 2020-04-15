﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace BrowserTools {
    class FindWindow {
        //Consts
        const string IE_MAIN_WINDOW_CLASS_NAME = "IEFrame";

        //Variables
        private static string windowMark;

        //Imports
        private delegate bool EnumWindowsProc (IntPtr hWnd);

        [DllImport("user32")]
        private static extern bool EnumWindows (EnumWindowsProc lpEnumFunc);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId (IntPtr hWnd, out uint lpdwProcessId);

        //Methods
        private static bool IsIEMainWindow (IntPtr hWnd) {
            return Utils.GetClassName(hWnd) == IE_MAIN_WINDOW_CLASS_NAME;
        }

        private static bool CheckWindowTitle (IntPtr hWnd) {
            string title = Utils.GetWindowTitle(hWnd).ToLower();

            if (!title.Contains(windowMark.ToLower()))
                return true;

            uint processID = 0;

            GetWindowThreadProcessId(hWnd, out processID);

            string processName = Process.GetProcessById((int)processID).ProcessName.ToLower();

            // NOTE: IE has two windows with the same title. We are searching for the main window by its class name.
            if (processName == "iexplore" && !IsIEMainWindow(hWnd))
                return true;

            // NOTE: MS Edge has different process ("applicationframehost" and "microsoftedgecp") with windows with the same title.
            // "microsoftedgecp" is the wrong one.
            if (processName == "microsoftedgecp")
                return true;

            Console.Out.WriteLine(hWnd);
            Console.Out.WriteLine(processName);
            Environment.Exit((int)EXIT_CODES.SUCCESS);

            return false;
        }

        static void Main (string[] args) {
            if (args.Length != 1) {
                Console.Error.Write("Incorrect arguments");
                Environment.Exit((int)EXIT_CODES.GENERAL_ERROR);
            }

            windowMark = args[0];

            // NOTE: Repeat the attempt to find the window ten times with 300ms delay
            for (int i = 0; i < 10; i++) {
                EnumWindows(CheckWindowTitle);
                Thread.Sleep(300);
            }

            Console.Error.Write("Window not found");
            Environment.Exit((int)EXIT_CODES.WINDOW_NOT_FOUND);
        }
    }
}
