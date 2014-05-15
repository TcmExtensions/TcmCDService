/**
 *   @description Cache Channel Connector
 *   @created April 5, 2014  
 *	 @author Rob van Oostenrijk
 */
package com.tridion.tcmcdservice.rmi;

import java.net.MalformedURLException;
import java.rmi.Naming;
import java.rmi.NotBoundException;
import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.UUID;

import com.tridion.cache.CacheEvent;
import com.tridion.cache.RemoteCacheChannelService;
import com.tridion.cache.RemoteCacheListener;

/**
 * CacheChannelConnector connects to a remote Tridion cache channel service and listens for events.
 */
@SuppressWarnings("serial")
public class CacheChannelConnector extends UnicastRemoteObject implements RemoteCacheListener
{
	private static final String DEFAULT_RMI_HOST = "127.0.0.1";
	private static final int DEFAULT_RMI_PORT = 1099;
	
	private static CacheChannelEventListener mNullListener = new CacheChannelEventListener()
	{
		public void onLog(String message) {	}

		public void onCacheEvent(String region, String key, int eventType) { }

		public void onDisconnect() { }

		public void onConnect() { }
	};	
	
	private volatile transient RemoteCacheChannelService mService = null;
	private String mIdentifier = "TcmCDService-" + UUID.randomUUID().toString();
	
	private String mHost = DEFAULT_RMI_HOST;
	private int mPort = DEFAULT_RMI_PORT;
	private String mInstanceIdentifier = null;
	
	private boolean mIsClosed = false;
		
    private CacheChannelEventListener mListener = mNullListener;	

	/**
	 * Verify if the service is available
	 */
	private void verifyOpenState()
	{
		if (mIsClosed) 
		{
			mListener.onLog("Method was called on closed instance");
			throw new IllegalStateException("Method was called on closed instance");
		}
	}
	
	/**
	 * Connects this cache channel connector to the remote service
	 */
	private void connect()  
			throws CacheException
	{		
		String serviceUri = "//" + mHost + ":" + mPort + "/" + "CacheChannelService";
	
		// Append instance identifier if requested
		if (mInstanceIdentifier != null) 
			serviceUri = serviceUri + "_" + mInstanceIdentifier;
		
		mListener.onLog("Attempting to look up cache channel service on " + serviceUri);
		
		try
		{
			mService = null;
			
			// Connect to the service and add our current instance as a new listener
			RemoteCacheChannelService service = (RemoteCacheChannelService)Naming.lookup(serviceUri);
			service.addListener(this);
	    
			mService = service;
				
			mListener.onLog("Successfully set cache channel service");
			
			mListener.onConnect();
			
			mIsClosed = false;
		}
		catch (NotBoundException e)
		{			
			throw new CacheException("The remote CacheChannelService [" + mService + "] was not bound to " + serviceUri, e);
		}
		catch (MalformedURLException e)
		{
			throw new CacheException("Check configuration for the CacheChannelService [" + mService + "] on " + serviceUri, e);
		}
		catch (RemoteException e)
		{
			throw new CacheException("Could not get a connection with the CacheChannelService[" + mService + "] on " + serviceUri, e);
		}		
	}
	
	/**
	 * Creates a new CacheChannelConnector
	 * 
	 * @param host Java RMI host to connect to
	 * @param port Java RMI port to connect to
	 */
	public CacheChannelConnector(String host, int port)
		throws RemoteException
	{
		this(host, port, null);		
	}
	
	/**
	 * Creates a new CacheChannelConnector
	 * 
	 * @param host Java RMI host to connect to
	 * @param port Java RMI port to connect to
	 * @param instanceIdentifier Tridion cache channel instance identifier
	 */
	public CacheChannelConnector(String host, int port, String instanceIdentifier)
		throws RemoteException
	{
		mHost = host;
		mPort = port;
		
		if ("".equals(instanceIdentifier)) 
			instanceIdentifier = null;
		
		mInstanceIdentifier = instanceIdentifier;
		
		mListener.onLog("RMIConnector Host " + mHost + " Port " + mPort + " (InstanceID:" + mInstanceIdentifier + ")");		
	}
	
	/**
	 * Configure the {@link CacheChannelEventListener} for this CacheChannelConnector
	 * 
	 * @param listener Event listener to pass events to, a value of null assigns an empty listener
	 */
	public void setListener(CacheChannelEventListener listener)
	{
		mListener = (listener != null ? listener : mNullListener);
	}	

	/**
	 * Broadcast a cache event to all registered cache channel service listeners.
	 * 
	 * @param cacheEvent Event to broadcast
	 * @throws CacheException Exception wrapper around any errors
	 */
	public void broadcastEvent(CacheEvent cacheEvent)
		throws CacheException
	{
		verifyOpenState();
		
		if (mService == null) 
			connect();
		
		try
		{
			mListener.onLog("Starting broadcasting event for key: " + cacheEvent.getKey());
			
			mService.broadcastEvent(mIdentifier, cacheEvent);
			
			mListener.onLog("Broadcasting event finished for key: " + cacheEvent.getKey());
		}
		catch (RemoteException e)
		{
			mService = null;
			mListener.onDisconnect();
			
			throw new CacheException("Unable to broadcast event", e);
		}
	}

	/**
	 * Validate the connection state of the CacheChannelConnector and connect if necessary
	 * 
	 * @throws CacheException Unexpected remote RMI error occurred
	 */
	public void validate()
		throws CacheException  
	{
		verifyOpenState();
		
		if (mService != null) 
		{
			try
			{
				if (!mService.isAlive(mIdentifier))
				{
					mListener.onLog("Client no longer valid according to service, will attempt to reconnect");
					
					mService = null;
					
					mListener.onDisconnect();					
				}
			}
			catch (RemoteException e)
			{
				mListener.onLog("RMI Cache channel connector lost connection");
				mService = null;
				
				mListener.onDisconnect();
				
				throw new CacheException("RMI Cache channel connector lost connection", e);
			}
		}
		
		if (mService == null) 
			connect();
	}

	/**
	 * Handle event forwards the {@link cacheEvent} to the .NET cacheClient
	 * 
	 * @param cacheEvent Event to forward
	 */
	public void handleEvent(CacheEvent cacheEvent)
	{
		if (!mIsClosed) 
			mListener.onCacheEvent(cacheEvent.getRegionPath(), cacheEvent.getKey().toString(), cacheEvent.getType());
	}

	/**
	 * Disconnect the remote cache channel connection
	 */
	public void disconnect()
	{
		mIsClosed = true;
		
		if (mService != null)
		{
			mListener.onDisconnect();    
			mService = null;
		}
	}

	/**
	 * Return this cache channel connector unique client identifier
	 */
	public String getGUID()
	{
		return mIdentifier;
	}
}


