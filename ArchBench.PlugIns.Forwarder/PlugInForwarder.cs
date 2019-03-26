using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HttpServer;
using HttpServer.Sessions;
using System.Linq;

namespace ArchBench.PlugIns.Forwarder
{
    public class PlugInForwarder : IArchServerHTTPPlugIn
    {
        //private readonly Dictionary<string,string> mServers = new Dictionary<string, string>();
        private readonly List<KeyValuePair<string, int>> mServers = new List<KeyValuePair<string, int>>();
        private readonly TcpListener mListener;
        private int                  mNextServer;
        private Thread               mRegisterThread;        

        public PlugInForwarder()
        {
            //AddServer( "sidoc", "127.0.0.1:8083" );
 			mListener = new TcpListener( IPAddress.Any, 9000 );            
        }

        /*private void AddServer( string aName, string aUrl )
        {
            if ( mServers.ContainsKey( aName ) )
                mServers[aName] = aUrl;
            else
                mServers.Add( aName, aUrl );
        }*/

		private void ReceiveThreadFunction()
        {
            try
            {
                // Start listening for client requests.
                mListener.Start();

                // Buffer for reading data
                byte[] bytes = new byte[256];

                // Enter the listening loop.
                while (true)
                {
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = mListener.AcceptTcpClient();

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int count = stream.Read(bytes, 0, bytes.Length);
                    if ( count != 0 )
                    {
                        // Translate data bytes to a ASCII string.
                        string data = Encoding.ASCII.GetString( bytes, 0, count );
                        char operation = data[0];
                        string server  = data.Substring( 1, data.IndexOf( '-', 1 ) - 1 );
                        string port    = data.Substring( data.IndexOf( '-', 1 ) + 1 );
                        switch ( operation )
                        {
                            case '+' : 
                                Regist( server, int.Parse( port ) );
                                break;
                            case '-' : 
                                Unregist( server, int.Parse( port ) );
                                break;
                        }                 						
                    }

                    client.Close();
                }
            }
            catch ( SocketException e )
            {
                Host.Logger.WriteLine( "SocketException: {0}", e );
            }
            finally
            {
               mListener.Stop();
            }
        }        

		private void Regist( string aAddress, int aPort )
        {
            if ( mServers.Any( p => p.Key == aAddress && p.Value == aPort ) ) return;
            mServers.Add( new KeyValuePair<string, int>( aAddress, aPort) );
            Host.Logger.WriteLine( "Added server {0}:{1}.", aAddress, aPort );
        }

        private void Unregist( string aAddress, int aPort )
        {
            if ( mServers.Remove( new KeyValuePair<string, int>( aAddress, aPort ) ) )
            {
                Host.Logger.WriteLine( "Removed server {0}:{1}.", aAddress, aPort );
            }
            else
            {
                Host.Logger.WriteLine( "The server {0}:{1} is not registered.", aAddress, aPort );
            }
        }        

        #region IArchServerModulePlugIn Members

        public bool Process( IHttpRequest aRequest, IHttpResponse aResponse, IHttpSession aSession )
        {
            /*if ( mServers.ContainsKey(aRequest.UriParts[0]) )
            {*/
                string sourceHost = $"{aRequest.Uri.Host}:{aRequest.Uri.Port}";
                string sourcePath = aRequest.UriPath;

 				if ( mServers.Count == 0 ) return false;
            	mNextServer = ( mNextServer + 1 ) % mServers.Count;

                //string targetHost = mServers[aRequest.UriParts[0]];
                //string targetPath = aRequest.UriPath.Substring(aRequest.UriPath.IndexOf( '/', 1 ) );

                //Host.Logger.WriteLine(aRequest.UriPath);

                string targetUrl = $"http://{mServers[mNextServer].Key}:{mServers[mNextServer].Value}{aRequest.UriPath}";
                Uri uri = new Uri( targetUrl );

                Host.Logger.WriteLine( $"Forwarding request from server {sourceHost} to server {targetUrl}" );

                WebClient client = new WebClient();
                try
                {
                    if ( aRequest.Headers["Cookie"] != null )
                    {
                        client.Headers.Add( "Cookie", aRequest.Headers["Cookie"] );    
                    }

                    byte[] bytes = null;
                    if ( aRequest.Method == Method.Post )
                    {
    	                NameValueCollection form = new NameValueCollection();
                        foreach ( HttpInputItem item in aRequest.Form )
                        {
                            form.Add( item.Name, item.Value );
                        }
		                bytes = client.UploadValues( uri, form );		
                    }
                    else
                    {
                        bytes = client.DownloadData( uri );
                    }

                    aResponse.ContentType = client.ResponseHeaders[HttpResponseHeader.ContentType];
                    if ( client.ResponseHeaders["Set-Cookie"] != null )
                    {
                        aResponse.AddHeader( "Set-Cookie", client.ResponseHeaders["Set-Cookie"] );
                    }


            
                    if ( aResponse.ContentType.StartsWith( "text/html" ) )
                    {
                        string data = client.Encoding.GetString( bytes );

                        Host.Logger.WriteLine(data);

                        data = data.Replace($"http://{mServers[mNextServer].Key}:{mServers[mNextServer].Value}/", "/" );
                        
                        /*data = data.Replace( "href=\"/", "href=\"/" + aRequest.UriParts[0] + "/" );
                        data = data.Replace( "src=\"/", "src=\"/" + aRequest.UriParts[0] + "/" );
                        data = data.Replace( "action=\"/", "action=\"/" + aRequest.UriParts[0] + "/" );*/

                        StreamWriter writer = new StreamWriter( aResponse.Body, client.Encoding );
                        writer.Write(data); writer.Flush();
                    }
                    else
                    {
                        aResponse.Body.Write(bytes, 0, bytes.Length);
                    }
                }
                catch (Exception e)
                {
                    Host.Logger.WriteLine( "Error on plugin Forwarder : {0}", e.Message );
                }

                return true;
            //}

            //return false;
        } 

        #endregion

        #region IArchServerPlugIn Members

        public string Name => "ArchServer Forwarder Plugin";

        public string Description => "Forward any request to port 8083";

        public string Author => "Leonel Nobrega";

        public string Version => "1.0";
        public bool Enabled { get; set; }

        public IDictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        public IArchServerPlugInHost Host { get; set; }

        public void Initialize()
        {
            mRegisterThread = new Thread( ReceiveThreadFunction );
            mRegisterThread.IsBackground = true;
            mRegisterThread.Start();        	
        }

        public void Dispose()
        {
        }

        #endregion
    }
}