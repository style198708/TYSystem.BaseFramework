using log4net.Appender;
using log4net.Core;
using log4net.Util;
using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TYSystem.BaseFramework.Logging.Log4Net
{
    public class Log4NetTcpAppender : AppenderSkeleton
    {
        private Encoding _encoding = Encoding.Unicode;
        private string _remoteAddress;
        private int _remotePort;
        private IPEndPoint _remoteEndPoint;
        private TcpClient _client;

        public string RemoteAddress
        {
            get
            {
                return this._remoteAddress;
            }
            set
            {
                this._remoteAddress = value;
            }
        }

        public int RemotePort
        {
            get
            {
                return this._remotePort;
            }
            set
            {
                if (value < 0 || value > (int)ushort.MaxValue)
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", (object)value, "The value specified is less than " + 0.ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + " or greater than " + ((int)ushort.MaxValue).ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + ".");
                this._remotePort = value;
            }
        }

        public Encoding Encoding
        {
            get
            {
                return this._encoding;
            }
            set
            {
                this._encoding = value;
            }
        }

        protected TcpClient Client
        {
            get
            {
                return this._client;
            }
            set
            {
                this._client = value;
            }
        }

        protected IPEndPoint RemoteEndPoint
        {
            get
            {
                return this._remoteEndPoint;
            }
            set
            {
                this._remoteEndPoint = value;
            }
        }

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (this.RemoteAddress == null)
                throw new ArgumentNullException("The required property 'Address' was not specified.");
            if (this.RemotePort < 0 || this.RemotePort > (int)ushort.MaxValue)
                throw SystemInfo.CreateArgumentOutOfRangeException("this.RemotePort", (object)this.RemotePort, "The RemotePort is less than " + 0.ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + " or greater than " + ((int)ushort.MaxValue).ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + ".");
            this.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(this.RemoteAddress), this.RemotePort);
            this.InitializeClientConnection();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                NetworkStream ntwStream = this.Client.GetStream();
                var aa = this.RenderLoggingEvent(loggingEvent).ToCharArray();
                byte[] bytes = this._encoding.GetBytes(this.RenderLoggingEvent(loggingEvent).ToCharArray());
                ntwStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                this.ErrorHandler.Error("Unable to send logging event to remote host " + this.RemoteAddress.ToString() + " on port " + (object)this.RemotePort + ".", ex, ErrorCode.WriteFailure);
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (this.Client == null)
                return;
            Client.Dispose();
            this.Client = null;
        }

        protected virtual void InitializeClientConnection()
        {
            try
            {
                this.Client = new TcpClient(this.RemoteAddress.ToString(), this.RemotePort);
            }
            catch (Exception ex)
            {
                this.ErrorHandler.Error("Could not initialize the UdpClient connection on port " + this.RemotePort.ToString((IFormatProvider)NumberFormatInfo.InvariantInfo) + ".", ex, ErrorCode.GenericFailure);
                this.Client = null;
            }
        }
    }
}
