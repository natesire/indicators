// NinjaScript File: FiveMinuteAlertTemplate.cs
// Description: Template that alerts every 5 minutes in real-time. Modify the condition in AlertMethod() for custom criteria.

#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

namespace NinjaTrader.NinjaScript.Indicators
{
    public class FiveMinuteAlertTemplate : Indicator
    {
        // Timer for 5-minute intervals
        private System.Timers.Timer _alertTimer;
        // Flag to track if timer was started
        private bool _timerStarted;
        // Unique ID for the alert to control rearm behavior
        private const string AlertId = "FiveMinuteAlertId";
		
		private const int oneMinutesInMilliseconds = 60_000; // Set interval to 1 minutes (60,000 milliseconds)

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Alerts every five minutes. Edit the AlertMethod() for custom logic.";
                Name = "FiveMinuteAlertTemplate";
                Calculate = Calculate.OnPriceChange; // Timer runs independently, so this setting doesn't affect it
                IsOverlay = true; // Doesn't plot anything on the chart
                IsSuspendedWhileInactive = true; // Save resources when chart is not visible
            }
            else if (State == State.DataLoaded)
            {
                // Instantiate and configure the timer
                _alertTimer = new System.Timers.Timer();
                
                _alertTimer.Interval = oneMinutesInMilliseconds; 
                _alertTimer.AutoReset = false; // Keep firing every interval
                _alertTimer.Elapsed += OnTimerElapsed;
            }
            else if (State == State.Terminated)
            {
                // Clean up timer resources to prevent memory leaks
                if (_alertTimer != null)
                {
                    _alertTimer.Stop();
                    _alertTimer.Elapsed -= OnTimerElapsed;
                    _alertTimer.Dispose();
                    _alertTimer = null;
                }
            }
        }

        protected override void OnBarUpdate()
        {
            // Timer is started here to ensure we are in Realtime state and chart is active.
            // OnBarUpdate runs on every bar, so we start it once.
            if (!_timerStarted && State == State.Realtime)
            {
                _alertTimer.Start();
                _timerStarted = true;
                Print("FiveMinuteAlertTemplate: Timer started. Alerts will trigger every 5 minutes.");
                
                // Optional: Fire an alert immediately to confirm the script is active
                // TriggerAlert("Template Loaded - Monitoring Started");
            }
        }

        /// <summary>
        /// Event handler for the timer elapsed event.
        /// IMPORTANT: Timer runs on a different thread. Use TriggerCustomEvent to safely access NinjaScript methods.
        /// </summary>
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Check if we are still in Realtime state and the chart/indicator is still valid
            if (State != State.Realtime)
                return;

            // TriggerCustomEvent synchronizes the timer event with NinjaTrader's bar processing thread.
            // This is critical for safely calling Alert() or accessing price data.
            TriggerCustomEvent(AlertMethod, null);
        }

        /// <summary>
        /// *** MODIFY THIS METHOD FOR YOUR CUSTOM ALERT CONDITIONS ***
        /// This is where you implement your own logic.
        /// The method is called every 5 minutes.
        /// </summary>
        private void AlertMethod(object state)
        {
            // ==================================================
            // 1. START: YOUR CUSTOM ALERT CONDITIONS GO HERE
            // ==================================================
            
            // Example Condition 1: Always true - fires every 5 minutes.
            // REPLACE THIS with your own logic (e.g., price crossing a level, indicator values, etc.)
            bool myCustomCondition = true; 
            
            // Example Condition 2: Check if price is above a moving average (uncomment to use)
            // bool aboveSma = Close[0] > SMA(20)[0];
            
            // Combine conditions as needed
            // bool shouldAlert = myCustomCondition; // Default for this template
            bool shouldAlert = myCustomCondition; // Change this to your combined condition
            
            // ==================================================
            // END: CUSTOM ALERT CONDITIONS
            // ==================================================
            
            // If conditions are met, trigger the alert
            if (shouldAlert)
            {
                TriggerAlert("5-Minute Alert Triggered at " + Time[0].ToString("HH:mm:ss"));
            }
            else
            {
                // Optional: Print a message to the Output window for debugging
                // Print("FiveMinuteAlertTemplate: Condition not met at " + Time[0]);
            }
        }
        
        /// <summary>
        /// Helper method to fire the NinjaTrader Alert.
        /// </summary>
        /// <param name="customMessage">The message to display in the Alerts Log</param>
        private void TriggerAlert(string customMessage)
        {
            // Get the default sound file path from NinjaTrader's installation directory
            string soundFile = NinjaTrader.Core.Globals.InstallDir + @"\sounds\Alert3.wav";
            
            // Fire the alert
            // Parameters:
            // "id" - Unique string identifier for this alert type
            // Priority - Display priority in the Alerts Log
            // message - The text you will see
            // soundLocation - Path to the .wav file
            // rearmSeconds - 0 means it can trigger again immediately (we are using timer, so this is fine)
            // backBrush/foreBrush - Colors for the alert row in the log
            Alert(AlertId, 
                  Priority.High, 
                  customMessage, 
                  soundFile, 
                  0, // Rearm immediately, timer provides the spacing
                  Brushes.DarkBlue, 
                  Brushes.White);
                  
            // Also send a message to the NinjaScript Output window for debugging
            Print(customMessage);
        }
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private FiveMinuteAlertTemplate[] cacheFiveMinuteAlertTemplate;
		public FiveMinuteAlertTemplate FiveMinuteAlertTemplate()
		{
			return FiveMinuteAlertTemplate(Input);
		}

		public FiveMinuteAlertTemplate FiveMinuteAlertTemplate(ISeries<double> input)
		{
			if (cacheFiveMinuteAlertTemplate != null)
				for (int idx = 0; idx < cacheFiveMinuteAlertTemplate.Length; idx++)
					if (cacheFiveMinuteAlertTemplate[idx] != null &&  cacheFiveMinuteAlertTemplate[idx].EqualsInput(input))
						return cacheFiveMinuteAlertTemplate[idx];
			return CacheIndicator<FiveMinuteAlertTemplate>(new FiveMinuteAlertTemplate(), input, ref cacheFiveMinuteAlertTemplate);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.FiveMinuteAlertTemplate FiveMinuteAlertTemplate()
		{
			return indicator.FiveMinuteAlertTemplate(Input);
		}

		public Indicators.FiveMinuteAlertTemplate FiveMinuteAlertTemplate(ISeries<double> input )
		{
			return indicator.FiveMinuteAlertTemplate(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.FiveMinuteAlertTemplate FiveMinuteAlertTemplate()
		{
			return indicator.FiveMinuteAlertTemplate(Input);
		}

		public Indicators.FiveMinuteAlertTemplate FiveMinuteAlertTemplate(ISeries<double> input )
		{
			return indicator.FiveMinuteAlertTemplate(input);
		}
	}
}

#endregion
