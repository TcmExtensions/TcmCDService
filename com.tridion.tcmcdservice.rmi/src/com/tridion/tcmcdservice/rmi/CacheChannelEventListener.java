/**
 *   @description Cache Channel Event Listener
 *   @created April 5, 2014  
 *	 @author Rob van Oostenrijk
 */
package com.tridion.tcmcdservice.rmi;

public abstract interface CacheChannelEventListener
{
	public abstract void onLog(String message);
	
	public abstract void onCacheEvent(String region, String key, int eventType);
  
	public abstract void onDisconnect();
  
	public abstract void onConnect();
}
