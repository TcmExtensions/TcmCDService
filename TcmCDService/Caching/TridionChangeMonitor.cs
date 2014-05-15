#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Tridion Change Monitor
// ---------------------------------------------------------------------------------
//	Date Created	: April 4, 2014
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
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace TcmCDService.Caching
{
	/// <summary>
	/// <see cref="TridionChangeMonitor" /> is a <see cref="T:System.Runtime.Caching.ChangeMonitor" />
	/// </summary>
	public sealed class TridionChangeMonitor : ChangeMonitor
	{
		private TridionDependency mTridionDependency;
		private String mUniqueId;

		/// <summary>
		/// Gets a value that represents the <see cref="T:System.Runtime.Caching.ChangeMonitor" /> class instance.
		/// </summary>
		/// <returns>The identifier for a change-monitor instance.</returns>
		public override String UniqueId
		{
			get
			{
				return mUniqueId;
			}
		}

		/// <summary>
		/// Prevents a default instance of the <see cref="TridionChangeMonitor"/> class from being created.
		/// </summary>
		private TridionChangeMonitor()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TridionChangeMonitor"/> class.
		/// </summary>
		/// <param name="tridionDependency">Associated <see cref="T:TcmCDService.Cache.TridionDependency" /></param>
		/// <exception cref="System.ArgumentNullException">tridionDependency</exception>
		public TridionChangeMonitor(TridionDependency tridionDependency)
		{
			if (tridionDependency == null)
				throw new ArgumentNullException("tridionDependency");

			bool flag = true;

			try
			{
				mTridionDependency = tridionDependency;
				mTridionDependency.OnChange += new EventHandler(this.OnDependencyChanged);
				mUniqueId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

				flag = false;
			}
			finally
			{
				base.InitializationComplete();

				if (flag)
					base.Dispose();
			}
		}

		/// <summary>
		/// Releases all managed and unmanaged resources and any references to the <see cref="T:System.Runtime.Caching.ChangeMonitor" /> instance. This overload must be implemented by derived change-monitor classes.
		/// </summary>
		/// <param name="disposing">true to release managed and unmanaged resources and any references to a <see cref="T:System.Runtime.Caching.ChangeMonitor" /> instance; false to release only unmanaged resources. When false is passed, the <see cref="M:System.Runtime.Caching.ChangeMonitor.Dispose(System.Boolean)" /> method is called by a finalizer thread and any external managed references are likely no longer valid because they have already been garbage collected.</param>
		protected override void Dispose(bool disposing)
		{
		}

		/// <summary>
		/// Called when the <see cref="T:TcmCDService.Cache.TridionDependency" /> is changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void OnDependencyChanged(object sender, EventArgs e)
		{
			base.OnChanged(null);
		}
	}
}
