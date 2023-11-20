using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoka.CSCommon.Threading
{
	/// <summary>Implementation of processing thread, which takes Func to be run in thread periodically</summary>
	public class ProcessingThreadFunc : ProcessingThread
	{
		private readonly Func<bool>							m_RepeatingOperationFunc;

		/// <summary>Constructor</summary>
		public ProcessingThreadFunc(ProcessingThread.ProcessingThreadOptions _options, Func<bool> _repeating_operation_func)
			: base(_options)
		{
			m_RepeatingOperationFunc = _repeating_operation_func;
		}

		/// <inheritdoc />
		protected override bool ThreadRepeatingOperation()
		{
			return m_RepeatingOperationFunc();
		}
	}
}
