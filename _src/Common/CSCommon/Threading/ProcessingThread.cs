using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zoka.CSCommon.Threading
{
	/// <summary>Class, which controls the new thread (start, stop) and repeating of some operation in specified intervals</summary>
	public abstract class ProcessingThread
	{
		/// <summary>Options for ProcessingThread class</summary>
		public class ProcessingThreadOptions
		{
			/// <summary>Name of the thread to create (default="ProcessingThread")</summary>
			public string									ThreadName { get; set; } = "ProcessingThread";
			/// <summary>Priority of the thread (default=Normal)</summary>
			public ThreadPriority							ThreadPriority { get; set; } = ThreadPriority.Normal;
			/// <summary>How long in ms, should the Stop function wait for thread to finish (default = -1)</summary>
			/// <remarks>0=do not wait, -1=wait indefinitely, >0=wait for X ms</remarks>
			public int										WaitForThreadFinishMs { get; set; } = -1;
			/// <summary>How long in ms, should the processing thread wait before next pass (default = 1000ms)</summary>
			/// <remarks>
			///		This value may be changed anytime and processing thread uses the actual value.
			///		Beware of not using 0 as the value, which would cause the thread to be CPU consuming.
			/// </remarks>
			public int										WaitBeforeNextPass { get; set; } = 1000;

			/// <summary>If set, it wait WaitBeforeNextPass miliseconds before calling ThreadRepeatingOperation for first time (default = false)</summary>
			public bool										WaitBeforeFirstRun { get; set; } = false;
		}

		#region Private data members

		private bool										m_Initialized = false;
		private Thread										m_Thread = null;
		private AutoResetEvent								m_EndEvent = null;
		/// <summary>Options of thread procedure</summary>
		protected readonly ProcessingThreadOptions			m_Options;

		#endregion // Private data members

		#region Construction

		/// <summary>Constructor</summary>
		public ProcessingThread(ProcessingThreadOptions _options)
		{
			m_Options = _options;
        }

		#endregion // Construction

		#region Initialize

		/// <summary>Will run/start the thread</summary>
		public void											Run() 
		{
			if (m_Initialized)
				return;

			m_Thread = new Thread(new ThreadStart(ThreadProc));
			m_EndEvent = new AutoResetEvent(false);
			m_Thread.Priority = m_Options.ThreadPriority;
			m_Thread.Name = m_Options.ThreadName;
			m_Thread.Start();

			m_Initialized = true;
		}

		/// <summary>Will try to stop thread</summary>
		/// <returns>true, if thread was finished successfully, false, if thread did not finished before specified timeout</returns>
		public Task<bool>									Stop()
		{
			if (!m_Initialized)
				return Task.FromResult<bool>(true);

			// inidicate end
			m_EndEvent.Set();
			// waits until finished
			bool res;
			if (m_Options.WaitForThreadFinishMs < 0)
			{
				m_Thread.Join();
				res = true;
			}
			else
				res = m_Thread.Join(m_Options.WaitForThreadFinishMs);
			// cleanup
			m_Thread = null;
			m_EndEvent = null;

			m_Initialized = false;

			return Task.FromResult(res);
		}

		#endregion // Initialize

		#region Thread proc

		private void										ThreadProc()
		{
			if (m_Options.WaitBeforeFirstRun)
			{
				if (Wait())
					return;
			}

			while (true)
			{
				var res = ThreadRepeatingOperation();
				if (!res)
					break;

				res = Wait();
				if (res) // end event was signaled
					break;
			}
		}

		/// <summary>Wait for time specified in options. If end signal is signaled it returns true, false otherwise.</summary>
		private bool										Wait()
		{
			int wait_time;
			if (m_Options.WaitBeforeNextPass < 0)
				wait_time = 1000;
			else
			{
				wait_time = m_Options.WaitBeforeNextPass;
			}
			return m_EndEvent.WaitOne(wait_time);
		}

		/// <summary>The abstract function, which is called on every pass of the thread loop and should contain the operation processed on the thread.</summary>
		/// <remarks>if false is returned, the thread is finished at the end of current thread loop.</remarks>
		protected abstract bool								ThreadRepeatingOperation();

        #endregion // Thread proc
    }
}
