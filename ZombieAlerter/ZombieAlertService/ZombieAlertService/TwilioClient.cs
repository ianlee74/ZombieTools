using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Twilio;
using System.Timers;

namespace ZombieAlertService
{
    public class TwilioClient
    {
        private static string _accountSid = "AC976ae9d3e910a63384ecf244daf0fe5b";
        private static string _authToken = "6eeffbb24ad62423e959d068c612c8b3";
        private static string _fromNumber = "+16152390406";
        private static TwilioRestClient _twilio;
        private DateTime _lastDateProcessed = DateTime.MinValue;

        public delegate void MessageReceivedEventHandler(object sender, SMSMessage e);
        public event MessageReceivedEventHandler MessageReceived;

        public TwilioClient()
        {
            _twilio = new TwilioRestClient(_accountSid, _authToken);

            var twilioTimer = new Timer();
            twilioTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            twilioTimer.Start();

        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Timer timer = (Timer)source;
            timer.Stop();

            var smsMessages = _twilio.ListSmsMessages();
            var messagesToProcess = smsMessages.SMSMessages.Where(m => m.DateSent >= _lastDateProcessed).Reverse();
            foreach (SMSMessage message in messagesToProcess)
            {
                if (_lastDateProcessed < message.DateSent)
                {
                    _lastDateProcessed = message.DateSent;
                    if (MessageReceived != null)
                    {
                        MessageReceived(this, message);
                    }
                }
            }

            timer.Interval = 10000;
            timer.Start();
        }

        public static void SendSms(string toNumber, string message)
        {
            var msg = _twilio.SendSmsMessage(_fromNumber, toNumber, message);

            //var smsMessages = _twilio.ListSmsMessages();
            //SMSMessage .GetSmsMessage(msg.Sid);
        }

        //public event MessageReceived;


        public static string AccountSid
        {
            set { _accountSid = value; }
        }

        public static string AuthToken
        {
            set { _authToken = value; }
        }

        public static string FromNumber
        {
            set { _fromNumber = value; }
        }
    }
}