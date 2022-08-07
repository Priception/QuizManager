using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;
using TestingFramework.Wrapper;

namespace QuizManagerTests
{
    public class Browser
    {
        private IWebDriver _webDriver = null;
        private IWebElement FindElement(By by, int increment = 0)
        {
            IWebElement element = null;
            Thread.Sleep(500);
            try
            {
                ReadOnlyCollection<IWebElement> elements = null;
                Driver.SwitchTo().DefaultContent();
                ReadOnlyCollection<IWebElement> detailsObj = null;
                detailsObj = Driver.FindElements(By.CssSelector("#detailsObj[src]"));

                if (detailsObj != null && detailsObj.Count > 0)
                {
                    Driver.SwitchTo().Frame("detailsObj");
                    elements = Driver.FindElements(by);
                }

                if (elements != null && elements.Count > 0)
                {
                    element = elements[0];
                }
                else
                {
                    Driver.SwitchTo().DefaultContent();
                    elements = Driver.FindElements(by);
                    if (elements != null && elements.Count > 0)
                    {
                        element = elements[0];
                    }
                }
                //handling disabled buttons due to them being disabled which will throw in error.
                element = CheckElemIsEnabled(element);
                if (element == null)
                {
                    if (increment <= 10)
                    {
                        Thread.Sleep(1000);
                        increment++;

                        element = FindElement(by, increment);
                    }
                }
            }
            catch (Exception ex)
            {
                if (increment <= 10)
                {
                    Thread.Sleep(1000);
                    increment++;

                    element = FindElement(by, increment);
                }
            }

            return element;
        }
        private IWebElement FindElement(By by, ParallelWrapper wrapper, int increment = 0)
        {
            IWebElement element = null;
            Thread.Sleep(500);
            try
            {
                ReadOnlyCollection<IWebElement> elements = null;
                Driver.SwitchTo().DefaultContent();
                ReadOnlyCollection<IWebElement> detailsObj = null;
                detailsObj = Driver.FindElements(By.CssSelector("#detailsObj[src]"));

                if (detailsObj != null && detailsObj.Count > 0)
                {
                    Driver.SwitchTo().Frame("detailsObj");
                    elements = Driver.FindElements(by);
                }

                if (elements != null && elements.Count > 0)
                {
                    element = elements[0];
                }
                else
                {
                    Driver.SwitchTo().DefaultContent();
                    elements = Driver.FindElements(by);
                    if (elements != null && elements.Count > 0)
                    {
                        element = elements[0];
                    }
                }
                element = CheckElemIsEnabled(element);
                if (element == null)
                {
                    if (increment <= 10)
                    {
                        //Browser.DoesElementExist(by);
                        Thread.Sleep(1000);
                        increment++;

                        element = FindElement(by, wrapper, increment);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                if (increment <= 10)
                {
                    //Browser.DoesElementExist(by);
                    Thread.Sleep(1000);
                    increment++;

                    element = FindElement(by, wrapper, increment);
                }
            }

            return element;
        }

        public void ClickById(string id, ParallelWrapper wrapper, bool required = true)
        {
            try
            {
                var element = FindElement(By.Id(id), wrapper);
                if (element == null)
                {
                    ClickByXPath(string.Concat("//*[contains(@id,'", id, "')]"), wrapper);
                }
                else
                {
                    Click(By.Id(id));
                }

            }
            catch (Exception e)
            {
               
               throw e;
                
            }
        }

        public void ClickByXPath(string id, ParallelWrapper wrapper)
        {
            wrapper.ParallelBrowser.Click(By.XPath(id));
        }
        public IWebElement FindElementById(string element, ParallelWrapper wrapper, int increment = 0)
        {
            return FindElement(By.Id(element), wrapper);
        }
        public IWebElement FindElementByXPath(string element, ParallelWrapper wrapper, int increment = 0)
        {
            return FindElement(By.XPath(element), wrapper);
        }

        public void ClickByCssSelector(string id, ParallelWrapper wrapper)
        {
            Click(By.CssSelector(id));
        }

        public IWebElement FindElementByTitle(string title, ParallelWrapper wrapper)
        {
            return FindElement(By.CssSelector("[title^='" + title + "']"), wrapper);
        }

        public void FindElementByIdAndSendEnterKey(string element, ParallelWrapper wrapper, int increment = 0)
        {
            FindElementById(element, wrapper).SendKeys($"{Keys.Enter}");
        }

        private void Click(By by)
        {
            Thread.Sleep(500);
            bool retry = false;
            int retryCount = 0;
            int noOfRetry = 5;
            IWebElement element = FindElement(by);
            do
            {
                try
                {

                    if (IsElementClickable(element))
                    {
                        Actions action = new Actions(Driver);
                        //NR: Modified to build to compile the action.
                        action.MoveToElement(element).Click().Build().Perform();
                        retry = false;
                    }
                    else if (!IsElementClickable(element))
                    {
                        throw new ArgumentException($"Unable to click '{element.ToString()}'");
                    }

                }
                catch (ArgumentException e)
                {
                    if (retryCount > noOfRetry)
                    {
                        throw e;
                    }
                    retry = true;
                    retryCount++;
                }
            } while (retry && retryCount <= noOfRetry);
        }

        private IWebElement CheckElemIsEnabled(IWebElement element)
        {
            if (element != null)
            {
                switch (element.TagName)
                {
                    case "input":
                        switch (element.GetAttribute("type"))
                        {
                            case "button":
                            case "submit":
                                if (!element.Displayed && !element.Enabled)
                                {
                                    element = null;
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case "button":
                        if (!element.Displayed && !element.Enabled)
                        {
                            element = null;
                        }
                        break;
                    default:
                        break;
                }
            }
            return element;
        }

        public bool IsElementClickable(By by)
        {
            return DoesLoad(ExpectedConditions.ElementToBeClickable(by));
        }

        public bool IsElementClickable(By by, ParallelWrapper wrapper)
        {
            return DoesLoad(ExpectedConditions.ElementToBeClickable(by));
        }

        public bool IsElementClickable(IWebElement element)
        {
            return DoesLoad(ExpectedConditions.ElementToBeClickable(element));
        }

        public bool IsElementClickable(IWebElement element, ParallelWrapper wrapper)
        {
            return DoesLoad(ExpectedConditions.ElementToBeClickable(element));
        }

        public void ClickByLinkText(string linkText, ParallelWrapper wrapper)
        {
            wrapper.ParallelBrowser.Click(By.LinkText(linkText));
        }

        public bool DoesLoad(Func<IWebDriver, IWebElement> condition, double seconds)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));
            try
            {
                wait.Until(condition);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            return true;
        }

        public bool DoesLoad(Func<IWebDriver, IWebElement> condition)
        {
            return DoesLoad(condition, 5);
        }

        public void Quit()
        {
            if (_webDriver != null)
            {
                _webDriver.Close();
                _webDriver.Quit();
                _webDriver.Dispose();
            }
        }

        public IWebDriver Driver
        {
            get { return _webDriver; }
        }
    }
}
