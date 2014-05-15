/**
 *   @description Cache Exception
 *   @created April 5, 2014  
 *	 @author Rob van Oostenrijk
 */
package com.tridion.tcmcdservice.rmi;

@SuppressWarnings("serial")
public class CacheException extends Exception
{
	public CacheException(String message)
	{
		super(message);
	}

	public CacheException(Throwable embedded)
	{
		super(embedded);
	}

	public CacheException(String message, Throwable embedded)
	{
		super(message, embedded);
	}
}