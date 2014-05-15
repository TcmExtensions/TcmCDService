#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Cache Event Type
// ---------------------------------------------------------------------------------
//	Date Created	: April 5, 2014
//	Author			: Rob van Oostenrijk
// ---------------------------------------------------------------------------------
// 	Change History
//	Date Modified       : 
//	Changed By          : 
//	Change Description  : 
//
////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace TcmCDService.CacheTypes
{
	/// <summary>
	/// <see cref="CacheEventType" /> defines all the different cache event types
	/// </summary>
	public enum CacheEventType
	{
		/// <summary>
		/// Instruction to flush all cache items in the current region
		/// </summary>
		Flush = 0,
		/// <summary>
		/// Instruction to invalidate the item in the current region identified by the key
		/// </summary>
		Invalidate = 1
	}
}
