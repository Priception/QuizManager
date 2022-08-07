using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using QuizManagerTests;

namespace TestingFramework.Wrapper
{
    public static class ParallelSource
    {
        public static int BrowsersUsed = 0;
        public static int MaxBrowsers = 4;
    }
    public class ParallelWrapper
    {
        public List<string> LogList { get; set; }
        public Browser ParallelBrowser { get; set; }
        public string LogFileName { get; set; }
        public System.Timers.Timer AutotestTimer { get; set; }
        public bool TimerEnded { get; set; }
        public bool Initialized { get; private set; }
        public WebDriverWait wait { get; set; }
        public ParallelWrapper()
        { 
            

            if (ParallelSource.BrowsersUsed < ParallelSource.MaxBrowsers)
            {
                ParallelSource.BrowsersUsed += 1;
                
                //NewBrowser.Driver.Manage().Window.Maximize();
                //NewBrowser.Initialize();
                Initialized = true;
            }
            else
            {
                Initialized = false;
                System.Threading.SpinWait.SpinUntil(() => ParallelSource.BrowsersUsed < ParallelSource.MaxBrowsers);

                ParallelSource.BrowsersUsed += 1;
                
                //NewBrowser.Driver.Manage().Window.Maximize();
                //NewBrowser.Initialize();
                Initialized = true;
            }


        }

        public void StartUpLogin(ParallelWrapper wrapper)
        {
            ParallelBrowser = new Browser();
            LogList = new List<string>();
            AutotestTimer = new System.Timers.Timer();
            TimerEnded = false;

            //Add Start up here
            wait = new WebDriverWait(ParallelBrowser.Driver, TimeSpan.FromSeconds(30));
        }

        //public void AdminLogin(ParallelWrapper wrapper)
        //{
        //    User user = 
        //    Login(user, wrapper);
        //}

        //private void Login(AmxUser user, ParallelWrapper wrapper)
        //{
        //    //wait = new WebDriverWait(NewBrowser.Driver, TimeSpan.FromSeconds(30));
        //    //The Loader appearing will sometimes be missed, so we don't check here. The Try Catch handles if the NewBrowser was slow here.
        //    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("Loader")));

        //    ParallelBrowser.FindElementById("UserName", wrapper).SendKeys(user.UserName);
        //    ParallelBrowser.FindElementById("Password", wrapper).SendKeys(user.Password);
        //    try
        //    {
        //        ParallelBrowser.ClickById("submitButton", wrapper);
        //    }
        //    catch (ElementClickInterceptedException)
        //    {
        //        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Loader")));
        //        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("Loader")));
        //        ParallelBrowser.ClickById("submitButton", wrapper);
        //    }

        //    //if (AmxBrowser.GetBrowserType() != "firefox")
        //    //{
        //    //    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("loadingImage")));
        //    //    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("loadingImage")));
        //    //}

        //    userHandlers.SetCurrentUser(user.UserName);
        //}

        public void Finalise()
        {
            ParallelBrowser.Quit();
            ParallelSource.BrowsersUsed -= 1;
        }

        public void StartTimer()
        {
            AutotestTimer.Interval = 1;
            AutotestTimer.Start();
            while (!TimerEnded)
            {
                AutotestTimer.Interval += 1;
                Thread.Sleep(1000);
            }
        }

        public string EndTimer()
        {
            try
            {
                AutotestTimer.Stop();
                TimerEnded = true;
                return string.Concat("Time taken: ", AutotestTimer.Interval.ToString(), " Seconds");
            }
            finally
            {
                AutotestTimer.Dispose();
            }
            
        }
    }
}
